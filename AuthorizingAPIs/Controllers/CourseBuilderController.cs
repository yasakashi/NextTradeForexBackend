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
    public class CourseBuilderController : ControllerBase
    {
        AuthorizationService _authorizationService;
        private IHttpContextAccessor _HttpContextAccessor;
        IWebHostEnvironment _webHostEnvironment;
        UserServices _userServices;
        SystemLogServices _systemLogServices;
        BlockedIPServices _blockedIPService;
        CategoriesServices _userTypeService;
        CourseBuilderServices _thisService;

        public CourseBuilderController(AuthorizationService authorizationService,
                                       IHttpContextAccessor httpContextAccessor,
                                       IWebHostEnvironment env,
                                       SystemLogServices systemLogServices,
                                       BlockedIPServices blockedIPServices,
                                       CategoriesServices userTypeServices,
                                       CourseBuilderServices courseBuilderServices,
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
        [Route("/api/coursebuilder/addcourse")]
        [AllowAnonymous]
        public async Task<IActionResult> InsertCourse([FromForm] CourseBuilderCourseDto model)
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

                model.Id = Guid.NewGuid();

                var sitePath = _webHostEnvironment.WebRootPath;

                if (model.courseFile != null)
                {

                    if (model.courseFile.FileName == null || model.courseFile.FileName.Length == 0)
                    {
                        // return Content("File not selected");
                    }

                    using (var ms = new MemoryStream())
                    {
                        model.courseFilecontent = model.courseFile.ContentType;
                        model.courseFilename = model.courseFile.FileName;

                        model.courseFile.CopyTo(ms);
                        message = await _thisService.SaveCourseFile(ms.ToArray(), model, userlogin.userid, model.courseFile.FileName, sitePath);
                    }
                    if (message.MessageCode < 0)
                        return BadRequest(message);

                    FileActionDto _fileinfo = message.MessageData as FileActionDto;
                    model.courseFilepath = _fileinfo.filepath;

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
                        model.featuredImagecontent = model.featuredImage.ContentType;
                        model.featuredImagename = model.featuredImage.FileName;

                        model.courseFile.CopyTo(ms);
                        message = await _thisService.SaveCourseFile(ms.ToArray(), model, userlogin.userid, model.featuredImage.FileName, sitePath);
                    }
                    if (message.MessageCode < 0)
                        return BadRequest(message);

                    FileActionDto _fileinfo = message.MessageData as FileActionDto;
                    model.featuredImagepath = _fileinfo.filepath;

                    if (message.MessageCode < 0)
                        return BadRequest(message);
                }


                message = await _thisService.AddNewCourses(model, null, processId, clientip, hosturl);

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
        [Route("/api/coursebuilder/addcoursemeeting")]
        [AllowAnonymous]
        public async Task<IActionResult> InsertCourseMeeting([FromForm] CourseBuilderMeetingDto model)
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

                model.Id = Guid.NewGuid();

                var sitePath = _webHostEnvironment.WebRootPath;

                if (model.meetingFile != null)
                {

                    if (model.meetingFile.FileName == null || model.meetingFile.FileName.Length == 0)
                    {
                        // return Content("File not selected");
                    }

                    using (var ms = new MemoryStream())
                    {
                        model.meetingfilecontetnttype = model.meetingFile.ContentType;
                        model.meetingFilename = model.meetingFile.FileName;

                        model.meetingFile.CopyTo(ms);
                        message = await _thisService.SaveCourseMeetingFile(ms.ToArray(), model, userlogin.userid, model.meetingFile.FileName, sitePath);
                    }
                    if (message.MessageCode < 0)
                        return BadRequest(message);

                    FileActionDto _fileinfo = message.MessageData as FileActionDto;
                    model.meetingFilepath = _fileinfo.filepath;

                    if (message.MessageCode < 0)
                        return BadRequest(message);
                }

                message = await _thisService.AddNewCourseMeetings(model, null, processId, clientip, hosturl);

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
        [Route("/api/coursebuilder/addcoursevideopdfurl")]
        [AllowAnonymous]
        public async Task<IActionResult> InsertCourseVideoPdfUrl([FromForm] CourseBuildeVideoPdfUrlDto model)
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

                model.Id = Guid.NewGuid();

                var sitePath = _webHostEnvironment.WebRootPath;

                if (model.pdfFile != null)
                {

                    if (model.pdfFile.FileName == null || model.pdfFile.FileName.Length == 0)
                    {
                        // return Content("File not selected");
                    }

                    using (var ms = new MemoryStream())
                    {
                        model.pdfFilecontenttype = model.pdfFile.ContentType;
                        model.pdfFilename = model.pdfFile.FileName;

                        model.pdfFile.CopyTo(ms);
                        message = await _thisService.SaveCourseVideoPdfUrlFile(ms.ToArray(), model, userlogin.userid, model.pdfFile.FileName, sitePath);
                    }
                    if (message.MessageCode < 0)
                        return BadRequest(message);

                    FileActionDto _fileinfo = message.MessageData as FileActionDto;
                    model.pdfFilepath = _fileinfo.filepath;

                    if (message.MessageCode < 0)
                        return BadRequest(message);
                }


                message = await _thisService.AddNewCourseVideoPdfUrls(model, null, processId, clientip, hosturl);

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
        [Route("/api/coursebuilder/getcourses")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCourses(CourseBuilderCourseDto model)
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


                message = await _thisService.GetCourses(model, null, processId, clientip, hosturl, false);

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
        [Route("/api/coursebuilder/addcoursetopic")]
        [AllowAnonymous]
        public async Task<IActionResult> InsertCourseTopic(CourseBuilderTopicDto model)
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

                model.Id = Guid.NewGuid();

                var sitePath = _webHostEnvironment.WebRootPath;


                message = await _thisService.AddCourseTopics(model, null, processId, clientip, hosturl);

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
        [Route("/api/coursebuilder/getcoursetopics")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCourseTopics(CourseBuilderTopicDto model)
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

                message = await _thisService.GetCourseTopics(model, null, processId, clientip, hosturl);

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
        [Route("/api/coursebuilder/addlesson")]
        [AllowAnonymous]
        public async Task<IActionResult> InsertLesson([FromForm] CourseBuilderLessonDto model)
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

                model.Id = Guid.NewGuid();

                var sitePath = _webHostEnvironment.WebRootPath;

                if (model.lessonFile != null)
                {

                    if (model.lessonFile.FileName == null || model.lessonFile.FileName.Length == 0)
                    {
                        // return Content("File not selected");
                    }

                    using (var ms = new MemoryStream())
                    {
                        model.lessonFilecontenttype = model.lessonFile.ContentType;
                        model.lessonFilename = model.lessonFile.FileName;

                        model.lessonFile.CopyTo(ms);
                        message = await _thisService.SaveCourseLessonFile(ms.ToArray(), model, userlogin.userid, model.lessonFile.FileName, sitePath);
                    }
                    if (message.MessageCode < 0)
                        return BadRequest(message);

                    FileActionDto _fileinfo = message.MessageData as FileActionDto;
                    model.lessonFilepath = _fileinfo.filepath;

                    if (message.MessageCode < 0)
                        return BadRequest(message);
                }

                if (model.featureImage != null)
                {

                    if (model.featureImage.FileName == null || model.featureImage.FileName.Length == 0)
                    {
                        // return Content("File not selected");
                    }

                    using (var ms = new MemoryStream())
                    {
                        model.featureImagecontenttype = model.featureImage.ContentType;
                        model.featureImagename = model.featureImage.FileName;

                        model.featureImage.CopyTo(ms);
                        message = await _thisService.SaveCourseLessonFile(ms.ToArray(), model, userlogin.userid, model.featureImage.FileName, sitePath);
                    }
                    if (message.MessageCode < 0)
                        return BadRequest(message);

                    FileActionDto _fileinfo = message.MessageData as FileActionDto;
                    model.featureImagepath = _fileinfo.filepath;

                    if (message.MessageCode < 0)
                        return BadRequest(message);
                }

                if (model.attachments != null)
                {
                    model.fileattachments = new List<CourseBuilderLessonFileDto>();
                    foreach (IFormFile attach in model.attachments)
                    {
                        if (attach.FileName != null && attach.FileName.Length > 0)
                        {
                            CourseBuilderLessonFileDto fileattachment = new CourseBuilderLessonFileDto();
                            using (var ms = new MemoryStream())
                            {
                                fileattachment.lessonFilecontenttype = attach.ContentType;
                                fileattachment.lessonFilename = attach.FileName;

                                attach.CopyTo(ms);
                                message = await _thisService.SaveCourseLessonAttachFile(ms.ToArray(), model, userlogin.userid, attach.FileName, sitePath);
                            }
                            FileActionDto _fileinfo = message.MessageData as FileActionDto;
                            fileattachment.lessonFilepath = _fileinfo.filepath;

                            model.fileattachments.Add(fileattachment);
                        }
                    }

                }

                message = await _thisService.AddNewCourseLesson(model, null, processId, clientip, hosturl);

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
        [Route("/api/coursebuilder/getLessons")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLessons(CourseBuilderLessonFilterDto model)
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

                message = await _thisService.GetCourseLessons(model, null, processId, clientip, hosturl, false);

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
        [Route("/api/coursebuilder/getLessonfiles")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLessonAttachedFiles(CourseBuilderLessonFilterDto model)
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

                message = await _thisService.GetCourseLessonAttachedFiles(model, null, processId, clientip, hosturl, false);

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
        [Route("/api/coursebuilder/addlessonquiz")]
        [AllowAnonymous]
        public async Task<IActionResult> InsertLessonQuiz( CourseBuilderQuizDto model)
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

                model.Id = Guid.NewGuid();

                var sitePath = _webHostEnvironment.WebRootPath;

                  message = await _thisService.AddNewCourseLessonQuez(model, userlogin, processId, clientip, hosturl);

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
        [Route("/api/coursebuilder/getLessonquizs")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLessonQuizs(CourseBuilderQuizFilterDto model)
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

                message = await _thisService.GetCourseLessonQuezs(model, null, processId, clientip, hosturl);

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



        [HttpPost("{id}")]
        [HttpGet("{id}")]
        [Route("/api/coursebuilder/getcoursefileurl/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFileURL(Guid id)
        {
            var sitePath = _webHostEnvironment.WebRootPath;
            string hosturl = ((Request.IsHttps) ? "https" : "http") + @"://" + Request.Host.ToString();
            var message = await _thisService.GetCourseFileURL(id, sitePath);
            if (message == null)
            {
                return NotFound();
            }
            else
            {
                CourseBuilderCourse data = message.MessageData as CourseBuilderCourse;
                string relativePath = hosturl + data.courseFilepath.Substring(data.courseFilepath.IndexOf("wwwroot\\")).Replace("wwwroot", "").Replace("\\", "/");

                data.courseFilepath = relativePath;

                FileActionDto fileInfo = new FileActionDto() { fileurl = relativePath, filename = data.courseFilename, filecontent = data.courseFilecontent };

                message.MessageData = fileInfo;
            }

            return Ok(message);
        }

        [HttpPost("{id}")]
        [HttpGet("{id}")]
        [Route("/api/coursebuilder/getcoursefile/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFile(Guid id)
        {
            var sitePath = _webHostEnvironment.WebRootPath;
            string hosturl = ((Request.IsHttps) ? "https" : "http") + @"://" + Request.Host.ToString();
            var message = await _thisService.GetCourseFileURL(id, sitePath);
            if (message == null)
            {
                return NotFound();
            }
            else
            {
                CourseBuilderCourse data = message.MessageData as CourseBuilderCourse;
                var messagefile = await _thisService.GetFileByteArray(data.courseFilepath);
                byte[] filearray = messagefile.MessageData as byte[];
                return File(filearray, data.courseFilecontent, data.courseFilename);
            }           
        }

        [HttpPost("{id}")]
        [HttpGet("{id}")]
        [Route("/api/coursebuilder/getcoursefeaturedimageurl/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetImageFileURL(Guid id)
        {
            var sitePath = _webHostEnvironment.WebRootPath;
            string hosturl = ((Request.IsHttps) ? "https" : "http") + @"://" + Request.Host.ToString();
            var message = await _thisService.GetCourseFileURL(id, sitePath);
            if (message == null)
            {
                return NotFound();
            }
            else
            {
                CourseBuilderCourse data = message.MessageData as CourseBuilderCourse;
                string relativePath = hosturl + data.courseFilepath.Substring(data.featuredImagepath.IndexOf("wwwroot\\")).Replace("wwwroot", "").Replace("\\", "/");

                data.courseFilepath = relativePath;

                FileActionDto fileInfo = new FileActionDto() { fileurl = relativePath, filename = data.courseFilename, filecontent = data.courseFilecontent };

                message.MessageData = fileInfo;
            }

            return Ok(message);
        }

        [HttpPost("{id}")]
        [HttpGet("{id}")]
        [Route("/api/coursebuilder/getcoursefeaturedimage/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetImageFile(Guid id)
        {
            var sitePath = _webHostEnvironment.WebRootPath;
            string hosturl = ((Request.IsHttps) ? "https" : "http") + @"://" + Request.Host.ToString();
            var message = await _thisService.GetCourseFileURL(id, sitePath);
            if (message == null)
            {
                return NotFound();
            }
            else
            {
                CourseBuilderCourse data = message.MessageData as CourseBuilderCourse;
                var messagefile = await _thisService.GetFileByteArray(data.featuredImagepath);
                byte[] filearray = messagefile.MessageData as byte[];
                return File(filearray, data.courseFilecontent, data.featuredImagecontent);
            }
        }

        [HttpPost("{id}")]
        [HttpGet("{id}")]
        [Route("/api/coursebuilder/getlessonfeaturedimageurl/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLessonfeatureImageURL(Guid id)
        {
            var sitePath = _webHostEnvironment.WebRootPath;
            string hosturl = ((Request.IsHttps) ? "https" : "http") + @"://" + Request.Host.ToString();
            var message = await _thisService.GetLessonFileURL(id, sitePath);
            if (message == null)
            {
                return NotFound();
            }
            else
            {
                CourseBuilderLesson data = message.MessageData as CourseBuilderLesson;
                string relativePath = hosturl + data.featureImagepath.Substring(data.featureImagepath.IndexOf("wwwroot\\")).Replace("wwwroot", "").Replace("\\", "/");

                data.featureImagepath = relativePath;

                FileActionDto fileInfo = new FileActionDto() { fileurl = relativePath, filename = data.featureImagename, filecontent = data.featureImagecontenttype };

                message.MessageData = fileInfo;
            }

            return Ok(message);
        }

        [HttpPost("{id}")]
        [HttpGet("{id}")]
        [Route("/api/coursebuilder/getlessonfeaturedimage/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLessonImageFile(Guid id)
        {
            var sitePath = _webHostEnvironment.WebRootPath;
            string hosturl = ((Request.IsHttps) ? "https" : "http") + @"://" + Request.Host.ToString();
            var message = await _thisService.GetLessonFileURL(id, sitePath);
            if (message == null)
            {
                return NotFound();
            }
            else
            {
                CourseBuilderLesson data = message.MessageData as CourseBuilderLesson;
                var messagefile = await _thisService.GetFileByteArray(data.featureImagepath);
                byte[] filearray = messagefile.MessageData as byte[];
                return File(filearray, data.featureImagecontenttype, data.featureImagename);
            }
        }

        [HttpPost("{id}")]
        [HttpGet("{id}")]
        [Route("/api/coursebuilder/getlessonfileurl/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLessonFileURL(Guid id)
        {
            var sitePath = _webHostEnvironment.WebRootPath;
            string hosturl = ((Request.IsHttps) ? "https" : "http") + @"://" + Request.Host.ToString();
            var message = await _thisService.GetLessonFileURL(id, sitePath);
            if (message == null)
            {
                return NotFound();
            }
            else
            {
                CourseBuilderLesson data = message.MessageData as CourseBuilderLesson;
                string relativePath = hosturl + data.lessonFilepath.Substring(data.lessonFilepath.IndexOf("wwwroot\\")).Replace("wwwroot", "").Replace("\\", "/");

                data.lessonFilepath = relativePath;

                FileActionDto fileInfo = new FileActionDto() { fileurl = relativePath, filename = data.lessonFilename, filecontent = data.lessonFilecontenttype };

                message.MessageData = fileInfo;
            }

            return Ok(message);
        }

        [HttpPost("{id}")]
        [HttpGet("{id}")]
        [Route("/api/coursebuilder/getlessonfile/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLessonFile(Guid id)
        {
            var sitePath = _webHostEnvironment.WebRootPath;
            string hosturl = ((Request.IsHttps) ? "https" : "http") + @"://" + Request.Host.ToString();
            var message = await _thisService.GetLessonFileURL(id, sitePath);
            if (message == null)
            {
                return NotFound();
            }
            else
            {
                CourseBuilderLesson data = message.MessageData as CourseBuilderLesson;
                var messagefile = await _thisService.GetFileByteArray(data.lessonFilepath);
                byte[] filearray = messagefile.MessageData as byte[];
                return File(filearray, data.lessonFilecontenttype, data.lessonFilename);
            }
        }

        [HttpPost("{id}")]
        [HttpGet("{id}")]
        [Route("/api/coursebuilder/getlessonattachedfile/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLessonAttachedFile(Guid id)
        {
            var sitePath = _webHostEnvironment.WebRootPath;
            string hosturl = ((Request.IsHttps) ? "https" : "http") + @"://" + Request.Host.ToString();
            var message = await _thisService.GetLessonAttachmentFileURL(id, sitePath);
            if (message == null)
            {
                return NotFound();
            }
            else
            {
                CourseBuilderLessonFile data = message.MessageData as CourseBuilderLessonFile;
                var messagefile = await _thisService.GetFileByteArray(data.lessonFilepath);
                byte[] filearray = messagefile.MessageData as byte[];
                return File(filearray, data.lessonFilecontenttype, data.lessonFilename);
            }
        }

        [HttpPost("{id}")]
        [HttpGet("{id}")]
        [Route("/api/coursebuilder/getlessonattachedfileurl/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLessonAttachedFileURL(Guid id)
        {
            var sitePath = _webHostEnvironment.WebRootPath;
            string hosturl = ((Request.IsHttps) ? "https" : "http") + @"://" + Request.Host.ToString();
            var message = await _thisService.GetLessonAttachmentFileURL(id, sitePath);
            if (message == null)
            {
                return NotFound();
            }
            else
            {
                CourseBuilderLessonFile data = message.MessageData as CourseBuilderLessonFile;
                string relativePath = hosturl + data.lessonFilepath.Substring(data.lessonFilepath.IndexOf("wwwroot\\")).Replace("wwwroot", "").Replace("\\", "/");

                data.lessonFilepath = relativePath;

                FileActionDto fileInfo = new FileActionDto() { fileurl = relativePath, filename = data.lessonFilename, filecontent = data.lessonFilecontenttype };

                message.MessageData = fileInfo;
            }

            return Ok(message);
        }

        /*
        [HttpPost]
        [Route("/api/removecommunitygroups")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteCommunityGroup(GroupSearchFilterDto model)
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

                message = await _communityGroupService.DeleteCommunityGroup(model, userlogin, processId, clientip, hosturl);

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
        [Route("/api/editcommunitygroup")]
        [AllowAnonymous]
        public async Task<IActionResult> EditCommunityGroup(CommunityGroupDto model)
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

                message = await _communityGroupService.EditCommunityGroup(model, userlogin, processId, clientip, hosturl);

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
        [Route("/api/gettopcommunitygroups")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTopCommunityGroup(GroupSearchFilterDto model)
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

                ////message = await _authorizationService.CheckToken(_bearer_token, processId);

                ////if (message.MessageCode < 0)
                ////    return Unauthorized(message);

                //UserModel userlogin = message.MessageData as UserModel;

                message = await _communityGroupService.GetTopCommunityGroup(model, null, processId, clientip, hosturl);

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
        [Route("/api/getusercommunitygroups")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserCommunityGroup(GroupSearchFilterDto model)
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

                model.owneruserid = userlogin.userid;

                message = await _communityGroupService.GetCommunityGroup(model, userlogin, processId, clientip, hosturl);

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
        [Route("/api/addcommunitygroup")]
        [AllowAnonymous]
        public async Task<IActionResult> AddCommunityGroup(CommunityGroupDto model)
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

                message = await _communityGroupService.CreateCommunityGroup(model, userlogin, processId, clientip, hosturl);

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


        [Route("/api/setcommunitygrouppic")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateCommunityGroupPic([FromForm] CommunityGroupImageDto model)
        {

            if (model == null)
            {
                return Content("Invalid Submission!");
            }
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

                if (model.photofile != null)
                {

                    if (model.photofile.FileName == null || model.photofile.FileName.Length == 0)
                    {
                        return Content("File not selected");
                    }

                    CommunityGroupDto modeldate = new CommunityGroupDto()
                    {
                        Id = model.Id,
                    };

                    using (var ms = new MemoryStream())
                    {
                        model.photofile.CopyTo(ms);
                        modeldate.coverimage = ms.ToArray();
                    }
                    message = await _communityGroupService.UpdateCommunityGroupImage(modeldate, userlogin, processId, clientip, hosturl);
                    if (message.MessageCode < 0)
                        return BadRequest(message);
                }
                else
                {
                    return BadRequest(new SystemMessageModel() { MessageCode = -501, MessageDescription = "Erroe Canot catch file", MessageData = null });
                }

                return Ok(message);
            }
            catch (Exception ex)
            {
                string log = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{ex.Message}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                _systemLogServices.InsertLogs(log, processId, clientip, hosturl, LogTypes.TokenError);
                return Unauthorized();
            }
        }

        [Route("/api/setcommunitycoverpic")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateCommunityCoverPic([FromForm] CommunityGroupImageDto model)
        {

            if (model == null)
            {
                return Ok(new SystemMessageModel()
                {
                    MessageCode = -501,
                    MessageDescription = "Invalid Submission!",
                    MessageData = null,
                    Meta = new
                    {
                        ApiCode = "file"
                    }
                });
                // return Content("Invalid Submission!");
            }
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
                {
                    //return Unauthorized(message);
                    return Ok(new SystemMessageModel()
                    {
                        MessageCode = -405,
                        MessageDescription = "Erroe Canot catch file",
                        MessageData = null,
                        Meta = new
                        {
                            ApiCode = ApiCode
                        }
                    });
                }
                UserModel userlogin = message.MessageData as UserModel;

                ApiCode = 5000;

                if (model.photofile != null)
                {

                    if (model.photofile.FileName == null || model.photofile.FileName.Length == 0)
                    {
                        return Content("File not selected");
                    }
                    ApiCode = 5001;
                    CommunityGroupDto modeldate = new CommunityGroupDto()
                    {
                        Id = model.Id,
                    };
                    ApiCode = 5002;
                    using (var ms = new MemoryStream())
                    {
                        model.photofile.CopyTo(ms);
                        modeldate.coverimage = ms.ToArray();
                    }
                    ApiCode = 5003;
                    message = await _communityGroupService.UpdateCommunityCoverImage(modeldate, userlogin, processId, clientip, hosturl);
                    ApiCode = 5004;
                    if (message.MessageCode < 0)
                    {
                        message.MessageData = ApiCode;
                        return Ok(message);
                    }
                }
                else
                {
                    return Ok(new SystemMessageModel()
                    {
                        MessageCode = -501,
                        MessageDescription = "Erroe Canot catch file",
                        MessageData = null,
                        Meta = new
                        {
                            ApiCode = ApiCode
                        }
                    });
                }

                return Ok(message);
            }
            catch (Exception ex)
            {
                string log = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{ex.Message}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                _systemLogServices.InsertLogs(log, processId, clientip, hosturl, LogTypes.TokenError);
                return Unauthorized();
            }
        }


        [HttpPost("{id}")]
        [HttpGet("{id}")]
        [Route("/api/getcommunitycoverimage/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCoverImage(Guid id)
        {
            var image = await _communityGroupService.GetCommunityCoverImage(id);
            if (image == null)
            {
                return NotFound();
            }

            return File(image, "image/png");
        }

        [HttpPost("{id}")]
        [HttpGet("{id}")]
        [Route("/api/getcommunitycoverimageurl/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCoverImageURL(Guid id)
        {
            var sitePath = _webHostEnvironment.WebRootPath;
            var message = await _communityGroupService.GetCommunityCoverImageURL(id, sitePath);
            if (message == null)
            {
                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "not find", MessageData = "" };
            }

            return Ok(message);
        }


        [HttpPost("{id}")]
        [HttpGet("{id}")]
        [Route("/api/getcommunitygroupimage/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetImage(Guid id)
        {
            var image = await _communityGroupService.GetCommunityGroupImage(id);
            if (image == null)
            {
                return NotFound();
            }

            return File(image, "image/png");
        }

        [HttpPost("{id}")]
        [HttpGet("{id}")]
        [Route("/api/getcommunitygroupimageurl/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetImageURL(Guid id)
        {
            var sitePath = _webHostEnvironment.WebRootPath;
            var message = await _communityGroupService.GetCommunityGroupImageURL(id, sitePath);
            if (message == null)
            {
                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "not find", MessageData = "" };
            }

            return Ok(message);
        }


        [HttpPost]
        [Route("/api/addcommunitygroupmember")]
        [AllowAnonymous]
        public async Task<IActionResult> AddCommunityGroupMember(CommunityGroupMemberDto model)
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
                //CommunityGroupMemberDto model = new CommunityGroupMemberDto() { communitygroupId = new Guid("05ff7d38-07f8-44e0-987b-a56d2b0d550f"), userId = 12 };

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

                message = await _communityGroupService.AddCommunityGroupMember(model, userlogin, processId, clientip, hosturl);

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
        [Route("/api/acceptcommunitygroupmember")]
        [AllowAnonymous]
        public async Task<IActionResult> AcceptCommunityGroupMember(CommunityGroupMemberDto model)
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

                message = await _communityGroupService.AcceptCommunityGroupMember(model, userlogin, processId, clientip, hosturl);

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
        [Route("/api/changecommunitygroupmembertype")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangeCommunityGroupMemberType(CommunityGroupMemberDto model)
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

                message = await _communityGroupService.ChangeCommunityGroupMemberType(model, userlogin, processId, clientip, hosturl);

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
        [Route("/api/removecommunitygroupmember")]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveCommunityGroupMember(CommunityGroupMemberDto model)
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

                message = await _communityGroupService.DeleteCommunityGroupMember(model, userlogin, processId, clientip, hosturl);

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
        [Route("/api/getcommunitygroupmember")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommunityGroupMember(CommunityGroupMemberDto model)
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
                //CommunityGroupMemberDto model = new CommunityGroupMemberDto() { communitygroupId = new Guid("05ff7d38-07f8-44e0-987b-a56d2b0d550f"), userId = 12 };

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

                message = await _communityGroupService.GetCommunityGroupMember(model, userlogin, processId, clientip, hosturl);

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
        [Route("/api/getcommunitygroupmemberwithstatus")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommunityGroupMemberWithStatus(CommunityGroupMemberDto model)
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
                //CommunityGroupMemberDto model = new CommunityGroupMemberDto() { communitygroupId = new Guid("05ff7d38-07f8-44e0-987b-a56d2b0d550f"), userId = 12 };

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

                message = await _communityGroupService.GetCommunityGroupMemberWithStatus(model, userlogin, processId, clientip, hosturl);

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

        /// <summary>
        /// ایست گروه هایی که یک کاربر عضو می باشد
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/getmycommunitygroup")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMyCommunityGroup(GroupSearchFilterDto model)
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
                //CommunityGroupMemberDto model = new CommunityGroupMemberDto() { communitygroupId = new Guid("05ff7d38-07f8-44e0-987b-a56d2b0d550f"), userId = 12 };

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

                message = await _communityGroupService.GetUserCommunityGroup(model, userlogin, processId, clientip, hosturl);

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
        */
    }
}
