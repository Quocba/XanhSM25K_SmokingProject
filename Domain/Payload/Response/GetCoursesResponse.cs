using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Payload.Response
{
    public class GetCoursesResponse
    {
        public Guid Id { get; set; }
        public string Image {  get; set; }
        public string Title { get; set; }
        public decimal Price { get;set; }
        public string Type {  get; set; }
    }
}
