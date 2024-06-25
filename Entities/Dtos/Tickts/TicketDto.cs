using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public bool? isanswerd { get; set; }
}
