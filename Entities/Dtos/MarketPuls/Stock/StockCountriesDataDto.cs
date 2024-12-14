using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class StockCountriesDataDto
    {
        public Guid? id { get; set; }
        public Guid? stockid { get; set; }
        public Guid? stockflexibleblockid { get; set; }
        public string? countries { get; set; }
        public string? pairthatcorrelate { get; set; }
        public string? highsandlows { get; set; }
        public string? pairtype { get; set; }
        public string? dailyaveragmovementinpips { get; set; }
    }
}
