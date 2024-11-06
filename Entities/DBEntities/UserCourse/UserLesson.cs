using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblCourseBuilder_UsersLessons")]
    public class UserLesson
    {
        [Key]
        public Guid id { get; set; }
        public Guid courseid { get; set; }
        public Guid lessonid { get; set; }
        public Guid topicid { get; set; }
        public long userid { get; set; }
        public int lesseonorderpassed { get; set; }
        public DateTime registerdatetime { get; set; }

        public virtual CourseBuilderCourse? course { get; set; }
        public virtual CourseBuilderLesson? lesson { get; set; }
        public virtual CourseBuilderTopic? topic { get; set; }
        public virtual User user { get; set; }
    }
}
