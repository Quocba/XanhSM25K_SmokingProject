using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Payload.Base;
using Domain.Payload.Request.Booking;
using Domain.Payload.Response;
using Domain.Share.Common;
using Domain.Share.Util;
using EmailService.DTO;
using EmailService.Interface;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;

namespace Repository.Implement
{
    public class BookingRepository(DBContext _context, IEmailSender _sender) : IBookingRepository
    {
        public async Task<ApiResponse<string>> Booking(CreateBookings reqeuest)
        {
            try
            {
                var checkBooking = await _context.Bookings
                                           .FirstOrDefaultAsync(x => x.CourseId == reqeuest.CourseId && x.UserId == reqeuest.UserId);
                if (checkBooking != null)
                {
                    return new ApiResponse<string>
                    {
                        StatusCode = StatusCodes.Conflict,
                        Message = "You have booked this course. Please check your booking history.",
                        Data = null
                    };
                }

                var getCourse = await _context.Courses
                                              .Include(x => x.Center)
                                              .FirstOrDefaultAsync(x => x.Id == reqeuest.CourseId);

                var newBooking = new Bookings
                {
                    Id = Guid.NewGuid(),
                    CourseId = reqeuest.CourseId,
                    UserId = reqeuest.UserId,
                    IsActive = true,
                    IsDeleted = false,
                    TotalPrice = getCourse.Price,
                    Type = reqeuest.PaymentType,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    ReasonCancel = null,
                    Status = PaymentStatusEnum.Pending

                };

                await _context.Bookings.AddAsync(newBooking);
                await _context.SaveChangesAsync();

                string template = CommonUtil.HTMLLoading("BookingNotification.html");

                string emailHtml = template
                    .Replace("[[thongbao]]", $"Bạn đã đặt thành công khóa học{getCourse.Title}")
                    .Replace("[[CourseTitle]]", getCourse.Title)
                    .Replace("[[Duration]]", getCourse.Duration)
                    .Replace("[[Price]]", getCourse.Price.ToString("N0"))
                    .Replace("[[Location]]", getCourse.Center.Location);

                var getUserInfo = await _context.Users.FirstOrDefaultAsync(x => x.Id == reqeuest.UserId);

                var mailBody = new EmailRequest<string>
                {
                    To = getUserInfo.Email,
                    Subject = "[Bạn Đã Booking Thành Công Khóa Học Tại Trung Tâm Cai Nghiện Nguyễn Thành Vinh]",
                    Body = emailHtml
                };

                await _sender.SendEmailAsync(mailBody);

                return new ApiResponse<string>
                {
                    StatusCode = StatusCodes.OK,
                    Message = "Booking course success",
                    Data = newBooking.Id.ToString()
                };
            }
            catch (Exception ex) {

                throw new Exception(ex.ToString());
            }
        }

        public async Task<ApiResponse<CancelBooking>> CancelBooking(CancelBooking request)
        {
            try
            {
                var getBooking = await _context.Bookings
                                               .Include(x => x.Course)
                                               .ThenInclude(x => x.Center)
                                               .FirstOrDefaultAsync(x => x.Id == request.BookingId);
                if (getBooking == null) return new ApiResponse<CancelBooking>
                {
                    StatusCode = StatusCodes.NotFound,
                    Message = "Booking not found",
                    Data = null
                };

                getBooking.Status = PaymentStatusEnum.Cancelled;
                getBooking.ReasonCancel = request.ReasonCancel;
                _context.Bookings.Update(getBooking);
                await _context.SaveChangesAsync();


                string template = CommonUtil.HTMLLoading("BookingNotification.html");

                string emailHtml = template
                    .Replace("[[thongbao]]",$"Bạn đã hủy thành công khóa học{getBooking.Course.Title}")
                    .Replace("[[CourseTitle]]", getBooking.Course.Title)
                    .Replace("[[Duration]]", getBooking.Course.Duration)
                    .Replace("[[Price]]", getBooking.Course.Price.ToString("N0"))
                    .Replace("[[Location]]", getBooking.Course.Center.Location);

                var getUserInfo = await _context.Users.FirstOrDefaultAsync(x => x.Id == getBooking.UserId);

                var mailBody = new EmailRequest<string>
                {
                    To = getUserInfo.Email,
                    Subject = "[Bạn Đã Hủy Thành Công Đăng Ký Khóa Học Tại Trung Tâm Cai Nghiện Nguyễn Thành Vinh]",
                    Body = emailHtml    
                };

                await _sender.SendEmailAsync(mailBody);

                return new ApiResponse<CancelBooking>
                {
                    StatusCode = StatusCodes.OK,
                    Message = "Cancel booking success",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<ApiResponse<PagingResponse<GetBookings>>> GetAllBooking(int pageNumber, int pageSize, string? status)
        {
            var getBookings = _context.Bookings
                                        .Include(x => x.Course)
                                        .AsQueryable();
            if (!string.IsNullOrEmpty(status))
            {
                getBookings = getBookings.Where(x => x.Status.Equals(status));
            }

            var mapItem = getBookings.Select(b => new GetBookings
            {
                BookingId = b.Id,
                Course = b.Course.Title,
                PaymentType = b.Type.ToString(),
                Price = b.Course.Price,
                Status = b.Status.ToString(),
                Created = b.CreatedAt
            });

            var pagingReponse = await mapItem.ToPagedListAsync(pageNumber, pageSize);

            return new ApiResponse<PagingResponse<GetBookings>>
            {
                StatusCode = StatusCodes.OK,
                Message = "Get bookings success",
                Data = pagingReponse
            };
        }

        public async Task<ApiResponse<GetBooking>> GetBooking(Guid bookingId)
        {
            var getBooking = await _context.Bookings
                                           .Include(x => x.Course)
                                           .Include(x => x.User)
                                           .FirstOrDefaultAsync(x => x.Id == bookingId);
            if (getBooking == null)
            {
                return new ApiResponse<GetBooking> { 
                    
                    StatusCode  = StatusCodes.NotFound,
                    Message = "Booking not found",
                    Data = null
                };
            }

            var mapItem = new GetBooking
            {
                Id = getBooking.Id,
                TotalPrice = getBooking.TotalPrice,
                Status = getBooking.Status.ToString(),           
                ReasonCancel = getBooking.ReasonCancel,
                Type = getBooking.Type.ToString(),              
                Created = getBooking.CreatedAt,

                Course = getBooking.Course?.Title ?? "",
                Duration = getBooking.Course?.Duration ?? "",
                Description = getBooking.Course?.Description ?? "",
                CourseType = getBooking.Course?.Type.ToString() ?? "",
                Price = getBooking.Course?.Price ?? 0,
                Image = getBooking.Course?.Image ?? "",

                FullName = getBooking.User?.FullName ?? "",
                Email = getBooking.User?.Email ?? "",
                Phone = getBooking.User?.Phone ?? "",
                Address = getBooking.User?.Address ?? ""
            };

            return new ApiResponse<GetBooking>
            {
                StatusCode = StatusCodes.OK,
                Message = "Get booking success",
                Data = mapItem
            };
        }
        public async Task<ApiResponse<PagingResponse<GetBookings>>> GetBookings(Guid userId, int pageNumber, int pageSize, string? status)
        {
            var getBookings = _context.Bookings
                                            .Include(x => x.Course)
                                            .Where(x => x.UserId == userId);
            if (!string.IsNullOrEmpty(status))
            {
                getBookings = getBookings.Where(x => x.Status.Equals(status));
            }

            var mapItem = getBookings.Select(b => new GetBookings
            {
                BookingId = b.Id,
                Course = b.Course.Title,
                PaymentType = b.Type.ToString(),
                Price = b.Course.Price,
                Status = b.Status.ToString(),
                Created = b.CreatedAt
            });

            var pagingReponse = await mapItem.ToPagedListAsync(pageNumber, pageSize);

            return new ApiResponse<PagingResponse<GetBookings>>
            {
                StatusCode = StatusCodes.OK,
                Message = "Get bookings success",
                Data = pagingReponse
            };
        }

    }
}
