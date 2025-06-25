using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Payload.Response
{
    public class GetBlogsResponse
    {
        public Guid Id {  get; set; }
        public string Title { get; set; }
        public string MainImage { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
