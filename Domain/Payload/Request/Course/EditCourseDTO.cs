using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Enum;
using Microsoft.AspNetCore.Http;

namespace Domain.Payload.Request.Course
{
    public class EditCourseDTO
    {
        public string? Title { get; set; }

        public CourseType? Type { get; set; }

        public string? Duration { get; set; }

        public string? Description { get; set; }

        public IFormFile? Image { get; set; }

        public decimal? Price { get; set; }
    }
}
