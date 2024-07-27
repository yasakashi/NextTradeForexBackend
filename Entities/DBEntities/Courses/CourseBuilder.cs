using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities;

[Table("tblCourseBuilders")]
public class CourseBuilder
{
    [Key]
    public Guid Id { get; set; }
    public Guid courseId { get; set; }
    public string title { get; set; }
    public string description { get; set; }

    public virtual Course course { get; set; }

}
