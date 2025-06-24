using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enum
{
    public enum CenterType
    {
        Public = 1,        // Government-run center
        Private = 2,       // Privately operated center
        Partnership = 3,   // Public-private collaboration
        Other = 4          // Other/unspecified type
    }
}
