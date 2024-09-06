using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBEntities;

[Table("tblCourseTopics")]
public class CourseTopic
{
    [Key]
    public Guid Id { get; set; }
    public Guid courseId { get; set; }
    public string name { get; set; }
    public DateTime registerdatetime { get; set; }
}
