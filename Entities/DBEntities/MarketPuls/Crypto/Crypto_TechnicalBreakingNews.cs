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
    public class Crypto_TechnicalBreakingNews
    {
        [Key]
        public Guid id { get; set; }
        public Guid cryptoid { get; set; }
        public Guid technicaltabid { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public string? descriptionfilename { get; set; }
        public string? descriptionfilepath { get; set; }
        public string? descriptionfileurl { get; set; }
        public string? descriptionfilecontenttype { get; set; }
        public string? link { get; set; }

        public virtual Crypto crypto { get; set; }
        public virtual Crypto_TechnicalTab technicaltab { get; set; }
    }
}
