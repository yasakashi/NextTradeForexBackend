using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class PeronCompleteInfoDto
    {
        public long personId { get; set; }
        public string? biographicalInfo { get; set; }
        public string? jobtitle { get; set; }
        public string? profilebio { get; set; }
        public string? timezone { get; set; }
        public string? hobbyOfTrading { get; set; }
        
    }
}
