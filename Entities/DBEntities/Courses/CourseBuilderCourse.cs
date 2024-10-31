using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{

    [Table("tblCourseBuilder_Courses")]
    public class CourseBuilderCourse
    {
        [Key]
        public Guid Id { get; set; }
        public string? courseName { get; set; }
        public string? courseDescription { get; set; }
        public string? courseFilename { get; set; }
        public string? courseFilepath { get; set; }
        public string? courseFilecontent { get; set; }
        public string? excerpt { get; set; }
        public long? authorId { get; set; }
        public int? maximumStudents { get; set; }
        public int? courseleveltypeId { get; set; }
        public bool? isPublicCourse { get; set; }
        public bool? allowQA { get; set; }
        public decimal? coursePrice { get; set; }
        public string? whatWillILearn { get; set; }
        public string? targetedAudience { get; set; }
        public int? courseDuration { get; set; }
        public string? materialsIncluded { get; set; }
        public string? requirementsInstructions { get; set; }
        public string? courseIntroVideo { get; set; }
        public string? courseTags { get; set; }
        public string? featuredImagename { get; set; }
        public string? featuredImagepath { get; set; }
        public string? featuredImagecontent { get; set; }
        public DateTime registerdatetime { get; set; }

        public int? coursestatusid { get; set; }

        public DateTime? changestatusdate { get; set; }

        public virtual User author { get; set; }
        public virtual CourseStatus coursestatus { get; set; }
        public virtual List<CourseBuilderMeeting>? meetings { get; set; }
        public virtual List<CourseBuildeVideoPdfUrl>? videoPdfUrls { get; set; }
        public virtual List<CourseCategory>? courseCategorys { get; set; }
    }


}
