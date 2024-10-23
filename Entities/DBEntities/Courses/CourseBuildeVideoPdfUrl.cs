using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblCourseBuilder_VideoPdfUrls")]
    public class CourseBuildeVideoPdfUrl
    {
        [Key]
        public Guid Id { get; set; }
        public Guid courseId { get; set; }
        public string? pdfTitle { get; set; }
        public string? pdfDescription { get; set; }
        public string? pdfFilename { get; set; }
        public string? pdfFilepath { get; set; }
        public string? pdfFilecontenttype { get; set; }
        public string? viewPdfFile { get; set; }
        public bool? downloadable { get; set; }

        public virtual CourseBuilderCourse course { get; set; }
    }
}
