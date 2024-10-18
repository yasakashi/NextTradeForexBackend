using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class CourseBuilderLessonDto : BaseFilterDto
    {
        public Guid? Id { get; set; }
        public Guid? courseId { get; set; }
        public Guid? topicId { get; set; }
        public string? lessonName { get; set; }
        public string? lessonDescription { get; set; }
        public string? lessonFilename { get; set; }
        public string? lessonFilecontenttype { get; set; }
        public IFormFile? lessonFile { get; set; }
        public string? lessonFilepath { get; set; }
        public IFormFile? featureImage { get; set; }
        public string? featureImagename { get; set; }
        public string? featureImagepath { get; set; }
        public string? featureImagecontenttype { get; set; }
        public string? videoSource { get; set; }
        public int videoPlaybackTime { get; set; }
        public List<IFormFile> attachments { get; set; }
        public List<CourseBuilderLessonFileDto> fileattachments { get; set; }
    }

    public class CourseBuilderLessonFileDto
    {
        public Guid? Id { get; set; }
        public Guid? lessonId { get; set; }
        public string? lessonFilename { get; set; }
        public string? lessonFilecontenttype { get; set; }
        public string? lessonFilepath { get; set; }
    }
}
