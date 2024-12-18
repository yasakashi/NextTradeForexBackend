using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DBEntities;

namespace Entities.Dtos
{
    public class ForexChartCountriesDataDto
    {
        public Guid? id { get; set; }
        public Guid? forexchartid { get; set; }
        public Guid? forexchartflexibleblockid { get; set; }
        public string? forexcontries { get; set; }
        public string? forexpairsthatcorrelate { get; set; }
        public string? highsandlows { get; set; }
        public string? forexpairtype { get; set; }
        public string? forexdailyaveragemovementinpair { get; set; }
    }
}
