using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.DBEntities;

[Table("tblUserRoles")]
public class UserRole
{
    public UserRole()
    {

    }
    /// <summary>
    /// شناسه سیستمی
    /// </summary>
    [Key]
    public long Id { get; set; }
    /// <summary>
    /// شناسه سیستمی کاربر
    /// </summary>
    public long UserId { get; set; }
    /// <summary>
    /// شناسه سیستمی نقش
    /// </summary>
    public long RoleId { get; set; }
    public bool IsActived { get; set; }


    #region Relations

    public virtual User User { get; set; }
    public virtual Role Role { get; set; }

    #endregion

}
