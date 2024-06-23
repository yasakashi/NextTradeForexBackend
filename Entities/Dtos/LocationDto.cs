using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class LocationDto
    {
        public int? countryid { get; set; }
        public string? countryname { get; set; }
        public int? stateid { get; set; }
        public string? statename { get; set; }
        public int? cityid { get; set; }
        public string? cityname { get; set; }
    }
}
