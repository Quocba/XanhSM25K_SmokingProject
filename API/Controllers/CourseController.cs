using Domain.Payload.Request.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/course")]
    public class CourseController(ICourseRepository _courseRepository, ILogger<CourseController> _logger) : Controller
    {
        [HttpPost("create-course")]
        [Authorize(Roles = "Center")]
        public async Task<IActionResult> CreateCourse([FromForm] CreateCourseDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var userId = JWTUtil.GetUserId(User);
                var response = await _courseRepository.CreateCourse(Guid.Parse(userId), request);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Create Course API" + ex.Message, ex.StackTrace);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch("edit-course/{courseId}")]
        [Authorize(Roles = "Center")]
        public async Task<IActionResult> EditCourse(Guid courseId, [FromForm] EditCourseDTO request)
        {
            try
            {
                var response = await _courseRepository.UpdateCourse(courseId, request);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Edit Course API " + ex.Message, ex.ToString());
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpGet("get-courses")]
        public async Task<IActionResult> GetCourses([FromQuery] int pageNumber, [FromQuery] int pageSize,
                                                    [FromQuery] string? searchKey, [FromQuery] string? type)
        {
            var response = await _courseRepository.GetCourses(pageNumber, pageSize, searchKey, type);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("get-course")]
        public async Task<IActionResult> GetCourse([FromQuery] Guid courseId)
        {
            var response = await _courseRepository.GetCourse(courseId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("hidden-course/{courseId}")]
        [Authorize(Roles = "Center")]
        public async Task<IActionResult> HiddenCourse(Guid courseId)
        {
            try
            {
                var response = await _courseRepository.HiddenCourse(courseId);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Hidden Course API " + ex.Message,ex.StackTrace);
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPut("delete-course/{courseId}")]
        [Authorize(Roles = "Center")]
        public async Task<IActionResult> DeleteCourse(Guid courseId)
        {
            try
            {
                var response = await _courseRepository.DeleteCourse(courseId);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Delete Course API " + ex.Message, ex.StackTrace);
                return StatusCode(500, ex.ToString());
            }
        }
    }
}
