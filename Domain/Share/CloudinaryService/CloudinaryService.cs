using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Payload.Base;
using Microsoft.AspNetCore.Http;

namespace OhBau.Service.CloudinaryService
{
    public class CloudinaryService(Cloudinary _cloudinary) : ICloudinaryService
    {
        public async Task<string> Upload(IFormFile file)
        {
            if (file == null)
            {
                return null;
            }

            using(var stream = file.OpenReadStream())
            {
                var uploadParam = new ImageUploadParams
                {
                    File = new FileDescription(file.Name, stream),
                    Folder = "EposhBooking",
                    PublicId = Guid.NewGuid().ToString(),
                    Transformation = new Transformation().Quality("auto:low")
                                                         .FetchFormat("webp")
                                                         .Width(1024)
                                                         .Crop("limit")
                };


                var uploadResult = await _cloudinary.UploadAsync(uploadParam);
                if (uploadResult.StatusCode == HttpStatusCode.OK)
                {
                   return uploadResult.SecureUrl.ToString();
                }
                else
                {
                    throw new Exception("Failed to upload image to Cloudinary.");
                }
            }
        }

        public async Task<ApiResponse<string>> UploadVideo(IFormFile file)
        {
            try
            {
                var uploadParams = new VideoUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    Folder = "EposhBooking",
                    PublicId = Guid.NewGuid().ToString(),
                    EagerTransforms = new List<Transformation>
            {
                new EagerTransformation().Width(300).Height(300).Crop("pad").AudioCodec("none"),
                new EagerTransformation().Width(160).Height(100).Crop("crop").Gravity("south").AudioCodec("none")
            },
                    EagerAsync = true,
                    EagerNotificationUrl = "https://your-callback-url.com/notify" 
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return new ApiResponse<string>
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Video uploaded successfully",
                        Data = uploadResult.SecureUrl.ToString()
                    };
                }
                else
                {
                    return new ApiResponse<string>
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Video upload failed: " + uploadResult.Error?.Message,
                        Data = null,
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
