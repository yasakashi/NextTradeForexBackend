using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class Crypto_TechnicalTabDto
    {
        public Guid? id { get; set; }
        public Guid? cryptoid { get; set; }
        public string? tabtitle { get; set; }
        public string? script { get; set; }
        public List<Crypto_TechnicalBreakingNewsDto>? newsmaincontentlist { get; set; }
    }

    public class Crypto_TechnicalBreakingNewsDto
    {
        public Guid? id { get; set; }
        public Guid? cryptoid { get; set; }
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
