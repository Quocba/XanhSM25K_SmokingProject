using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Payload.Base;
using Domain.Payload.Request.Blogs;
using Domain.Payload.Response;

namespace Repository.Interface
{
    public interface IBlogRepository
    {
        Task<ApiResponse<string>> CreateBlog(Guid userId,CreateBlogRequest request);
        Task<ApiResponse<PagingResponse<GetBlogsResponse>>>GetBlogs(int pageNumber,int pageSize, string? searchKey);
        Task<ApiResponse<string>> HiddenBlog(Guid blogId);
        Task<ApiResponse<GetBlogRespone>> GetBlog(Guid blogId);
        Task<ApiResponse<string>> DeleteBlog(Guid blogId);
        Task<ApiResponse<string>> EditBlog(Guid blogId, CreateBlogRequest request);
    }
}
