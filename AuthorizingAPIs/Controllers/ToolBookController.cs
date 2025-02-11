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

    public class ADBookController : ControllerBase
    {
        AuthorizationService _authorizationService;
        private IHttpContextAccessor _HttpContextAccessor;
        IWebHostEnvironment _webHostEnvironment;
        UserServices _userServices;
        SystemLogServices _systemLogServices;
        BlockedIPServices _blockedIPService;
        CategoriesServices _userTypeService;
        ADBookServices _thisService;

        public ADBookController(AuthorizationService authorizationService,
                                       IHttpContextAccessor httpContextAccessor,
                                       IWebHostEnvironment env,
                                       SystemLogServices systemLogServices,
                                       BlockedIPServices blockedIPServices,
                                       CategoriesServices userTypeServices,
                                       ADBookServices courseBuilderServices,
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
        [Route("/api/adminpanel/tools/addnewbook")]
        [AllowAnonymous]
        public async Task<IActionResult> InsertNewBook([FromForm] ADBookDto model)
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

                if (model.featuredimage != null)
                {

                    if (model.featuredimage.FileName == null || model.featuredimage.FileName.Length == 0)
                    {
                        // return Content("File not selected");
                    }

                    using (var ms = new MemoryStream())
                    {
                        model.featuredimagecontenttype = model.featuredimage.ContentType;
                        model.featuredimagename = model.featuredimage.FileName;

                        model.featuredimage.CopyTo(ms);
                        message = await _thisService.SaveFile(ms.ToArray(), model, userlogin.userid, model.featuredimage.FileName, sitePath, hosturl);
                    }
                    if (message.MessageCode < 0)
                        return BadRequest(message);

                    FileActionDto _fileinfo = message.MessageData as FileActionDto;
                    model.featuredimagepath = _fileinfo.filepath;
                    model.featuredimageurl = _fileinfo.fileurl;

                    if (message.MessageCode < 0)
                        return BadRequest(message);
                }

                if (model.bgimage != null)
                {

                    if (model.bgimage.FileName == null || model.bgimage.FileName.Length == 0)
                    {
                        // return Content("File not selected");
                    }

                    using (var ms = new MemoryStream())
                    {
                        model.bgimagecontenttype = model.bgimage.ContentType;
                        model.bgimagename = model.bgimage.FileName;

                        model.bgimage.CopyTo(ms);
                        message = await _thisService.SaveFile(ms.ToArray(), model, userlogin.userid, model.bgimage.FileName, sitePath, hosturl);
                    }
                    if (message.MessageCode < 0)
                        return BadRequest(message);

                    FileActionDto _fileinfo = message.MessageData as FileActionDto;
                    model.bgimagepath = _fileinfo.filepath;
                    model.bgimageurl = _fileinfo.fileurl;

                    if (message.MessageCode < 0)
                        return BadRequest(message);
                }

                if (model.pdfthumbnailimage != null)
                {

                    if (model.pdfthumbnailimage.FileName == null || model.pdfthumbnailimage.FileName.Length == 0)
                    {
                        // return Content("File not selected");
                    }

                    using (var ms = new MemoryStream())
                    {
                        model.pdfthumbnailimagecontenttype = model.pdfthumbnailimage.ContentType;
                        model.pdfthumbnailimagename = model.pdfthumbnailimage.FileName;

                        model.pdfthumbnailimage.CopyTo(ms);
                        message = await _thisService.SaveFile(ms.ToArray(), model, userlogin.userid, model.pdfthumbnailimage.FileName, sitePath, hosturl);
                    }
                    if (message.MessageCode < 0)
                        return BadRequest(message);

                    FileActionDto _fileinfo = message.MessageData as FileActionDto;
                    model.pdfthumbnailimagepath = _fileinfo.filepath;
                    model.pdfthumbnailimageurl = _fileinfo.fileurl;

                    if (message.MessageCode < 0)
                        return BadRequest(message);
                }

                if (model.pdffile != null)
                {

                    if (model.pdffile.FileName == null || model.pdffile.FileName.Length == 0)
                    {
                        // return Content("File not selected");
                    }

                    using (var ms = new MemoryStream())
                    {
                        model.pdffilecontenttype = model.pdffile.ContentType;
                        model.pdffilename = model.pdffile.FileName;

                        model.pdffile.CopyTo(ms);
                        message = await _thisService.SaveFile(ms.ToArray(), model, userlogin.userid, model.pdffile.FileName, sitePath, hosturl);
                    }
                    if (message.MessageCode < 0)
                        return BadRequest(message);

                    FileActionDto _fileinfo = message.MessageData as FileActionDto;
                    model.pdffilepath = _fileinfo.filepath;
                    model.pdffileurl = _fileinfo.fileurl;

                    if (message.MessageCode < 0)
                        return BadRequest(message);
                }

                if (model.pageimages != null)
                {
                    List<BookPageImage> BookPageImageList = new List<BookPageImage>();
                    BookPageImage BookPageImage = null;
                    foreach (IFormFile pageimage in model.pageimages)
                    {       
                        if (pageimage.FileName != null && pageimage.FileName.Length > 0)
                        {
                             BookPageImage = new BookPageImage() { id = Guid.NewGuid()};
                            using (var ms = new MemoryStream())
                            {
                                BookPageImage.pageimagecontenttype = pageimage.ContentType;
                                BookPageImage.pageimagename = pageimage.FileName;

                                pageimage.CopyTo(ms);
                                message = await _thisService.SaveFile(ms.ToArray(), model, userlogin.userid, pageimage.FileName, sitePath, hosturl);
                            }
                            if (message.MessageCode > 0)
                            {
                                FileActionDto _fileinfo = message.MessageData as FileActionDto;
                                BookPageImage.pageimagepath = _fileinfo.filepath;
                                BookPageImage.pageimageurl = _fileinfo.fileurl;
                            }
                            BookPageImageList.Add(BookPageImage);
                        }
                    }
                    message = await _thisService.GetBookPageImages(BookPageImageList, userlogin, processId, clientip, hosturl);
                }

                message = await _thisService.AddNewNewAdminPanel_Tools_Book(model, userlogin, processId, clientip, hosturl);

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
        [Route("/api/adminpanel/tools/deletebook")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteNewBook(ADBookSearchDto model)
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

                message = await _thisService.RemoveNewAdminPanel_Tools_Book(model, userlogin, processId, clientip, hosturl);

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
        [Route("/api/adminpanel/tools/getbooks")]
        [AllowAnonymous]
        public async Task<IActionResult> GetNewBooks(ADBookSearchDto model)
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


                message = await _thisService.GetNewAdminPanel_Tools_Books(model, userlogin, processId, clientip, hosturl);

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
        [Route("/api/adminpanel/tools/editbook")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateNewBook([FromForm] ADBookDto model)
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

                if (model.featuredimage != null)
                {

                    if (model.featuredimage.FileName == null || model.featuredimage.FileName.Length == 0)
                    {
                        // return Content("File not selected");
                    }

                    using (var ms = new MemoryStream())
                    {
                        model.featuredimagecontenttype = model.featuredimage.ContentType;
                        model.featuredimagename = model.featuredimage.FileName;

                        model.featuredimage.CopyTo(ms);
                        message = await _thisService.SaveFile(ms.ToArray(), model, userlogin.userid, model.featuredimage.FileName, sitePath, hosturl);
                    }
                    if (message.MessageCode < 0)
                        return BadRequest(message);

                    FileActionDto _fileinfo = message.MessageData as FileActionDto;
                    model.featuredimagepath = _fileinfo.filepath;
                    model.featuredimageurl = _fileinfo.fileurl;

                    if (message.MessageCode < 0)
                        return BadRequest(message);
                }

                if (model.bgimage != null)
                {

                    if (model.bgimage.FileName == null || model.bgimage.FileName.Length == 0)
                    {
                        // return Content("File not selected");
                    }

                    using (var ms = new MemoryStream())
                    {
                        model.bgimagecontenttype = model.bgimage.ContentType;
                        model.bgimagename = model.bgimage.FileName;

                        model.bgimage.CopyTo(ms);
                        message = await _thisService.SaveFile(ms.ToArray(), model, userlogin.userid, model.bgimage.FileName, sitePath, hosturl);
                    }
                    if (message.MessageCode < 0)
                        return BadRequest(message);

                    FileActionDto _fileinfo = message.MessageData as FileActionDto;
                    model.bgimagepath = _fileinfo.filepath;
                    model.bgimageurl = _fileinfo.fileurl;

                    if (message.MessageCode < 0)
                        return BadRequest(message);
                }

                if (model.pdfthumbnailimage != null)
                {

                    if (model.pdfthumbnailimage.FileName == null || model.pdfthumbnailimage.FileName.Length == 0)
                    {
                        // return Content("File not selected");
                    }

                    using (var ms = new MemoryStream())
                    {
                        model.pdfthumbnailimagecontenttype = model.pdfthumbnailimage.ContentType;
                        model.pdfthumbnailimagename = model.pdfthumbnailimage.FileName;

                        model.pdfthumbnailimage.CopyTo(ms);
                        message = await _thisService.SaveFile(ms.ToArray(), model, userlogin.userid, model.pdfthumbnailimage.FileName, sitePath, hosturl);
                    }
                    if (message.MessageCode < 0)
                        return BadRequest(message);

                    FileActionDto _fileinfo = message.MessageData as FileActionDto;
                    model.pdfthumbnailimagepath = _fileinfo.filepath;
                    model.pdfthumbnailimageurl = _fileinfo.fileurl;

                    if (message.MessageCode < 0)
                        return BadRequest(message);
                }

                if (model.pdffile != null)
                {

                    if (model.pdffile.FileName == null || model.pdffile.FileName.Length == 0)
                    {
                        // return Content("File not selected");
                    }

                    using (var ms = new MemoryStream())
                    {
                        model.pdffilecontenttype = model.pdffile.ContentType;
                        model.pdffilename = model.pdffile.FileName;

                        model.pdffile.CopyTo(ms);
                        message = await _thisService.SaveFile(ms.ToArray(), model, userlogin.userid, model.pdffile.FileName, sitePath, hosturl);
                    }
                    if (message.MessageCode < 0)
                        return BadRequest(message);

                    FileActionDto _fileinfo = message.MessageData as FileActionDto;
                    model.pdffilepath = _fileinfo.filepath;
                    model.pdffileurl = _fileinfo.fileurl;

                    if (message.MessageCode < 0)
                        return BadRequest(message);
                }

                if (model.pageimages != null)
                {
                    List<BookPageImage> BookPageImageList = new List<BookPageImage>();
                    BookPageImage BookPageImage = null;
                    foreach (IFormFile pageimage in model.pageimages)
                    {
                        if (pageimage.FileName != null && pageimage.FileName.Length > 0)
                        {
                            BookPageImage = new BookPageImage() { id = Guid.NewGuid() };
                            using (var ms = new MemoryStream())
                            {
                                BookPageImage.pageimagecontenttype = pageimage.ContentType;
                                BookPageImage.pageimagename = pageimage.FileName;

                                pageimage.CopyTo(ms);
                                message = await _thisService.SaveFile(ms.ToArray(), model, userlogin.userid, pageimage.FileName, sitePath, hosturl);
                            }
                            if (message.MessageCode > 0)
                            {
                                FileActionDto _fileinfo = message.MessageData as FileActionDto;
                                BookPageImage.pageimagepath = _fileinfo.filepath;
                                BookPageImage.pageimageurl = _fileinfo.fileurl;
                            }
                            BookPageImageList.Add(BookPageImage);
                        }
                    }
                    message = await _thisService.GetBookPageImages(BookPageImageList, userlogin, processId, clientip, hosturl);
                }


                message = await _thisService.EditNewAdminPanel_Tools_Book(model, userlogin, processId, clientip, hosturl);

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
