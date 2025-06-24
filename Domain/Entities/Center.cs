using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Entities.Enum;

namespace Domain.Entities
{
    [Table("Centers")]
    public class Center : BaseEntity<Guid>
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Location { get; set; }

        [MaxLength(20)]
        public string HotLine { get; set; }

        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; }
        [MaxLength(255)]
        public string DirectorName { get; set; }
        public DateTime? EstablishedDate { get; set; }
        public int? Capacity { get; set; }
        public int? CurrentPatientCount { get; set; }
        [MaxLength(50)]
        public CenterType Type { get; set; } 
        public string Notes { get; set; }

        public string Image { get; set; }

        public Guid UserId { get;set; }
        [ForeignKey("UserId")]
        public virtual Users User { get; set; }
    }
 }
