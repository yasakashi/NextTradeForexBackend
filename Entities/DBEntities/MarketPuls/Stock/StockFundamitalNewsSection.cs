using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Stocks_FundamitalNewsSections")]
    public class StockFundametalNewsSection
    {
        [Key]
        public Guid id { get; set; }
        public Guid stockid { get; set; }
        public string? maintitle { get; set; }
        public string? script { get; set; }

        public virtual Stock stock { get; set; }
    }

}
