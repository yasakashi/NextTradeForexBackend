using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class AnnouncementDto
    {
        public Guid? id { get; set; }
        public Guid? courseId { get; set; }
        public string? coursename { get; set; }
        public string? title { get; set; }
        public string? summary { get; set; }
        public DateTime? registerdatetime { get; set; }
    }
    public class AnnouncementSearchFilterDto : BaseFilterDto
    {
        public Guid? id { get; set; }
        public Guid? courseId { get; set; }
        public string? title { get; set; }
        public DateTime? fromregisterdatetime { get; set; }
        public DateTime? toregisterdatetime { get; set; }
    }
}
