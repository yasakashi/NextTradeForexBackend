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
    public string name { get; set; }


    public virtual List<SubCategory> SubCategories { get; set; }
}
