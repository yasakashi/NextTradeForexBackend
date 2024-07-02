using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Entities.DBEntities;
[Table("tblWallets")]
public class Wallet
{
    [Key]
    [Column("Id")]
    public Guid Id { get; set; }
    public long userId { get; set; }
    public decimal walletbalance { get; set; }
    public decimal blockamount { get; set; }
    public virtual User user { get; set; }
}
