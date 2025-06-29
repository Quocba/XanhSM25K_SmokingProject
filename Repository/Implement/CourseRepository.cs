using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Payload.Base;
using Domain.Payload.Request.Course;
using Domain.Payload.Response;
using Domain.Share.Common;
using Microsoft.EntityFrameworkCore;
using OhBau.Service.CloudinaryService;
using Repository.Interface;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Repository.Implement
{
    public class CourseRepository(DBContext _context, ICloudinaryService _cloudinaryService) : ICourseRepository
    {
        public async Task<ApiResponse<string>> CreateCourse(Guid userId,CreateCourseDTO request)
        {
            try
            {
                var getCenterByUser = await _context.Centers.FirstOrDefaultAsync(x => x.UserId == userId);
                var createNew = new Courses
                {
                    Id = Guid.NewGuid(),
                    Title = request.Title,
                    Duration = request.Duration,
                    Type = request.Type,
                    Description = request.Description,
                    Image = await _cloudinaryService.Upload(request.Image),
                    Price = request.Price,
                    CenterId = getCenterByUser.Id,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    UpdatedAt = DateTime.Now
                };

                await _context.Courses.AddAsync(createNew);
                await _context.SaveChangesAsync();
                return new ApiResponse<string>
                {
                    StatusCode = 200,
                    Message = "Create course success",
                    Data  = null
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        public async Task<ApiResponse<PagingResponse<GetCoursesResponse>>> GetCourses(
            int pageNumber, int pageSize,
            string? searchKey,
            string? type)
        {
            var getCourse = _context.Courses
                .Where(x => x.IsDeleted == false && x.IsActive == true);

            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                getCourse = getCourse.Where(x => x.Title.Contains(searchKey));
            }

            if (!string.IsNullOrWhiteSpace(type))
            {
                if (Enum.TryParse<CourseType>(type, true, out var parsedType))
                {
                    getCourse = getCourse.Where(x => x.Type == parsedType);
                }
                else
                {
                    return new ApiResponse<PagingResponse<GetCoursesResponse>>
                    {
                        StatusCode = StatusCodes.BadRequest,
                        Message = "Invalid course type",
                        Data = null
                    };
                }
            }

            var courses = getCourse.Select(x => new GetCoursesResponse
            {
                Id = x.Id,
                Title = x.Title,
                Image = x.Image,
                Price = x.Price,
                Type = x.Type.ToString()
            });

            var response = await courses.ToPagedListAsync(pageNumber, pageSize);

            return new ApiResponse<PagingResponse<GetCoursesResponse>>
            {
                StatusCode = StatusCodes.OK,
                Message = "Get courses success",
                Data = response
            };
        }

        public async Task<ApiResponse<string>> UpdateCourse(Guid courseId, EditCourseDTO request)
        {
            try
            {
                var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId && !c.IsDeleted);
                if (course == null)
                {
                    return new ApiResponse<string>
                    {
                        StatusCode = StatusCodes.NotFound,
                        Message = "Course not found",
                        Data = null
                    };
                }

                course.Title = request.Title ?? course.Title;
                course.Type = request.Type ?? course.Type;
                course.Duration = request.Duration ?? course.Duration;
                course.Description = request.Description ?? course.Description;
                course.Price = request.Price ?? course.Price;
                course.UpdatedAt = DateTime.Now;
                course.CreatedAt = DateTime.Now;

                if (request.Image != null)
                {
                    var newImageUrl = await _cloudinaryService.Upload(request.Image);
                    course.Image = newImageUrl;
                }

                _context.Courses.Update(course);
                await _context.SaveChangesAsync();

                return new ApiResponse<string>
                {
                    StatusCode = StatusCodes.OK,
                    Message = "Update course success",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Update failed: {ex.Message}");
            }
        }
        public async Task<ApiResponse<string>> DeleteCourse(Guid courseId)
        {
            try
            {
                var getCourse = await _context.Courses.FirstOrDefaultAsync(x => x.Id == courseId);
                if(getCourse == null)
                {
                    return new ApiResponse<string>
                    {
                        StatusCode  = StatusCodes.NotFound,
                        Message = "Course not found",
                        Data = null
                    };
                }

                getCourse.IsDeleted = false;
                _context.Courses.Update(getCourse);
                await _context.SaveChangesAsync();
                return new ApiResponse<string>
                {
                    StatusCode = StatusCodes.OK,
                    Message = "Delete success",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<ApiResponse<GetCourse>> GetCourse(Guid courseId)
        {
            var getCourse = await _context.Courses.Include(x => x.Center).FirstOrDefaultAsync(x => x.Id.Equals(courseId));
            if (getCourse == null) return new ApiResponse<GetCourse>
            {
                StatusCode = StatusCodes.NotFound,
                Message = "Course not found",
                Data = null
            };

            var response = new GetCourse
            {
                Id = getCourse.Id,
                Title = getCourse.Title,
                Duration = getCourse.Duration,
                Type = getCourse.Type,
                Description = getCourse.Description,
                Image = getCourse.Image,
                Price = getCourse.Price,
                CreatedAt = getCourse.CreatedAt,
                UpdatedAt = getCourse.UpdatedAt,
                IsActive = getCourse.IsActive,
                IsDeleted = getCourse.IsDeleted,

                CenterId = getCourse.CenterId,
                Name = getCourse.Center?.Name,
                Location = getCourse.Center?.Location,
                Hotline = getCourse.Center.HotLine,
                Email = getCourse.Center?.Email,
                DirectorName = getCourse.Center?.DirectorName,
                EstablishedDate = getCourse.Center?.EstablishedDate,
                Capacity = getCourse.Center?.Capacity,
                CurrentPatientCount = getCourse.Center?.CurrentPatientCount,
                CenterType = getCourse.Center?.Type,
                Notes = getCourse.Center?.Notes,
                MainImage = getCourse.Center?.Image
            };

            return new ApiResponse<GetCourse>
            {
                StatusCode = StatusCodes.OK,
                Message = "Get course success",
                Data = response
            };
        }

        public async Task<ApiResponse<string>> HiddenCourse(Guid courseId)
        {
            try
            {
                var getCourse = await _context.Courses.FirstOrDefaultAsync(x => x.Id == courseId);
                if (getCourse == null)
                {
                    return new ApiResponse<string>
                    {
                        StatusCode = StatusCodes.NotFound,
                        Message = "Course not found",
                        Data = null
                    };
                }

                getCourse.IsActive = false;
                _context.Courses.Update(getCourse);
                await _context.SaveChangesAsync();
                return new ApiResponse<string>
                {
                    StatusCode = StatusCodes.OK,
                    Message = "Hidden success",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
