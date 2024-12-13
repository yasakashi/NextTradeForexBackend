using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Stocks_URLSections")]
    public class StockURLSection
    {
        [Key]
        public Guid id { get; set; }
        public Guid stockid { get; set; }
        public string? urltitle { get; set; }
        public string? url { get; set; }

        public virtual Stock stock { get; set; }
    }
}
