using Base.Common.Enums;
using DataLayers;
using Entities.DBEntities;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NextTradeAPIs.Dtos;
using System.Diagnostics;

namespace NextTradeAPIs.Services
{
    public class WalletServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;

        public WalletServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
        }

        public async Task<SystemMessageModel> Transfer(WalletTransactionDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                Wallet Source = await _Context.Wallets.FindAsync(model.sourcewalletId);
                Wallet Distiniation = await _Context.Wallets.FindAsync(model.destiationwalletId);

                if (Source == null || Distiniation == null)
                {
                    new SystemMessageModel() { MessageCode = -340, MessageDescription = "Bad request", MessageData = "Wallet not find"};
                }
                
                if(Source.walletbalance < model.transactionamount)
                {
                    new SystemMessageModel() { MessageCode = -340, MessageDescription = "Bad request", MessageData = "lack the money" };
                }

                Source.walletbalance -= model.transactionamount;
                Distiniation.walletbalance += model.transactionamount;

                WalletTransaction Wdata = new WalletTransaction() { 
                    Id = Guid.NewGuid(),
                    walletId = (Guid) model.sourcewalletId,
                    transactionamount = model.transactionamount,
                    transactiondatetime= DateTime.Now,
                    transactiontypeId = (int)TrasactionTypes.Withdraw,
                    transactiondiscription = model.transactiondiscription
                };

                WalletTransaction ddata = new WalletTransaction()
                {
                    Id = Guid.NewGuid(),
                    walletId = (Guid)model.sourcewalletId,
                    transactionamount = model.transactionamount,
                    transactiondatetime = DateTime.Now,
                    transactiontypeId = (int)TrasactionTypes.Deposite,
                    transactiondiscription = model.transactiondiscription
                };

                _Context.Wallets.Update(Source);
                _Context.Wallets.Update(Source);
                await _Context.WalletTransactions.AddAsync(Wdata);
                await _Context.WalletTransactions.AddAsync(ddata);

                await _Context.SaveChangesAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = model };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }
    }
}
