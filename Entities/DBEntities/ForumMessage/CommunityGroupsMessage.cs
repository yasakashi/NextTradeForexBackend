using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities;

[Table("tblCommunityGroupsMessages")]
    public class CommunityGroupsMessage
    {
    [Key]
    public Guid Id { get; set; }
    public Guid communitygroupid { get; set; }
    public long creatoruserid { get; set; }
    public DateTime? registerdatetime { get; set; }
    public string messagebody { get; set; }
    public string messagetitle { get; set; }
    public string? fileurl { get; set; }
    public string? filecontenttype { get; set; }
    public bool? issignaltemplate { get; set; }
    public virtual User creatoruser { get; set; }

}

