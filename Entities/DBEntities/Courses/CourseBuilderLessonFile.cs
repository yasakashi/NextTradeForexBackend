using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblCourseBuilder_LessonFiles")]
    public class CourseBuilderLessonFile
    {
        [Key]
        public Guid Id { get; set; }
        public Guid lessonId { get; set; }
        public string? lessonFilename { get; set; }
        public string? lessonFilecontenttype { get; set; }
        public string? lessonFilepath { get; set; }

        public virtual CourseBuilderLesson lesson { get; set; }
    }
}
