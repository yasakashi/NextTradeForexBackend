using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class CourseMeetFileDto : BaseFilterDto
{
    public Guid? Id { get; set; }
    public Guid coursemeetId { get; set; }
    public string? coursemeetname { get; set; }
    public byte[]? attachment { get; set; }
    public string? fileextention { get; set; }
    public string? filename { get; set; }
    public string? contenttype { get; set; }
}

public class MeetFileAttachmentDto
{

    public Guid? Id { get; set; }

    public IFormFile? attachtment { get; set; }
}

public class MeetAttachmentFileDto : BaseFilterDto
{
    public Guid? Id { get; set; }
    public byte[]? attachment { get; set; }
    public string? fileextention { get; set; }
    public string? filename { get; set; }
    public string? contenttype { get; set; }
}