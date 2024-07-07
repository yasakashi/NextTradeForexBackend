using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public enum TrasactionTypes : int
{
    Withdraw=2,
    Deposite = 1
}


public static class SitWallet
{
    public static Guid SitWalletId { get { return new Guid("b968862b-732e-43fb-9cef-4b4318e5fdca"); } }
}