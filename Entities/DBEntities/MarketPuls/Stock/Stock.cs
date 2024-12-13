using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Dtos;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Stocks")]
    public class Stock
    {
        [Key]
        public Guid id { get; set; }
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
        public string? fundamentalandtechnicaltabsection_instrumentname { get; set; }
        public string? fundamentalandtechnicaltabsection_fundamentalheading { get; set; }
        public string? fundamentalandtechnicaltabsection_technicalheading { get; set; }
        public string? fundamentalandtechnicaltabsection_marketsesstiontite { get; set; }
        public string? fundamentalandtechnicaltabsection_marketsesstionscript { get; set; }
        public string? fundamentalandtechnicaltabsection_marketsentimentstitle { get; set; }
        public string? fundamentalandtechnicaltabsection_marketsentimentsscript { get; set; }
        public string? fundamentalandtechnicaltabsection_relatedresorces { get; set; }
        public string? fundamentalandtechnicaltabsection_privatenotes { get; set; }
        public string? stocksection_newstickernew { get; set; }
        public string? stocksection_newstickerupdate { get; set; }
        public string? stocksection_newstickerimportant { get; set; }
        public string? stocksection_established { get; set; }
        public string? stocksection_exchange { get; set; }
        public string? stocksection_companytype { get; set; }
        public string? stocksection_ownership { get; set; }
        public string? stocksection_mainofficecountr { get; set; }
        public string? stocksection_url { get; set; }
        public string? stocksection_totalbranches { get; set; }
        public string? stocksection_otherimportantlocation { get; set; }
        public string? stocksection_overalllocations { get; set; }
        public string? stocksection_servicesoffered { get; set; }
        public string? stocksection_marketfocus { get; set; }
        public string? stocksection_briefdescriptionofcompany { get; set; }
        public string? stocksection_importantresearchnotes { get; set; }
        public string? stocksection_chart { get; set; }
        public string? stocksection_briefdescriptionofratio { get; set; }
        public string? financialdata_estturnoverus_year1 { get; set; }
        public string? financialdata_estturnoverus_year2 { get; set; }
        public string? financialdata_estturnoverus_year3 { get; set; }
        public string? financialdata_estturnoverus_year4 { get; set; }
        public string? financialdata_estturnoverus_year5 { get; set; }
        public string? financialdata_estgrossprofit_year1 { get; set; }
        public string? financialdata_estgrossprofit_year2 { get; set; }
        public string? financialdata_estgrossprofit_year3 { get; set; }
        public string? financialdata_estgrossprofit_year4 { get; set; }
        public string? financialdata_estgrossprofit_year5 { get; set; }
        public string? financialdata_estnetprofit_year1 { get; set; }
        public string? financialdata_estnetprofit_year2 { get; set; }
        public string? financialdata_estnetprofit_year3 { get; set; }
        public string? financialdata_estnetprofit_year4 { get; set; }
        public string? financialdata_estnetprofit_year5 { get; set; }
        public string? currentfinancial_estturnoverus_q1 { get; set; }
        public string? currentfinancial_estturnoverus_q2 { get; set; }
        public string? currentfinancial_estturnoverus_q3 { get; set; }
        public string? currentfinancial_estturnoverus_q4 { get; set; }
        public string? currentfinancial_estgrossprofit_q1 { get; set; }
        public string? currentfinancial_estgrossprofit_q2 { get; set; }
        public string? currentfinancial_estgrossprofit_q3 { get; set; }
        public string? currentfinancial_estgrossprofit_q4 { get; set; }
        public string? currentfinancial_estnetprofit_q1 { get; set; }
        public string? currentfinancial_estnetprofit_q2 { get; set; }
        public string? currentfinancial_estnetprofit_q3 { get; set; }
        public string? currentfinancial_estnetprofit_q4 { get; set; }
        public string? workingcapotalratio_ratio { get; set; }
        public string? workingcapotalratio_analysisisgood { get; set; }
        public string? quickratio_ratio { get; set; }
        public string? quickratio_analysisisgood { get; set; }
        public string? earningpershareratio_ratio { get; set; }
        public string? earningpershareratio_analysisisgood { get; set; }
        public string? priceearninsratio_ratio { get; set; }
        public string? priceearninsratio_analysisisgood { get; set; }
        public string? earningpersdebttoequityratio_ratio { get; set; }
        public string? earningpersdebttoequityratio_analysisisgood { get; set; }
        public string? returnonequityratio_ratio { get; set; }
        public string? returnonequityratio_analysisisgood { get; set; }

    }
}
