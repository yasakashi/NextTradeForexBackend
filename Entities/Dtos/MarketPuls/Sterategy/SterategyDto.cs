using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DBEntities;

namespace Entities.Dtos
{
    public class SterategyDto
    {
        public Guid? id { get; set; }
        public long? categoryid { get; set; }
        public string? title { get; set; }
        public string? tags { get; set; }
        public string? excerpt { get; set; }
        public string? authorname { get; set; }
        public long? authorid { get; set; }
        public bool? isvisible { get; set; }
        public int? courseleveltypeId { get; set; }
        public int? coursestatusid { get; set; }
        public long? creatoruserid { get; set; }
        public DateTime? createdatetime { get; set; }
        public DateTime? changestatusdate { get; set; }

        public SterategyIndicesCourseDto IndicesCourses { get; set; }
        public SterategyFundamentalAndTechnicalTabSectionDto fundamentalandtechnicaltabsection { get; set; }

    }

    public class SterategyFilterDto : BaseFilterDto
    {
        public Guid? id { get; set; }
        public long? categoryid { get; set; }
        public string? title { get; set; }
        public string? tags { get; set; }
    }
}
