using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class StockFundamitalNewsSectionDto
    {
        public Guid? id { get; set; }
        public Guid? stockid { get; set; }
        public string? maintitle { get; set; }
        public string? script { get; set; }
        public List<StockNewsMainContentDto>? newsmaincontentlist { get; set; }
    }
    public class StockNewsMainContentDto
    {
        public Guid? id { get; set; }
        public Guid? stockid { get; set; }
        public Guid? fundamentalandnewssectionid { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public string? descriptionfilename { get; set; }
        public string? descriptionfilepath { get; set; }
        public string? descriptionfileurl { get; set; }
        public string? descriptionfilecontenttype { get; set; }
        public string? link { get; set; }
    }
}
