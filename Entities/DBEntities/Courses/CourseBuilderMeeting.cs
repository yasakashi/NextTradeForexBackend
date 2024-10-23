using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{

    [Table("tblCourseBuilder_Meetings")]
    public class CourseBuilderMeeting
    {
        [Key]
        public Guid Id { get; set; }
        public Guid courseId { get; set; }
        public string? meetingTitle { get; set; }
        public string? meetingDescription { get; set; }
        public string? meetingFilename { get; set; }
        public string? meetingfilecontetnttype { get; set; }
        public string? meetingFilepath { get; set; }
        public string? meetingURL { get; set; }
        public DateTime? meetingDateTime { get; set; }

        public virtual CourseBuilderCourse course { get; set; }
    }

}
