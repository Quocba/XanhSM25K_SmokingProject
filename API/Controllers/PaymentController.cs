using Domain;
using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Share.Util;
using EmailService.DTO;
using EmailService.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayOSService.DTO;
using PayOSService.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/payment")]
    public class PaymentController(DBContext _context, ILogger<PaymentController> _logger, IPayOSService _payOs, IEmailSender _sender) : Controller
    {

        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromForm] Guid bookingId)
        {
            var getBooking = await _context.Bookings.FirstOrDefaultAsync(x => x.Id == bookingId);
            if (getBooking == null)
            {
                return NotFound();
            }

            var checkTransaction = await _context.Transaction
                                                 .Include(x => x.Bookings)
                                                 .FirstOrDefaultAsync(x => x.BookingId == bookingId);
            if (checkTransaction != null)
            {
                return Ok(new { Message = "Get payment url", Data = checkTransaction.Url });
            }

            var newPaymentUrl = new CreatePaymentDTO
            {
                OrderCode = CommonUtil.GenerateOrderCode(),
                Content = "Payment Booking",
                RequiredAmount = (int)getBooking.TotalPrice
            };

            var url = await _payOs.CreatePaymentAsync(newPaymentUrl);

            var createTransaction = new Transaction
            {

                Id = newPaymentUrl.OrderCode,
                BookingId = bookingId,
                CreateAt = DateTime.Now,
                Url = url.ToString()!
            };

            await _context.Transaction.AddAsync(createTransaction);
            await _context.SaveChangesAsync();
            return Ok(url);
        }

        [HttpPost("return-payment")]
        public async Task<IActionResult> PaymentReturn([FromQuery] string code, [FromQuery] long orderId, [FromQuery] string status, [FromQuery] bool cancel)
        {
            var getBooking = await _context.Transaction
                                               .Include(x => x.Bookings)
                                               .ThenInclude(x => x.Course)
                                               .ThenInclude(x => x.Center)
                                               .FirstOrDefaultAsync(x => x.Id == orderId);
            if (code == "00" && status == "PAID")
            {

                if (getBooking == null) { return NotFound(); }

                getBooking.Bookings.Status = PaymentStatusEnum.Paid;
                _context.Transaction.Update(getBooking);
                await _context.SaveChangesAsync();

                var rawHtml = CommonUtil.HTMLLoading("PaymentNotification.html");

                var finalHtml = rawHtml
                    .Replace("[[thongbao]]", "Thanh Toán Thành Công")
                    .Replace("[[CourseTitle]]", getBooking.Bookings.Course.Title)
                    .Replace("[[Duration]]", getBooking.Bookings.Course.Duration)
                    .Replace("[[Price]]", getBooking.Bookings.TotalPrice.ToString())
                    .Replace("[[Location]]", getBooking.Bookings.Course.Center.Location)
                    .Replace("[[PaymentStatus]]", PaymentStatusEnum.Paid.ToString());


                var getUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == getBooking.Bookings.UserId);

                var emailRequest = new EmailRequest<string>
                {
                    To = getUser.Email,
                    Subject = "[Thanh Toán Thành Công]",
                    Body = finalHtml
                };

                await _sender.SendEmailAsync(emailRequest);
                return Ok("Confirm Payment Success");
            }

            else
            {
                getBooking.Bookings.Status = PaymentStatusEnum.Cancelled;
                _context.Transaction.Update(getBooking);
                await _context.SaveChangesAsync();
                var rawHtml = CommonUtil.HTMLLoading("PaymentNotification.html");

                var finalHtml = rawHtml
                    .Replace("[[thongbao]]", "Hủy Bỏ Thanh Toán Thành Công")
                    .Replace("[[CourseTitle]]", getBooking.Bookings.Course.Title)
                    .Replace("[[Duration]]", getBooking.Bookings.Course.Duration)
                    .Replace("[[Price]]", getBooking.Bookings.TotalPrice.ToString())
                    .Replace("[[Location]]", getBooking.Bookings.Course.Center.Location)
                    .Replace("[[PaymentStatus]]", PaymentStatusEnum.Cancelled.ToString());


                var getUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == getBooking.Bookings.UserId);

                var emailRequest = new EmailRequest<string>
                {
                    To = getUser.Email,
                    Subject = "[Hủy Bỏ Thanh Toán Thành Công]",
                    Body = finalHtml
                };

                await _sender.SendEmailAsync(emailRequest);
                return Ok("Confirm Payment Success");
            }
        }
    }
}
