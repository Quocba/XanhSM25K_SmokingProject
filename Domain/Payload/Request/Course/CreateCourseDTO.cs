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
    public class CreateCourseDTO
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title {  get; set; }

        [Required(ErrorMessage = "Type is required")]
        public CourseType Type { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        public string Duration {  get; set; }

        [Required(ErrorMessage = "Descrtiption is required")]
        public string Description {  get; set; }

        [Required(ErrorMessage = "Image is required")]
        public IFormFile Image {  get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1000, int.MaxValue, ErrorMessage = "Price can be not less than 1000")]
        public decimal Price {  get; set; }
    }
}
