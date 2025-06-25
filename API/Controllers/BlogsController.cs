using Domain.Payload.Request.Blogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/blog")]
    public class BlogsController(IBlogRepository _blogRepository, ILogger<BlogsController> _logger) : Controller
    {
        [HttpPost("create-blog")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateBlog([FromForm]CreateBlogRequest request)
        {
            try
            {
                var userId = Guid.Parse(JWTUtil.GetUserId(User));

                var response = await _blogRepository.CreateBlog(userId,request);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Create Blog API]" +ex.Message, ex.StackTrace);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("get-blogs")]
        public async Task<IActionResult> GetBlogs([FromQuery]int pageNumber, [FromQuery]int pageSize, [FromQuery]string? searchKey)
        {
            var response = await _blogRepository.GetBlogs(pageNumber, pageSize, searchKey);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("hidden-blog")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> HiddenBlog(Guid blogId)
        {
            try
            {
                var response = await _blogRepository.HiddenBlog(blogId);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Hidden Blog API]" + ex.Message, ex.StackTrace);
                return StatusCode(500, ex.ToString());
            }
        }


        [HttpDelete("delete-blog")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteBlog(Guid blogId)
        {
            try
            {
                var response = await _blogRepository.DeleteBlog(blogId);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Delete Blog API]" + ex.Message, ex.StackTrace);
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpGet("get-blog")]
        public async Task<IActionResult> GetBlog([FromQuery]Guid blogId)
        {
            var response = await _blogRepository.GetBlog(blogId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPatch("edit-blog/{blogId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult>EditBlog(Guid blogId, [FromForm]CreateBlogRequest request)
        {
            try
            {
                var response = await _blogRepository.EditBlog(blogId, request);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Edit Blog API] " + ex.ToString());
                return StatusCode(500, ex.ToString());
            }
        }

    }
}
