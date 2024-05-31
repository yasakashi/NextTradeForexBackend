using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataLayers;
using Entities.Dtos;

using NextTradeAPIs.Interfaces;
using NextTradeAPIs.Dtos;
using Base.Common.Enums;
using Entities.DBEntities;

namespace NextTradeAPIs.Services
{
    public class AuthorizationService //: IOghafAuthorizationService
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        private readonly Jwt _jwt;
        private readonly IJwtHandler _jwtHandler;
        SystemLogServices _systemLogServices;

        public AuthorizationService(SBbContext contex, LogSBbContext logcontex, IOptionsSnapshot<Jwt> jwt, IJwtHandler jwtHandler, SystemLogServices systemLogServices)
        {
            _Context = contex;
            _LogContext = logcontex;
            _jwt = jwt.Value;
            _jwtHandler = jwtHandler;
            _systemLogServices = systemLogServices;
        }

        /// <summary>
        /// تولید توکن برای ورود
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<SystemMessageModel> GetToken(LoginLog user,string processId ="", string clientip="")
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            if (string.IsNullOrEmpty(processId)) processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 201000;
            string token = string.Empty;
            var authentication = new Authentication();
            try
            {
                JwtSecurityToken jwtSecurityToken = await _jwtHandler.Create(user);

                authentication.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authentication.IsAuthenticated = true;
                //authentication.Username = user.Username;

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "ورود با موفقیت انجام شد", MessageData = authentication };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "خطا در انجام درخواست", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }
        /// <summary>
        /// ایجاد و ارسال توکن
        /// </summary>
        /// <param name="user"></param>
        /// <param name="expireday"></param>
        /// <param name="processId"></param>
        /// <param name="clientip"></param>
        /// <returns></returns>
        public async Task<SystemMessageModel> GetToken(LoginLog user,int expireday=1, string processId = "" ,string clientip = "")
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            if (string.IsNullOrEmpty(processId)) processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 202000;
            string token = string.Empty;
            var authentication = new Authentication();
            try
            {
                JwtSecurityToken jwtSecurityToken = await _jwtHandler.Create(user, expireday);

                authentication.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authentication.IsAuthenticated = true;
                //authentication.Username = user.Username;

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "ورود با موفقیت انجام شد", MessageData = authentication };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "خطا در انجام درخواست", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        /// <summary>
        /// ایجاد لاگ لاگین
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<SystemMessageModel> CreateLoginLog(User user, string processId = "", string clientip = "")
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            if (string.IsNullOrEmpty(processId)) processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 203000;
            try
            {
                return new SystemMessageModel() { MessageCode = 200, MessageDescription = "درخواست با موفقیت انجام شد", MessageData = "" };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "خطا در انجام درخواست", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
                return message;
            }
        }
        /// <summary>
        /// بررسی اعتبار توکن
        /// </summary>
        /// <param name="token"></param>
        /// <param name="processId"></param>
        /// <param name="clientip"></param>
        /// <returns></returns>
        public async Task<SystemMessageModel> CheckToken(string token, string processId = "", string clientip = "")
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            if (string.IsNullOrEmpty(processId)) processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 204000;
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return new SystemMessageModel() { MessageCode = -401, MessageDescription = "توکن معتبر نمی باشد", MessageData = token };
                }

                if (!await _jwtHandler.CkeckTokenIsValid(token))
                {
                    return new SystemMessageModel() { MessageCode = -401, MessageDescription = "توکن معتبر نمی باشد", MessageData = token };
                }

                string LoginLogId = await _jwtHandler.GetTokenParameterValue(token, "uid");

                LoginLog loginLog = await _LogContext.LoginLogs.FindAsync(new Guid(LoginLogId));

                if(loginLog == null)
                {
                    return new SystemMessageModel() { MessageCode = -401, MessageDescription = "توکن معتبر نمی باشد", MessageData = token };
                }
                if (loginLog.LogoutDate != null)
                {
                    return new SystemMessageModel() { MessageCode = -401, MessageDescription = "توکن معتبر نمی باشد", MessageData = token };
                }

                if (loginLog.LoginDate.AddMinutes(loginLog.Expiretime) < DateTime.Now)
                {
                    return new SystemMessageModel() { MessageCode = -401, MessageDescription = "توکن معتبر نمی باشد", MessageData = token };
                }

                User user = await _Context.Users.FindAsync(loginLog.UserId);
                UserModel usermodel = new UserModel() { loginlogid = new Guid(LoginLogId), organizename = "", userid = user.UserId, username = user.Username,fname = user.Fname,lname = user.Lname, UserTypeId = (long)user.UserTypeId , ispaid=user.ispaied};

                //usermodel.userservicelist = await GetUserServiceAccessIds(user.ID);

                return new SystemMessageModel() { MessageCode = 200, MessageDescription = "درخواست با موفقیت انجام شد", MessageData = usermodel };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "خطا در انجام درخواست", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
                return message;
            }
        }

        /// <summary>
        /// خروج از سیستم
        /// </summary>
        /// <param name="token"></param>
        /// <param name="processId"></param>
        /// <param name="clientip"></param>
        /// <returns></returns>
        public async Task<SystemMessageModel> SigOut(string token, string processId = "", string clientip = "")
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            if (string.IsNullOrEmpty(processId)) processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 206000;
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return new SystemMessageModel() { MessageCode = -401, MessageDescription = "توکن معتبر نمی باشد", MessageData = token };
                }

                string LoginLogId = await _jwtHandler.GetTokenParameterValue(token, "uid");

                LoginLog loginLog = await _LogContext.LoginLogs.FindAsync(new Guid(LoginLogId));

                if (loginLog == null)
                {
                    return new SystemMessageModel() { MessageCode = -401, MessageDescription = "توکن معتبر نمی باشد", MessageData = token };
                }
                loginLog.LogoutDate = DateTime.Now;

                User user = await _Context.Users.FindAsync(loginLog.UserId);

                _LogContext.LoginLogs.Update(loginLog);

                await _Context.SaveChangesAsync();
                await _LogContext.SaveChangesAsync();

                return new SystemMessageModel() { MessageCode = 200, MessageDescription = "کاربر با موفقیت خارج شد", MessageData = new { username = user.Username } };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "خطا در انجام درخواست", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
                return message;
            }
        }

        public async Task<SystemMessageModel> GetUserToken(string token, string processId = "", string clientip = "")
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            if (string.IsNullOrEmpty(processId)) processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 205000;
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return new SystemMessageModel() { MessageCode = -401, MessageDescription = "توکن معتبر نمی باشد", MessageData = token };
                }

                string LoginLogId = await _jwtHandler.GetTokenParameterValue(token, "uid");

                LoginLog loginLog = await _LogContext.LoginLogs.FindAsync(new Guid(LoginLogId));

                if (loginLog == null)
                {
                    return new SystemMessageModel() { MessageCode = -401, MessageDescription = "توکن معتبر نمی باشد", MessageData = token };
                }

                User user = await _Context.Users.FindAsync(loginLog.UserId);
                UserModel usermodel = new UserModel() { loginlogid = new Guid(LoginLogId), organizename = "", userid = user.UserId, username = user.Username };

                //usermodel.userservicelist = await GetUserServiceAccessIds(user.ID);

                return new SystemMessageModel() { MessageCode = 200, MessageDescription = "درخواست با موفقیت انجام شد", MessageData = usermodel };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "خطا در انجام درخواست", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
                return message;
            }
        }
        /*
        public async Task<List<Service>> GetUserServiceAccessList(int userId)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            if (string.IsNullOrEmpty(processId)) processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;

            try
            {
                List<int> servidIds = await _Context.UserAccesses.Where(x => x.UserID == userId).Select(X => X.ServiceID).ToListAsync();
                return await _Context.Services.Where(x => servidIds.Contains(x.ID)).ToListAsync();
            }
            catch (Exception ex)
            {
                string error = $"ErrorLocation:{methodpath},ProccessId={processId},ErrorMessage={JsonConvert.SerializeObject(ex)}";
                return null;
            }
        }

        public async Task<List<int>> GetUserServiceAccessIds(int userId)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            if (string.IsNullOrEmpty(processId)) processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;

            try
            {
                return await _Context.UserAccesses.Where(x => x.UserID == userId).Select(X => X.ServiceID).ToListAsync();
            }
            catch (Exception ex)
            {
                string error = $"ErrorLocation:{methodpath},ProccessId={processId},ErrorMessage={JsonConvert.SerializeObject(ex)}";
                return null;
            }
        }

        public async Task<SystemMessageModel> CheckUserAccess(string token, int ServiceCode)
        {
            SystemMessageModel message = await CheckToken(token);
            if (message.MessageCode < 0)
                return message;

            UserModel user = message.MessageData as UserModel;

            if (user.userservicelist == null || user.userservicelist.Count == 0)
                return new SystemMessageModel() { MessageCode = -401, MessageDescription = "شما درسترسی به این سرویس را ندارید" };

            if (user.userservicelist.Contains(ServiceCode))
                return new SystemMessageModel() { MessageCode = 200, MessageDescription = "درسترسی دارد" };
            else
                return new SystemMessageModel() { MessageCode = -401, MessageDescription = "شما درسترسی به این سرویس را ندارید" };

        }
        */
    }
}
