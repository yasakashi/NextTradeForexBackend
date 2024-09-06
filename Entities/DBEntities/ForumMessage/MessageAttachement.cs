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
    public string? photofileurl { get; set; }
    public string? videofileurl { get; set; }
    public string? audiofileurl { get; set; }
    public string? photofilecontenttype { get; set; }
    public string? videofilecontenttype { get; set; }
    public string? audiofilecontenttype { get; set; }
}
