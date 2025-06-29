using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Payload.Base;
using Domain.Payload.Request.Center;
using Domain.Payload.Response;

namespace Repository.Interface
{
    public interface ICenterRepository 
    {
        Task<ApiResponse<string>> EditCenterInformation(Guid centerId,EditCenterInfomationRequest request);
        Task<ApiResponse<GetCenterByCenterAdmin>> GetCenterByAdmin(Guid userId);
        Task<ApiResponse<GetCenterByCenterAdmin>> GetCenter(Guid centerId);

    }
}
