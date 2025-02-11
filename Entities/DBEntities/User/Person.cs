using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Entities.DBEntities;

[Table("tblPeople")]
public class Person
{
    /// <summary>
    /// شناسه سیستمی کاربر
    /// </summary>
    [Key]
    [Column("Id")]
    public long PersonId { get; set; }
    [Display(Name = "نام")]
    [MaxLength(2000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    [Encrypted]
    public string? FName { get; set; }
    [Encrypted]
    public string? LName { get; set; }
    [Encrypted]
    public string? Fathername { get; set; }
    //[Display(Name = "کد ملی")]
    //// [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    //[MaxLength(2000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    //[Encrypted]
    //public string? Nationalcode { get; set; }
    ///<Summery>
    /// 1 : اشخاص حقیقی
    /// 2 : اشخاص حقوقی
    ///</Summery>
    [Display(Name = "نوع شخص")]
    public long PersonTypeId { get; set; }
    /// <summary>
    /// وضعیت تاهل
    /// </summary>
    [Display(Name = "وضعیت تاهل")]
    public long? MarriedStatusId { get; set; }
    /// <summary>
    /// جنسیت کاربر
    /// </summary>
    [Display(Name = "جنسیت")]
    public int? Sex { get; set; }
    /// <summary>
    /// تاریخ تولد
    /// </summary>
    [Display(Name = "تاریخ تولد")]
    [Column(TypeName = "Date")]
    public DateTime? BirthDate { get; set; }
    [Encrypted]
    public string? Companyname { get; set; }
    [Display(Name = "شماره موبایل")]
    //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(2000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    [Encrypted]
    public string? Mobile { get; set; }

    public string? nickname { get; set; }
    [Encrypted]
    public string? taxcode { get; set; }
    /// <summary>
    /// تعداد اعضای خانواده
    /// </summary>
    [Display(Name = "تعداد اعضای خانواده")]

    public int? FamilyCount { get; set; }
    /// <summary>
    /// آدرس کاربر
    /// </summary>
    [MaxLength(4900, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    [Encrypted]
    public string? Address { get; set; }
    [Display(Name = "آدرس قانونی براش شخص حقوقی")]
    public string? legaladdress { get; set; }
    [Encrypted]
    [Display(Name = "تلفن")]
    public string? telephone { get; set; }
    [Encrypted]
    [Display(Name = "کد پستی")]
    public string? postalcode { get; set; }
    [Display(Name = "شناسه ملی شرکت")]
    [Encrypted]
    public string? legalNationalCode { get; set; }
    public int? countryId { get; set; }
    public int? stateId { get; set; }
    public int? cityId { get; set; }

    public virtual Country country { get; set; }
    public virtual State state { get; set; }
    public virtual City city { get; set; }
}
