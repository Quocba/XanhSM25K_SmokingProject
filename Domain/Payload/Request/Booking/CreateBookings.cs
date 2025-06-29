using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Entities.Enum;

namespace Domain.Payload.Request.Booking
{
    public class CreateBookings
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public PaymentType PaymentType { get; set; }
    }
}
