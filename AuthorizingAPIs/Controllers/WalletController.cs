using Base.Common.Enums;
using Entities.DBEntities;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using NextTradeAPIs.Services;

namespace NextTradeAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]

    public class WalletController : ControllerBase
    {
        AuthorizationService _authorizationService;
        private IHttpContextAccessor _HttpContextAccessor;
        IWebHostEnvironment _webHostEnvironment;
        UserServices _userServices;
        SystemLogServices _systemLogServices;
        BlockedIPServices _blockedIPService;
        CategoriesServices _userTypeService;
        WebinarServices _thisService;

        public WalletController(AuthorizationService authorizationService,
                                       IHttpContextAccessor httpContextAccessor,
                                       IWebHostEnvironment env,
                                       SystemLogServices systemLogServices,
                                       BlockedIPServices blockedIPServices,
                                       CategoriesServices userTypeServices,
                                       WebinarServices webinarBuilderServices,
                                       UserServices userServices)
        {
            _authorizationService = authorizationService;
            _userServices = userServices;
            _HttpContextAccessor = httpContextAccessor;
            _webHostEnvironment = env;
            _systemLogServices = systemLogServices;
            _blockedIPService = blockedIPServices;
            _userTypeService = userTypeServices;
            _thisService = webinarBuilderServices;
        }

        [HttpPost]
        [Route("/api/wallet/adddeposit")]
        [AllowAnonymous]
        public async Task<IActionResult> DepositMoney([FromForm] WebinarDto model)
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

                model.id = Guid.NewGuid();

                var sitePath = _webHostEnvironment.WebRootPath;

                if (model.videoFile != null)
                {

                    if (model.videoFile.FileName == null || model.videoFile.FileName.Length == 0)
                    {
                        // return Content("File not selected");
                    }

                    using (var ms = new MemoryStream())
                    {
                        model.videofilecontenttype = model.videoFile.ContentType;
                        model.videofilename = model.videoFile.FileName;

                        model.videoFile.CopyTo(ms);
                        message = await _thisService.SaveFile(ms.ToArray(), model, userlogin.userid, model.videoFile.FileName, sitePath, hosturl);
                    }
                    if (message.MessageCode < 0)
                        return BadRequest(message);

                    FileActionDto _fileinfo = message.MessageData as FileActionDto;
                    model.videofilepath = _fileinfo.filepath;
                    model.videofileurl = _fileinfo.fileurl;

                    if (message.MessageCode < 0)
                        return BadRequest(message);
                }

                if (model.featuredImage != null)
                {

                    if (model.featuredImage.FileName == null || model.featuredImage.FileName.Length == 0)
                    {
                        // return Content("File not selected");
                    }

                    using (var ms = new MemoryStream())
                    {
                        model.featuredimagecontenttype = model.featuredImage.ContentType;
                        model.featuredimagename = model.featuredImage.FileName;

                        model.featuredImage.CopyTo(ms);
                        message = await _thisService.SaveFile(ms.ToArray(), model, userlogin.userid, model.featuredImage.FileName, sitePath, hosturl);
                    }
                    if (message.MessageCode < 0)
                        return BadRequest(message);

                    FileActionDto _fileinfo = message.MessageData as FileActionDto;
                    model.featuredimagepath = _fileinfo.filepath;
                    model.featuredimageurl = _fileinfo.fileurl;

                    if (message.MessageCode < 0)
                        return BadRequest(message);
                }


                message = await _thisService.AddNewWebinar(model, userlogin, processId, clientip, hosturl);

                if (message.MessageCode < 0)
                    return BadRequest(message);


                return Ok(message);
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
        [HttpDelete]
        [Route("/api/wallet/withdraw")]
        [AllowAnonymous]
        public async Task<IActionResult> Withdraw(WebinarSearchDto model)
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

                var sitePath = _webHostEnvironment.WebRootPath;

                message = await _thisService.RemoveWebinar(model, userlogin, processId, clientip, hosturl);

                if (message.MessageCode < 0)
                    return BadRequest(message);


                return Ok(message);
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
        [Route("/api/wallet/getalltransactions")]
        [AllowAnonymous]
        public async Task<IActionResult> Getwebinars(WebinarSearchDto model)
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

                UserModel userlogin = null;
                if (message.MessageCode > 0)
                { userlogin = message.MessageData as UserModel; }

                var sitePath = _webHostEnvironment.WebRootPath;


                message = await _thisService.GetWebinars(model, userlogin, processId, clientip, hosturl);

                if (message.MessageCode < 0)
                    return BadRequest(message);


                return Ok(message);
            }
            catch (Exception ex)
            {
                string log = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{ex.Message}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                _systemLogServices.InsertLogs(log, processId, clientip, hosturl, LogTypes.TokenError);
                return Unauthorized();
                //return BadRequest(new SystemMessageModel() { MessageCode = -501, MessageDescription = "Error In doing Request", MessageData = ex.Message });
            }
        }

    }
}
