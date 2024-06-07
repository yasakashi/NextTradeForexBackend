using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBEntities;

[Table("tblSignalFileAttachments")]
public class SignalFileAttachment
{
    [Key]
    public Guid Id { get; set; }
    public Guid signalId { get; set; }
    public byte[]? attachment { get; set; }

    public virtual Signal Signal { get; set; }
}
