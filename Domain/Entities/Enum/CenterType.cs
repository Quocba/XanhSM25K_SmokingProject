using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enum
{
    public enum CenterType
    {
        Public,     // Công lập
        Private,    // Tư nhân
        NGO,        // Phi chính phủ
        Military,   // Quân đội (nếu có)
        Other       // Khác
    }
}
