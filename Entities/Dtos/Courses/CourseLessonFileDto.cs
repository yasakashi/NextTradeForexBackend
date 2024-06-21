using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class CourseLessonFileDto : BaseFilterDto
{
    public Guid? Id { get; set; }
    public Guid courselessonId { get; set; }
    public string? courselessonname { get; set; }
    public byte[]? attachment { get; set; }
    public string? fileextention { get; set; }
    public string? filename { get; set; }
    public string? contenttype { get; set; }
}

public class FileAttachmentDto
{

    public Guid? Id { get; set; }

    public IFormFile? attachtment { get; set; }
}

