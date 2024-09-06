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

public class CommunityGroupGalleryDto : BaseFilterDto
{
    public Guid? Id { get; set; }
    public long? userId { get; set; }
    public string? username { get; set; }
    public string? userfullname { get; set; }
    public Guid? communitygroupId { get; set; }
    public string? communitygroupname { get; set; }
    public string? description { get; set; }
    public string? galleryname { get; set; }
    public int? gallerytypeId { get; set; }
    public string? gallerytypename { get; set; }
    public string? galleryaccesslevelname { get; set; }
    public int? galleryaccesslevelId { get; set; }
    public DateTime? createdatetime { get; set; }
}
