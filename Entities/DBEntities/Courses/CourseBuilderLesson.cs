using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblCourseBuilder_Lessons")]
    public class CourseBuilderLesson
    {
        [Key]
        public Guid Id { get; set; }
        public Guid courseId { get; set; }
        public Guid topicId { get; set; }
        public string? lessonName { get; set; }
        public string? lessonDescription { get; set; }
        public string? lessonFilename { get; set; }
        public string? lessonFilepath { get; set; }
        public string? featureImagename { get; set; }
        public string? featureImagepath { get; set; }
        public string? videoSource { get; set; }
        public int videoPlaybackTime { get; set; }
        public string attachments { get; set; }
    }
}
