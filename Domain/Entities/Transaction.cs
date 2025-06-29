using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Payload.Response;

namespace Domain.Entities
{
    public class Transaction
    {
        public long Id { get; set; }
        public DateTime CreateAt { get; set; }
        public string Url { get; set; }
        public Guid BookingId { get; set; }

        [ForeignKey(nameof(BookingId))]
        public virtual Bookings Bookings { get; set; }
    }
}
