using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_rypto_FundamentalandNewsSection_RelatedReSorces_PDFSections")]
    public  class Crypto_FundamentalandNewsSection_RelatedReSorces_PDFSection
    {
        public Guid id { get; set; }
        public Guid cryptoid { get; set; }
        public string? pdftitle { get; set; }
        public string? pdfshortcodeid { get; set; }
        public string? author { get; set; }
        public string? shortdescription { get; set; }

        public virtual Crypto crypto { get; set; }
    }
}
