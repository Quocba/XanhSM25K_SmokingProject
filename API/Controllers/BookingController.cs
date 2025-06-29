using Domain.Payload.Request.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/booking")]
    public class BookingController(IBookingRepository _bookingRepository, ILogger<BookingController> _logger) : Controller
    {
        [HttpPost("booking")]
        public async Task<IActionResult> Booking([FromBody]CreateBookings request)
        {
            try
            {
                var userId = Guid.Parse(JWTUtil.GetUserId(User));
                request.UserId = userId;
                var response = await _bookingRepository.Booking(request);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Booking API" + ex.Message,ex.StackTrace);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("get-bookings-by-user")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetBookingsByUser([FromQuery]int pageNumber, [FromQuery]int pageSize, [FromQuery]string? status)
        {
            var userId = JWTUtil.GetUserId(User);
            var response = await _bookingRepository.GetBookings(Guid.Parse(userId), pageNumber, pageSize, status);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("get-booking")]
        public async Task<IActionResult> GetBooking([FromQuery]Guid bookingId)
        {
            var response = await _bookingRepository.GetBooking(bookingId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("cancel-booking/{bookingId}")]
        [Authorize(Roles = "User, Center")]
        public async Task<IActionResult> CancelBooking(Guid bookingId,[FromBody] CancelBooking request)
        {
            try
            {
                request.BookingId = bookingId;
                var response = await _bookingRepository.CancelBooking(request);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Cancel Booking API" + ex.Message, ex.StackTrace);
                return StatusCode(500,ex.Message);
            }
        }

        [HttpGet("get-all")]
        [Authorize(Roles = "Center")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string? status)
        {
            var response = await _bookingRepository.GetAllBooking(pageNumber, pageSize, status);
            return StatusCode(response.StatusCode, response);
        }
    }
}
