using Entities.DBEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Entities.Dtos
{
    public class CourseBuilderCourseDto : BaseFilterDto
    {
        public Guid? Id { get; set; }
        public string? courseName { get; set; }
        public string? courseDescription { get; set; }
        public string? courseFilepath { get; set; }
        public string? courseFilename { get; set; }
        public string? courseFilecontent { get; set; }
        public IFormFile? courseFile { get; set; }
        public string? excerpt { get; set; }
        public long? authorId { get; set; }
        public string? authorusername { get; set; }
        public string? authorname { get; set; }
        public int? maximumStudents { get; set; }
        public int? difficultyLevelId { get; set; }
        public bool? isPublicCourse { get; set; }
        public bool? allowQA { get; set; }
        public bool? isvisible { get; set; }
        public bool? isvisibledropdown { get; set; }
        public decimal? coursePrice { get; set; }
        public string? whatWillILearn { get; set; }
        public string? targetedAudience { get; set; }
        public int? courseDuration { get; set; }
        public string? materialsIncluded { get; set; }
        public string? requirementsInstructions { get; set; }
        public string? courseIntroVideo { get; set; }
        public List<long>? courseCategoryIds { get; set; }
        public string? courseTags { get; set; }
        public IFormFile? featuredImage { get; set; }
        public string? featuredImagename { get; set; }
        public string? featuredImagepath { get; set; }
        public string? featuredImagecontent { get; set; }
        public int? lessoncount { get; set; }
        public int? topiccount { get; set; }
        public int? quizcount { get; set; }
        public int? meetingcount { get; set; }
        public int? videoPdfcount { get; set; }
        public DateTime? registerdatetime { get; set; }
        public int? coursestatusid { get; set; }
        public string? coursestatusname { get; set; }
        public DateTime? changestatusdate { get; set; }
        public List<CourseBuilderMeetingDto>? meetings { get; set; }
        public List<CourseBuildeVideoPdfUrlDto>? videoPdfUrls { get; set; }
        public List<CourseCategoryDto>? courseCategorys { get; set; }
    }
}
