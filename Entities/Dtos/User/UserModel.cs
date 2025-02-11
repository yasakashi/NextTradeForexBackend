using Entities.DBEntities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Entities.Dtos;

/// <summary>
/// مدل کاربر
/// </summary>
public class UserModel
{
    public Guid loginlogid { get; set; }
    public string organizename { get; set; }
    /// <summary>
    /// کد کاربر
    /// </summary>
    public long userid { get; set; }
    /// <summary>
    /// نام کاربری
    /// </summary>
    public string username { get; set; }
    public string fname { get; set; }
    public string lname { get; set; }
    public string nationalcode { get; set; }
    public string email { get; set; }
    public bool iskyc { get; set; }
    public bool ispaid { get; set; }
    public bool IsActive { get; set; }
    public long? ParentUserId { get; set; }
    public long UserTypeId { get; set; }
    public string? UserTypeName { get; set; }
    public DateTime? registerDate { get; set; }
    public int? activeSubscriber { get; set; }
    public int? posts { get; set; }
    public int? forumRoleId { get; set; }

    public string? forumRoleName { get; set; }
    public string? financialinstrumentIds { get; set; }
    public string? forexexperiencelevelId { get; set; }
    public string? trainingmethodIds { get; set; }
    public string? targettrainerIds { get; set; }
    public int? interestforexId { get; set; }
    public bool? hobbyoftradingfulltime { get; set; }
    public int? EmployeeLevel { get; set; }
    public int? pagecount { get; set; }
    public decimal? walletbalance { get; set; }
    public bool? isuserroyality { get; set; }
    public List<TrainingMethodDto>? usertrainingmethods { get; set; }
    public List<FinancialInstrumentDto>? userfinancialinstruments { get; set; }
    public List<CourseLevelTypeDto>? usertargettrainers { get; set; }

}


public class UserRegisterWithMobileMOdel
{
    public string mobile { get; set; }
}

public class UserRegisterModel
{
    /// <summary>
    /// شناسه سیستمی کاربر
    /// </summary>
    public long? userid { get; set; }
    /// <summary>
    /// نام کاربری
    /// </summary>
    public string username { get; set; }
    /// <summary>
    /// کلمه عبور
    /// </summary>
    public string Password { get; set; }
    /// <summary>
    /// آدرس کاربر
    /// </summary>
    [MaxLength(4900, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string? Address { get; set; }
    /// <summary>
    /// ایمیل کاربر
    /// </summary>
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string? Email { get; set; }
    /// <summary>
    /// شماره موبایل کاربر
    /// </summary>
    [MaxLength(2000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    //[Encrypted]
    public string? Mobile { get; set; }
    /// <summary>
    /// کد ملی کاربر
    /// </summary>
    [MaxLength(2000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string? Nationalcode { get; set; }
    /// <summary>
    /// نام کاربر
    /// </summary>
    [Display(Name = "نام")]
    [MaxLength(2000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string? Fname { get; set; }
    /// <summary>
    /// نام خانوادگی کاربر
    /// </summary>
    [Display(Name = "نام خانوادگی")]
    [MaxLength(2000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string? Lname { get; set; }
    /// <summary>
    /// تاریخ تولد
    /// </summary>
    [Display(Name = "تاریخ تولد")]
    [Column(TypeName = "Date")]
    public DateTime? BirthDate { get; set; }
    /// <summary>
    /// جنسیت کاربر
    /// </summary>
    [Display(Name = "جنسیت")]
    public int? Sex { get; set; }
    ///<Summery>
    /// 1 : اشخاص حقیقی
    /// 2 : اشخاص حقوقی
    ///</Summery>
    [Display(Name = "نوع شخص")]
    public long PersonTypeId { get; set; }
    ///<Summery>
    /// 1 : اشخاص حقیقی
    /// 2 : اشخاص حقوقی
    ///</Summery>
    [Display(Name = "نوع کاربر")]
    public long UserTypeId { get; set; }

    /// <summary>
    /// عکس یا لگوی کاربر
    /// </summary>
    [Display(Name = "عکس یا لگوی کاربر")]
    public byte[]? UserPic { get; set; }
    public int? cityId { get; set; }
    public int? countryId { get; set; }
    public int? stateId { get; set; }
    public string? fathername { get; set; }
    public long? marriedstatusid { get; set; }
    public string? companyname { get; set; }
    public string? taxcode { get; set; }
    public int? familycount { get; set; }
    public string? legaladdress { get; set; }
    public string? telephone { get; set; }
    public string? postalcode { get; set; }
    public string? legalnationalcode { get; set; }
    public long? parentuserId { get; set; }
    public List<int>? financialinstrumentIds { get; set; }
    public int? forexexperiencelevelId { get; set; }
    public List<int>? trainingmethodIds { get; set; }
    public List<int>? targettrainerIds { get; set; }
    public int? interestforexId { get; set; }
    public bool? hobbyoftradingfulltime { get; set; }
    public bool? sendNotification { get; set; }
    public string? website { get; set; }
    public string? language { get; set; }
    public int? forumRoleId { get; set; }
}

public class UserReferralModel
{
    public long? id { get; set; }
    public string? username { get; set; }
    public int? EmployeeLevel { get; set; }
    public string? fname { get; set; }
    public string? lname { get; set; }
    public bool? ispaid { get; set; }
    public bool? IsActive { get; set; }
    public long? UserTypeId { get; set; }
    public long? ParentUserId { get; set; }
    public string? email { get; set; }
    public string? referalcode { get; set; }
}

public class AdminUserModel
{
    public string username { get; set; }
    public string email { get; set; }
    public string fname { get; set; }
    public string lname { get; set; }
    public string? website { get; set; }
    public string? bio { get; set; }
    public string? mobile { get; set; }
    public string? language { get; set; }
    public string password { get; set; }
    public bool? sendNotification { get; set; }
    public long usertypeid { get; set; }
    public int? forumRoleId { get; set; }
    public string? jobtitle { get; set; }
}