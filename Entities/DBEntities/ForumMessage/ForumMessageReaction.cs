using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities;

[Table("tblForumMessageReactions")]
public class ForumMessageReaction
{
    public int reactiontypeId { get; set; }
    public long userId { get; set; }
    public Guid forummessageId { get; set; }
    [Key]
    public Guid Id { get; set; }

    public virtual ReactionType reactiontype { get; set; }
    public virtual User user { get; set; }
}
