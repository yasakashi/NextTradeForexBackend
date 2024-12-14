using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class ForexChartFlexibleBlockDto
    {
        public Guid? id { get; set; }
        public string? maintitle { get; set; }
        public List<ForexChartCountriesDataDto>? countriesdatalist { get; set; }
        public string? forexoneyeardescription { get; set; }
        public string? forexoneyeardescriptionfilename { get; set; }
        public string? forexoneyeardescriptionfilepath { get; set; }
        public string? forexoneyeardescriptionfileurl { get; set; }
        public string? forexoneyeardescriptionfilecontenttype { get; set; }
        public string? forexchartdescription { get; set; }
        public string? forexchartdescriptionfilename { get; set; }
        public string? forexchartdescriptionfilepath { get; set; }
        public string? forexchartdescriptionfileurl { get; set; }
        public string? forexchartdescriptionfilecontenttype { get; set; }
        public string? forexfirstcountryheading { get; set; }
        public string? forexfirstcountrydescription { get; set; }
        public string? forexfirstcountrydescriptionfilename { get; set; }
        public string? forexfirstcountrydescriptionfilepath { get; set; }
        public string? forexfirstcountrydescriptionfileurl { get; set; }
        public string? forexfirstcountrydescriptionfilecontenttype { get; set; }
        public List<ForexChartFirstCountryDataDto>? forexfirstcountrydatalist { get; set; }
        public string? forexsecondcountryheading { get; set; }
        public string? forexsecondcountrydescription { get; set; }
        public string? forexsecondcountrydescriptionfilename { get; set; }
        public string? forexsecondcountrydescriptionfilepath { get; set; }
        public string? forexsecondcountrydescriptionfileurl { get; set; }
        public string? forexsecondcountrydescriptionfilecontenttype { get; set; }
        public List<ForexChartSecondCountryDataDto>? forexsecondcountrydatalist { get; set; }
        public string? forexbottomdescription { get; set; }
        public string? forexmaindescription { get; set; }
        public string? forexmaindescriptionfilename { get; set; }
        public string? forexmaindescriptionfilepath { get; set; }
        public string? forexmaindescriptionfileurl { get; set; }
        public string? forexmaindescriptionfilecontenttype { get; set; }
        public string? forexsinglepagechartimage { get; set; }
    }
}
