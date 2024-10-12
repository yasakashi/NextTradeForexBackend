using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class CourseBuildeVideoPdfUrlDto
    {
        public Guid? Id { get; set; }
        public Guid? courseId { get; set; }
        public string? pdfTitle { get; set; }
        public string? pdfDescription { get; set; }
        public IFormFile? pdfFile{ get; set; }
        public string? pdfFilename { get; set; }
        public string? pdfFilepath { get; set; }
        public string? viewPdfFile { get; set; }
        public bool? downloadable { get; set; }
    }
}
