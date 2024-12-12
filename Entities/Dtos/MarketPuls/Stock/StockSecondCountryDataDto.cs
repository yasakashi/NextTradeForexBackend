using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class StockSecondCountryDataDto
    {
        public Guid? id { get; set; }
        public Guid? stockid { get; set; }
        public Guid? stockflexibleblockid { get; set; }
        public string? contry { get; set; }
        public string? centeralbank { get; set; }
        public string? nickname { get; set; }
        public string? ofaveragedailyturnover { get; set; }
    }
}
