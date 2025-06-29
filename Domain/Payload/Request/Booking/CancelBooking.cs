using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Payload.Request.Booking
{
    public class CancelBooking
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public Guid BookingId { get; set; }
        public string ReasonCancel {  get; set; }
    }
}
