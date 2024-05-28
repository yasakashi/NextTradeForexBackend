using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class SubCategoryDto
{
    public long? Id { get; set; }
    public string name { get; set; }
    public long categotyId { get; set; }
}
