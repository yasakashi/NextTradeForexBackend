using Entities.DBEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Entities.Dtos
{
    public class ComodityModel
    {
        public Guid? id { get; set; }
        public List<Comodities_FlexibleBlockDto> comodities { get; set; }
        public FundamentalAndTechnicalTabSection fundamentalandtechnicaltabsection { get; set; }
        public long? categoryid { get; set; }
        public string? title { get; set; }
        public string? tags { get; set; }
        public string? excerpt { get; set; }
        public string? authorname { get; set; }
        public long? authorid { get; set; }
        public bool? isvisible { get; set; }
        public int? courseleveltypeId { get; set; }
        public int? coursestatusid { get; set; }
        public long? creatoruserid { get; set; }
        public DateTime? createdatetime { get; set; }
        public DateTime? changestatusdate { get; set; }
    }

    public class FundamentalAndTechnicalTabSection
    {
        public string? instrumentname { get; set; }
        public string? fundamentalheading { get; set; }
        public string? technicalheading { get; set; }
        public string? marketsentimentstitle { get; set; }
        public string? marketsentimentsscript { get; set; }
        public string? marketsessiontitle { get; set; }
        public string? marketsessionscript { get; set; }
        public string? relatedresorces { get; set; }
        public string? privatenotes { get; set; }
        public List<Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSectionDto>? comoditiespdfsectionlist { get; set; }
        public List<Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSectionDto>? comoditiesurlsectionlist { get; set; }
    }
    public class ComodityDto
    {
        public Guid? id { get; set; }

        public long? categoryid { get; set; }
        public DateTime? createdatetime { get; set; }
        public DateTime? changestatusdate { get; set; }
        public string? title { get; set; }
        public string? tags { get; set; }
        public string? excerpt { get; set; }
        public string? authorname { get; set; }
        public long? authorid { get; set; }
        public bool? isvisible { get; set; }
        public int? courseleveltypeId { get; set; }
        public int? coursestatusid { get; set; }
        public long? creatoruserid { get; set; }
        public string? maintitle { get; set; }
        public string? oneyeardescription { get; set; }
        public string? oneyeardescriptionfilename { get; set; }
        public string? oneyeardescriptionfilecontenttype { get; set; }
        public string? oneyeardescriptionfilepath { get; set; }
        public string? chartdescription { get; set; }
        public string? chartdescriptionfilename { get; set; }
        public string? chartdescriptionfilepath { get; set; }
        public string? chartdescriptionfilecontenttype { get; set; }
        public string? firstcountryheading { get; set; }
        public string? firstcountrydescription { get; set; }
        public string? firstcountrydescriptionfilename { get; set; }
        public string? firstcountrydescriptionfilepath { get; set; }
        public string? firstcountrydescriptionfilecontenttype { get; set; }
        public string? secondcountryheading { get; set; }
        public string? secondcountrydescription { get; set; }
        public string? secondcountrydescriptionfilename { get; set; }
        public string? secondcountrydescriptionfilepath { get; set; }
        public string? secondcountrydescriptionfilecontenttype { get; set; }
        public string? bottomdescription { get; set; }
        public string? bottomdescriptionfilename { get; set; }
        public string? bottomdescriptionfilepath { get; set; }
        public string? bottomdescriptionfilecontenttype { get; set; }
        public string? maindescription { get; set; }
        public string? maindescriptionfilecontenttype { get; set; }
        public string? maindescriptionfilename { get; set; }
        public string? maindescriptionfilepath { get; set; }
        public string? singlepagechartimage { get; set; }
        public string? fundamentalandtechnicaltabsection_instrumentname { get; set; }
        public string? fundamentalandtechnicaltabsection_fundamentalheading { get; set; }
        public string? fundamentalandtechnicaltabsection_technicalheading { get; set; }
        public string? fundamentalandtechnicaltabsection_marketsentimentstitle { get; set; }
        public string? fundamentalandtechnicaltabsection_marketsentimentsscript { get; set; }
        public string? fundamentalandtechnicaltabsection_marketsessiontitle { get; set; }
        public string? fundamentalandtechnicaltabsection_marketsessionscript { get; set; }
        public string? fundamentalandtechnicaltabsection_relatedresorces { get; set; }
        public string? fundamentalandtechnicaltabsection_privatenotes { get; set; }


        public List<Comodities_FlexibleBlockDto>? comoditiesflexibleblocklist { get; set; }

        public List<Comodities_fundamentalandtechnicaltabsection_fundamentalnewssectionDto>? Comodities_fundamentalandtechnicaltabsection_fundamentalnewssectionlist { get; set; }
        public List<Comodities_Fundamentalandtechnicaltabsection_TechnicalTabsDto>? comoditiesfundamentalandtechnicaltabsectiontechnicaltabslist { get; set; }
        public List<Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSectionDto>? comoditiesfundamentalandtechnicaltabsection_relatedresorces_pdfsectionlist { get; set; }
        public List<Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSectionDto>? comoditiesfundamentalandtechnicaltabsection_relatedresorces_urlsectionlist { get; set; }
    }

    public class ComodityFilterDto : BaseFilterDto
    {
        public Guid? id { get; set; }

        public long? categoryid { get; set; }
        public long? creatoruserid { get; set; }
        public string? title { get; set; }
        public string? authorname { get; set; }
        public long? authorid { get; set; }
        public bool? isvisible { get; set; }
        public int? courseleveltypeId { get; set; }
        public int? coursestatusid { get; set; }
    }
}
