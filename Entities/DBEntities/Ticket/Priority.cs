using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities;

[Table("tblPriorities")]
public class Priority
{
    [Key]
    [Column("Id")]
    public long Id { get; set; }

    public string name { get; set; }
}
