using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class CourseLessonDto:BaseFilterDto
{
    public Guid? Id { get; set; }
    public Guid? courseId { get; set; }
    public string? coursename { get; set; }
    public long? aoutoruserid { get; set; }
    public string? aoutorusername { get; set; }
    public string? author { get; set; }
    public string? lessonname { get; set; }
    public string? lessondescription { get; set; }
    public DateTime? lessontime { get; set; }
    public DateTime? starttime { get; set; }
    public DateTime? endtime { get; set; }
    public DateTime? registerdatetime { get; set; }
}
