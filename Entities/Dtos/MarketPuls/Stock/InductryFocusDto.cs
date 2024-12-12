using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class StockInductryFocusDto
    {
        public Guid? id { get; set; }
        public Guid? stockid { get; set; }
        public string? industryfocus { get; set; }
        public string? clientnameifapplicable { get; set; }
        public string? revenueshare { get; set; }
    }
}
