using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Payload.Response
{
    public class GetBlogRespone
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string MainImage { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public List<BlogImages> BlogImage { get; set; } = new List<BlogImages>();
        public GetUserResponse User { get; set; }

    }

    public class BlogImages
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }

    }
}
