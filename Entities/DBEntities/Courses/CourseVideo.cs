using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBEntities;

[Table("tblCourseVideos")]
public class CourseVideo
{
    [Key]
    public Guid Id { get; set; }
    public Guid courseId { get; set; }
    public string name { get; set; }
    public DateTime registerdatetime { get; set; }
    public DateTime duedatetime { get; set; }
    public Boolean allowDownload { get; set; }
    
}
