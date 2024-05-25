using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBEntities;

[Table("tblSubCategoryGroups")]
public class SubCategoryGroup
{
    [Key]
    public long Id { get; set; }
    public string name { get; set; }
    public long categotyId { get; set; }
    public long subcategotyId { get; set; }

    public virtual Category category { get; set; }
    public virtual SubCategory subcategory { get; set; }
}
