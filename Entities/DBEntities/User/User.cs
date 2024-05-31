using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.DBEntities;

[Table("tblUsers")]
public class User
{
    /// <summary>
    /// شناسه سیستمی کاربر
    /// </summary>
    [Key]
    [Column("Id")]
    public long UserId { get; set; }
    /// <summary>
    /// شناسه سیستمی کاربر بالا سری
    /// </summary>
    public long? ParentUserId { get; set; }
    /// <summary>
    /// نام کاربری
    /// </summary>
    [Display(Name = "نام کاربری")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(2000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    //[Encrypted]
    public string Username { get; set; }
    /// <summary>
    /// کلمه عبور
    /// </summary>
    [Display(Name = "کلمه عبور")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(2000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    [Encrypted]
    public string Password { get; set; }

    /// <summary>
    /// کد فعال سازی
    /// </summary>
    [Display(Name = "کد فعال سازی")]
    [MaxLength(2000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string? ActiveCode { get; set; }
    /// <summary>
    /// تاریخ اعتبار کد فعال سازی
    /// </summary>
    public DateTime? ActiveCodeExpire { get; set; }
    /// <summary>
    /// کد فعال سازی مخصوص  موبایل
    /// </summary>
    //[Display(Name = " کد فعال سازی مخصوص  موبایل")]
    //[MaxLength(5)]
    //public string? MobileActiovateCode { get; set; }
    /// <summary>
    /// وضعیت فعالیت
    /// </summary>
    [Display(Name = "وضعیت")]
    public bool IsActive { get; set; }
    /// <summary>
    /// وضعیت تایید نهایی
    /// </summary>
    [Display(Name = "تایید نهایی")]
    public bool IsAccepted { get; set; }
    /// <summary>
    /// آواتار
    /// </summary>
    [Display(Name = "آواتار")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string? UserAvatar { get; set; }
    /// <summary>
    /// وضعیت حذف منطقی
    /// </summary>
    public bool IsDelete { get; set; }
    /// <summary>
    /// تاریخ ثبت نام
    /// </summary>
    [Display(Name = "تاریخ ثبت نام")]
    [Column(TypeName = "datetime")]
    public DateTime registerDate { get; set; }
    /// <summary>
    /// ایمیل کاربر
    /// </summary>
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    [Encrypted]
    public string? Email { get; set; }
    /// <summary>
    /// شماره موبایل کاربر
    /// </summary>
    [Display(Name = "شماره موبایل")]
    //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(2000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    [Encrypted]
    public string? Mobile { get; set; }
    /// <summary>
    /// نام کاربر
    /// </summary>
    [Display(Name = "نام")]
    [MaxLength(2000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    [Encrypted]
    public string? Fname { get; set; }
    /// <summary>
    /// نام خانوادگی کاربر
    /// </summary>
    [Display(Name = "نام خانوادگی")]
    [MaxLength(2000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    [Encrypted]
    public string? Lname { get; set; }
    /// <summary>
    /// عکس یا لگوی کاربر
    /// </summary>
    [Display(Name = "عکس یا لگوی کاربر")]
    public byte[]? UserPic { get; set; }

    /// <summary>
    /// شناسه اطلاعات شخص
    /// </summary>
    public long? PersonId { get; set; }
    /// <summary>
    /// نوع کاربر
    /// </summary>
    public long UserTypeId { get; set; }
    public bool ispaied { get; set; }

    #region [ relations ]
    public UserType UserType { get; set; }
    #endregion

}
