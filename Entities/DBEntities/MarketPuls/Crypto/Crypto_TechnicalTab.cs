using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Crypto_TechnicalTabs")]
    public class Crypto_TechnicalTab
    {
        [Key]
        public Guid? id { get; set; }
        public Guid? cryptoid { get; set; }
        public string? tabtitle { get; set; }
        public string? script { get; set; }

        public virtual Crypto crypto { get; set; }
    }
}
