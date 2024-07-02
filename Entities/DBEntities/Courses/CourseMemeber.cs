using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entities.DBEntities;

/// <summary>
/// 
/// </summary>
[Table("tblCourseMemebers")]
public class CourseMemeber
{
    [Key]
    [Column("Id")]
    public Guid Id { get; set; }
    public long userId { get; set; }
    public Guid courseId { get; set; }
    public DateTime? registerdatetime { get; set; }


    public virtual User user { get; set; }
    public virtual Course course { get; set; }
}