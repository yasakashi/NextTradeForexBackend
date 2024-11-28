using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class CategorisDto
{
    public long? Id { get; set; }
    public long? parentId { get; set; }
    public string? name { get; set; }
    public int? categorytypeid { get; set; }
    public string? categorytypename { get; set; }
    public string? Description { get; set; }
    public string? Slug { get; set; }
    public bool? IsVisibleDropdown { get; set; }
    public bool? IsThisTopCategory { get; set; }
    public bool? CoursesOfCategory { get; set; }
    public bool? IsVisible { get; set; }
    public int? categorytypeid_old { get; set; }
    public string? categoryimagefileurl { get; set; }
    public string? categoryimagefilepath { get; set; }
    public string? categoryimagefilecontenttype { get; set; }
    public string? customfileurl { get; set; }
    public string? customfilepath { get; set; }
    public string? customfilename { get; set; }
    public string? customfilecontenttype { get; set; }
}

public class CategoryDto
{
    public long? Id { get; set; }
    public long? parentId { get; set; }
    public string? name { get; set; }
    public int? categorytypeid { get; set; }
    public string? categorytypename { get; set; }
    public string? Description { get; set; }
    public string? Slug { get; set; }
    public bool? IsVisibleDropdown { get; set; }
    public bool? IsThisTopCategory { get; set; }
    public bool? CoursesOfCategory { get; set; }
    public bool? IsVisible { get; set; }
    public int? categorytypeid_old { get; set; }
    public string? categoryimagefileurl { get; set; }
    public string? categoryimagefilepath { get; set; }
    public string? categoryimagefilecontenttype { get; set; }
    public IFormFile? categoryimage { get; set; }
    public IFormFile? customfile { get; set; }
    public string? customfileurl { get; set; }
    public string? customfilepath { get; set; }
    public string? customfilename { get; set; }
    public string? customfilecontenttype { get; set; }
}

public class CategorisTreeDto
{
    public long? Id { get; set; }
    public long? parentId { get; set; }
    public string? name { get; set; }
    public int? categorytypeid { get; set; }
    public virtual List<CategorisTreeDto> children { get; set; }
}

public class CategoryBaseDto 
{
    public long? Id { get; set; }
    public long? parentId { get; set; }
    public string? name { get; set; }
    public int? categorytypeid { get; set; }
    public string? categorytypename { get; set; }
}

public class CategoryFilterDto : BaseFilterDto
{
    public long? Id { get; set; }
    public long? parentId { get; set; }
    public string? name { get; set; }
    public int? categorytypeid { get; set; }
    public bool? isvisible { get; set; }
}