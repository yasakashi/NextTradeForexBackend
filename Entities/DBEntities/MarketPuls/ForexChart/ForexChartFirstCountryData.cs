using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_ForexCharts_FirstCountryDatas")]
    public class ForexChartFirstCountryData
    {
        [Key]
        public Guid id { get; set; }
        public Guid forexchartid { get; set; }
        public Guid forexchartflexibleblockid { get; set; }
        public string? forexlcontriy { get; set; }
        public string? forexlcentralbank { get; set; }
        public string? forexlnickname { get; set; }
        public string? forexlofaeragedailyturnover { get; set; }

        public virtual ForexChartFlexibleBlock forexchartflexibleblock { get; set; }
        public virtual ForexChart forexchart { get; set; }
    }
}
