
namespace Entities.Dtos
{
    public class EWalletTransactionModel
    {
        /// <summary>
        /// آدرس کیف پول
        /// </summary>
        public string WalletClinetId { get; set; }
        /// <summary>
        /// مبلغ تراکنش
        /// </summary>
        public decimal TransactionAmount { get; set; }
        /// <summary>
        /// دلیل تراکنش
        /// </summary>
        public long CiCoTypeId { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public string Description { get; set; }
    }
}
