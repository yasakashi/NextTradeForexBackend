using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class ComoditiesFirstCountryDataCountriesDataDto
    {
        public Guid? id { get; set; }
        public Guid? comodityid { get; set; }
        public string? contries { get; set; }
        public string? centeralbank { get; set; }
        public string? nickname { get; set; }
        public string? ofaveragedailyturnover { get; set; }
    }
}
