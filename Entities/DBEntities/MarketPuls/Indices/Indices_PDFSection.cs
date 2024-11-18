using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Indices_PDFSections")]
    public class Indices_PDFSection
    {
        [Key]
        public Guid id { get; set; }
        public Guid marketpulsindiceid { get; set; }
        public string? pdftitle { get; set; }
        public string? pdfshortcodeid { get; set; }
        public string? author { get; set; }
        public string? shortdescription { get; set; }

        public virtual Indice? marketpulsindice { get; set; }
    }
}
