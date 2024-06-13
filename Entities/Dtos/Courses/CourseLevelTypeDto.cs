using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class CourseLevelTypeDto
{
    public int? Id { get; set; }
    public string? name { get; set; }
    public string? imageurl { get; set; }
    public string? description { get; set; }
}
