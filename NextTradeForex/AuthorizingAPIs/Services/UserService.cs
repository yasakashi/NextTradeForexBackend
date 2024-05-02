using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DataLayers;
using System.Diagnostics;
using Entities.Dtos;

using Microsoft.Extensions.Configuration;
using Base.Common.Enums;
using System.Collections.Generic;
using AuthorizingAPIs.Dtos;
using RestSharp;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Entities.DBEntities;
using System.Net.Sockets;
using DocumentFormat.OpenXml.InkML;
using Entities.Systems;
using AtlasCoreAPI.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Reflection.Metadata.Ecma335;

namespace AuthorizingAPIs.Services
{
    public class UserServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;
        private SMSServices _sMSService;
        public UserServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices, SMSServices sMSService)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
            _sMSService = sMSService;
        }

        /// <summary>
        /// دریافت کاربر با کلمه کاربری و رمز عبور 
        /// </summary>
        /// <param name="username">نام کاربری</param>
        /// <param name="password">رمز عبور</param>
        /// <returns></returns>
        public async Task<SystemMessageModel> GetUser(string username, string password, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 111000;
            try
            {
                UserModel user = await _Context.Users.Where(x => x.Username == username && x.Password == password).Select(x=> new UserModel() { 
                    userid = x.UserId,
                    username = x.Username,
                    iskyc = x.IsKYCAccepted,
                    fname = x.Fname,
                    lname = x.Lname,
                    IsActive = x.IsActive
                }).FirstOrDefaultAsync();

                if (user == null)
                {
                    message = new SystemMessageModel() { MessageCode = -300, MessageDescription = "کاربر موجود نمی باشد", MessageData = null };
                }
                else
                {
                    if (user.IsActive)
                        message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "درخواست با موفقیت انجام شد", MessageData = user };
                    else
                        message = new SystemMessageModel() { MessageCode = -301, MessageDescription = "کاربر غیر فعال می باشد", MessageData = null };
                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "خطا در انجام درخواست", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }


        public async Task<User> GetUserById(long userid, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 111000;
            try
            {
                User user = await _Context.Users.Where(x => x.UserId == userid ).FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "خطا در انجام درخواست", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
                return null;
            }
        }

        public async Task<SystemMessageModel> GetUser4Check(string username, string password, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 112000;

            try
            {
                User user = await _Context.Users.Where(x => x.Username == username).FirstOrDefaultAsync();

                if (user == null)
                {
                    message = new SystemMessageModel() { MessageCode = -300, MessageDescription = "کاربر موجود نمی باشد", MessageData = null };
                }
                else
                {
                    message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "درخواست با موفقیت انجام شد", MessageData = user };
                }
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
        /// ثبت کاربر 
        /// </summary>
        /// <param name="model">نام کاربری</param>
        /// <param name="processId">رمز عبور</param>
        /// <returns></returns>
        public async Task<SystemMessageModel> SaveUser(UserRegisterModel model, string processId, string clientip, string hosturl, string token)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 11300;

            try
            {
                Person person = new Person()
                {
                    BirthDate = model.BirthDate,
                    PersonTypeId = model.PersonTypeId,
                    FName = model.Fname,
                    LName = model.Lname,
                    Nationalcode = model.Nationalcode,
                    Sex = model.Sex ?? 1,
                    Fathername = model.fathername ?? "",
                    MarriedStatusId = model.marriedstatusid,
                    Companyname = model.companyname,
                    taxcode = model.taxcode,
                    FamilyCount = model.familycount ?? 0,
                    Address = model.Address,
                    legaladdress = model.legaladdress,
                    telephone = model.telephone,
                    postalcode = model.postalcode,
                    legalNationalCode = model.legalnationalcode,
                    Mobile = model.Mobile
                };
                _Context.People.Add(person);

                await _Context.SaveChangesAsync();


                User data = new User()
                {
                    Username = model.username,
                    Password = model.Password,
                    Email = model.Email,
                    Fname = model.Fname,
                    Lname = model.Lname,
                    Nationalcode = model.Nationalcode,
                    IsActive = true,
                    IsDelete = false,
                    IsAccepted = false,
                    IsKYCAccepted = false,
                    IsShahinKYCAccepted = false,
                    registerDate = DateTime.Now,
                    PersonId = person.PersonId,
                    Mobile = model.Mobile,
                    UserTypeId = (long)UserTypes.NormalUser
                };

                _Context.Users.Add(data);

                await _Context.SaveChangesAsync();

                model.userid = data.UserId;

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "درخواست با موفقیت انجام شد", MessageData = model };
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
        /// ثبت کاربر
        /// </summary>
        /// <param name="model">اطلاعات کاربر</param>
        /// <param name="userloginmodel">اطلاعات توکن درخواست کننده</param>
        /// <param name="processId">شناسه درخواست</param>
        /// <param name="clientip">آدرس درخواست کننده</param>
        /// <param name="hosturl">آدرس سرور اجرا کننده</param>
        /// <param name="token">توکن درخواست کننده</param>
        /// <returns></returns>
        public async Task<SystemMessageModel> SaveUser(UserRegisterModel model, UserModel userloginmodel, string processId, string clientip, string hosturl, string token)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 114000;

            try
            {
                if (await UserExist(model.username, processId))
                    return new SystemMessageModel() { MessageCode = -512, MessageDescription = "کاربر تکراری است", MessageData = model };

                Person person = new Person()
                {
                    BirthDate = model.BirthDate,
                    PersonTypeId = model.PersonTypeId,
                    FName = model.Fname,
                    LName = model.Lname,
                    Nationalcode = model.Nationalcode,
                    Sex = model.Sex ?? 1,
                    Fathername = model.fathername,
                    MarriedStatusId = model.marriedstatusid,
                    Companyname = model.companyname,
                    taxcode = model.taxcode,
                    FamilyCount = model.familycount ?? 0,
                    Address = model.Address,
                    legaladdress = model.legaladdress,
                    telephone = model.telephone,
                    postalcode = model.postalcode,
                    legalNationalCode = model.legalnationalcode,
                    Mobile = model.Mobile
                };
                _Context.People.Add(person);

                await _Context.SaveChangesAsync();


                User data = new User()
                {
                    ParentUserId = (userloginmodel != null) ? userloginmodel.userid : null,
                    Username = model.username,
                    Password = model.Password,
                    Email = model.Email,
                    Fname = model.Fname,
                    Lname = model.Lname,
                    Nationalcode = model.Nationalcode,
                    IsActive = true,
                    IsDelete = false,
                    IsAccepted = false,
                    IsKYCAccepted = false,
                    IsShahinKYCAccepted = false,
                    registerDate = DateTime.Now,
                    PersonId = person.PersonId,
                    Mobile = model.Mobile,
                    UserTypeId = (long)UserTypes.NormalUser
                };

                _Context.Users.Add(data);

                await _Context.SaveChangesAsync();

                model.userid = data.UserId;

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "درخواست با موفقیت انجام شد", MessageData = model };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "خطا در انجام درخواست", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }
        public async Task<SystemMessageModel> SaveUserWithMobile(string mobile, UserModel userloginmodel, string processId, string clientip, string hosturl, string token)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 115000;

            try
            {
                if (await UserExist(mobile, processId))
                    return new SystemMessageModel() { MessageCode = 512, MessageDescription = "کاربر تکراری است", MessageData = mobile };

                if (!mobile.StartsWith("0")) mobile = "0" + mobile;

                Person person = new Person()
                {
                    BirthDate = null,
                    PersonTypeId = (long)PersonTypes.RealPerson,
                    FName = null,
                    LName = null,
                    Nationalcode = null,
                    Sex = null,
                    Fathername = null,
                    MarriedStatusId = null,
                    Companyname = null,
                    taxcode = null,
                    FamilyCount = null,
                    Address = null,
                    legaladdress = null,
                    telephone = null,
                    postalcode = null,
                    legalNationalCode = null,
                    Mobile = mobile
                };
                _Context.People.Add(person);

                await _Context.SaveChangesAsync();

                User data = new User()
                {
                    ParentUserId = (userloginmodel != null) ? userloginmodel.userid : null,
                    Username = mobile,
                    Password = mobile,
                    Email = null,
                    Fname = null,
                    Lname = null,
                    Nationalcode = null,
                    IsActive = true,
                    IsDelete = false,
                    IsAccepted = false,
                    IsKYCAccepted = false,
                    IsShahinKYCAccepted = false,
                    registerDate = DateTime.Now,
                    PersonId = person.PersonId,
                    Mobile = mobile,
                    UserTypeId = (long)UserTypes.NormalUser
                };

                _Context.Users.Add(data);

                await _Context.SaveChangesAsync();

                UserRegisterModel model = new UserRegisterModel()
                {
                    userid = data.UserId,
                    username = mobile,
                    Password = mobile,
                    Mobile = mobile
                };

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "درخواست با موفقیت انجام شد", MessageData = model };
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
        ///  ثبت تایید کاربر
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userloginmodel">اطلاعات توکن درخواست کننده</param>
        /// <param name="processId">شناسه درخواست</param>
        /// <param name="clientip">آدرس درخواست کننده</param>
        /// <param name="hosturl">آدرس سرور اجرا کننده</param>
        /// <param name="token">توکن درخواست کننده</param>
        /// <returns></returns>
        public async Task<SystemMessageModel> KYCUserAccepted(string username, UserModel userloginmodel, string processId, string clientip, string hosturl, string token)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 116000;

            try
            {
                User data = await _Context.Users.Where(x => x.Username == username).FirstOrDefaultAsync();
                if (data != null)
                {
                    data.IsAccepted = true;
                    data.IsKYCAccepted = true;
                    data.IsShahinKYCAccepted = true;

                    _Context.Users.Add(data);

                    await _Context.SaveChangesAsync();
                }

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "درخواست با موفقیت انجام شد", MessageData = username };
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
        /// اضافه کردن لاگ برای لاگین کاربر
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<SystemMessageModel> InsertLoginLog(UserModel user, string clientip, string context, string processId, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            LoginLog loginlog = null;
            long SerrvieCode = 117000;

            try
            {
                using (var dbContextTransaction = _LogContext.Database.BeginTransaction())
                {
                    List<LoginLog> ativetokens = await _LogContext.LoginLogs.Where(x => x.UserId == user.userid && x.LogoutDate == null).ToListAsync();

                    if (user.ParentUserId != null && user.UserTypeId == (long)UserTypes.NormalUser)
                    {
                        foreach (LoginLog item in ativetokens) item.LogoutDate = DateTime.Now;
                        _LogContext.LoginLogs.UpdateRange(ativetokens);

                    }
                    else if(ativetokens!= null && ativetokens.Count > 0)
                    {
                        loginlog = ativetokens.OrderByDescending(x => x.LoginDate).First();
                    }
                    if (loginlog == null)
                    {
                        loginlog = new LoginLog()
                        {
                            ClientIp = clientip,
                            LoginDate = DateTime.Now,
                            Expiretime = Convert.ToInt64(_config["JWT:ExpireMin"]),
                            UserId = user.userid,
                            Username = user.username,
                            SesstionInfo = context,
                            LoginIsSuccessfull = true,
                            Token = string.Empty
                        };

                        _LogContext.LoginLogs.Add(loginlog);

                        await _LogContext.SaveChangesAsync();

                        await dbContextTransaction.CommitAsync();
                    }

                }

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "ورود با موفقیت انجام شد", MessageData = loginlog };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "خطا در انجام درخواست", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> InsertLoginLog(string username, string clientip, string context, string processId, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 118000;

            try
            {
                LoginLog loginlog = new LoginLog()
                {
                    ClientIp = clientip,
                    LoginDate = DateTime.Now,
                    Expiretime = Convert.ToInt64(_config["JWT:ExpireMin"]),
                    UserId = null,
                    Username = username,
                    SesstionInfo = context,
                    LoginIsSuccessfull = false,
                    Token = string.Empty
                };

                _LogContext.LoginLogs.Add(loginlog);

                await _LogContext.SaveChangesAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "ورود با موفقیت انجام شد", MessageData = loginlog };
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
        /// به روز رسانی و اضافه کردن  لاگین پس از تغییر
        /// </summary>
        /// <param name="loginlog"></param>
        /// <returns></returns>
        public async Task<SystemMessageModel> UpdateLoginLog(LoginLog loginlog, string processId, string clientip = "")
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 119000;

            try
            {

                _LogContext.LoginLogs.Update(loginlog);
                await _LogContext.SaveChangesAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "ورود با موفقیت انجام شد", MessageData = loginlog };
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
        /// جستجوی کاربران 
        /// </summary>
        /// <param name="filter">اطلاعات دریافت کاربران</param>
        /// <param name="password">رمز عبور</param>
        /// <returns></returns>
        public async Task<SystemMessageModel> GetUsers(UserSearchModel filter, UserModel userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            List<UserModel> datas = null;
            long SerrvieCode = 120000;

            try
            {
                IQueryable<User> query = _Context.Users;

                if (!string.IsNullOrEmpty(filter.username))
                    query = query.Where(x => x.Username == filter.username);

                if (!string.IsNullOrEmpty(filter.mobile))
                    query = query.Where(x => x.Mobile == filter.mobile);

                if (!string.IsNullOrEmpty(filter.nationalcode))
                    query = query.Where(x => x.Nationalcode == filter.nationalcode);

                if (!string.IsNullOrEmpty(filter.fname))
                    query = query.Where(x => x.Fname.StartsWith(filter.fname));

                if (!string.IsNullOrEmpty(filter.lname))
                    query = query.Where(x => x.Lname.StartsWith(filter.lname));

                datas = await query.Select(x => new UserModel()
                {
                    userid = x.UserId,
                    username = x.Username,
                    fname = x.Fname,
                    lname = x.Lname,
                    iskyc = x.IsKYCAccepted,
                }).ToListAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "درخواست با موفقیت انجام شد", MessageData = datas };
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
        /// دریافت کاربر با کلمه کاربری  
        /// </summary>
        /// <param name="username">نام کاربری</param>
        /// <param name="password">رمز عبور</param>
        /// <returns></returns>
        public async Task<SystemMessageModel> GetUserByUsername(string username, UserModel userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 121000;

            try
            {
                User user = await _Context.Users.Where(x => x.Username == username).FirstOrDefaultAsync();

                if (user == null)
                {
                    message = new SystemMessageModel() { MessageCode = -300, MessageDescription = "کاربر موجود نمی باشد", MessageData = null };
                }
                else
                {
                    Person person = await _Context.People.Where(x => x.PersonId == user.PersonId).SingleOrDefaultAsync();

                    UserPersonModel userModel = new UserPersonModel()
                    {
                        fname = person.FName,
                        iskyc = user.IsShahinKYCAccepted || user.IsKYCAccepted,
                        lname = person.LName,
                        nationalcode = user.Nationalcode,
                        username = user.Username,
                        userid = user.UserId,
                        BirthDate = person.BirthDate,
                        Companyname = person.Companyname,
                        companyregisterdate = "",
                        legalNationalCode = person.legalNationalCode,
                        Mobile = person.Mobile,
                        PersonTypeId = person.PersonTypeId,
                        taxcode = person.taxcode
                    };
                    if (user.IsActive)
                        message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "درخواست با موفقیت انجام شد", MessageData = userModel };
                    else
                        message = new SystemMessageModel() { MessageCode = -301, MessageDescription = "کاربر غیر فعال می باشد", MessageData = userModel };
                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "خطا در انجام درخواست", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> KYCUserCheck(UserSearchModel model, UserModel? userlogin, string processId, string clientip, string hosturl, string bearer_token)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 122000;

            try
            {
                User user = await _Context.Users.Where(x => x.Username == model.username).FirstOrDefaultAsync();

                if (user == null)
                {
                    message = new SystemMessageModel() { MessageCode = -300, MessageDescription = "کاربر موجود نمی باشد", MessageData = null };
                }
                else
                {
                    if (user.Nationalcode == model.nationalcode && user.IsActive)
                        message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "درخواست با موفقیت انجام شد", MessageData = user };
                    else
                    {
                        message = await CheckUserInShahkar(model, userlogin, processId, clientip, hosturl, bearer_token);
                        if (message.MessageCode > 0)
                        {
                            message = await KYCUserAccepted(model.username, userlogin, processId, clientip, hosturl, bearer_token);
                        }
                    }
                }
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
        /// Check User in Shahkar system
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userlogin"></param>
        /// <param name="processId"></param>
        /// <param name="clientip"></param>
        /// <param name="hosturl"></param>
        /// <param name="bearer_token"></param>
        /// <returns></returns>
        public async Task<SystemMessageModel> CheckUserInShahkar(UserSearchModel model, UserModel? userlogin, string processId, string clientip, string hosturl, string bearer_token)
        {
            SystemMessageModel message = new SystemMessageModel();
            StackTrace stackTrace = new StackTrace();
            if (string.IsNullOrEmpty(processId)) processId = Guid.NewGuid().ToString();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            string requesturl = "api/satpi/checknationalcodewithmobile";
            long SerrvieCode = 123000;

            try
            {
                if (string.IsNullOrEmpty(bearer_token))
                {
                    return new SystemMessageModel() { MessageCode = -401, MessageDescription = "توکن معتبر نمی باشد", MessageData = bearer_token };
                }

                ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

                RestClientOptions options = new RestClientOptions(ServiceUrlConfig.IOServiceSystem) //("http://localhost:5278/")
                {
                    RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true

                };
                RestSharp.RestClient client = new RestSharp.RestClient(options);

                RestSharp.RestRequest request = new RestSharp.RestRequest(requesturl, RestSharp.Method.Post);


                // //Header
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "*/*");
                request.AddHeader("Authorization", string.Format("Bearer {0}", bearer_token));


                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(new { mobile = model.mobile, nationalcode = model.nationalcode });

                RestSharp.RestResponse response = client.Execute(request);
                SystemMessageModel registerResponces = null;
                IdentityResultDto identityResult = null;
                try
                {
                    registerResponces = JsonConvert.DeserializeObject<SystemMessageModel>(response.Content);
                }
                catch
                { }
                try
                {
                    identityResult = registerResponces.MessageData as IdentityResultDto;
                    if (identityResult == null)
                        identityResult = JsonConvert.DeserializeObject<IdentityResultDto>(registerResponces.MessageData.ToString());
                }
                catch
                {
                    identityResult = JsonConvert.DeserializeObject<IdentityResultDto>(registerResponces.MessageData.ToString());
                }
                if (Convert.ToInt32(response.StatusCode) == 200)
                {
                    try
                    {
                        if (registerResponces.MessageCode < 0)
                        {
                            return message;
                        }

                        List<User> users = await _Context.Users.Where(x => x.Username == model.username || x.Mobile == model.mobile).ToListAsync();
                        foreach (User user in users)
                        {
                            if (user != null && identityResult.isValid)
                            {
                                user.IsShahinKYCAccepted = true;
                                user.IsKYCAccepted = true;
                                user.Nationalcode = model.nationalcode;
                                _Context.Users.Update(user);

                                await _Context.SaveChangesAsync();
                            }
                        }
                        message = new SystemMessageModel() { MessageCode = 200, MessageData = identityResult, MessageDescription = "درخواست با موفقیت انجام شد" };

                    }
                    catch (Exception ex)
                    {
                        string error = $"'ErrorLocation':'{methodpath}','ProccessId':{processId},'ErrorMessage':'{ex.Message}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                        await _systemLogServices.InsertLogs(error, processId, "", methodpath, LogTypes.SystemError, bearer_token, "");

                        message = new SystemMessageModel() { MessageCode = -401, MessageData = ex.Message, MessageDescription = "خطا در ارتباط با سرور " };
                    }
                }
                else
                {
                    message = new SystemMessageModel() { MessageCode = -401, MessageData = (identityResult == null) ? JsonConvert.SerializeObject(response) : identityResult, MessageDescription = "خطا در ارتباط با سرور " };
                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "خطا در انجام درخواست", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        private async Task<bool> UserExist(string username, string processId, string clientip = "")
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            bool isExist = false;
            long SerrvieCode = 124000;
            try
            {
                isExist = await _Context.Users.Where(x => x.Username == username).AnyAsync();
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "خطا در انجام درخواست", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return isExist;
        }

        public async Task<SystemMessageModel> CreateAndSendActiveCode(string mobile, string processId, string clientip = "")
        {
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            SystemMessageModel message = new SystemMessageModel();
            long SerrvieCode = 125000;

            try
            {
                User data = _Context.Users.SingleOrDefault(x => x.Mobile == mobile);
                if (data == null)
                {
                    return new SystemMessageModel() { MessageCode = -220, MessageDescription = "خطا در انجام درخواست", MessageData = "شماره موبایل ارسالی صحیح نیست" };
                }
                else
                {
                    data.ActiveCode = Base.Common.Generator.KeyGenerator.Generate(4, 4);
                    data.Password = data.ActiveCode;
                    data.ActiveCodeExpire = DateTime.Now.AddMinutes(15);
                    _Context.Users.Update(data);
                    await _Context.SaveChangesAsync();


                    SMSDto smsmodel = new SMSDto()
                    {
                        sourcenumber = string.Empty,
                        distinationnumber = data.Mobile,
                        //smsbody = " your activationcode for atlas ewallet is : " + data.ActiveCode,
                        smsbody = data.ActiveCode,
                    };

                    return await _sMSService.ActionManagment(smsmodel, 1, "", "", processId, null, "");

                    //return new SystemMessage() { returncode = 1, returndescription = "درخواست با موفقیت انجام شد", returnvalue = data.ActiveCode };

                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 401) * -1), MessageDescription = "خطا در انجام درخواست", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
                return message;
            }
        }

        public async Task<SystemMessageModel> ClearActivation(long userid, string processId, string clientip = "")
        {
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            SystemMessageModel message = new SystemMessageModel();
            long SerrvieCode = 126000;
            try
            {
                User data = _Context.Users.SingleOrDefault(x => x.UserId == userid);
                if (data == null)
                {
                    return new SystemMessageModel() { MessageCode = -220, MessageDescription = "خطا در انجام درخواست", MessageData = "کاربر صحیح نیست" };
                }
                else
                {
                    data.ActiveCode = string.Empty;
                    data.ActiveCodeExpire = null;

                    _Context.Users.Update(data);
                    await _Context.SaveChangesAsync();


                    return new SystemMessageModel() { MessageCode = 1, MessageDescription = "درخواست با موفقیت انجام شد", MessageData = data.ActiveCode };

                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 401) * -1), MessageDescription = "خطا در انجام درخواست", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
                return message;
            }
        }

        /// <summary>
        /// فعال کردن کاربر
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="processId"></param>
        /// <param name="clientip"></param>
        /// <returns></returns>
        public async Task<SystemMessageModel> ActiveAccount(long userid, string processId, string clientip = "")
        {
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            SystemMessageModel message = new SystemMessageModel();
            long SerrvieCode = 127000;
            try
            {
                User data = _Context.Users.SingleOrDefault(x => x.UserId == userid);
                if (data == null)
                {
                    return new SystemMessageModel() { MessageCode = -220, MessageDescription = "خطا در انجام درخواست", MessageData = "کاربر صحیح نیست" };
                }
                else
                {
                    if (data.Password == data.ActiveCode)
                        data.Password = data.Username;

                    data.ActiveCodeExpire = null;
                    data.ActiveCode = string.Empty;
                    data.IsActive = true;
                    data.IsAccepted = true;

                    _Context.Users.Update(data);
                    await _Context.SaveChangesAsync();


                    return new SystemMessageModel() { MessageCode = 1, MessageDescription = "درخواست با موفقیت انجام شد", MessageData = data.ActiveCode };

                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "خطا در انجام درخواست", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
                return message;

            }
        }
        public async Task<SystemMessageModel> AcceptUsrKYC(string username, string processId, string clientip = "")
        {
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            SystemMessageModel message = new SystemMessageModel();
            long SerrvieCode = 128000;
            try
            {
                User data = _Context.Users.FirstOrDefault(x => x.Username == username);
                if (data == null)
                {
                    return new SystemMessageModel() { MessageCode = -220, MessageDescription = "خطا در انجام درخواست", MessageData = "کاربر صحیح نیست" };
                }
                else
                {
                    Person _pseron = _Context.People.SingleOrDefault(x => x.PersonId == data.PersonId);
                    if (string.IsNullOrEmpty(data.Mobile) || string.IsNullOrEmpty(data.Nationalcode) || _pseron.BirthDate == null)
                    {
                        return new SystemMessageModel() { MessageCode = -221, MessageDescription = "به علت ناقص بودن اطلاعات کاربر امکان تایید وجود ندارد", MessageData = new { username = username } };
                    }

                    data.ActiveCode = string.Empty;
                    data.IsActive = true;
                    data.IsAccepted = true;
                    data.IsKYCAccepted = true;

                    _Context.Users.Update(data);
                    await _Context.SaveChangesAsync();


                    return new SystemMessageModel() { MessageCode = 1, MessageDescription = "درخواست با موفقیت انجام شد", MessageData = new { username = username } };

                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "خطا در انجام درخواست", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
                return message;

            }
        }

        public async Task<SystemMessageModel> UpdatePeronInfo(UserPersonModel model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                User _user = await _Context.Users.Where(x => x.UserId == userlogin.userid).SingleOrDefaultAsync();
                Person person = await _Context.People.Where(x => x.PersonId == _user.PersonId).SingleOrDefaultAsync();
                person.FName = model.fname;
                person.LName = model.lname;
                person.BirthDate = model.BirthDate;

                _Context.People.Update(person);
                await _Context.SaveChangesAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "درخواست با موفقیت انجام شد", MessageData = model };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "خطا در انجام درخواست", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }
    }
}
