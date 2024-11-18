using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Indices_AlternateIndices")]
    public class Indice_AlternateIndice
    {
        [Key]
        public Guid id { get; set; }
        public Guid marketpulsindiceid { get; set; }
        public string? name { get; set; }
        public string? link { get; set; }

        public virtual Indice? marketpulsindice { get; set; }
    }
}
