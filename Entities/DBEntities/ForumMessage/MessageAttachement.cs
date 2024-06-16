using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBEntities;

[Table("tblForumMessageFileAttachments")]
public class MessageAttachement
{
    [Key]
    public Guid Id { get; set; }
    public Guid ForumMessageId { get; set; }
    public byte[]? attachment { get; set; }
}
