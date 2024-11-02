using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Comodities_fundamentalandtechnicaltabsection_fundamentalnewssections")]
    public class Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection
    {
        [Key]
        public Guid id { get; set; }
        public Guid comodityid { get; set; }
        public string? maintitle { get; set; }
        public string? script { get; set; }

        public virtual Comodity? comodity { get; set; }
    }

}
