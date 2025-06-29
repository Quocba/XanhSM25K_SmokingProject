using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Payload.Base;
using Domain.Payload.Request.Course;
using Domain.Payload.Response;

namespace Repository.Interface
{
    public interface ICourseRepository
    {
        Task<ApiResponse<string>> CreateCourse(Guid userId,CreateCourseDTO request);
        Task<ApiResponse<string>> UpdateCourse(Guid courseId, EditCourseDTO request);
        Task<ApiResponse<PagingResponse<GetCoursesResponse>>> GetCourses(int pageNumber, int pageSize,string? searchKey, string? type);
        Task<ApiResponse<GetCourse>>GetCourse(Guid courseId);
        Task<ApiResponse<string>> HiddenCourse(Guid courseId);
        Task<ApiResponse<string>>DeleteCourse(Guid courseId);
    }
}
