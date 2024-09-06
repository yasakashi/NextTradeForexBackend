using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Dtos;

public class ForumMessageReactionDto
{
    public int? reactiontypeId { get; set; }
    public string? reactiontypename { get; set; }
    public long? userId { get; set; }
    public string? username { get; set; }
    public Guid? forummessageId { get; set; }
    public Guid? Id { get; set; }
    public int? reactioncount { get; set; }

}

public class ForumMessageReactionModel
{
    public int? reactiontypeId { get; set; }
    public string? reactiontypename { get; set; }
    public int? reactioncount { get; set; }
    public Guid? forummessageId { get; set; }
}
