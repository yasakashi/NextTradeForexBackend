using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class StockURLSectionDto
    {
        public Guid? id { get; set; }
        public Guid? stockid { get; set; }
        public string? urltitle { get; set; }
        public string? url { get; set; }
    }
}
