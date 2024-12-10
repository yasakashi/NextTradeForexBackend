using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Crypto_FundamentalandNewsSection_RelatedReSorces_URLSections")]
    public class Crypto_FundamentalandNewsSection_RelatedReSorces_URLSection
    {
        [Key]
        public Guid? id { get; set; }
        public Guid? cryptoid { get; set; }
        public string? urltitle { get; set; }
        public string? url { get; set; }

        public virtual Crypto crypto { get; set; }
    }
}
