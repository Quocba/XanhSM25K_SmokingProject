using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Payload.Base;
using Domain.Payload.Request;
using Domain.Payload.Response;

namespace Repository.Interface
{
    public interface IAuthenicationRepository
    {
        Task<ApiResponse<string>> Register(RegisterDTO request);
        Task<ApiResponse<string>> Active(string token);
        Task<ApiResponse<LoginResponse>> Login(LoginRequest request);
    }
}
