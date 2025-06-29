using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Enum;
using Microsoft.AspNetCore.Http;

namespace Domain.Payload.Request.Center
{
    public class EditCenterInfomationRequest
    {
        public string? Name { get; set; }
        public string? Location {  get; set; }
        public string? Hotline {  get; set; }
        public string? Email {  get; set; }
        public string? DirectorName {  get; set; }
        public DateTime? EstablishedDate {  get; set; }
        public int? Capacity {  get; set; }
        public int? CurrentPatientCount {  get; set; }
        public CenterType? Type { get; set; }
        public string? Notes {  get; set; }
        public IFormFile MainImage { get; set; }
        public List<IFormFile> CeneterImages { get; set; }
    }
}
