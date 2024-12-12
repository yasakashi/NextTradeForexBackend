using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class StockManagementTeamDto
    {
        public Guid? id { get; set; }
        public Guid? stockid { get; set; }
        public string? name { get; set; }
        public string? designation { get; set; }
    }
}
