using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class WalletTransactionDto
{
    public Guid? Id { get; set; }
    public Guid? sourcewalletId { get; set; }
    public Guid? destiationwalletId { get; set; }
    public decimal transactionamount { get; set; }
    public DateTime? transactiondatetime { get; set; }
    public int? transactiontypeId { get; set; }
    public string? transactiontypename { get; set; }
    public string? transactiondiscription { get; set; }

}
