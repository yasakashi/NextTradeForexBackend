using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class ComoditiesCountriesDataDto
    {
        public Guid? id { get; set; }
        public Guid? comodityid { get; set; }
        public Guid? comodityflexibleblockid { get; set; }
        public string? contries { get; set; }
        public string? pairsthatcorrelate { get; set; }
        public string? highslows { get; set; }
        public string? pairtype { get; set; }
        public string? dailyaveragemovementinpips { get; set; }
    }
}
