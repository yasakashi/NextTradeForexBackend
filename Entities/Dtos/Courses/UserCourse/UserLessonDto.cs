using Entities.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class UserLessonDto :BaseFilterDto
    {
        public Guid? id { get; set; }
        public Guid? courseid { get; set; }
        public string? coursename { get; set; }
        public Guid? lessonid { get; set; }
        public string? lessonname { get; set; }
        public Guid? topicid { get; set; }
        public string? topicname { get; set; }
        public long? userid { get; set; }
        public int? lesseonorderpassed { get; set; }
        public DateTime? registerdatetime { get; set; }

    }
}
