using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Crypto_FundamentalandNewsSections")]
    public class Crypto_FundamentalandNewsSection
    {
        [Key]
        public Guid id { get; set; }
        public Guid cryptoid { get; set; }
        public string? maintitle { get; set; }
        public string? script { get; set; }

        public virtual Crypto crypto { get; set; }
    }
}
