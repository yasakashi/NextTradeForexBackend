using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblAdminPanel_Announcements")]
    public class Announcement
    {
        [Key]
        public Guid id { get; set; }
        public Guid courseId { get; set; }
        public string? title { get; set; }
        public string? summary { get; set; }
        public DateTime? registerdatetime { get; set; }

        public virtual CourseBuilderCourse course { get; set; }
    }

}
