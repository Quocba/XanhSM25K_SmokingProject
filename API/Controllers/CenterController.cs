using Domain.Payload.Request.Center;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/center")]
    public class CenterController(ICenterRepository _centerRepository, ILogger<CenterController> _logger) : Controller
    {
        [HttpPatch("edit-center/{centerId}")]
        [Authorize(Roles = "Center")]
        public async Task<IActionResult> EditCenter(Guid centerId,[FromForm]EditCenterInfomationRequest request)
        {
            try
            {
                var response = await _centerRepository.EditCenterInformation(centerId, request);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Edit Center Info] " + ex.Message, ex.StackTrace);
                return StatusCode(500, ex.ToString());

            }
        }

        [HttpGet("get-center")]
        [Authorize(Roles = "Center")]
        public async Task<IActionResult> GetCenter()
        {
            Guid userId = Guid.Parse(JWTUtil.GetUserId(User)!);
            var response = await _centerRepository.GetCenterByAdmin(userId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("get-center-info")]
        public async Task<IActionResult> GetCenterInfo([FromQuery]Guid centerId)
        {
            var response = await _centerRepository.GetCenter(centerId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
