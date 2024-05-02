using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.DBEntities
{
    /// <summary>
    /// مدل ثبت اطلاعات کاربر برای ورود به سیستم
    /// </summary>
    [Table("tblLoginLogs")]
    public class LoginLog
    {
        /// <summary>
        /// شناسه سیستمی 
        /// </summary>
        [Key]
        [Column("Id")]
        public Guid LoginLogId { get; set; }
        /// <summary>
        /// شناسه سیستمی کاربر
        /// </summary>
        [Display(Name = "کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long? UserId { get; set; }
        /// <summary>
        /// کلمه کاربری
        /// </summary>
        [Display(Name = "کلمه کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Username{ get; set; }
        /// <summary>
        /// تاریخ ورود
        /// </summary>
        [Display(Name = "تاریخ ورود")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public DateTime LoginDate { get; set; }
        /// <summary>
        /// وضعیت ورود 
        /// </summary>
        [Display(Name = "وضعیت ورود")]
        public bool LoginIsSuccessfull { get; set; }
        /// <summary>
        /// IP سیستم کاربر در هنگام ورود
        /// </summary>
        [Display(Name = "IP")]
        [MaxLength(32)]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ClientIp { get; set; }

        [Display(Name = "clinetnsesstioninfo")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string SesstionInfo { get; set; }
        /// <summary>
        /// رمز ارسالی جهت احراز هویت
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// زمان خروج
        /// </summary>
        [Display(Name = "زمان خروج")]
        public DateTime? LogoutDate { get; set; }
        /// <summary>
        /// تاریخ انقضاء ورود کاربر
        /// </summary>
        public long Expiretime { get; set; }

    }
}
