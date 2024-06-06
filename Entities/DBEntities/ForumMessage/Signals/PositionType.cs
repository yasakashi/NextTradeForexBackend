using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBEntities;
[Table("tblPositionTypes")]
public class PositionType
{
    [Key]
    public int Id { get; set; }
    public string name { get; set; }
}
