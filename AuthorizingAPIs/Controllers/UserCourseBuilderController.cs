using Base.Common.Enums;
using Entities.DBEntities;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using NextTradeAPIs.Services;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace NextTradeAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UserCourseBuilderController : ControllerBase
    {
        AuthorizationService _authorizationService;
        private IHttpContextAccessor _HttpContextAccessor;
        IWebHostEnvironment _webHostEnvironment;
        UserServices _userServices;
        SystemLogServices _systemLogServices;
        BlockedIPServices _blockedIPService;
        CategoriesServices _userTypeService;
        CourseBuilderUserServices _thisService;

        public UserCourseBuilderController(AuthorizationService authorizationService,
                                       IHttpContextAccessor httpContextAccessor,
                                       IWebHostEnvironment env,
                                       SystemLogServices systemLogServices,
                                       BlockedIPServices blockedIPServices,
                                       CategoriesServices userTypeServices,
                                       CourseBuilderUserServices courseBuilderServices,
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
        [Route("/api/usercourse/addcourse")]
        [AllowAnonymous]
        public async Task<IActionResult> AddUserCourse(UserCourseMemberDto model)
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

                message = await _thisService.AddUserCourses(model, null, processId, clientip, hosturl);

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
        [Route("/api/usercourse/deletecourse")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteCourse(UserCourseFilterDto model)
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

                message = await _thisService.DeleteUserCourse(model, userlogin, processId, clientip, hosturl);

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
        [Route("/api/usercourse/getcourses")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserCourses(UserCourseFilterDto model)
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


                message = await _thisService.GetUserCourses(model, userlogin, processId, clientip, hosturl);

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
        [Route("/api/usercourse/updateusercourse")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateUserCourse(UserCourseMemberDto model)
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


                message = await _thisService.UpdateUserCourse(model, userlogin, processId, clientip, hosturl);

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
        [Route("/api/usercourse/adduserlessonpassed")]
        [AllowAnonymous]
        public async Task<IActionResult> AddUserLessonPassed(UserLessonDto model)
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

                message = await _thisService.AddUserLessenPassed(model, userlogin, processId, clientip, hosturl);

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
        [Route("/api/coursebuilder/getcoursestudents")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCourseStudents(CourseBuilderMemberFilterDto model)
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


                message = await _thisService.GetCourseStudents(model, userlogin, processId, clientip, hosturl, false);

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
        [HttpGet]
        [Route("/api/coursebuilder/getinstructorsreport")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCourseInstractorReport()
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


                message = await _thisService.GetCourseInstractorReport( userlogin, processId, clientip, hosturl);

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
        [Route("/api/coursebuilder/getcourseinstructors")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCourseInstructors(CourseBuilderMemberFilterDto model)
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


                message = await _thisService.GetCourseInstructors(model, userlogin, processId, clientip, hosturl, false);

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
