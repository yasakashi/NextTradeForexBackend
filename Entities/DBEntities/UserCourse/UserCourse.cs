using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblCourseBuilder_UsersCourses")]
    public class UserCourse
    {
        [Key]
        public Guid id { get; set; }
        public Guid courseid { get; set; }
        public long userid { get; set; }
        public bool isrequested { get; set; }
        public bool ispaid { get; set; }
        public bool ispassed { get; set; }
        public DateTime registerdatetime { get; set; }

        public virtual CourseBuilderCourse? course { get; set; }
        public virtual User user { get; set; }

    }
}
