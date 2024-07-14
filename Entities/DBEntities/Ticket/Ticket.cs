using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities;
[Table("tblTickets")]

public class Ticket
{
    [Key]
    [Column("Id")]
    public Guid Id { get; set; }
    public long priorityId { get; set; }

    public long creatoruserId { get; set; }
    public DateTime? registerdatetime { get; set; }
    public DateTime? responsedatetime { get; set; }
    public string? subject { get; set; }
    public string? textbody { get; set; }
    public string? responsedescription { get; set; }
    public long? responseuserid { get; set; }
    public byte[]? fileattachment { get; set; }
    public string? attachmentcontexttype { get; set; }
    public string? fileattachmentname { get; set; }

    public virtual Priority priority { get; set; }
    public virtual User creatoruser { get; set; }
    public virtual User responseuser { get; set; }
}
