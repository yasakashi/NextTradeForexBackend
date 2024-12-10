using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DBEntities;

namespace Entities.Dtos
{
    public class Crypto_FlexibleBlockDto
    {
        public Guid? id { get; set; }
        public Guid? cryptoid { get; set; }
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
        public string? contryheading { get; set; }
        public string? contrydescription { get; set; }
        public string? contrydescriptionfilename { get; set; }
        public string? contrydescriptionfilecontentype { get; set; }
        public string? contrydescriptionfilepath { get; set; }
        public string? contrydescriptionfileurl { get; set; }
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

        public List<CryptoCountryDataDto>? countrydatalist { get; set; }
        public List<CryptoCountriesDataDto>? countriesdatalist { get; set; }
    }


}
