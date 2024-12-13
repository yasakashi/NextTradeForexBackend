using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class StockTechnicalTabsDto
    {
        public Guid? id { get; set; }
        public Guid? stockid { get; set; }
        public string? tabtitle { get; set; }
        public string? script { get; set; }
        public List<StockTechnicalBreakingNewsDto>? newsmaincontentlist { get; set; }
    }

    public class StockTechnicalBreakingNewsDto
    {
        public Guid? id { get; set; }
        public Guid? stockid { get; set; }
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
