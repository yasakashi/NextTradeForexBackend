using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    /// <summary>
    /// مدل مبلغ مسدود شده
    /// </summary>
    public class WalletBlockModel
    {
        /// <summary>
        /// آدرس کیف پول
        /// </summary>
        public string WalletClientID { get; set; }
        /// <summary>
        /// مبلغ مسدود شده
        /// </summary>
        public decimal blockAmount { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public string Description { get; set; }
    }
}
