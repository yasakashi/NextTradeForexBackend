using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities;

[Table("tblCategoryTypes")]
public class CategoryType
{
    [Key]
    public int Id { get; set; }
    public string name { get; set; }

    public virtual List<Category> Categorys { get; set; }
}
