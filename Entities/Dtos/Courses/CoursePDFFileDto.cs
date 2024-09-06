using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class CoursePDFFileDto : BaseFilterDto
{
    public Guid? Id { get; set; }
    public Guid coursepdfId { get; set; }
    public string? coursepdfname { get; set; }
    public byte[]? attachment { get; set; }
    public string? fileextention { get; set; }
    public string? filename { get; set; }
    public string? contenttype { get; set; }
}

public class PDFFileAttachmentDto
{

    public Guid? Id { get; set; }

    public IFormFile? attachtment { get; set; }
}

public class PDFAttachmentFileDto : BaseFilterDto
{
    public Guid? Id { get; set; }
    public byte[]? attachment { get; set; }
    public string? fileextention { get; set; }
    public string? filename { get; set; }
    public string? contenttype { get; set; }
}