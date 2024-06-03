using NextTradeAPIs.Dtos;
using Base.Common.Enums;
using DataLayers;
using Entities.DBEntities;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace NextTradeAPIs.Services
{
    /// <summary>
    /// سرویس های مربوطه برای بسته بودن
    /// </summary>
    public class BlockedIPServices
    {
        SBbContext _Context { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;

        public BlockedIPServices(SBbContext context, 
                                 IConfiguration config, 
                                 SystemLogServices systemLogServices)
        {
            _Context = context;
            _config = config;
            _systemLogServices = systemLogServices;
        }

        /// <summary>
        /// ثبت IP 
        /// </summary>
        /// <param name="model">نام کاربری</param>
        /// <param name="processId">رمز عبور</param>
        /// <returns></returns>
        public async Task<SystemMessageModel> SaveIPs(BlockedIPModel model, string processId, string clientip, string hosturl, string token)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 30100;

            try
            {
                BlockedIP data = new BlockedIP()
                {
                    BIP = model.bip
                };
                _Context.BlockedIPs.Add(data);

                await _Context.SaveChangesAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = data };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> GetIPs(string processId, string clientip, string hosturl, string token)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 30200;
            List<BlockedIPModel> result = new List<BlockedIPModel>();
            try
            {
                result = await _Context.BlockedIPs.Select(x=> new BlockedIPModel() { 
                    id = x.Id,
                    bip = x.BIP
                }).ToListAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = result };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<bool> IsIPBlocked(string ip, string processId,string token)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 30200;
            List<BlockedIPModel> result = new List<BlockedIPModel>();
            try
            {
                return await _Context.BlockedIPs.Where(x=>x.BIP == ip).AnyAsync();
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, ip, methodpath, LogTypes.SystemError);
                return false;
            }
        }

    }
}
