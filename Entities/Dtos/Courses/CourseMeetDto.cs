using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class CourseMeetDto : BaseFilterDto
{
    public Guid? Id { get; set; }
    public Guid? courseId { get; set; }    
    public string? name { get; set; }
    public DateTime? registerdatetime { get; set; }
    public DateTime? duedatetime { get; set; }
}
