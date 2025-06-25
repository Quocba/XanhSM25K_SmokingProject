using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Domain.Payload.Request.Blogs
{
    public class CreateBlogRequest
    {
        public string Title {  get; set; }
        public IFormFile MainImage {  get; set; }
        public string Content { get; set; }

       public List<IFormFile>? BlogsImage {  get; set; }
    }
}
