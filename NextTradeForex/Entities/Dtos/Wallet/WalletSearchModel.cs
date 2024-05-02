using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    /// <summary>
    ///  برای جستجوی کیف پول
    /// </summary>
    public class WalletSearchModel
    {
        /// <summary>
        /// آدرس کیف پول بالاسسری
        /// </summary>
        public string? ParentWalletClientID { get; set; }
        /// <summary>
        /// آدرس کیف پول
        /// </summary>
        public string? WalletClientID { get; set; }
        /// <summary>
        /// کد کاربر مالک
        /// </summary>
        public string? UserId { get; set; }
        /// <summary>
        /// شماره صفحه گزارش درخواست شده
        /// </summary>
        public int pageindex { get; set; }
        /// <summary>
        /// تداد رکورد در خواست شده در گزارش
        /// </summary>
        public int pagecount { get; set; }
    }
}
