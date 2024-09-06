using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBEntities;

[Table("tblCommunityGroupMembers")]
public class CommunityGroupMember
    {
    [Key]
    public Guid Id { get; set; }
    public Guid communitygroupId { get; set; }

    public long userId { get; set; }
    public DateTime requestdatetime { get; set; }
    public bool isaccepted { get; set; }
    public bool isadmin { get; set; }
    public DateTime? accepteddatetime { get; set; }

    public virtual CommunityGroup communitygroup { get; set; }
    public virtual User user { get; set; }

}

