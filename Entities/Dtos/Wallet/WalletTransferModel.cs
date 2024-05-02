
namespace Entities.Dtos
{
    public class WalletTransferModel
    {
        public string SourceWalletClinetId { get; set; }
        public string DistinationWalletClinetId { get; set; }
        public decimal TransactionAmount { get; set; }
        public string Description { get; set; }
    }

}
