using AuthorizingAPIs.Dtos;
using AuthorizingAPIs.Services;
using Base.Common.Enums;
using DataLayers;
using Entities.Dtos;
using Newtonsoft.Json;
using RestSharp;
using System.Diagnostics;
using System.Net;

namespace AtlasCoreAPI.Services
{
    public class SMSServices
    {
        private SystemLogServices _systemLogService;
        SBbContext _context;
        private AuthorizationService _authorizationService;

        public SMSServices(SBbContext contex,
                    AuthorizationService authorizationService,
                                 SystemLogServices systemLogService)
        {
            _context = contex;
            _systemLogService = systemLogService;
            _authorizationService = authorizationService;
        }

        public async Task<SystemMessageModel> ActionManagment(object model, int actionid, string hosturl, string clientip, string processId, UserModel usertoken, string _token)
        {
            SystemMessageModel message = new SystemMessageModel();
            StackTrace stackTrace = new StackTrace();
            if (string.IsNullOrEmpty(processId)) processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            string requesturl = string.Empty;
            SMSDto vmodel = null;
            try
            {
                requesturl = "api/sms/send";
                switch (actionid)
                {
                    case 1:
                        try
                        {
                            vmodel = JsonConvert.DeserializeObject<SMSDto>(model.ToString());
                        }
                        catch
                        {
                            vmodel = model as SMSDto;
                        }
                        break;
                    case 2: 
                        SMSRegisterDto vmodel2 = JsonConvert.DeserializeObject<SMSRegisterDto>(model.ToString());
                        vmodel = new SMSDto() { distinationnumber = vmodel2.mobile, smsbody = string.Empty, sourcenumber = string.Empty };
                        break;
                    default:
                        return new SystemMessageModel() { MessageCode = -405, MessageDescription = "درخواست نا معتبر", MessageData = $"action/{actionid}" };
                        break;
                }


                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                RestClientOptions options = new RestClientOptions(ServiceUrlConfig.IOServiceSystem)
                {
                    RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
                };

                RestSharp.RestClient client = new RestSharp.RestClient(options);

                RestSharp.RestRequest request = new RestSharp.RestRequest(requesturl, RestSharp.Method.Post);


                // //Header
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "*/*");

                // Json Body
                if (vmodel != null)
                {
                    request.AddBody(vmodel);
                }
                await _systemLogService.InsertLogs(JsonConvert.SerializeObject(request), processId, clientip, "ActionManagment call " + requesturl, (long)LogTypes.ApiRequest, "", hosturl);


                RestSharp.RestResponse response = client.Execute(request);

                try
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        message = JsonConvert.DeserializeObject<SystemMessageModel>(response.Content);
                    }
                    else
                    {
                        message = new SystemMessageModel() { MessageCode = -401, MessageData = response.ErrorException.Message, MessageDescription = "خطا در انجام درخواست" };
                    }
                }
                catch (Exception ex)
                {
                    string error = $"'RequestRequest':'{requesturl}','ErrorLocation':'{methodpath}','ProccessId':{processId},'ErrorMessage':'{ex.Message}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                    _systemLogService.InsertLogs(error, processId, "", methodpath, LogTypes.SystemError, _token, "");

                    message = new SystemMessageModel() { MessageCode = -401, MessageData = ex.Message, MessageDescription = "خطا در انجام درخواست" };
                }
            }
            catch (Exception ex)
            {
                string error = $"'ErrorLocation':'{methodpath}','ProccessId':{processId},'ErrorMessage':'{ex.Message}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                _systemLogService.InsertLogs(error, processId, "", methodpath, LogTypes.SystemError, "", "");

                message = new SystemMessageModel() { MessageCode = -401, MessageData = ex.Message, MessageDescription = "خطا در انجام درخواست" };
            }
            return message;
        }
    }
}
