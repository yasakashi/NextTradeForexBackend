using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Indices_TechnicalTabss")]
    public class Indice_TechnicalTabs
    {
        [Key]
        public Guid id { get; set; }
        public Guid marketpulsindiceid { get; set; }
        public string? tabtitle { get; set; }
        public string? script { get; set; }
        public virtual Indice? marketpulsindice { get; set; }
    }
}
