using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Stocks_SecondCountryDatas")]
    public class StockSecondCountryData
    {
        [Key]
        public Guid id { get; set; }
        public Guid stockid { get; set; }
        public Guid stockflexibleblockid { get; set; }
        public string? contry { get; set; }
        public string? centeralbank { get; set; }
        public string? nickname { get; set; }
        public string? ofaveragedailyturnover { get; set; }
        public virtual Stock stock { get; set; }
        public virtual StockFlexibleBlock stockflexibleblock { get; set; }
    }
}
