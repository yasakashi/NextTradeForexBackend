using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class UserCourseMemberDto
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
        public string? fname { get; set; }
        public string? lname { get; set; }
        public string? emial { get; set; }
        public string? profilePic { get; set; }
        public int? courseTakenCount { get; set; }
        public int? totalCourses { get; set; }
        public int? coursestatusid { get; set; }

        public string? coursestatusname { get; set; }

    }
    public class UserCourseFilterDto : BaseFilterDto
    {
        public Guid? id { get; set; }
        public Guid? courseid { get; set; }
        public long? userid { get; set; }
        public DateTime? fromregisterdatetime { get; set; }
        public DateTime? toregisterdatetime { get; set; }
    }
}
