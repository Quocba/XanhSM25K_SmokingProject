using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Payload.Base;
using Domain.Payload.Request.Booking;
using Domain.Payload.Response;

namespace Repository.Interface
{
    public interface IBookingRepository
    {
        Task<ApiResponse<string>> Booking(CreateBookings request);
        Task<ApiResponse<PagingResponse<GetBookings>>> GetBookings(Guid userId, int pageNumber, int pageSize, string? status);
        Task<ApiResponse<PagingResponse<GetBookings>>> GetAllBooking(int pageNumber, int pageSize, string? status);
        Task<ApiResponse<GetBooking>> GetBooking(Guid bookingId);
        Task<ApiResponse<CancelBooking>> CancelBooking(CancelBooking request);


    }
}
