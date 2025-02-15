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

    public class FeedBackController : ControllerBase
    {
        AuthorizationService _authorizationService;
        private IHttpContextAccessor _HttpContextAccessor;
        IWebHostEnvironment _webHostEnvironment;
        UserServices _userServices;
        SystemLogServices _systemLogServices;
        BlockedIPServices _blockedIPService;
        CategoriesServices _userTypeService;
        FeedBackServices _thisService;

        public FeedBackController(AuthorizationService authorizationService,
                                       IHttpContextAccessor httpContextAccessor,
                                       IWebHostEnvironment env,
                                       SystemLogServices systemLogServices,
                                       BlockedIPServices blockedIPServices,
                                       CategoriesServices userTypeServices,
                                       FeedBackServices courseBuilderServices,
                                       UserServices userServices)
        {
            _authorizationService = authorizationService;
            _userServices = userServices;
            _HttpContextAccessor = httpContextAccessor;
            _webHostEnvironment = env;
            _systemLogServices = systemLogServices;
            _blockedIPService = blockedIPServices;
            _userTypeService = userTypeServices;
            _thisService = courseBuilderServices;
        }
        [HttpPost]
        [Route("/api/adminpanel/addnewfeedback")]
        [AllowAnonymous]
        public async Task<IActionResult> InsertFeedBack([FromForm] FeedBackDto model)
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

                if (model.feedbackfile != null)
                {

                    if (model.feedbackfile.FileName == null || model.feedbackfile.FileName.Length == 0)
                    {
                        // return Content("File not selected");
                    }

                    using (var ms = new MemoryStream())
                    {
                        model.feedbackfilecontenttype = model.feedbackfile.ContentType;
                        model.feedbackfilename = model.feedbackfile.FileName;

                        model.feedbackfile.CopyTo(ms);
                        message = await _thisService.SaveFile(ms.ToArray(), model, userlogin.userid, model.feedbackfile.FileName, sitePath, hosturl);
                    }
                    if (message.MessageCode < 0)
                        return BadRequest(message);

                    FileActionDto _fileinfo = message.MessageData as FileActionDto;
                    model.feedbackfilepath = _fileinfo.filepath;
                    model.feedbackfileurl = _fileinfo.fileurl;

                    if (message.MessageCode < 0)
                        return BadRequest(message);
                }

                if (model.featuredimagefile != null)
                {

                    if (model.featuredimagefile.FileName == null || model.featuredimagefile.FileName.Length == 0)
                    {
                        // return Content("File not selected");
                    }

                    using (var ms = new MemoryStream())
                    {
                        model.featuredimagefilecontenttype = model.featuredimagefile.ContentType;
                        model.featuredimagefilename = model.featuredimagefile.FileName;

                        model.featuredimagefile.CopyTo(ms);
                        message = await _thisService.SaveFile(ms.ToArray(), model, userlogin.userid, model.featuredimagefile.FileName, sitePath, hosturl);
                    }
                    if (message.MessageCode < 0)
                        return BadRequest(message);

                    FileActionDto _fileinfo = message.MessageData as FileActionDto;
                    model.featuredimagefilepath = _fileinfo.filepath;
                    model.featuredimagefileurl = _fileinfo.fileurl;

                    if (message.MessageCode < 0)
                        return BadRequest(message);
                }


                message = await _thisService.AddNewFeedBack(model, userlogin, processId, clientip, hosturl);

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
        [Route("/api/adminpanel/deletefeedback")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteFeedBack(FeedBackSearchDto model)
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

                message = await _thisService.RemoveFeedBack(model, userlogin, processId, clientip, hosturl);

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
        [Route("/api/adminpanel/getfeedbacks")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFeedBacks(FeedBackSearchDto model)
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


                message = await _thisService.GetFeedBacks(model, userlogin, processId, clientip, hosturl);

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
        [HttpPut]
        [Route("/api/adminpanel/editfeedback")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateFeedBack([FromForm] FeedBackDto model)
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

                if (model.feedbackfile != null)
                {

                    if (model.feedbackfile.FileName == null || model.feedbackfile.FileName.Length == 0)
                    {
                        // return Content("File not selected");
                    }

                    using (var ms = new MemoryStream())
                    {
                        model.feedbackfilecontenttype = model.feedbackfile.ContentType;
                        model.feedbackfilename = model.feedbackfile.FileName;

                        model.feedbackfile.CopyTo(ms);
                        message = await _thisService.SaveFile(ms.ToArray(), model, userlogin.userid, model.feedbackfile.FileName, sitePath, hosturl);
                    }
                    if (message.MessageCode < 0)
                        return BadRequest(message);

                    FileActionDto _fileinfo = message.MessageData as FileActionDto;
                    model.feedbackfilepath = _fileinfo.filepath;
                    model.feedbackfileurl = _fileinfo.fileurl;

                    if (message.MessageCode < 0)
                        return BadRequest(message);
                }

                if (model.featuredimagefile != null)
                {

                    if (model.featuredimagefile.FileName == null || model.featuredimagefile.FileName.Length == 0)
                    {
                        // return Content("File not selected");
                    }

                    using (var ms = new MemoryStream())
                    {
                        model.featuredimagefilecontenttype = model.featuredimagefile.ContentType;
                        model.featuredimagefilename = model.featuredimagefile.FileName;

                        model.featuredimagefile.CopyTo(ms);
                        message = await _thisService.SaveFile(ms.ToArray(), model, userlogin.userid, model.featuredimagefile.FileName, sitePath, hosturl);
                    }
                    if (message.MessageCode < 0)
                        return BadRequest(message);

                    FileActionDto _fileinfo = message.MessageData as FileActionDto;
                    model.featuredimagefilepath = _fileinfo.filepath;
                    model.featuredimagefileurl = _fileinfo.fileurl;

                    if (message.MessageCode < 0)
                        return BadRequest(message);
                }


                message = await _thisService.EditFeedBack(model, userlogin, processId, clientip, hosturl);

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
