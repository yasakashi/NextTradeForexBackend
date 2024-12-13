using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Stocks_TechnicalBreakingNewss")]
    public class StockTechnicalBreakingNews
    {
        [Key]
        public Guid id { get; set; }
        public Guid stockid { get; set; }
        public Guid? technicaltabid { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public string? descriptionfilename { get; set; }
        public string? descriptionfilepath { get; set; }
        public string? descriptionfileurl { get; set; }
        public string? descriptionfilecontenttype { get; set; }
        public string? link { get; set; }

        public virtual Stock stock { get; set; }
        public virtual StockTechnicalTab technicaltab { get; set; }
    }
}
