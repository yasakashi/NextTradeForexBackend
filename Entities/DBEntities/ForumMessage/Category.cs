using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBEntities;

[Table("tblCategories")]
public class Category
{
    [Key]
    public long Id { get; set; }
    public long? parentId { get; set; }
    public string name { get; set; }
    public int? categorytypeid { get; set; }
    public string? Description { get; set; }
    public string? Slug { get; set; }
    public bool? IsVisibleDropdown { get; set; }
    public bool? IsThisTopCategory { get; set; }
    public bool? CoursesOfCategory { get; set; }
    public bool? IsVisible { get; set; }
    public int? categorytypeid_old { get; set; }
    public byte[] CategoryImage { get; set; }
    public string? categoryimagefileurl { get; set; }
    public string? categoryimagefilepath { get; set; }


    //public virtual List<SubCategory> SubCategories { get; set; }
    public virtual List<ForumMessage> ForumMessages { get; set; }
    public virtual CategoryType categorytype { get; set; }

}

