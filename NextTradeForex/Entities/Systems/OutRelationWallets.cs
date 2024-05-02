using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Systems
{
    [Table("tblOutRelationWallets")]
    public class OutRelationWallet
    {
        /// <summary>
        /// شناسه سیستمی 
        /// </summary>
        public long Id { get; set; }

        //[Encrypted]
        /// <summary>
        /// شناسه کیف پول
        /// </summary>
        public string WalletClientID { get; set; }

        //[Encrypted]
        /// <summary>
        /// شناسه کیف پول خارجی
        /// </summary>
        public string OutWalletAddress { get; set; }

        /// <summary>
        /// کد سیستم خارجی
        /// </summary>
        public long OutSystemId { get; set; }
    }
}
