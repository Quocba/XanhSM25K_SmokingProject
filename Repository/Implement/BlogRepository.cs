using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using Domain.Payload.Base;
using Domain.Payload.Request.Blogs;
using Domain.Payload.Response;
using Domain.Share.Common;
using Domain.Share.Util;
using Microsoft.EntityFrameworkCore;
using OhBau.Service.CloudinaryService;
using Repository.Interface;
using static System.Net.Mime.MediaTypeNames;

namespace Repository.Implement
{
    public class BlogRepository(DBContext _context, ICloudinaryService _cloudinary) : IBlogRepository
    {
        public async Task<ApiResponse<string>> CreateBlog(Guid userId,CreateBlogRequest request)
        {
                using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                var getAuthor = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
                var createBlog = new Blog
                {
                    Id = Guid.NewGuid(),
                    Title = request.Title,
                    Content = HTMLUtil.Sanitize(request.Content),
                    MainImage =  await _cloudinary.Upload(request.MainImage),
                    Author =  getAuthor.FullName,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    UpdatedAt = DateTime.Now,
                    UserId = userId,
                };

                await _context.Blogs.AddAsync(createBlog);
             
                if (request.BlogsImage == null)
                {
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ApiResponse<string>
                    {
                        StatusCode = StatusCodes.Created,
                        Message = "Create Blog Success",
                        Data = null
                    };
                }

                foreach (var image in request.BlogsImage)
                {
                    var addImage = new BlogImage
                    {
                        Id = Guid.NewGuid(),
                        ImageUrl = await _cloudinary.Upload(image),
                        BlogId = createBlog.Id,
                        CreatedAt = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        UpdatedAt = DateTime.Now
                    };

                    await _context.BlogImages.AddAsync(addImage);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Created,
                    Message = "Create Blog Success",
                    Data = null
                };
          
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.ToString());
            }
        }
        public async Task<ApiResponse<PagingResponse<GetBlogsResponse>>> GetBlogs(int pageNumber, int pageSize, string? searchKey)
        {
            var query =  _context.Blogs.Where(x => x.IsActive == true && x.IsDeleted == false);
            if (!string.IsNullOrEmpty(searchKey))
            {
                query = query.Where(b =>
                                   b.Title.Contains(searchKey) ||
                                   b.Content.Contains(searchKey));

            }

            var getBlogs = query.Select(c => new GetBlogsResponse
            {
                Id = c.Id,
                Title = c.Title,
                Author = c.Author,
                CreatedAt = c.CreatedAt,
                MainImage = c.MainImage
            });

            var pagedResult = await getBlogs.ToPagedListAsync(pageNumber, pageSize);

            return new ApiResponse<PagingResponse<GetBlogsResponse>>
            {
                StatusCode = StatusCodes.OK,
                Message = "Get blogs success",
                Data = pagedResult
            };

            throw new NotImplementedException();
        }
        public async Task<ApiResponse<string>> HiddenBlog(Guid blogId)
        {
            try
            {
                var getBlog = await _context.Blogs.FirstOrDefaultAsync(x => x.Id ==  blogId);
                if (getBlog == null)
                {
                    return new ApiResponse<string>
                    {
                        StatusCode = StatusCodes.NotFound,
                        Message = "Blog not found",
                        Data = null
                    };
                }

                getBlog.IsActive = false;
                _context.Blogs.Update(getBlog);
                await _context.SaveChangesAsync();
                return new ApiResponse<string>
                {
                    StatusCode = StatusCodes.OK,
                    Message  = "Hidden blog success",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<ApiResponse<string>> DeleteBlog(Guid blogId)
        {
            try
            {
                var getBlog = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == blogId);
                if (getBlog == null)
                {
                    return new ApiResponse<string>
                    {
                        StatusCode = StatusCodes.NotFound,
                        Message = "Blog not found",
                        Data = null
                    };
                }

                getBlog.IsDeleted = true;
                _context.Blogs.Update(getBlog);
                await _context.SaveChangesAsync();
                return new ApiResponse<string>
                {
                    StatusCode = StatusCodes.OK,
                    Message = "Delete blog success",
                    Data = null
                };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<ApiResponse<GetBlogRespone>> GetBlog(Guid blogId)
        {
            var getBlog = await _context.Blogs
                                        .Include(x => x.User)
                                        .Include(x => x.BlogImages)
                                        .FirstOrDefaultAsync(x => x.Id == blogId);
            if (getBlog == null)
            {
                return new ApiResponse<GetBlogRespone>
                {
                    StatusCode = StatusCodes.NotFound,
                    Message  = "Blog not found",
                    Data = null
                };
            }

            var mapItem = new GetBlogRespone
            {
                Id = getBlog.Id,
                Title = getBlog.Title,
                Author = getBlog.Author,
                Content = getBlog.Content,
                MainImage = getBlog.MainImage,
                BlogImage = getBlog.BlogImages
            .Where(img => !img.IsDeleted && img.IsActive)
            .Select(c => new BlogImages
            {
                Id = c.Id,
                ImageUrl = c.ImageUrl
            }).ToList(),

             User = new GetUserResponse
             {
                 UserId = getBlog.User.Id,
                 Phone = getBlog.User.Phone,
                 Address = getBlog.User.Address,
                 Email = getBlog.User.Email,
                 FullName = getBlog.User.FullName
             }

            };

            return new ApiResponse<GetBlogRespone>
            {
                StatusCode = StatusCodes.OK,
                Message = "Get blog success",
                Data = mapItem
            };

        }

        public async Task<ApiResponse<string>> EditBlog(Guid blogId, CreateBlogRequest request)
        {
            try
            {
                var getBlog = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == blogId);
                if (getBlog == null)
                {
                    return new ApiResponse<string>
                    {
                        StatusCode = StatusCodes.NotFound,
                        Message = "Blog not found",
                        Data = null
                    };
                }

                getBlog.Title = request.Title ?? getBlog.Title;
                getBlog.Content = request.Content ?? getBlog.Content;
                getBlog.UpdatedAt = DateTime.Now;
                if (request.MainImage == null)
                {
                    getBlog.MainImage = getBlog.MainImage;
                }

                else
                {
                    getBlog.MainImage = await _cloudinary.Upload(request.MainImage);
                }

                _context.Blogs.Update(getBlog);
                await _context.SaveChangesAsync();

                return new ApiResponse<string>
                {
                    StatusCode = StatusCodes.OK,
                    Message = "Edit blog success",
                    Data = null
                };
            }
            catch (Exception ex)
            {
               throw new Exception(ex.ToString());
            }
        }
    }
}
