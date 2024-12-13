using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Stocks_InductryFocuss")]
    public class StockInductryFocus
    {
        [Key]
        public Guid id { get; set; }
        public Guid stockid { get; set; }
        public string? industryfocus { get; set; }
        public string? clientnameifapplicable { get; set; }
        public string? revenueshare { get; set; }
        public virtual Stock stock { get; set; }
    }
}
