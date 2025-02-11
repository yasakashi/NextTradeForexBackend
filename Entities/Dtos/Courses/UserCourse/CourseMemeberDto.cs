using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class CourseMemeberDto
    {
        public Guid? Id { get; set; }
        public long? userId { get; set; }
        public Guid? courseId { get; set; }
        public string? coursename { get; set; }
        public string? fname { get; set; }
        public string? lname { get; set; }
        public string? emial { get; set; }
        public string? profilePic { get; set; }
        public int? courseTakenCount { get; set; }
        public DateTime? registerdatetime { get; set; }
        public string? username { get;set; }
    }
}
