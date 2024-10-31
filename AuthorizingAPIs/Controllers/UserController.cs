using NextTradeAPIs.Dtos;
using NextTradeAPIs.Interfaces;
using NextTradeAPIs.Services;
using Base.Common.Enums;
using DocumentFormat.OpenXml.Spreadsheet;
using Entities;
using Entities.Dtos;
using Entities.Systems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using Entities.DBEntities;

namespace NextTradeAPIs.Controllers
{
    /// <summary>
    /// مدیریت کاربران
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        AuthorizationService _authorizationService;
        private IHttpContextAccessor _HttpContextAccessor;
        UserServices _userServices;
        CategoriesServices _userTypeService;
        BaseInformationServices _baseInformationService;
        SystemLogServices _systemLogServices;
        PeopleServices _peopleService;

        public UserController(AuthorizationService authorizationService,
                                       IHttpContextAccessor httpContextAccessor,
                                       SystemLogServices systemLogServices,
                                       CategoriesServices userTypeServices,
                                       PeopleServices peopleServices,
                                       BaseInformationServices baseInformationServices,
                                       UserServices userServices)
        {
            _authorizationService = authorizationService;
            _userServices = userServices;
            _HttpContextAccessor = httpContextAccessor;
            _systemLogServices = systemLogServices;
            _peopleService = peopleServices;
            _userTypeService = userTypeServices;
            _baseInformationService = baseInformationServices;
        }


        [HttpGet]
        [HttpPost]
        [Route("/api/users/getusertypes")]
        public async Task<IActionResult> GetUserTypes()
        {
            StackTrace stackTrace = new StackTrace();
            string processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            string _bearer_token = string.Empty;
            string clientip = string.Empty;
            string hosturl = string.Empty;
            string hostname = string.Empty;
            SystemMessageModel message = new SystemMessageModel();
            long ApiCode = 1000;

            try
            {
                _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
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

                string requestlog = $"'ApiCode':{ApiCode},'tokne':'{_bearer_token}','clientip':'{clientip}','hosturl':'{hosturl}','hostname':'{hostname}'";


                _systemLogServices.InsertLogs(requestlog, processId, clientip, hosturl, (long)LogTypes.ApiRequest);

                //message = await _authorizationService.CheckToken(_bearer_token);

                //if (message.MessageCode < 0)
                //    return Unauthorized(message);

                //UserModel userlogin = message.MessageData as UserModel;

                message = await _baseInformationService.GetUserTypes(null, processId, clientip, hosturl);

                if (message.MessageCode < 0)
                    return BadRequest(message);


                return Ok(message);
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + ApiCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string log = $"'ApiCode':{ApiCode},'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                _systemLogServices.InsertLogs(log, processId, clientip, hosturl, (long)LogTypes.TokenError);
                return BadRequest(message);
            }
        }



        [HttpPost]
        [Route("/api/users/create")]
        public async Task<IActionResult> SaveUsers(UserRegisterModel model)
        {
            StackTrace stackTrace = new StackTrace();
            string processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            string _bearer_token = string.Empty;
            string clientip = string.Empty;
            string hosturl = string.Empty;
            string hostname = string.Empty;
            SystemMessageModel message = new SystemMessageModel();
            long ApiCode = 1000;

            try
            {
                _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
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

                string requestlog = $"'ApiCode':{ApiCode},'tokne':'{_bearer_token}','clientip':'{clientip}','hosturl':'{hosturl}','hostname':'{hostname}','LogDescription':'{JsonConvert.SerializeObject(model)}'";


                _systemLogServices.InsertLogs(requestlog, processId, clientip, hosturl, (long)LogTypes.ApiRequest);

                //message = await _authorizationService.CheckToken(_bearer_token);

                //if (message.MessageCode < 0)
                //    return Unauthorized(message);

                UserModel userlogin = message.MessageData as UserModel;

                message = await _userServices.SaveUser(model, userlogin, processId, clientip, hosturl, _bearer_token);

                if (message.MessageCode < 0)
                    return BadRequest(message);


                return Ok(message);
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + ApiCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string log = $"'ApiCode':{ApiCode},'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                _systemLogServices.InsertLogs(log, processId, clientip, hosturl, (long)LogTypes.TokenError);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// جستحوی و دریافت لیست کاربران
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/users/search")]
        public async Task<IActionResult> GetUsers(UserSearchModel model)
        {
            StackTrace stackTrace = new StackTrace();
            string processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            string _bearer_token = string.Empty;
            string clientip = string.Empty;
            string hosturl = string.Empty;
            string hostname = string.Empty;
            SystemMessageModel message = new SystemMessageModel();
            long ApiCode = 3000;

            try
            {
                _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
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

                string requestlog = $"'tokne':'{_bearer_token}','clientip':'{clientip}','hosturl':'{hosturl}','hostname':'{hostname}','LogDescription':'{JsonConvert.SerializeObject(model)}'";


                _systemLogServices.InsertLogs(requestlog, processId, clientip, hosturl, (long)LogTypes.ApiRequest);

                message = await _authorizationService.CheckToken(_bearer_token, processId);

                if (message.MessageCode < 0)
                    return Unauthorized(message);

                UserModel userlogin = message.MessageData as UserModel;

                if ((userlogin.UserTypeId == (long)UserTypes.Admin) || (userlogin.UserTypeId == (long)UserTypes.SuperAdmin))

                    message = await _userServices.GetUsers(model, userlogin, processId, clientip, hosturl);
                else
                    return Unauthorized(new SystemMessageModel() { MessageCode = -403, MessageDescription = "You do not have permissim for this service" });
                if (message.MessageCode < 0)
                    return BadRequest(message);


                return Ok(message);
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + ApiCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string log = $"'ApiCode':{ApiCode},'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                _systemLogServices.InsertLogs(log, processId, clientip, hosturl, (long)LogTypes.TokenError);
                return BadRequest(message);
            }
        }

        [HttpPost]
        [Route("/api/users/searchperson")]
        public async Task<IActionResult> GetPeople(UserSearchModel model)
        {
            StackTrace stackTrace = new StackTrace();
            string processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            string _bearer_token = string.Empty;
            string clientip = string.Empty;
            string hosturl = string.Empty;
            string hostname = string.Empty;
            SystemMessageModel message = new SystemMessageModel();
            long ApiCode = 4000;
            try
            {
                _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
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

                string requestlog = $"'tokne':'{_bearer_token}','clientip':'{clientip}','hosturl':'{hosturl}','hostname':'{hostname}','LogDescription':'{JsonConvert.SerializeObject(model)}'";


                _systemLogServices.InsertLogs(requestlog, processId, clientip, hosturl, (long)LogTypes.ApiRequest);

                message = await _authorizationService.CheckToken(_bearer_token, processId);

                if (message.MessageCode < 0)
                    return Unauthorized(message);

                UserModel userlogin = message.MessageData as UserModel;

                message = await _peopleService.GetPeople(model, userlogin, processId, clientip, hosturl);

                if (message.MessageCode < 0)
                    return BadRequest(message);


                return Ok(message);
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + ApiCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string log = $"'ApiCode':{ApiCode},'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                _systemLogServices.InsertLogs(log, processId, clientip, hosturl, (long)LogTypes.TokenError);
                return BadRequest(message);
            }
        }


        /// <summary>
        /// دریافت کابر با نام کاربری
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/users/getuserbyusername")]
        public async Task<IActionResult> GetUsersByUsername(UserSearchModel model)
        {
            StackTrace stackTrace = new StackTrace();
            string processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            string _bearer_token = string.Empty;
            string clientip = string.Empty;
            string hosturl = string.Empty;
            string hostname = string.Empty;
            SystemMessageModel message = new SystemMessageModel();
            long ApiCode = 5000;

            try
            {
                _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
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

                string requestlog = $"'tokne':'{_bearer_token}','clientip':'{clientip}','hosturl':'{hosturl}','hostname':'{hostname}','LogDescription':'{JsonConvert.SerializeObject(model)}'";


                _systemLogServices.InsertLogs(requestlog, processId, clientip, hosturl, (long)LogTypes.ApiRequest);

                message = await _authorizationService.CheckToken(_bearer_token);

                if (message.MessageCode < 0)
                    return Unauthorized(message);

                UserModel userlogin = message.MessageData as UserModel;

                message = await _userServices.GetUserByUsername(model.username, userlogin, processId, clientip, hosturl);

                if (message.MessageCode < 0)
                    return BadRequest(message);


                return Ok(message);
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + ApiCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string log = $"'ApiCode':{ApiCode},'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                _systemLogServices.InsertLogs(log, processId, clientip, hosturl, (long)LogTypes.TokenError);
                return BadRequest(message);
            }
        }


        [HttpPost]
        [Route("/api/users/checkkyc")]
        public async Task<IActionResult> KYCUser(UserSearchModel model)
        {
            StackTrace stackTrace = new StackTrace();
            string processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            string _bearer_token = string.Empty;
            string clientip = string.Empty;
            string hosturl = string.Empty;
            string hostname = string.Empty;
            SystemMessageModel message = new SystemMessageModel();
            long ApiCode = 6000;

            try
            {
                _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
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

                string requestlog = $"'tokne':'{_bearer_token}','clientip':'{clientip}','hosturl':'{hosturl}','hostname':'{hostname}','LogDescription':'{JsonConvert.SerializeObject(model)}'";


                _systemLogServices.InsertLogs(requestlog, processId, clientip, hosturl, (long)LogTypes.ApiRequest);

                message = await _authorizationService.CheckToken(_bearer_token);

                if (message.MessageCode < 0)
                    return Unauthorized(message);

                UserModel userlogin = message.MessageData as UserModel;

                message = await _userServices.KYCUserCheck(model, userlogin, processId, clientip, hosturl, _bearer_token);

                if (message.MessageCode < 0)
                    return BadRequest(message);


                return Ok(message);
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + ApiCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string log = $"'ApiCode':{ApiCode},'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                _systemLogServices.InsertLogs(log, processId, clientip, hosturl, (long)LogTypes.TokenError);
                return BadRequest(message);
            }
        }

        [HttpPost]
        [Route("/api/users/checuserkkyc")]
        public async Task<IActionResult> CheckUserInShahkar(UserSearchModel model)
        {
            StackTrace stackTrace = new StackTrace();
            string processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            string _bearer_token = string.Empty;
            string clientip = string.Empty;
            string hosturl = string.Empty;
            string hostname = string.Empty;
            SystemMessageModel message = new SystemMessageModel();
            long ApiCode = 7000;

            try
            {
                _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
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

                string requestlog = $"'tokne':'{_bearer_token}','clientip':'{clientip}','hosturl':'{hosturl}','hostname':'{hostname}','LogDescription':'{JsonConvert.SerializeObject(model)}'";


                _systemLogServices.InsertLogs(requestlog, processId, clientip, hosturl, (long)LogTypes.ApiRequest);

                message = await _authorizationService.CheckToken(_bearer_token);

                if (message.MessageCode < 0)
                    return Unauthorized(message);

                UserModel userlogin = message.MessageData as UserModel;

                message = await _userServices.CheckUserInShahkar(model, userlogin, processId, clientip, hosturl, _bearer_token);

                if (message.MessageCode < 0)
                    return BadRequest(message);


                return Ok(message);
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + ApiCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string log = $"'ApiCode':{ApiCode},'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                _systemLogServices.InsertLogs(log, processId, clientip, hosturl, (long)LogTypes.TokenError);
                return BadRequest(message);
            }
        }

        #region Activate Account
        /// <summary>
        /// بررسی کد فعال سازی ارسال شده
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/users/checkactivattioncode")]
        public async Task<IActionResult> ActivateAccount4mobile(CheckUserActivationCodeDto model)
        {
            StackTrace stackTrace = new StackTrace();
            SystemMessageModel message = new SystemMessageModel();
            string processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            string authHeader = string.Empty;
            string clientip = string.Empty;
            string hosturl = string.Empty;
            string hostname = string.Empty;
            LoginLog loginLog = null;
            long ApiCode = 9000;

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


                message = await _userServices.GetUser4Check(model.mobile, null, "", "", "");
                if (message.MessageCode < 0)
                    return BadRequest(message);

                User user = message.MessageData as User;

                if (user == null)
                {
                    message.MessageCode = ((ServiceUrlConfig.SystemCode + ApiCode + 301) * -1);
                    message.MessageDescription = "کاربر پیدا نشد";
                    message.MessageData = model;

                    return BadRequest(message);
                }
                string contex = JsonConvert.SerializeObject(new ClientInfoModel() { clientname = hostname, clientip = clientip, clinetmacaddress = clientmac, clientosinfo = clinetosinfo });

                //User user = await _userServices.GetUserById(usermodel.UserId, processId,clientip,hosturl);

                if (user.ActiveCodeExpire != null && user.ActiveCodeExpire <= DateTime.Now)
                {
                    return BadRequest(new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + ApiCode + 503) * -1), MessageDescription = "اعتبار کد ارسالی به پایان رسیده است" });
                }

                if (user.ActiveCode == model.activationcode)
                {
                    SystemMessageModel tokenmessage = await GenerateToken(user, contex, clientip, processId, hosturl);

                    if (user.IsActive && user.IsAccepted)
                    {
                        await _userServices.ClearActivation(user.UserId, processId);
                        return Ok(tokenmessage);
                    }

                    message = await _userServices.ActiveAccount(user.UserId, processId);
                    if (message.MessageCode > 0)
                    {
                        return Ok(tokenmessage);
                    }
                }
                else
                {
                    message.MessageCode = ((ServiceUrlConfig.SystemCode + ApiCode + 303) * -1);
                    message.MessageDescription = "کد ارسالی صحیح نمی باشد ";
                    message.MessageData = model;
                    return BadRequest(message);
                }

            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + ApiCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string log = $"'ApiCode':{ApiCode},'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                _systemLogServices.InsertLogs(log, processId, clientip, hosturl, (long)LogTypes.TokenError);
                return BadRequest(message);
            }
            return Ok(message);
        }
        #endregion

        #region Activate Account  From W88
        [HttpPost]
        [Route("/api/users/acceptuserkyc")]
        public async Task<IActionResult> AcceptUserKYC(UserSearchModel model)
        {
            StackTrace stackTrace = new StackTrace();
            SystemMessageModel message = new SystemMessageModel();
            string processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            string authHeader = string.Empty;
            string clientip = string.Empty;
            string hosturl = string.Empty;
            string hostname = string.Empty;
            LoginLog loginLog = null;
            SystemMessageModel tokenmessage = null;
            long ApiCode = 10000;
            try
            {
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
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

                message = await _authorizationService.CheckToken(_bearer_token);

                if (message.MessageCode < 0)
                    return BadRequest(message);

                UserModel tokenuser = message.MessageData as UserModel;

                if (tokenuser == null)
                {
                    message.MessageCode = -1002;
                    message.MessageDescription = "توکن معتبر نمی باشد";
                    message.MessageData = model;

                    return BadRequest(message);
                }
                if (tokenuser.userid != 10001)
                {
                    message.MessageCode = -1003;
                    message.MessageDescription = "سرویس درخواستی وجود ندارد";
                    message.MessageData = model;

                    return BadRequest(message);
                }

                string contex = JsonConvert.SerializeObject(new ClientInfoModel() { clientname = hostname, clientip = clientip, clinetmacaddress = clientmac, clientosinfo = clinetosinfo });

                message = await _userServices.GetUser4Check(model.username, null, "", "", "");
                if (message.MessageCode < 0)
                    return BadRequest(message);

                User user = message.MessageData as User;

                message = await _userServices.AcceptUsrKYC(user.Username, processId);
                if (message.MessageCode > 0)
                {
                    //User user = await _userServices.GetUserById(usermodel.userid, processId,clientip,hosturl);

                    tokenmessage = await GenerateToken(user, contex, clientip, processId, hosturl);


                    return Ok(message);
                }
                return BadRequest(new SystemMessageModel() { MessageCode = -560, MessageData = model, MessageDescription = "خطا در تایید کاربر" });

            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + ApiCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string log = $"'ApiCode':{ApiCode},'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                _systemLogServices.InsertLogs(log, processId, clientip, hosturl, (long)LogTypes.TokenError);
                return BadRequest(message);
            }
            return Ok(message);
        }
        #endregion

        /// <summary>
        /// اصلاح اطلاعات شخص
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/users/update")]
        public async Task<IActionResult> UpdatePeronInfo(UserPersonModel model)
        {
            StackTrace stackTrace = new StackTrace();
            string processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            string _bearer_token = string.Empty;
            string clientip = string.Empty;
            string hosturl = string.Empty;
            string hostname = string.Empty;
            SystemMessageModel message = new SystemMessageModel();
            long ApiCode = 11000;

            try
            {
                _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
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

                string requestlog = $"'tokne':'{_bearer_token}','clientip':'{clientip}','hosturl':'{hosturl}','hostname':'{hostname}','LogDescription':'{JsonConvert.SerializeObject(model)}'";


                _systemLogServices.InsertLogs(requestlog, processId, clientip, hosturl, (long)LogTypes.ApiRequest);

                message = await _authorizationService.CheckToken(_bearer_token);

                if (message.MessageCode < 0)
                    return Unauthorized(message);

                UserModel userlogin = message.MessageData as UserModel;

                message = await _userServices.UpdatePeronInfo(model, userlogin, processId, clientip, hosturl);

                if (message.MessageCode < 0)
                    return BadRequest(message);


                return Ok(message);
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + ApiCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string log = $"'ApiCode':{ApiCode},'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                _systemLogServices.InsertLogs(log, processId, clientip, hosturl, (long)LogTypes.TokenError);
                return BadRequest(message);
            }
        }

        private async Task<SystemMessageModel> GenerateToken(User user, string contex, string clientip, string processId, string hosturl)
        {
            StackTrace stackTrace = new StackTrace();
            SystemMessageModel message = new SystemMessageModel();
            processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            string authHeader = string.Empty;
            string hostname = string.Empty;
            LoginLog loginLog = null;
            long ApiCode = 12000;
            try
            {
                message = await _userServices.InsertLoginLog(new UserModel()
                {
                    userid = user.UserId,
                    username = user.Username,
                    IsActive = user.IsActive,
                    UserTypeId = (user.UserTypeId != null) ? 2 : (long)user.UserTypeId,
                    ParentUserId = user.ParentUserId
                }, clientip, contex, processId, hosturl);

                if (message.MessageCode < 0)
                    return message;

                loginLog = message.MessageData as LoginLog;

                SystemMessageModel messsage = await _authorizationService.GetToken(loginLog, 180, processId);

                if (messsage.MessageCode < 0)
                    return messsage;

                Authentication jwtSecurityToken = messsage.MessageData as Authentication;

                loginLog.Token = jwtSecurityToken.Token;

                messsage = await _userServices.UpdateLoginLog(loginLog, processId);

                if (messsage.MessageCode < 0)
                    return messsage;

                return new SystemMessageModel() { MessageCode = 200, MessageDescription = "ورود با موفقیت انجام شد", MessageData = jwtSecurityToken.Token };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + ApiCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string log = $"'ApiCode':{ApiCode},'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                _systemLogServices.InsertLogs(log, processId, clientip, hosturl, (long)LogTypes.TokenError);
                return message;
            }
        }

        [HttpPost]
        [Route("/api/users/changeuserpass")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangeUserPassword(UserDto model)
        {
            StackTrace stackTrace = new StackTrace();
            SystemMessageModel message;
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
                var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
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

                string requestlog = $"'tokne':'{_bearer_token}','clientip':'{clientip}','hosturl':'{hosturl}','hostname':'{hostname}'";


                _systemLogServices.InsertLogs(requestlog, processId, clientip, hosturl, (long)LogTypes.ApiRequest);

                message = await _authorizationService.CheckToken(_bearer_token, processId);

                if (message.MessageCode < 0)
                    return Unauthorized(message);

                UserModel userlogin = message.MessageData as UserModel;

                message = await _userServices.ChangeUserPassword(model, userlogin, processId, clientip, hosturl);

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
        [HttpGet]
        [Route("/api/users/getrefferalcode")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReferralCode()
        {
            StackTrace stackTrace = new StackTrace();
            SystemMessageModel message;
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
                var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
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

                string requestlog = $"'tokne':'{_bearer_token}','clientip':'{clientip}','hosturl':'{hosturl}','hostname':'{hostname}'";


                _systemLogServices.InsertLogs(requestlog, processId, clientip, hosturl, (long)LogTypes.ApiRequest);

                message = await _authorizationService.CheckToken(_bearer_token, processId);

                if (message.MessageCode < 0)
                    return Unauthorized(message);

                UserModel userlogin = message.MessageData as UserModel;

                message = new SystemMessageModel() { MessageCode = 200, MessageData = new { refferalcode = userlogin.userid }, MessageDescription = "request success" };

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

        /// <summary>
        /// دریافت کابر با نام کاربری
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/users/setactiveordisactive")]
        public async Task<IActionResult> ActiveOrDisActive(UserSearchModel model)
        {
            StackTrace stackTrace = new StackTrace();
            string processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            string _bearer_token = string.Empty;
            string clientip = string.Empty;
            string hosturl = string.Empty;
            string hostname = string.Empty;
            SystemMessageModel message = new SystemMessageModel();
            long ApiCode = 5000;

            try
            {
                _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
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

                string requestlog = $"'tokne':'{_bearer_token}','clientip':'{clientip}','hosturl':'{hosturl}','hostname':'{hostname}','LogDescription':'{JsonConvert.SerializeObject(model)}'";


                _systemLogServices.InsertLogs(requestlog, processId, clientip, hosturl, (long)LogTypes.ApiRequest);

                message = await _authorizationService.CheckToken(_bearer_token);

                if (message.MessageCode < 0)
                    return Unauthorized(message);

                UserModel userlogin = message.MessageData as UserModel;

                if ((userlogin.UserTypeId == (long)UserTypes.Admin) || (userlogin.UserTypeId == (long)UserTypes.SuperAdmin))

                    message = await _userServices.SetUserActiveOrDisactive(model, userlogin, processId, clientip, hosturl);
                else
                    message = new SystemMessageModel() { MessageCode=403,MessageDescription="you have not access"};

                if (message.MessageCode < 0)
                    return BadRequest(message);


                return Ok(message);
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + ApiCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string log = $"'ApiCode':{ApiCode},'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                _systemLogServices.InsertLogs(log, processId, clientip, hosturl, (long)LogTypes.TokenError);
                return BadRequest(message);
            }
        }


        
        [HttpPost]
        [Route("/api/users/activedisactiveuser")]
        public async Task<IActionResult> ChangeUserAccountAtivationStatus(UserSearchModel model)
        {
            StackTrace stackTrace = new StackTrace();
            SystemMessageModel message = new SystemMessageModel();
            string processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            string authHeader = string.Empty;
            string clientip = string.Empty;
            string hosturl = string.Empty;
            string hostname = string.Empty;
            LoginLog loginLog = null;
            SystemMessageModel tokenmessage = null;
            long ApiCode = 10000;
            try
            {
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
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

                message = await _authorizationService.CheckToken(_bearer_token);

                if (message.MessageCode < 0)
                    return BadRequest(message);

                UserModel tokenuser = message.MessageData as UserModel;

                if (tokenuser == null)
                {
                    message.MessageCode = -1002;
                    message.MessageDescription = "توکن معتبر نمی باشد";
                    message.MessageData = model;

                    return BadRequest(message);
                }
                if (tokenuser.userid != 10001)
                {
                    message.MessageCode = -1003;
                    message.MessageDescription = "سرویس درخواستی وجود ندارد";
                    message.MessageData = model;

                    return BadRequest(message);
                }

                string contex = JsonConvert.SerializeObject(new ClientInfoModel() { clientname = hostname, clientip = clientip, clinetmacaddress = clientmac, clientosinfo = clinetosinfo });

                message = await _userServices.GetUser4Check(model.username, null, "", "", "");
                if (message.MessageCode < 0)
                    return BadRequest(message);

                User user = message.MessageData as User;

                message = await _userServices.ChangeUserAccountAtivationStatus(user.Username,user.IsActive, processId);
                if (message.MessageCode > 0)
                {
                    //User user = await _userServices.GetUserById(usermodel.userid, processId,clientip,hosturl);

                    tokenmessage = await GenerateToken(user, contex, clientip, processId, hosturl);


                    return Ok(message);
                }
                return BadRequest(new SystemMessageModel() { MessageCode = -560, MessageData = model, MessageDescription = "خطا در تایید کاربر" });

            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + ApiCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string log = $"'ApiCode':{ApiCode},'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                _systemLogServices.InsertLogs(log, processId, clientip, hosturl, (long)LogTypes.TokenError);
                return BadRequest(message);
            }
            return Ok(message);
        }

        [HttpPost]
        [HttpGet]
        [Route("/api/users/getuserbyrefferalcode")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserListByReferralCode(UserSearchModel model)
        {
            StackTrace stackTrace = new StackTrace();
            SystemMessageModel message;
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
                var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
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

                string requestlog = $"'tokne':'{_bearer_token}','clientip':'{clientip}','hosturl':'{hosturl}','hostname':'{hostname}'";


                _systemLogServices.InsertLogs(requestlog, processId, clientip, hosturl, (long)LogTypes.ApiRequest);

                message = await _authorizationService.CheckToken(_bearer_token, processId);

                if (message.MessageCode < 0)
                    return Unauthorized(message);

                UserModel userlogin = message.MessageData as UserModel;
                message = await _userServices.GetUserListByReferralCode(model, userlogin, processId, clientip, hosturl);

                if (message.MessageCode < 0)
                    return BadRequest(message);

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
        [HttpGet]
        [Route("/api/users/getuserinstructors")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserInstructors(UserPartnerSearchDto model)
        {
            StackTrace stackTrace = new StackTrace();
            SystemMessageModel message;
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
                var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
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

                string requestlog = $"'tokne':'{_bearer_token}','clientip':'{clientip}','hosturl':'{hosturl}','hostname':'{hostname}'";


                _systemLogServices.InsertLogs(requestlog, processId, clientip, hosturl, (long)LogTypes.ApiRequest);

                //message = await _authorizationService.CheckToken(_bearer_token, processId);

                //if (message.MessageCode < 0)
                //    return Unauthorized(message);

                //UserModel userlogin = message.MessageData as UserModel;
                message = await _userServices.GetUserInstructors(model, null, processId, clientip, hosturl);

                if (message.MessageCode < 0)
                    return BadRequest(message);

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
        [HttpGet]
        [Route("/api/users/getuserinstructorslist")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserInstructorslist(UserPartnerSearchDto model)
        {
            StackTrace stackTrace = new StackTrace();
            SystemMessageModel message;
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
                var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
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

                string requestlog = $"'tokne':'{_bearer_token}','clientip':'{clientip}','hosturl':'{hosturl}','hostname':'{hostname}'";


                _systemLogServices.InsertLogs(requestlog, processId, clientip, hosturl, (long)LogTypes.ApiRequest);

                //message = await _authorizationService.CheckToken(_bearer_token, processId);

                //if (message.MessageCode < 0)
                //    return Unauthorized(message);

                //UserModel userlogin = message.MessageData as UserModel;
                message = await _userServices.GetUserInstructorsList(model, null, processId, clientip, hosturl);

                if (message.MessageCode < 0)
                    return BadRequest(message);

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
