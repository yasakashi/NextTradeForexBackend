using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class GroupSearchFilterDto:BaseFilterDto
{
    public Guid? id { get; set; }
    public long? owneruserid { get; set; }
    public long? grouptypeId { get; set; }
    public bool showdetail { get; set; } = false;

}
