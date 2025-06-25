using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Payload.Base;
using Microsoft.AspNetCore.Http;

namespace OhBau.Service.CloudinaryService
{
    public interface ICloudinaryService
    {
        Task<string> Upload(IFormFile file);
        Task<ApiResponse<string>> UploadVideo(IFormFile file);
    }
}
