using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Comodities_SecondCountryData_CountriesDatas")]
    public class ComoditiesSecondCountryDataCountriesData
    {
        public Guid id { get; set; }
        public Guid comodityid { get; set; }
        public Guid comodityflexibleblockid { get; set; }
        public string? contries { get; set; }
        public string? centeralbank { get; set; }
        public string? nickname { get; set; }
        public string? ofaveragedailyturnover { get; set; }

        public virtual Comodity? comodity { get; set; }
        public virtual Comodities_FlexibleBlock? comodityflexibleblock { get; set; }

    }
}
