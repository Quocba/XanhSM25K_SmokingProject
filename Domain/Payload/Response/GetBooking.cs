using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Enum;

namespace Domain.Payload.Response
{
    public class GetBooking
    {
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public string? ReasonCancel { get; set; }
        public string Type { get; set; }
        public DateTime Created { get; set; }
        public string Course { get; set; }
        public string Duration {  get; set; }
        public string Description {  get; set; }
        public string CourseType { get; set; }
        public decimal Price {  get; set; }
        public string Image {  get; set; }
        public string FullName {  get; set; }
        public string Email {  get; set; }
        public string Phone { get; set; }
        public string Address {  get; set; }
    }
}
