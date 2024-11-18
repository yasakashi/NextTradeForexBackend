using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class ForumMessageFilterDto: BaseFilterDto
{
    public Guid? Id { get; set; }
    public Guid? parentId { get; set; }
    public long? categoryid { get; set; }
    public List<long>? categoryids { get; set; }
    public long? creatoruserid { get; set; }
    public string? creatorusername { get; set; }
    public DateTime? fromregisterdatetime { get; set; }
    public DateTime? toregisterdatetime { get; set; }
    public Guid? communitygroupid { get; set; }
    public string? title { get; set; }
    public string? messagebody { get; set; }
    public bool? isneedpaid { get; set; }
    public bool? showpost { get; set; }
    public bool? issignaltemplate { get; set; }

}
public class ForumMessageDto
{
    public Guid? Id { get; set; }
    public Guid? parentId { get; set; }
    public long? categoryid { get; set; }
    public List<long>? categoryids { get; set; }
    public Guid? communitygroupid { get; set; }
    public string? categoryname { get; set; }
    public long? creatoruserid { get; set; }
    public string? creatorusername { get; set; }
    public DateTime? registerdatetime { get; set; }
    public DateTime? fromregisterdatetime { get; set; }
    public DateTime? toregisterdatetime { get; set; }
    public string title { get; set; }
    public string messagebody { get; set; }
    public bool isneedpaid { get; set; }
    public bool allowtoshow { get; set; }
    public int? commentcount { get; set; }
    public int? pagecount { get; set; }
    public IFormFile? photofile { get; set; }
    public string? photofileurl { get; set; }
    public string? videofileurl { get; set; }
    public string? audiofileurl { get; set; }
    public string? photofilecontenttype { get; set; }
    public string? videofilecontenttype { get; set; }
    public string? audiofilecontenttype { get; set; }
    public IFormFile? videofile { get; set; }
    public IFormFile? audiofile { get; set; }
    public bool? issignaltemplate { get; set; }
    public virtual List<ForumMessageCategoryDto> categories { get; set; }
    public virtual List<ForumMessageReactionModel> reactions{get;set;}

}

public class ForumMessageShortDto
{
    public Guid? Id { get; set; }
    public Guid? parentId { get; set; }
    public long? categoryid { get; set; }
    public List<long>? categoryids { get; set; }
    public Guid? communitygroupid { get; set; }
    public string? categoryname { get; set; }
    public long? creatoruserid { get; set; }
    public string? creatorusername { get; set; }
    public DateTime? registerdatetime { get; set; }
    public DateTime? fromregisterdatetime { get; set; }
    public DateTime? toregisterdatetime { get; set; }
    public string title { get; set; }
    public string messagebody { get; set; }
    public bool isneedpaid { get; set; }
    public bool allowtoshow { get; set; }
    public int? commentcount { get; set; }
    public string? photofileurl { get; set; }
    public string? photofilecontenttype { get; set; }
    public bool? issignaltemplate { get; set; }

}
