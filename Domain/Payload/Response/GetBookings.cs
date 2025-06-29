using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Enum;

namespace Domain.Payload.Response
{
    public class GetBookings
    {
        public Guid BookingId { get; set; }
        public string Course {  get; set; }
        public Decimal Price {  get; set; }
        public string Status { get; set; }
        public string PaymentType { get; set; }
        public DateTime Created { get; set; }


    }
}
