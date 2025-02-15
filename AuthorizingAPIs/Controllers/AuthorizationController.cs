using NextTradeAPIs.Dtos;
using NextTradeAPIs.Services;
using Azure.Core;
using Base.Common.Enums;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Reflection.Emit;
using System.ServiceModel;
using System.Text;
using Entities.DBEntities;

namespace NextTradeAPIs
{
    /// <summary>
    /// ایجاد و بررسی توکن برای مدیریت دسترسی به سیستم
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class AuthorizationController : ControllerBase
    {
        AuthorizationService _authorizationService;
        private IHttpContextAccessor _HttpContextAccessor;
        UserServices _userServices;
        SystemLogServices _systemLogServices;
        BlockedIPServices _blockedIPService;

        public AuthorizationController(AuthorizationService authorizationService,
                                       IHttpContextAccessor httpContextAccessor,
                                       SystemLogServices systemLogServices,
                                       BlockedIPServices blockedIPServices,
                                       UserServices userServices)
        {
            _authorizationService = authorizationService;
            _userServices = userServices;
            _HttpContextAccessor = httpContextAccessor;
            _systemLogServices = systemLogServices;
            _blockedIPService = blockedIPServices;
        }

        [HttpPost]
        [Route("/api/gettoken")]
        [AllowAnonymous]
        public async Task<IActionResult> GetToken()
        {
            StackTrace stackTrace = new StackTrace();
            string processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            string authHeader = string.Empty;
            string clientip = string.Empty;
            string hosturl = string.Empty;
            string hostname = string.Empty;
            UserModel user = null;
            LoginLog loginLog = null;

            long ApiCode = 2000;

            try
            {
                authHeader = this.HttpContext.Request.Headers["Authorization"];
                clientip = _HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                hosturl = ((Request.IsHttps) ? "https" : "http") + @"://" + Request.Host.ToString();

                try
                {
                    hostname = Dns.GetHostEntry(HttpContext.Connection.RemoteIpAddress).HostName;
                }
                catch
                {
                    hostname = HttpContext.Connection.RemoteIpAddress.ToString();
                }

                string clientmac = NetworkFunctions.GetClientMAC(clientip);

                string clinetosinfo = _HttpContextAccessor.HttpContext.Request.Headers["User-Agent"];

                string requestlog = $"'authHeader':'{authHeader}','clientip':'{clientip}','hosturl':'{hosturl}','hostname':'{hostname}'";


                _systemLogServices.InsertLogs(requestlog, processId, clientip, hosturl, LogTypes.GetToken);


                if (string.IsNullOrEmpty(authHeader) || authHeader.Trim().Length == 0)
                    return NotFound();

                Encoding encoding = Encoding.GetEncoding("iso-8859-1");


                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();

                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

                int seperatorIndex = usernamePassword.IndexOf(':');

                string username = usernamePassword.Substring(0, seperatorIndex);
                string password = usernamePassword.Substring(seperatorIndex + 1);

                string contex = JsonConvert.SerializeObject(new ClientInfoModel() { clientname = hostname, clientip = clientip, clinetmacaddress = clientmac, clientosinfo = clinetosinfo });

                SystemMessageModel message = await _userServices.GetUser(username, password, processId, clientip, hosturl);
                if (message.MessageCode < 0)
                    return BadRequest(message);

                user = message.MessageData as UserModel;

                if (user == null)
                {
                    message = await _userServices.InsertLoginLog(username, clientip, contex, processId, hosturl);
                    return NotFound();
                }

                message = await _userServices.InsertLoginLog(user, clientip, contex, processId, hosturl);

                if (message.MessageCode < 0)
                    return BadRequest(message);

                loginLog = message.MessageData as LoginLog;

                if (string.IsNullOrEmpty(loginLog.Token))
                {
                    SystemMessageModel messsage = await _authorizationService.GetToken(loginLog, 180, processId);

                    if (messsage.MessageCode < 0)
                        return BadRequest(messsage);

                    Authentication jwtSecurityToken = messsage.MessageData as Authentication;

                    loginLog.Token = jwtSecurityToken.Token;

                    messsage = await _userServices.UpdateLoginLog(loginLog, processId);

                    if (messsage.MessageCode < 0)
                        return BadRequest(messsage);
                }
                return Ok(new SystemMessageModel() { MessageCode = 200, MessageDescription = "ورود با موفقیت انجام شد", MessageData = loginLog.Token });
            }
            catch (Exception ex)
            {
                string log = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{ex.Message}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                _systemLogServices.InsertLogs(log, processId, clientip, hosturl, LogTypes.TokenError);
                return Unauthorized();
                //return BadRequest(new SystemMessageModel() { MessageCode = -501, MessageDescription = "Error In doing Request", MessageData = ex.Message });
            }
        }

        [HttpPost]
        [Route("/api/checktoken")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckToken(string clientip = "")
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            string hosturl = string.Empty;
            long ApiCode = 3000;
            try
            {
                var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(clientip))
                    clientip = _HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

                /// بررسی IP 
                if (await _blockedIPService.IsIPBlocked(clientip, processId, _bearer_token))
                {
                    return BadRequest(new SystemMessageModel() {MessageCode = ((ServiceUrlConfig.SystemCode + ApiCode + 501) * -1) , MessageDescription="دسترسی برای شما بسته شده است"} );
                }
                hosturl = ((Request.IsHttps) ? "https" : "http") + @"://" + Request.Host.ToString();


                message = await _authorizationService.CheckToken(_bearer_token);

                if (message.MessageCode < 0)
                    return BadRequest(message);
                else
                    return Ok(message);
            }
            catch (Exception ex)
            {
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{ex.Message}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                _systemLogServices.InsertLogs(error, processId, clientip, hosturl, LogTypes.SystemError);

                message = new SystemMessageModel() { MessageCode = -501, MessageDescription = "Error In doing Request", MessageData = $"'ProccessID':'{processId}','ErrorMessage':'{ex.Message}'" };
                return BadRequest(message);
            }

        }

        [HttpPost]
        [Route("/api/getuserfromtoken")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserFromToken()
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            string clientip = string.Empty;
            string hosturl = string.Empty;

            try
            {
                clientip = _HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                hosturl = ((Request.IsHttps) ? "https" : "http") + @"://" + Request.Host.ToString();

                var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

                message = await _authorizationService.GetUserToken(_bearer_token);

                if (message.MessageCode < 0)
                    return BadRequest(message);
                else
                    return Ok(message);
            }
            catch (Exception ex)
            {
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{ex.Message}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                _systemLogServices.InsertLogs(error, processId, clientip, hosturl, LogTypes.SystemError);

                message = new SystemMessageModel() { MessageCode = -501, MessageDescription = "Error In doing Request", MessageData = $"'ProccessID':'{processId}','ErrorMessage':'{ex.Message}'" };
                return BadRequest(message);
            }

        }

        [HttpPost]
        [Route("/api/signout")]
        [AllowAnonymous]
        public async Task<IActionResult> SignOut(string clientip = "")
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            string hosturl = string.Empty;
            long ApiCode = 3000;
            try
            {
                var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(clientip))
                    clientip = _HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

                /// بررسی IP 
                //if (await _blockedIPService.IsIPBlocked(clientip, processId, _bearer_token))
                //{
                //    return BadRequest(new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + ApiCode + 501) * -1), MessageDescription = "دسترسی برای شما بسته شده است" });
                //}
                hosturl = ((Request.IsHttps) ? "https" : "http") + @"://" + Request.Host.ToString();


                message = await _authorizationService.SigOut(_bearer_token);

                if (message.MessageCode < 0)
                    return BadRequest(message);
                else
                    return Ok(message);
            }
            catch (Exception ex)
            {
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{ex.Message}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                _systemLogServices.InsertLogs(error, processId, clientip, hosturl, LogTypes.SystemError);

                message = new SystemMessageModel() { MessageCode = -501, MessageDescription = "Error In doing Request", MessageData = $"'ProccessID':'{processId}','ErrorMessage':'{ex.Message}'" };
                return BadRequest(message);
            }

        }

        [HttpPost]
        [Route("/api/adminpanel/userloginhistory")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserLoginHistory()
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            string hosturl = string.Empty;
            string clientip = string.Empty;
            long ApiCode = 3000;
            try
            {
                clientip = _HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                hosturl = ((Request.IsHttps) ? "https" : "http") + @"://" + Request.Host.ToString();
                var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(clientip))
                    clientip = _HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

                /// بررسی IP 
                if (await _blockedIPService.IsIPBlocked(clientip, processId, _bearer_token))
                {
                    return BadRequest(new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + ApiCode + 501) * -1), MessageDescription = "دسترسی برای شما بسته شده است" });
                }
                hosturl = ((Request.IsHttps) ? "https" : "http") + @"://" + Request.Host.ToString();


                message = await _authorizationService.GetUserLoginHistory(_bearer_token, processId);

                if (message.MessageCode < 0)
                    return BadRequest(message);
                else
                    return Ok(message);
            }
            catch (Exception ex)
            {
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{ex.Message}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                _systemLogServices.InsertLogs(error, processId, clientip, hosturl, LogTypes.SystemError);

                message = new SystemMessageModel() { MessageCode = -501, MessageDescription = "Error In doing Request", MessageData = $"'ProccessID':'{processId}','ErrorMessage':'{ex.Message}'" };
                return BadRequest(message);
            }

        }

    }
}
