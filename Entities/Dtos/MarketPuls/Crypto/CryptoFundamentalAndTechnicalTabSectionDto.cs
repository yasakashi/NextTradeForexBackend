using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class CryptoFundamentalAndTechnicalTabSectionDto
    {
        public string? instrumentname { get; set; }
        public string? fundamentalheading { get; set; }
        public string? technicalheading { get; set; }
        public string? marketsessiontitle { get; set; }
        public string? marketsessionscript { get; set; }
        public string? marketsentimentstitle { get; set; }
        public string? marketsentimentsscript { get; set; }
        public string? relatedresorces { get; set; }
        public string? privatenotes { get; set; }
        public List<Crypto_FundamentalandNewsSectionDto> fundamentalnewssections { get; set; }
        public List<Crypto_TechnicalTabDto> technicaltabs { get; set; }
        public List<Crypto_FundamentalandNewsSection_RelatedReSorces_PDFSectionDto>? pdfsectionlist { get; set; }
        public List<Crypto_FundamentalandNewsSection_RelatedReSorces_URLSectionDto>? urlsectionlist { get; set; }
    }

}
