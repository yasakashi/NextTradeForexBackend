using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Common.Enums
{
    public enum Hasin_PaymentRequest
    {
        /// <summary>
        /// درخواست جدید یا تکمیل نشده
        /// </summary>
        NewRequest = 0 ,
        /// <summary>
        ///  درخواست پرداخت در مرحله ارسال به فرم پرداخت
        /// </summary>
        SendToPayment = 1 ,
        /// <summary>
        ///  پرداخت با موفقیت انجام شده است
        /// </summary>
        PaymentComplete = 2,
        /// <summary>
        /// پرداخت برگشت خوورده است
        /// </summary>
        PaymentRejectedByUser = 3,
        /// <summary>
        /// پرداخت با خطا مواجه شده است
        /// </summary>
        PaymentRejectedByError = 4,
        /// <summary>
        /// این درخواست پرداخت شده است و بسته شده است
        /// </summary>
        PaymentCompletedAndClosed = 5
       
    }

    public enum Hasin_ReturnActionCode
    {
        /// <summary>
        /// رمز صحیح نمی باشد
        /// </summary>
        WrongPassword = 28,
        /// <summary>
        /// خطای داخلی
        /// </summary>
        InternalError =27,
        /// <summary>
        /// خطای داخلی
        /// </summary>
        Internalerror = 26,
        /// <summary>
        /// کارت غیر فعال می باشد
        /// </summary>
        CardIsDesactive = 39,
        /// <summary>
        /// موجودی کافی نیست
        /// </summary>
        NotEnoughMoney = 29
    }
}
