using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.DBEntities;

/// <summary>
/// مدل ثبت اطلاعات سیستم کاربر برای ورود به سیستم
/// </summary>
[Table("tblClientUserLogin")]
public class ClientUserLogin
{
    /// <summary>
    /// شناسه سیستمی 
    /// </summary>
    [Key]
    public string Id { get; set; }
    /// <summary>
    /// IP کاربر
    /// </summary>
    public string ClientIP { get; set; }
    /// <summary>
    /// شناسه سیستمی کاربر
    /// </summary>
    public long UserId { get; set; }
    /// <summary>
    /// زمان ورود
    /// </summary>
    public DateTime LoginDateTime { get; set; }
    /// <summary>
    /// زمان خروج
    /// </summary>
    public DateTime? LogoutDate { get; set; }

}
