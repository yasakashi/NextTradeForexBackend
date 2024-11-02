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

        public string? fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontent_title { get; set; }
        public string? fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontent_description { get; set; }
        public string? fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontent_link { get; set; }
        public string? fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontent_filename { get; set; }
        public string? fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontent_filepath { get; set; }
        public string? fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontent_filecontenttype { get; set; }

    }
}
