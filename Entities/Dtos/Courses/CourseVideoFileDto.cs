using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class CourseVideoFileDto : BaseFilterDto
{
    public Guid? Id { get; set; }
    public Guid coursepdfId { get; set; }
    public string? coursevideoname { get; set; }
    public byte[]? attachment { get; set; }
    public string? fileextention { get; set; }
    public string? filename { get; set; }
    public string? contenttype { get; set; }
}

public class VideoFileAttachmentDto
{

    public Guid? Id { get; set; }

    public IFormFile? attachtment { get; set; }
}

public class VideoAttachmentFileDto : BaseFilterDto
{
    public Guid? Id { get; set; }
    public byte[]? attachment { get; set; }
    public string? fileextention { get; set; }
    public string? filename { get; set; }
    public string? contenttype { get; set; }
}