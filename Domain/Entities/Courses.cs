using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Base;

namespace Domain.Entities
{
    [Table("Courses")]
    public class Courses : BaseEntity<Guid>
    {
         public string Title { get; set; }
         public string Duration { get; set; }
         public string Type { get; set; } 
         public string Description { get; set; }
         public string Image { get; set; }
         public decimal Price { get; set; }
         public Guid CenterId { get; set; }
       
        [ForeignKey("CenterId")]
        public virtual Center Center { get; set; }
    }
}
