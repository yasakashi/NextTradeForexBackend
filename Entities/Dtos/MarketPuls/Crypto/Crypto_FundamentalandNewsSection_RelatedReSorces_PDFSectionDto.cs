using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class Crypto_FundamentalandNewsSection_RelatedReSorces_PDFSectionDto
    {
        public Guid? id { get; set; }
        public Guid? cryptoid { get; set; }
        public string? pdftitle { get; set; }
        public string? pdfshortcodeid { get; set; }
        public string? author { get; set; }
        public string? shortdescription { get; set; }
    }
}
