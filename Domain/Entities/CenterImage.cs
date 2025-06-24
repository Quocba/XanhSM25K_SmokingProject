using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Base;

namespace Domain.Entities
{
    [Table("CenterImages")]
    public class CenterImages : BaseEntity<Guid>
    {
        public string Url { get; set; }
        public Guid CenterId { get; set; }

        [ForeignKey("CenterId")]
        public virtual Center Center { get; set; }  
    }
}
