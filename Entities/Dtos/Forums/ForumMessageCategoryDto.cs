using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class ForumMessageCategoryDto
{
    public Guid? Id { get; set; }
    public Guid? forummessageId { get; set; }
    public long? categoryid { get; set; }
    public string? categoryname { get; set; }
}
