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
}


public class CategorisTreeDto
{
    public long? Id { get; set; }
    public long? parentId { get; set; }
    public string? name { get; set; }
    public int? categorytypeid { get; set; }
    public virtual List<CategorisTreeDto> children { get; set; }
}