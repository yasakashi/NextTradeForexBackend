using Entities.DBEntities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class CommunityGroupGalleryFileDto : BaseFilterDto
{
    public Guid? Id { get; set; }
    public long? userId { get; set; }
    public string? username { get; set; }
    public string? userfulname { get; set; }
    public Guid? galleryId { get; set; }
    public string? galleryname { get; set; }
    public string? fileurl { get; set; }
    public string? fileextention { get; set; }
    public string? filename { get; set; }
    public string? contenttype { get; set; }
    public string? description { get; set; }
    public string? title { get; set; }
    public DateTime? createdatetime { get; set; }
    public IFormFile? galleryfile { get; set; }
}
