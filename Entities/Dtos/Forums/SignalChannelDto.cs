using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class SignalChannelDto : BaseFilterDto
{
    public Guid? Id { get; set; }
    public Guid communitygroupId { get; set; }
    public string? title { get; set; }
    public DateTime? createdatetime { get; set; }
    public long? owneruserid { get; set; }
    public bool? isneedpaid { get; set; }
    public string? communitygroupname { get; set; }
    public long? grouptypeId { get; set; }
    public string? grouptypename { get; set; }

}
