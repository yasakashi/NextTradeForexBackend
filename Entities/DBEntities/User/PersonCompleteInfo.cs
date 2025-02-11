using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblPeopleCompleteInfo")]
    public class PersonCompleteInfo
    {
        [Key]
        public long personId { get; set; }
        public string? biographicalInfo { get; set; }
        public string? jobtitle { get; set; }
        public string? profilebio { get; set; }
        public string? timezone { get; set; }
        public string? hobbyOfTrading { get; set; }
    }
}
