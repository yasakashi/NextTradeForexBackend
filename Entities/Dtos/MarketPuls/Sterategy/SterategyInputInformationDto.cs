using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class SterategyInputInformationDto
    {
        public Guid? id { get; set; }
        public Guid? sterategid { get; set; }
        public string? name { get; set; }
        public string? expression { get; set; }
        public string? inputdefault { get; set; }
        public string? description { get; set; }
    }
}
