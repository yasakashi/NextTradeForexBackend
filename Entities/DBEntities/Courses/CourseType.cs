using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBEntities;

[Table("tblCourses")]
public class CourseType
{
    [Key]
    public int Id { get; set; }
    public string name { get; set; }
}
