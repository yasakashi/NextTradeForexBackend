using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class CommunityGroupMemberDto:BaseFilterDto
{
    public Guid? Id { get; set; }
    public Guid communitygroupId { get; set; }

    public long? userId { get; set; }
    public DateTime? requestdatetime { get; set; }
    public bool? isaccepted { get; set; }
    public bool? isadmin { get; set; }
    public DateTime? accepteddatetime { get; set; }
    public string? communitygrouptitle { get; set; }
    public string? username { get; set; }
    public bool? isonline { get; set; }
    public int? pagecount { get; set; }

}
