using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Stocks_FlexibleBlocks")]
    public class StockFlexibleBlock
    {
        [Key]
        public Guid id { get; set; }
        public Guid stockid { get; set; }
        public string? maintitle { get; set; }
        public string? oneyeardescription { get; set; }
        public string? oneyeardescriptionfilename { get; set; }
        public string? oneyeardescriptionfilepath { get; set; }
        public string? oneyeardescriptionfileurl { get; set; }
        public string? oneyeardescriptionfilecontenttype { get; set; }
        public string? chartdescription { get; set; }
        public string? chartdescriptionfilename { get; set; }
        public string? chartdescriptionfilepath { get; set; }
        public string? chartdescriptionfileurl { get; set; }
        public string? chartdescriptionfilecontenttype { get; set; }
        public string? firstcountryheading { get; set; }
        public string? firstcountrydescription { get; set; }
        public string? firstcountrydescriptionfilename { get; set; }
        public string? firstcountrydescriptionfilepath { get; set; }
        public string? firstcountrydescriptionfileurl { get; set; }
        public string? firstcountrydescriptionfilecontenttype { get; set; }
        public string? secondcountryheading { get; set; }
        public string? secondcountrydescription { get; set; }
        public string? secondcountrydescriptionfilename { get; set; }
        public string? secondcountrydescriptionfilepath { get; set; }
        public string? secondcountrydescriptionfileurl { get; set; }
        public string? secondcountrydescriptionfilecontenttype { get; set; }
        public string? bottomdescription { get; set; }
        public string? bottomdescriptionfilename { get; set; }
        public string? bottomdescriptionfilepath { get; set; }
        public string? bottomdescriptionfileurl { get; set; }
        public string? bottomdescriptionfilecontenttype { get; set; }
        public string? maindescription { get; set; }
        public string? maindescriptionfilename { get; set; }
        public string? maindescriptionfilepath { get; set; }
        public string? maindescriptionfileurl { get; set; }
        public string? maindescriptionfilecontenttype { get; set; }
        public string? singlepagechartimage { get; set; }
        public virtual Stock stock { get; set; }
    }
}
