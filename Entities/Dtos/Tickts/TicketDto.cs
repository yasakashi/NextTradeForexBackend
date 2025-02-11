using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Entities.Dtos;

public class TicketDto : BaseFilterDto
{
    public Guid? Id { get; set; }
    public long? priorityId { get; set; }
    public string? priorityname { get; set; }

    public long? creatoruserId { get; set; }
    public string? creatorusername { get; set; }
    public DateTime? registerdatetime { get; set; }
    public DateTime? responsedatetime { get; set; }
    public string? subject { get; set; }
    public string? textbody { get; set; }
    public string? responsedescription { get; set; }
    public long? responseuserid { get; set; }
    public string? responseusername { get; set; }
    public string? attachmentfileurl { get; set; }
    public string? attachmentfilepath { get; set; }
    public string? attachmentcontexttype { get; set; }
    public string? fileattachmentname { get; set; }

    public bool? isanswerd { get; set; }
}
public class TicketAnswerDto 
{
    public Guid? Id { get; set; }
    public string? responsedescription { get; set; }
    public long? bodyText { get; set; }
    public IFormFile? fileanswer { get; set; }
    public string? responseusername { get; set; }
    public string? attachmentfileurl { get; set; }
    public string? attachmentcontexttype { get; set; }
    public string? fileattachmentname { get; set; }
    public string? attachmentfilepath { get; set; }
}

public class TicketReportDto
{
    public int? newticketsCount { get; set; }
    public int? activeticketsCount { get; set; }
    public int? closedticketsCount { get; set; }
    public int? responsesCount { get; set; }
    public int? interactionsCount { get; set; }
    public int? unassignedTickets { get; set; }
    public int? allTickets { get; set; }
}