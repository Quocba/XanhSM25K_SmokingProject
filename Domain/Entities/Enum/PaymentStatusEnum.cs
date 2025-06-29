using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enum
{
    public enum PaymentStatusEnum
    {
        Pending = 0,
        Completed = 1,
        Failed = 2,
        Refunded = 3,
        Cancelled = 4,
        Paid = 5
    }
}
