using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using static System.Collections.Specialized.BitVector32;

namespace Entities.Dtos
{
    public class ForexChartFundamentalAndTechnicalTabSectionDto
    {
        public string? instrumentname { get; set; }
        public string? fundamentalheading { get; set; }
        public string? technicalheading { get; set; }
        public List<ForexChartFundamentalNewsSectionDto>? fundamentalnewssectionlist { get; set; }
        public List<ForexChartTechnicalTabDto>? technicaltablist { get; set; }
        public string? marketsessiontitle { get; set; }
        public string? marketsessionscript { get; set; }
        public string? marketsentimentstitle { get; set; }
        public string? marketsentimentsscript { get; set; }
        public string? relatedresorces { get; set; }
        public List<ForexChartPDFSectionDto>? pdfsectionlist { get; set; }
        public List<ForexChartURLSectionDto>? urlsectionlist { get; set; }
        public string? privatenotes { get; set; }
    }
}
