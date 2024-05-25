using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities;

[Table("tblForumMessages")]
public class ForumMessage
{
    [Key]
    public Guid Id { get; set; }
    public Guid parentId { get; set; }
    public long categoryid { get; set; }
    public long subcategoryid { get; set; }
    public long subcategorygroupid { get; set; }

    public string title { get; set; }
    public string messagebody { get; set; }
    public virtual List<MessageAttachement> sttachements { get; set; }
    public Category category { get; set; }
}
