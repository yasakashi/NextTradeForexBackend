using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBEntities;

[Table("tblSignalChannels")]
public class SignalChannel
{
    [Key]
    public Guid Id { get; set; }
    public Guid communitygroupId { get; set; }
    public string title { get; set; }
    public DateTime createdatetime { get; set; }
    public long owneruserid { get; set; }
    public bool isneedpaid { get; set; }

    public virtual CommunityGroup communitygroup { get; set; }
    public virtual User owneruser { get; set; }

}
