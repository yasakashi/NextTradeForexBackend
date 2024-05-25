using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBEntities;

[Table("tblSubCategories")]
public class SubCategory
{
    [Key]
    public long Id { get; set; }
    public string name { get; set; }
    public long categotyId { get; set; }

    public virtual Category category { get; set; }
}
