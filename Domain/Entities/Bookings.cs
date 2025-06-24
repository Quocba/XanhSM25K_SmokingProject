using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Entities.Enum;

namespace Domain.Entities
{
    [Table("Bookings")]
    public class Bookings : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Users User { get; set; }
        public Guid CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Courses Course { get; set; }
        public decimal TotalPrice { get; set; }
        public PaymentStatusEnum Status { get; set; }
        public string? ReasonCancel { get; set; }
        public PaymentType Type { get; set; }

    }
}
