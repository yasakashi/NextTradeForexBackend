using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Comodities_Fundamentalandtechnicaltabsection_Technicaltabs")]
    public class Comodities_Fundamentalandtechnicaltabsection_TechnicalTabs
    {
        [Key]
        public Guid id { get; set; }
        public Guid comodityid { get; set; }
        public string? tabtitle { get; set; }
        public string? script { get; set; }
        public virtual Comodity? comodity { get; set; }
    }
}
