using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Base;

namespace Domain.Entities
{
    [Table("Blogs")]
    public class Blog : BaseEntity<Guid>
    {
        public string Title { get; set; }
        public string MainImage { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Users User { get; set; }

        public virtual ICollection<BlogImage> BlogImages { get; set; } = new List<BlogImage>();

    }
}
