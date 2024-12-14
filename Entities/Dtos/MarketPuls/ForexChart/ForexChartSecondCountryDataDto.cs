using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class ForexChartSecondCountryDataDto
    {
        public Guid? id { get; set; }
        public Guid? forexchartflexibleblockid { get; set; }
        public string? forexrcontriy { get; set; }
        public string? forexrcentralbank { get; set; }
        public string? forexrnickname { get; set; }
        public string? forexrofaeragedailyturnover { get; set; }
    }
}
