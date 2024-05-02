using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Common.Enums
{

    /// <summary>
    /// نوع جنسیت مربوط به شاپرک
    /// </summary>
    public enum SE_Gender
    {
        Man = 1,
        Famale = 0
    }
    /// <summary>
    /// نوع اشخاص مربوط به شاپرک
    /// </summary>
    public enum SE_MerchantType
    {
        /// <summary>
        /// حقیقی
        /// </summary>
        NormalPerson = 0,
        /// <summary>
        /// حقوقی
        /// </summary>
        Legal = 1
    }
    /// <summary>
    /// نوع تابعیت در شاپرک
    /// </summary>
    public enum SE_ResidencyType
    {
        /// <summary>
        /// ایرانی
        /// </summary>
        Irani = 0,
        /// <summary>
        /// غیر ایرانی
        /// </summary>
        NoneIrani = 1
    }
    /// <summary>
    /// وضعیت حیات در شاپرک
    /// </summary>
    public enum SE_VitalStatus
    {
        /// <summary>
        /// در غید حیات
        /// </summary>
        alive = 0,
        /// <summary>
        /// فوت شده
        /// </summary>
        Dead = 1
    }
    /// <summary>
    /// نوع فروشگاه
    /// </summary>
    public enum SE_BusinessType
    {
        /// <summary>
        /// فقط فیزیکی
        /// </summary>
        JustPhysical = 0,
        /// <summary>
        /// فیزیکی و مجازی
        /// </summary>
        PhysicalAndVirtual = 1,
        /// <summary>
        /// فقط مجازی
        /// </summary>
        JustVirtual = 2
    }
    /// <summary>
    /// نوع درخواست  یا سرویس به شاپرک
    /// </summary>
    public enum sh_RequestType
    {
        /// <summary>
        /// ایجاد ترمینال جدید
        /// </summary>
        NewTerminal = 5,
        /// <summary>
        /// تغییر آدرس شبا
        /// </summary>
        ChangeShaba = 6,
        /// <summary>
        /// غیر فعال کردن ترمینال
        /// </summary>
        DisactiveTerminal = 7,
        /// <summary>
        /// ثبت مشتری یا فروشگاه
        /// </summary>
        NewCustomer = 13,
        /// <summary>
        /// تغییر اطلاعات اطلاعات مشتری یا فروشگاه
        /// </summary>
        EditData = 14,
        /// <summary>
        /// تغییر آدرس فروشگاه یا مشتری
        /// </summary>
        ChangeAddress = 17,
        /// <summary>
        /// فعال سازی مجدد ترمینال
        /// </summary>
        ReactiveTerminal = 18
    }
    /// <summary>
    /// نوع مالکیت ملک
    /// </summary>
    public enum SE_OwnershipType
    {
        /// <summary>
        /// صاحب مالک
        /// </summary>
        Owner = 0,
        /// <summary>
        /// مستاجر
        /// </summary>
        tenant = 1
    }
    /// <summary>
    /// نوع ترمینال
    /// </summary>
    public enum SE_TerminalType
    {
        /// <summary>
        /// ترمینال روی میزی
        /// </summary>
        OnDeskTerminal = 0,
        /// <summary>
        /// درگاه اینترنتی
        /// </summary>
        OnInternetTerminal = 1,
        /// <summary>
        /// درگاه پرداخت موبایلی
        /// </summary>
        OnMobileTerminal = 2,
        /// <summary>
        /// پایانه فروش  بی سیم
        /// </summary>
        WirelessTerminal = 3
    }
    /// <summary>
    /// نوع نماد الکترونیک
    /// </summary>
    public enum SE_ElectonicSign
    {
        /// <summary>
        /// یک ستاره
        /// </summary>
        OneStar = 0,
        /// <summary>
        /// دو ستاره
        /// </summary>
        DuobleStar = 1
    }
    /// <summary>
    /// امکان تقسیم وجه
    /// </summary>
    public enum SE_AllowScatteredSettlement
    { 
        NotAllowed=0,
        InOneBank=1,
        InMultyBank=2
    }
    public enum SE_EtrustCertificateType
    {
        onstar
    }
    public enum SE_UpdateAction
    { 
        Edit=0,
        Delete =1,
        NoChange=2
    }

}
