using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class CourseBuilderDto
{
    public Guid? Id { get; set; }
    public Guid? courseId { get; set; }
    public string? title { get; set; }
    public string? description { get; set; }
}
