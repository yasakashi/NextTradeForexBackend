using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Indice_TechnicalTabs_TechnicalBreakingNewss")]
    public class Indice_TechnicalTabs_TechnicalBreakingNews
    {
        [Key]
        public Guid id { get; set; }
        public Guid marketpulsindiceid { get; set; }
        public Guid? technicaltabsid { get; set; }

        public string title { get; set; }

        public string description { get; set; }
        public string link { get; set; }
        public string? newsmaincontentfilename { get; set; }
        public string? newsmaincontentfilepath { get; set; }
        public string? newsmaincontentfilecontenttype { get; set; }

        public virtual Indice? marketpulsindice { get; set; }
        public virtual Indice_TechnicalTabs? technicaltabs { get; set; }
    }
}
