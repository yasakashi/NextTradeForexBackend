using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class CourseCommentDto
    {
        public Guid? Id { get; set; }
        public Guid? courseid { get; set; }
        public long? userid { get; set; }
        public DateTime? registerdatetime { get; set; }
        public string? commenttext { get; set; }
        public Guid? parentcommentid { get; set; }
        public string? coursename { get; set; }
        public string? username { get; set; }
    }

    public class CourseCommentSearchDto : BaseFilterDto
    {
        public Guid? Id { get; set; }
        public Guid? courseid { get; set; }
        public long? userid { get; set; }
        public DateTime? fromregisterdatetime { get; set; }
        public DateTime? toregisterdatetime { get; set; }
        public Guid? parentcommentid { get; set; }

    }
}
