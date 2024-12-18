using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class ForexChartFirstCountryDataDto
    {
        public Guid? id { get; set; }
        public Guid? forexchartid { get; set; }
        public Guid? forexchartflexibleblockid { get; set; }
        public string? forexlcontriy { get; set; }
        public string? forexlcentralbank{ get; set; }
        public string? forexlnickname { get; set; }
        public string? forexlofaeragedailyturnover { get; set; }
    }
}
