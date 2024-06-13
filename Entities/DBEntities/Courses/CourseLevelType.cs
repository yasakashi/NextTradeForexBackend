using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities;

[Table("tblCourseLevelTypes")]
public class CourseLevelType
{
    [Key]
    public int Id { get; set; }
    public string name { get; set; }
    public string imageurl { get; set; }
    public string description { get; set; }
}
