using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entities.DBEntities;

[Table("tblWalletTransactions")]

public class WalletTransaction
{
    [Key]
    [Column("Id")]
    public Guid Id { get; set; }
    public Guid walletId { get; set; }
    public decimal transactionamount { get; set; }
    public DateTime? transactiondatetime { get; set; }
    public int transactiontypeId { get; set; }

    public virtual  Wallet wallet { get; set; }
    public virtual TransactionType transactiontype { get; set; }
}
