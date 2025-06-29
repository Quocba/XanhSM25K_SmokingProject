using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using Domain;
using Domain.Entities;
using Domain.Payload.Base;
using Domain.Payload.Request.Center;
using Domain.Payload.Response;
using Domain.Share.Common;
using Microsoft.EntityFrameworkCore;
using OhBau.Service.CloudinaryService;
using Repository.Interface;

namespace Repository.Implement
{
    public class CenterRepository(DBContext _context, ICloudinaryService _cloudinary) : ICenterRepository
    {
        public async Task<ApiResponse<string>> EditCenterInformation(Guid centerId,EditCenterInfomationRequest request)
        {
            await _context.Database.BeginTransactionAsync();
            try
            {
                var getCenter = await _context.Centers.FirstOrDefaultAsync(x => x.Id == centerId);
                if (getCenter == null)
                {
                    return new ApiResponse<string>
                    {
                        StatusCode = StatusCodes.NotFound,
                        Message = "Center not found",
                        Data = null
                    };
                }

                getCenter.Name = request.Name ?? getCenter.Name;
                getCenter.Location = request.Location ?? getCenter.Location;
                getCenter.HotLine = request.Hotline ?? getCenter.HotLine;
                getCenter.Email = request.Email ?? getCenter.Email;
                getCenter.DirectorName = request.DirectorName ?? getCenter.DirectorName;
                getCenter.EstablishedDate = request.EstablishedDate != default ? getCenter.EstablishedDate : getCenter.EstablishedDate;
                getCenter.Capacity = request.Capacity ?? getCenter.Capacity;
                getCenter.CurrentPatientCount = request.Capacity ?? getCenter.CurrentPatientCount;
                getCenter.Type = request.Type ?? getCenter.Type;
                getCenter.Notes = request.Notes ?? getCenter.Notes;
                if (request.MainImage != null)
                {
                    getCenter.Image = await _cloudinary.Upload(request.MainImage);
                }
                else
                {
                    getCenter.Image = getCenter.Image;
                }

                getCenter.CreatedAt = getCenter.CreatedAt;
                getCenter.UpdatedAt = DateTime.Now;
                getCenter.IsDeleted = false;
                getCenter.IsActive = true;

                _context.Centers.Update(getCenter);
                if (request.CeneterImages != null)
                {
                    foreach(var image in request.CeneterImages)
                    {
                        var centerImage = new CenterImages
                        {
                            Id = Guid.NewGuid(),
                            CenterId = getCenter.Id,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            IsActive = true,
                            IsDeleted = false,
                            Url = await _cloudinary.Upload(image)
                        };

                        await _context.CenterImages.AddAsync(centerImage);
                    }
                }

                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return new ApiResponse<string>
                {
                    StatusCode = StatusCodes.OK,
                    Message = "Edit center success",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception(ex.ToString());
            }
        }

        public async Task<ApiResponse<GetCenterByCenterAdmin>> GetCenter(Guid centerId)
        {
            var getCenter = await _context.Centers
             .Include(x => x.Images)
             .FirstOrDefaultAsync(x => x.Id == centerId);

            if (getCenter == null)
            {
                return new ApiResponse<GetCenterByCenterAdmin>
                {
                    StatusCode = StatusCodes.NotFound,
                    Message = "Center not found",
                    Data = null
                };
            }

            var mapItem = new GetCenterByCenterAdmin
            {
                CenterId = getCenter.Id,
                Name = getCenter.Name,
                Location = getCenter.Location,
                Hotline = getCenter.HotLine,
                Email = getCenter.Email,
                DirectorName = getCenter.DirectorName,
                EstablishedDate = getCenter.EstablishedDate,
                Capacity = getCenter.Capacity,
                CurrentPatientCount = getCenter.CurrentPatientCount,
                Type = getCenter.Type,
                Notes = getCenter.Notes,
                MainImage = getCenter.Image,
                CeneterImages = getCenter.Images?.Select(i => i.Url).ToList()
            };

            return new ApiResponse<GetCenterByCenterAdmin>
            {
                StatusCode = StatusCodes.OK,
                Message = "Success",
                Data = mapItem
            };
        }

        public async Task<ApiResponse<GetCenterByCenterAdmin>> GetCenterByAdmin(Guid userId)
        {
            var getCenter = await _context.Centers
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (getCenter == null)
            {
                return new ApiResponse<GetCenterByCenterAdmin>
                {
                    StatusCode = StatusCodes.NotFound,
                    Message = "Center not found",
                    Data = null
                };
            }

            var mapItem = new GetCenterByCenterAdmin
            {
                CenterId = getCenter.Id,
                Name = getCenter.Name,
                Location = getCenter.Location,
                Hotline = getCenter.HotLine,
                Email = getCenter.Email,
                DirectorName = getCenter.DirectorName,
                EstablishedDate = getCenter.EstablishedDate,
                Capacity = getCenter.Capacity,
                CurrentPatientCount = getCenter.CurrentPatientCount,
                Type = getCenter.Type,
                Notes = getCenter.Notes,
                MainImage = getCenter.Image, 
                CeneterImages = getCenter.Images?.Select(i => i.Url).ToList() 
            };

            return new ApiResponse<GetCenterByCenterAdmin>
            {
                StatusCode = StatusCodes.OK,
                Message = "Success",
                Data = mapItem
            };
        }

    }
}
