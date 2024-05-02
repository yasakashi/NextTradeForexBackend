using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Common.Enums
{
    public enum EnmRequestStatusIs
    {
        /// <summary>
        /// موقت
        /// </summary>
        Temp = 0,
        /// <summary>
        /// در انتظار سوییچ
        /// </summary>
        WaitSwitch = 1,
        /// <summary>
        /// در انتظار شاپرک
        /// </summary>
        WaitShaparak = 2,
        /// <summary>
        /// در جریان شاپرک
        /// </summary>
        FlowinShaparak = 3,
        /// <summary>
        ///  در جریان PSP
        /// </summary>
        FlowInPSP = 4,
        /// <summary>
        /// خطا در شاپرک
        /// </summary>
        ShaparakError = 5,
        /// <summary>
        /// خطا در سوییچ
        /// </summary>
        SwitchError = 6,
        /// <summary>
        /// Rejected
        /// </summary>
        Rejected = 7,
        /// <summary>
        /// تکمیل شده
        /// </summary>
        Complete = 8
    }

    public enum EnmResRequest
    {
        /// <summary>
        /// موفق
        /// </summary>
        Susccess = 0,
        /// <summary>
        /// کد پیگیری موجود نیست
        /// </summary>
        FollowCodeNotExist = 34,
        /// <summary>
        /// متقاضی از نوع پرداخت یار نیست
        /// </summary>
        YouAreNotPardakhtYar = 38,
        /// <summary>
        /// طول کد صنف باید 8 رقم باشد
        /// </summary>
        BussinessCodeError = 39,
        /// <summary>
        /// کد صنف پیدا نشد
        /// </summary>
        CodeError = 40,
        /// <summary>
        /// کلید عمومی موجود نیست
        /// </summary>
        KeyError = 41,
        /// <summary>
        /// متقاضی در لیست سیاه است
        /// </summary>
        InBlackList = 42,
        /// <summary>
        /// درخواست باز وجود دارد
        /// </summary>
        RequestIsOpen = 43
    }
}
