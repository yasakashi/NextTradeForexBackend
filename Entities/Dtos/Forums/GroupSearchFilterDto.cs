using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class GroupSearchFilterDto
{
    public long? owneruserid { get; set; }
    public long? categoryid { get; set; }
}
