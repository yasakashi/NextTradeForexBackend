using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities;

[Table("tblUserTargetTrainers")]
public class UserTargetTrainer
{
    [Key]
    public long Id { get; set; }
    public long userId { get; set; }
    public int targettrainerId { get; set; }

    #region [ Relations ]
    public virtual User user { get; set; }
    public virtual CourseLevelType targettrainer { get; set; }
    #endregion [ Relations ]
}
