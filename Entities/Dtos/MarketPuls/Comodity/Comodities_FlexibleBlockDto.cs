using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class Comodities_FlexibleBlockDto
    {
        public Guid? id { get; set; }
        public Guid? comodityid { get; set; }
        public string? maintitle { get; set; }
        public string? oneyeardescription { get; set; }
        public string? oneyeardescriptionfilename { get; set; }
        public string? oneyeardescriptionfilecontenttype { get; set; }
        public string? oneyeardescriptionfilepath { get; set; }
        public string? oneyeardescriptionfileurl { get; set; }
        public string? chartdescription { get; set; }
        public string? chartdescriptionfilename { get; set; }
        public string? chartdescriptionfilecontenttype { get; set; }
        public string? chartdescriptionfilepath { get; set; }
        public string? chartdescriptionfileurl { get; set; }
        public string? firstcontryheading { get; set; }
        public string? firstcontrydescription { get; set; }
        public string? firstcontrydescriptionfilename { get; set; }
        public string? firstcontrydescriptionfilecontentype { get; set; }
        public string? firstcontrydescriptionfilepath { get; set; }
        public string? firstcontrydescriptionfileurl { get; set; }
        public string? secoundcontrydescription { get; set; }
        public string? secoundcontrydescriptionfilename { get; set; }
        public string? secoundcontrydescriptionfilecontenttype { get; set; }
        public string? secoundcontrydescriptionfilepath { get; set; }
        public string? secoundcontrydescriptionfileurl { get; set; }
        public string? bottomdescription { get; set; }
        public string? bottomdescriptionfilename { get; set; }
        public string? bottomdescriptionfilecontenttype { get; set; }
        public string? bottomdescriptionfilepath { get; set; }
        public string? bottomdescriptionfileurl { get; set; }
        public string? maindescrition { get; set; }
        public string? maindescritionfilename { get; set; }
        public string? maindescritionfilecontenttype { get; set; }
        public string? maindescritionfilepath { get; set; }
        public string? maindescritionfileurl { get; set; }
        public string? singlepagechartimage { get; set; }


        public List<ComoditiesCountriesDataDto>? comoditiescountriesdatalist { get; set; }
        public List<ComoditiesFirstCountryDataCountriesDataDto>? comoditiesfirstcountrydatacountriesdatalist { get; set; }
        public List<ComoditiesSecondCountryDataCountriesDataDto>? comoditiessecondcountrydatacountriesdatalist { get; set; }

    }
}
