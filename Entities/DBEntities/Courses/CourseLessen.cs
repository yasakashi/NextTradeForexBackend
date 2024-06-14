using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBEntities;

[Table("tblCourseLessens")]
public class CourseLessen
{
    [Key]
    public Guid Id { get; set; }
    public Guid courseId { get; set; }

    public virtual Course course { get; set; }
}
