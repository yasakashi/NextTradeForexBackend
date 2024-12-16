using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_ForexCharts_ChartCountriesDatas")]
    public class ForexChartCountriesData
    {
        [Key]
        public Guid id { get; set; }
        public Guid forexchartid { get; set; }
        public Guid forexchartflexibleblockid { get; set; }
        public string? forexcontries { get; set; }
        public string? forexpairsthatcorrelate { get; set; }
        public string? highsandlows { get; set; }
        public string? forexpairtype { get; set; }
        public string? forexdailyaveragemovementinpair { get; set; }

        public virtual ForexChartFlexibleBlock forexchartflexibleblock { get; set; }
        public virtual ForexChart forexchart { get; set; }
    }
}
