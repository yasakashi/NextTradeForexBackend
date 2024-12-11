using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class Comodities_Fundamentalandtechnicaltabsection_TechnicalTabsDto
    {
        public Guid? id { get; set; }
        public Guid? comodityid { get; set; }
        public string? tabtitle { get; set; }
        public string? script { get; set; }

       public  List<Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewsDto>? technicalbreakingnewslist { get;set;}
    }
}
