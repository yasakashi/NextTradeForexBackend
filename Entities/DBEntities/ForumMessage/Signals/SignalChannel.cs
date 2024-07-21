using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Dtos;

namespace Entities.DBEntities;

[Table("tblSignalChannels")]
public class SignalChannel
{
    [Key]
    public Guid Id { get; set; }
    public Guid communitygroupId { get; set; }
    public string title { get; set; }
    public string? description { get; set; }
    public DateTime createdatetime { get; set; }
    public long owneruserid { get; set; }
    public bool isneedpaid { get; set; }
    public long grouptypeId { get; set; }

    public virtual CommunityGroup communitygroup { get; set; }
    public virtual GroupType grouptype { get; set; }
    public virtual User owneruser { get; set; }

}
