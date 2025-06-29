using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Enum;

namespace Domain.Payload.Response
{
    public class GetCourse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public CourseType Type { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public Guid CenterId { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? Hotline { get; set; }
        public string? Email { get; set; }
        public string? DirectorName { get; set; }
        public DateTime? EstablishedDate { get; set; }
        public int? Capacity { get; set; }
        public int? CurrentPatientCount { get; set; }
        public CenterType? CenterType { get; set; }
        public string? Notes { get; set; }
        public string? MainImage { get; set; }

    }
}
