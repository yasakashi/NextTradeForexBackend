using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    /// <summary>
    /// مدل  نمایش اطلاعات کیف پول
    /// </summary>
    public class WalletViewModel
    {
        /// <summary>
        /// شناسه کیف پول
        /// </summary>
        public string WalletClientID { get; set; }
        /// <summary>
        /// شناسه سیستمی کیف پدر
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// نوع ارز موجود
        /// </summary>
        public long CurrencyTypeId { get; set; }
        /// <summary>
        /// نام نوع ارز موجود
        /// </summary>
        public string CurrencyTypeName { get; set; }
        /// <summary>
        /// نوع کیف پول
        /// </summary>
        public long WalletTypeId { get; set; }
        /// <summary>
        /// نام نوع کیف پول
        /// </summary>
        public string WalletTypeName { get; set; }
        /// <summary>
        /// کد کاربر مالک
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// تعداد تراکنش در روز
        /// </summary>
        public int DailyTransactionLimitation { get; set; }
        /// <summary>
        /// حداکثر مبلغ برداشت
        /// </summary>
        public decimal MaxDailyCachoutLimitation { get; set; }
        /// <summary>
        /// تایین وضیعت فعالیت
        /// </summary>
        public bool? IsActived { get; set; }
        /// <summary>
        /// مبلغ موجودی
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// مبلغ مسدودی
        /// </summary>
        public decimal BlockedAmount { get; set; }
        /// <summary>
        /// مبلغ قابل تراکنش
        /// </summary>
        public decimal AvailableAmount { get; set; }
    }



}
