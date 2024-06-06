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
    public Guid? parentId { get; set; }
    public Guid? communitygroupid { get; set; }
    public long creatoruserid { get; set; }
    public long categoryid { get; set; }
    public string title { get; set; }
    public string messagebody { get; set; }
    public bool isneedpaid { get; set; }
    public DateTime registerdatetime { get; set; }
    public virtual List<MessageAttachement> sttachements { get; set; }
    public virtual Category category { get; set; }
    public virtual CommunityGroup communitygroup { get; set; }
    public virtual User creatoruser { get; set; }
}
