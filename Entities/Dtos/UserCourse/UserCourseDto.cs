using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class UserCourseDto
    {
        public Guid? id { get; set; }
        public Guid? courseid { get; set; }
        public long? userid { get; set; }
        public bool? isrequested { get; set; }
        public bool? ispaid { get; set; }
        public bool? ispassed { get; set; }
        public DateTime? registerdatetime { get; set; }
        public string? courseName { get; set; }
        public string? username { get; set; }
        public string? userfullname { get; set; }
    }

    public class UserCourseFilterDto : BaseFilterDto
    {
        public Guid? id { get; set; }
        public Guid? courseid { get; set; }
        public long? userid { get; set; }

    }
}
