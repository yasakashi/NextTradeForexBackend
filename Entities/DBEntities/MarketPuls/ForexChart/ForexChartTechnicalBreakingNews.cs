using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_ForexCharts_TechnicalBreakingNewss")]
    public class ForexChartTechnicalBreakingNews
    {
        [Key]
        public Guid id { get; set; }
        public Guid forexchartid { get; set; }
        public Guid forexchartflexibleblockid { get; set; }
        public Guid technicaltabid { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public string? descriptionfilename { get; set; }
        public string? descriptionfilepath { get; set; }
        public string? descriptionfileurl { get; set; }
        public string? descriptionfilecontenttype { get; set; }
        public string? link { get; set; }
        public virtual ForexChartFlexibleBlock forexchartflexibleblock { get; set; }
        public virtual ForexChart forexchart { get; set; }
        public virtual ForexChartTechnicalTab technicaltab { get; set; }
    }
}
