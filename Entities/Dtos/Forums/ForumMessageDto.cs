using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class ForumMessageFilterDto
{
    public Guid? Id { get; set; }
    public Guid? parentId { get; set; }
    public long? categoryid { get; set; }
    public long? creatoruserid { get; set; }
    public string creatorusername { get; set; }
    public DateTime? fromregisterdatetime { get; set; }
    public DateTime? toregisterdatetime { get; set; }
    public long subcategoryid { get; set; }
    public long subcategorygroupid { get; set; }
    public string title { get; set; }
    public string messagebody { get; set; }
    public bool isneedpaid { get; set; }

}
public class ForumMessageDto
{
    public Guid? Id { get; set; }
    public Guid? parentId { get; set; }
    public long? categoryid { get; set; }
    public string? categoryname { get; set; }
    public long? creatoruserid { get; set; }
    public string? creatorusername { get; set; }
    public DateTime? registerdatetime { get; set; }
    public DateTime? fromregisterdatetime { get; set; }
    public DateTime? toregisterdatetime { get; set; }
    public long subcategoryid { get; set; }
    public string subcategoryname { get; set; }
    public long subcategorygroupid { get; set; }
    public string subcategorygroupname { get; set; }
    public string title { get; set; }
    public string messagebody { get; set; }
    public bool isneedpaid { get; set; }
    public bool issignal { get; set; }
    public bool allowtoshow { get; set; }

}
