using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Comodities")]
    public class Comodity
    {
        [Key]
        public Guid id { get; set; }

        public long categoryid { get; set; }
        public DateTime? createdatetime { get; set; }
        public long? creatoruserid { get; set; }
        public string? title { get; set; }
        public string? tags { get; set; }
        public string? excerpt { get; set; }
        public string? authorname { get; set; }
        public long? authorid { get; set; }
        public bool? isvisible { get; set; }
        public int? courseleveltypeId { get; set; }
        public int? coursestatusid { get; set; }
        public DateTime? changestatusdate { get; set; }
        public string? comodities_maintitle { get; set; }
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

        public virtual Category? category { get; set; }
        public virtual User? author { get; set; }
    }
}

/*
 

 
tblMarketPuls_Comodities_FlexibleBlock_FirstCountryDatas
tblMarketPuls_Comodities_FlexibleBlock_SecondCountryDatas
tblMarketPuls_Comodities_fundamentalandtechnicaltabsection_fundamentalnewssections
tblMarketPuls_Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents
tblMarketPuls_Comodities_Fundamentalandtechnicaltabsection_Technicaltabs
tblMarketPuls_Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses
tblMarketPuls_Comodities_PDFSections
tblMarketPuls_Comodities_URLSections
 */