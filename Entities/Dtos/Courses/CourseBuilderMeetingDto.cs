using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class CourseBuilderMeetingDto
    {
        public Guid? Id { get; set; }
        public Guid? courseId { get; set; }
        public string? meetingTitle { get; set; }
        public string? meetingDescription { get; set; }
        public IFormFile? meetingFile { get; set; }
        public string? meetingFilename { get; set; }
        public string? meetingfilecontetnttype { get; set; }
        public string? meetingFilepath { get; set; }
        public string? meetingURL { get; set; }
        public DateTime? meetingDateTime { get; set; }
    }


    public class CourseBuilderMeetingFilterDto :BaseFilterDto
    {
        public Guid? Id { get; set; }
        public Guid? courseId { get; set; }
        public string? meetingTitle { get; set; }
        public string? meetingDescription { get; set; }
        public string? meetingFilename { get; set; }
        public DateTime? frommeetingdatetime { get; set; }
        public DateTime? tomeetingdatetime { get; set; }
    }
}
