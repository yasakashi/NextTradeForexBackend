using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBEntities;

[Table("tblCourseLessons")]
public class CourseLesson
{
    [Key]
    public Guid Id { get; set; }
    public Guid courseId { get; set; }
    public long? aoutoruserid { get; set; }
    public string author { get; set; }
    public string lessonname { get; set; }
    public string lessondescription { get; set; }
    public DateTime lessontime { get; set; }
    public DateTime starttime { get; set; }
    public DateTime endtime { get; set; }
    public DateTime registerdatetime { get; set; }

    public virtual Course course { get; set; }
    public virtual User aoutoruser { get; set; }
}
