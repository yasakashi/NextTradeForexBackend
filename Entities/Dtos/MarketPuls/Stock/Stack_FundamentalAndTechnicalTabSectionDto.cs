using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class StockFundamentalAndTechnicalTabSectionDto
    {
        public string? instrumentname { get; set; }
        public string? fundamentalheading { get; set; }
        public string? technicalheading { get; set; }
        public string? marketsesstiontite { get; set; }
        public string? marketsesstionscript { get; set; }
        public string? marketsentimentstitle { get; set; }
        public string? marketsentimentsscript { get; set; }
        public string? relatedresorces { get; set; }
        public string? privatenotes { get; set; }

        public List<StockFundamitalNewsSectionDto>? fndamentalnewssectionlist { get; set; }
        public List<StockTechnicalTabsDto>? technicaltablist { get; set; }
        public List<StockPDFSectionDto>? pdfsectionlist { get; set; }
        public List<StockURLSectionDto>? urlsectionlist { get; set; }
    }
}
