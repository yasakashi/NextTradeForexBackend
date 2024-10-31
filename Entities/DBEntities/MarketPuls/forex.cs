using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Forex")]
    public class Forex
    {
        [Key]
        public Guid id { get; set; }

        public long categoryid { get; set; }
        public DateTime createdatetime { get; set; }
        public long creatoruserid { get; set; }
        public decimal price { get; set; }
        public bool isvisible { get; set; }
        public int courseleveltypeId { get; set; }
        public string? coursetitle { get; set; }
        public string? oneyeardescription { get; set; }
        public string? chartdescription { get; set; }
        public string? firstcountryheading { get; set; }
        public string? firstcountrydescription { get; set; }
        public string? secondcountryheading { get; set; }
        public string? secondcountrydescription { get; set; }
        public string? bottomdescription { get; set; }
        public string? maindescription { get; set; }
        public string? singlepagechartimage { get; set; }
        public string? instrumentname { get; set; }
        public string? fundamentalheading { get; set; }
        public string? technicalheading { get; set; }
        public string? marketsessiontitle { get; set; }
        public string? marketsessionscript { get; set; }
        public string? marketsentimentstitle { get; set; }
        public string? marketsentimentsscript { get; set; }
        public string? privatenotes { get; set; }
        public string? excerpt { get; set; }
        public string? author { get; set; }
    }
}