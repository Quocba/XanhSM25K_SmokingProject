using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Payload.Base;
using Domain.Payload.Request;

namespace Repository.Interface
{
    public interface IAuthenicationRepository
    {
        Task<ApiResponse<string>> Register(RegisterDTO request);
        Task<ApiResponse<string>> Active(string token);
    }
}
