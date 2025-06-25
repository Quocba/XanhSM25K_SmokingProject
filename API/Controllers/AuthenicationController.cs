using Domain.Payload.Request;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthenicationController(IAuthenicationRepository _authenicationRepository, ILogger<AuthenicationController> _logger) : Controller
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _authenicationRepository.Register(request);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Register API]" + ex.Message, ex.StackTrace);
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "Internal Server Error",
                    Error = ex.Message
                });
            }
        }

        [HttpPut("active-account")]
        public async Task<IActionResult>ActiveAccount([FromQuery] string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "Token không được để trống"
                    });
                }
                var response = await _authenicationRepository.Active(token);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("[ActiveAccount API]" + ex.Message, ex.StackTrace);
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "Internal Server Error",
                    Error = ex.Message
                });
            }
        }

    }
}
