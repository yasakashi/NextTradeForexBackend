using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents")]
    public class Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontent
    {
        [Key]
        public Guid id { get; set; }
        public Guid comodityid { get; set; }
        public Guid comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_id { get; set; }
        public virtual Comodity? comodity { get; set; }
        public virtual Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection? comodities_fundamentalandtechnicaltabsection_fundamentalnewssection { get; set; }

        public string? title { get; set; }
        public string? description { get; set; }
        public string? link { get; set; }
        public string? filename { get; set; }
        public string? filepath { get; set; }
        public string? filecontenttype { get; set; }

    }
}
