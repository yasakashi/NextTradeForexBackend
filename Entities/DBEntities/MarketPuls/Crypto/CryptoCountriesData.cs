using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_CryptoCountriesDatas")]
    public class CryptoCountriesData
    {
        public Guid id { get; set; }
        public Guid cryptoid { get; set; }
        public Guid cryptoflexibleblockid { get; set; }
        public string? contry { get; set; }
        public string? centeralbank { get; set; }
        public string? nickname { get; set; }
        public string? ofaveragedailyturnover { get; set; }

        public virtual Crypto crypto { get; set; }
        public virtual Crypto_FlexibleBlock cryptoflexibleblock { get; set; }
    }
}
