using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblCourseBuilder_CourseComments")]
    public class CourseComment
    {
        [Key]
        public Guid Id { get; set; }
        public Guid courseid { get; set; }
        public long userid { get; set; }
        public DateTime? registerdatetime { get; set; }
        public string commenttext { get; set; }
        public Guid? parentcommentid { get; set; }

        public virtual User user { get; set; }
        public virtual CourseBuilderCourse course { get; set; }
    }
}
