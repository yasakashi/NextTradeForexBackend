using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class CommunityGroupsMessageDto : BaseFilterDto
{
    public Guid? Id { get; set; }
    public Guid? communitygroupid { get; set; }
    public long? creatoruserid { get; set; }
    public string? creatorusername { get; set; }
    public string? messagebody { get; set; }
    public string? messagetitle { get; set; }
    public string? fileurl { get; set; }
    public DateTime? registerdatetime { get; set; }
    public DateTime? fromdate { get; set; }
    public DateTime? todate { get; set; }
    public IFormFile? messagefile { get; set; }
    public string? filecontenttype { get; set; }
    public bool? issignaltemplate { get; set; }
    public int? pagecount { get; set; }

}
