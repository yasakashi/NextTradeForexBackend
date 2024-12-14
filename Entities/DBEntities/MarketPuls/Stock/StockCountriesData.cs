using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Stocks_CountriesDatas")]
    public class StockCountriesData
    {
        [Key]
        public Guid id { get; set; }
        public Guid stockid { get; set; }
        public Guid stockflexibleblockid { get; set; }
        public string? countries { get; set; }
        public string? pairthatcorrelate { get; set; }
        public string? highsandlows { get; set; }
        public string? pairtype { get; set; }
        public string? dailyaveragmovementinpips { get; set; }

        public virtual Stock stock { get; set; }
        public virtual StockFlexibleBlock stockflexibleblock { get; set; }
    }
}
