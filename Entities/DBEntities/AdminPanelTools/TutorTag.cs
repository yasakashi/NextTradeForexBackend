using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblTutorTags")]
    public class TutorTag
    {
        [Key]
        public Guid Id { get; set; }
        public string? name { get; set; }
        public string? slug { get; set; }
        public string? description { get; set; }
    }
}
