using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses")]
    public class Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNews
    {
        [Key]
        public Guid id { get; set; }
        public Guid comodityid { get; set; }
        public Guid comoditiesfundamentalandtechnicaltabsectiontechnicaltabsid { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public string? link { get; set; }
        public string? filename { get; set; }
        public string? filepath { get; set; }
        public string? filecontenttype { get; set; }
        public virtual Comodity? comodity { get; set; }
        public virtual Comodities_Fundamentalandtechnicaltabsection_TechnicalTabs? comoditiesfundamentalandtechnicaltabsectiontechnicaltabs { get; set; }
    }
}







