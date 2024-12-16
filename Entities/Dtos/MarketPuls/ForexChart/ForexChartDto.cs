using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class ForexChartDto
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
        public string? sendtrackbacks { get; set; }
        public bool? discussion_allowcomments { get; set; }
        public bool? discussion_allowtrackbacksandpingbacks { get; set; }

        public List<ForexChartFlexibleBlockDto> forexflexibleblocks { get; set; }
        public ForexChartFundamentalAndTechnicalTabSectionDto fundamentalandtechnicaltabsection { get; set; }
    }

    public class ForexChartFilterDto:BaseFilterDto
    {
        public Guid? id { get; set; }
        public long? categoryid { get; set; }
        public string? title { get; set; }
        public string? tags { get; set; }
    }
}
