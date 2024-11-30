using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Comodities_URLSections")]
    public class Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSection
    {
        [Key]
        public Guid id { get; set; }
        public Guid comodityid { get; set; }
        public string? urltitle { get; set; }
        public string? url { get; set; }

        public virtual Comodity? comodity { get; set; }
    }
}
