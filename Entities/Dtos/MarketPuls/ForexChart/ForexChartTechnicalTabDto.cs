using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class ForexChartTechnicalTabDto
    {
        public Guid? id { get; set; }
        public Guid? forexchartid { get; set; }
        public string? tabtitle { get; set; }
        public string? script { get; set; }
        public List<ForexChartTechnicalBreakingNewsDto>? newsmaincontentlist { get; set; }
    }

    public class ForexChartTechnicalBreakingNewsDto
    {
        public Guid? id { get; set; }
        public Guid? forexchartid { get; set; }
        public Guid? technicaltabid { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public string? descriptionfilename { get; set; }
        public string? descriptionfilepath { get; set; }
        public string? descriptionfileurl { get; set; }
        public string? descriptionfilecontenttype { get; set; }
        public string? link { get; set; }
    }
}
