using AuthorizingAPIs.Dtos;
using Base.Common.Enums;
using DataLayers;
using Entities.DBEntities;
using Entities.Dtos;

using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Diagnostics;

namespace AuthorizingAPIs.Services
{
    /// <summary>
    ///  فراخوانی سرویس های مرتبط به کیف پول
    /// </summary>
    public class WalletServices
    {
        private SystemLogServices _systemLogService;
        SBbContext _Context;
        private AuthorizationService _authorizationService;

        public WalletServices(SBbContext contex,
                    AuthorizationService authorizationService,
                                 SystemLogServices systemLogService)
        {
            _Context = contex;
            _systemLogService = systemLogService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// مدیریت سرویس فراخوانی کیف پول
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<SystemMessageModel> ActionManagment(string mobile, string hosturl, string clientip, string processId, UserModel usertoken, string _token)
        {
            SystemMessageModel message = new SystemMessageModel();
            StackTrace stackTrace = new StackTrace();
            if (string.IsNullOrEmpty(processId)) processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            string requesturl = string.Empty;



            requesturl = "api/wallet/create";



            WalletModel model = new WalletModel()
            {
                CurrencyTypeId = (long)CurrencyTypes.IRR,
                IsActived = true,
                WalletTypeId = (long)WalletTypes.Both
            };

            try
            {
                //User user = await _Context.Users.Where(x => x.Mobile == mobile).SingleOrDefaultAsync();
                model.Username = mobile;

                RestSharp.RestClient client = new RestSharp.RestClient(ServiceUrlConfig.EWalletServiceSystem);
                RestSharp.RestRequest request = new RestSharp.RestRequest(requesturl, RestSharp.Method.Post);


                // //Header
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "*/*");
                request.AddHeader("Authorization", string.Format("Bearer {0}", _token));

                // Json Body
                if (model != null)
                {
                    request.AddBody(model, ContentType.Json);
                    //request.AddJsonBody(JsonConvert.SerializeObject(model));
                }

                RestSharp.RestResponse response = client.Execute(request);

                try
                {
                    message = JsonConvert.DeserializeObject<SystemMessageModel>(response.Content);
                    try
                    {
                        message.MessageData = JsonConvert.DeserializeObject<Dictionary<string, object>>(message.MessageData.ToString());
                        if (message.MessageData == null)
                            message.MessageData = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(message.MessageData.ToString());
                    }
                    catch { }
                }
                catch (Exception ex)
                {
                    string error = $"'RequestRequest':'{requesturl}','ErrorLocation':'{methodpath}','ProccessId':{processId},'ErrorMessage':'{ex.Message}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                    _systemLogService.InsertLogs(error, processId, "", methodpath, LogTypes.SystemError, _token, "");

                    message = new SystemMessageModel() { MessageCode = -401, MessageData = _token, MessageDescription = "خطا در انجام درخواست" };
                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = -401, MessageData = ex, MessageDescription = "خطا در انجام درخواست" };
                string error = $"'RequestRequest':'{requesturl}','ErrorLocation':'{methodpath}','ProccessId':{processId},'ErrorMessage':'{ex.Message}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                _systemLogService.InsertLogs(error, processId, "", methodpath, LogTypes.SystemError, _token, "");
            }
            return message;
        }

    }
}
