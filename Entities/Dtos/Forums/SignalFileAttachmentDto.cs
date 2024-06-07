using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class SignalFileAttachmentDto
{
    public Guid? Id { get; set; }
    public Guid? signalId { get; set; }
    public byte[]? attachment { get; set; }
}
