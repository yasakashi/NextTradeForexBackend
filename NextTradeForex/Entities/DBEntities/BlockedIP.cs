using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    /// <summary>
    /// لیست IP
    /// از سیستم باید قطع شود
    /// </summary>
    [Table("tblBlockedIPs")]
    public class BlockedIP
    {
        [Key]
        public long Id { get; set; }
        /// <summary>
        /// IP
        /// که باید بسته شود
        /// </summary>
        public string BIP { get; set; }
    }
}
