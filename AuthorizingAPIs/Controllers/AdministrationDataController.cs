using Base.Common.Enums;
using Entities.DBEntities;
using Entities.Dtos;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using NextTradeAPIs.Services;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace NextTradeAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AdministrationDataController : ControllerBase
    {
        AuthorizationService _authorizationService;
        private IHttpContextAccessor _HttpContextAccessor;
        UserServices _userServices;
        SystemLogServices _systemLogServices;
        BlockedIPServices _blockedIPService;
        UserTypeServices _userTypeService;
        CategoriesServices _categoryService;

        public AdministrationDataController(AuthorizationService authorizationService,
                                       IHttpContextAccessor httpContextAccessor,
                                       SystemLogServices systemLogServices,
                                       BlockedIPServices blockedIPServices,
                                       UserTypeServices userTypeServices,
                                       CategoriesServices courseServices,
                                       UserServices userServices)
        {
            _authorizationService = authorizationService;
            _userServices = userServices;
            _HttpContextAccessor = httpContextAccessor;
            _systemLogServices = systemLogServices;
            _blockedIPService = blockedIPServices;
            _userTypeService = userTypeServices;
            _categoryService = courseServices;
        }

        [HttpPost]
        [HttpPut]
        [Route("/api/categorymangment/updatecategory")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCourse(CategorisDto model)
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

                message = await _categoryService.UpdateCategory(model, userlogin, processId, clientip, hosturl);

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
        [Route("/api/categorymangment/deletecategory")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteCourse(CategorisDto model)
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

                message = await _categoryService.DeleteCategory(model, userlogin, processId, clientip, hosturl);

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
