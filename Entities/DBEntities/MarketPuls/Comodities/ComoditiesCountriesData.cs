using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Comodities_FlexibleBlock_CountriesDatas")]
    public class ComoditiesCountriesData
    {
        [Key]
        public Guid id { get; set; }

        public Guid comodityid { get; set; }
        public Guid comodityflexibleblockid { get; set; }
        public string? contries { get; set; }
        public string? pairsthatcorrelate { get; set; }
        public string? highslows { get; set; }
        public string? pairtype { get; set; }
        public string? dailyaveragemovementinpips { get; set; }

        public virtual Comodity? comodity { get; set; }
        public virtual Comodities_FlexibleBlock? comodityflexibleblock { get; set; }
        

    }
}
