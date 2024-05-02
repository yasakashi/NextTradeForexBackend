using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    /// <summary>
    /// مدل کیف پول
    /// </summary>
    public class WalletModel
    {
        /// <summary>
        /// شناسه کیف پول
        /// </summary>
        public string? WalletClientID { get; set; }
        /// <summary>
        /// شناسه سیستمی کیف پدر
        /// </summary>
        public string? ParentId { get; set; }
        /// <summary>
        /// نوع ارز موجود
        /// </summary>
        public long? CurrencyTypeId { get; set; }
        /// <summary>
        /// نوع کیف پول
        /// </summary>
        public long? WalletTypeId { get; set; }
        /// <summary>
        /// کلمه کاربری مالک
        /// </summary>
        public string? Username { get; set; }
        /// <summary>
        /// تعداد تراکنش در روز
        /// </summary>
        public int? DailyTransactionLimitation { get; set; }
        /// <summary>
        /// حداکثر مبلغ برداشت
        /// </summary>
        public decimal? MaxDailyCachoutLimitation { get; set; }
        /// <summary>
        /// تایین وضیعت فعالیت
        /// </summary>
        public bool? IsActived { get; set; }
    }



}
