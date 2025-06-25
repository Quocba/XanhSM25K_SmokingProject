using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Config;
using Domain.Entities;
using Domain.Payload.Base;
using Domain.Payload.Request;
using Domain.Payload.Response;
using Domain.Share.Common;
using Domain.Share.Util;
using EmailService.DTO;
using EmailService.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;

namespace Repository.Implement
{
    public class AuthenicationRepository(DBContext _context, IEmailSender _email, IOptions<JwtSettings> jwtSettings) : IAuthenicationRepository
    {

        private readonly JwtSettings _jwtSettings = jwtSettings.Value;
        public async Task<ApiResponse<string>> Active(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();

                if (string.IsNullOrWhiteSpace(token))
                {
                    return new ApiResponse<string>
                    {
                        StatusCode = StatusCodes.BadRequest,
                        Message = "Token không được để trống"
                    };
                }

                if (!handler.CanReadToken(token))
                {
                    return new ApiResponse<string>
                    {
                        StatusCode = StatusCodes.BadRequest,
                        Message = "Token không hợp lệ. Vui lòng kiểm tra định dạng JWT"
                    };
                }

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = handler.ValidateToken(token, validationParameters, out var validatedToken);

                var email = principal.FindFirst(JwtRegisteredClaimNames.Email)?.Value
                          ?? principal.FindFirst("email")?.Value
                          ?? principal.FindFirst(ClaimTypes.Email)?.Value
                          ?? principal.Claims.FirstOrDefault(c => c.Type.ToLower().Contains("email"))?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return new ApiResponse<string>
                    {
                        StatusCode = StatusCodes.BadRequest,
                        Message = "Token không hợp lệ hoặc không chứa thông tin email"
                    };
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return new ApiResponse<string>
                    {
                        StatusCode = StatusCodes.NotFound,
                        Message = "Không tìm thấy người dùng"
                    };
                }

                if (user.IsActive)
                {
                    return new ApiResponse<string>
                    {
                        StatusCode = StatusCodes.Conflict,
                        Message = "Tài khoản đã được kích hoạt trước đó"
                    };
                }

                user.IsActive = true;
                user.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                return new ApiResponse<string>
                {
                    StatusCode = StatusCodes.OK,
                    Message = "Kích hoạt tài khoản thành công",
                    Data = user.Email
                };
            }
            catch (SecurityTokenExpiredException)
            {
                return new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Unauthorized,
                    Message = "Token đã hết hạn"
                };
            }
            catch (SecurityTokenException ex)
            {
                return new ApiResponse<string>
                {
                    StatusCode = StatusCodes.BadRequest,
                    Message = $"Token không hợp lệ: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    StatusCode = StatusCodes.InternalServerError,
                    Message = $"Kích hoạt thất bại: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<LoginResponse>> Login(LoginRequest request)
        {
            try
            {
                var passwordHash = PasswordUtil.HashPassword(request.Password);

                var checkLogin = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(request.PhoneOrEmail) && x.Password.Equals(passwordHash)
                 && x.IsActive == true && x.IsDeleted == false
                 || x.Phone.Equals(request.PhoneOrEmail) && x.Password.Equals(passwordHash)
                 && x.IsActive == true && x.IsDeleted == false
                );

                if (checkLogin == null)
                {
                    return new ApiResponse<LoginResponse>
                    {
                        StatusCode = StatusCodes.NotFound,
                        Message = "Tài khoản đã bị chặn hoặc không tồn tài",
                        Data = null
                    };
                }

                var token = await JWTUtil.GenerateToken(checkLogin);

                return new ApiResponse<LoginResponse>
                {
                    StatusCode = StatusCodes.OK,
                    Message = "Login success",
                    Data = new LoginResponse
                    {
                        Role = checkLogin.Role.ToString(),
                        Token = token
                    }
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<ApiResponse<string>> Register(RegisterDTO request)
        {
            try
            {
                var checkEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (checkEmail != null)
                {
                    return new ApiResponse<string>
                    {
                        StatusCode = StatusCodes.Conflict,
                        Message = "Email đã được đăng ký",
                        Data = request.Email
                    };
                }


                var checkPhoneNumber = await _context.Users.FirstOrDefaultAsync(u => u.Phone == request.PhoneNumber);
                if (checkPhoneNumber != null)
                {
                    return new ApiResponse<string>
                    {
                        StatusCode = StatusCodes.Conflict,
                        Message = "Số điện thoại đã được đăng ký",
                        Data = request.PhoneNumber
                    };
                }

                var register = new Users
                {
                    Id = Guid.NewGuid(),
                    Address = "Chưa cập nhật",
                    Email = request.Email,
                    FullName = "Chưa cập nhật",
                    IsActive = false,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false,
                    Role = Domain.Entities.Enum.RoleEnum.User,
                    Password = PasswordUtil.HashPassword(request.Password),
                    Phone = request.PhoneNumber,
                    UpdatedAt = DateTime.Now
                };

                await _context.Users.AddAsync(register);

                var rawHtml = CommonUtil.HTMLLoading("RegisterActiveEmail.html");

                var token = JWTUtil.GenerateActivationToken(register.Email);
                var link = $"https://warm-cascaron-29c431.netlify.app/?token={token}";

                var finalHtml = rawHtml.Replace("{ACTIVATION_LINK}", link);

                var emailRequest = new EmailRequest<string>
                {
                    To = register.Email,
                    Subject = "[Winner Tech - Cai Nghiện Thuốc Lá] - [Kích Hoạt Tài Khoản]",
                    Body = finalHtml
                };
                
               await _email.SendEmailAsync(emailRequest);
               await _context.SaveChangesAsync();

                return new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Created,
                    Message = "Đăng ký thành công. Vui lòng kiểm tra email để kích hoạt tài khoản.",
                };
            }

            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

    }
}
