using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DataLayers;
using System.Diagnostics;
using Entities.Dtos;

using Microsoft.Extensions.Configuration;
using Base.Common.Enums;
using System.Collections.Generic;
using NextTradeAPIs.Dtos;
using RestSharp;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Entities.DBEntities;
using System.Net.Sockets;
using DocumentFormat.OpenXml.InkML;
using Entities.Systems;
using NextTradeAPIs.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Reflection.Metadata.Ecma335;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Linq;

namespace NextTradeAPIs.Services
{
    public class UserServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;
        public UserServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
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
                UserModel user = await _Context.Users.Where(x => x.Username == username && x.Password == password).Select(x => new UserModel()
                {
                    userid = x.UserId,
                    username = x.Username,
                    fname = x.Fname,
                    lname = x.Lname,
                    IsActive = x.IsActive,
                    ispaid = x.ispaied,
                    UserTypeId = x.UserTypeId
                }).FirstOrDefaultAsync();

                if (user == null)
                {
                    message = new SystemMessageModel() { MessageCode = -300, MessageDescription = "کاربر موجود نمی باشد", MessageData = null };
                }
                else
                {
                    if (user.IsActive)
                        message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = user };
                    else
                        message = new SystemMessageModel() { MessageCode = -301, MessageDescription = "کاربر غیر فعال می باشد", MessageData = null };
                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
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
                User user = await _Context.Users.Where(x => x.UserId == userid).FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
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
                    message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = user };
                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
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
                    IsActive = true,
                    IsDelete = false,
                    IsAccepted = false,
                    registerDate = DateTime.Now,
                    PersonId = person.PersonId,
                    Mobile = model.Mobile,
                    UserTypeId = (model.UserTypeId != null) ? model.UserTypeId : (long)UserTypes.Student
                };

                _Context.Users.Add(data);

                await _Context.SaveChangesAsync();

                model.userid = data.UserId;

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = model };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
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
                    Mobile = model.Mobile,
                    cityId = model.cityId,
                    countryId = model.countryId,
                    stateId = model.stateId
                };
                _Context.People.Add(person);

                await _Context.SaveChangesAsync();


                User data = new User()
                {
                    ParentUserId = (model.parentuserId != null) ? 1 : model.parentuserId,
                    Username = model.username,
                    Password = model.Password,
                    Email = model.Email,
                    Fname = model.Fname,
                    Lname = model.Lname,
                    IsActive = true,
                    IsDelete = false,
                    IsAccepted = false,
                    ispaied = false,
                    registerDate = DateTime.Now,
                    PersonId = person.PersonId,
                    Mobile = model.Mobile,
                    UserTypeId = (model.UserTypeId != null) ? model.UserTypeId : (long)UserTypes.Student,
                    financialinstrumentIds = (model.financialinstrumentIds== null)?null: string.Join(",",(List<int>)model.financialinstrumentIds),
                    forexexperiencelevelId = model.forexexperiencelevelId,
                    trainingmethodIds = (model.trainingmethodIds == null) ? null : string.Join(",", (List<int>)model.trainingmethodIds),
                    targettrainerIds = (model.targettrainerIds == null) ? null : string.Join(",", (List<int>)model.targettrainerIds) ,
                    interestforexId = model.interestforexId,
                    hobbyoftradingfulltime = model.hobbyoftradingfulltime
                };

                _Context.Users.Add(data);

                await _Context.SaveChangesAsync();

                model.userid = data.UserId;

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = model };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
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
                    IsActive = true,
                    IsDelete = false,
                    IsAccepted = false,
                    registerDate = DateTime.Now,
                    PersonId = person.PersonId,
                    Mobile = mobile,
                    UserTypeId = (long)UserTypes.Student
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

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = model };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
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
                    _Context.Users.Add(data);

                    await _Context.SaveChangesAsync();
                }

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = username };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
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

                    if (user.ParentUserId != null && user.UserTypeId == (long)UserTypes.Student)
                    {
                        foreach (LoginLog item in ativetokens) item.LogoutDate = DateTime.Now;
                        _LogContext.LoginLogs.UpdateRange(ativetokens);

                    }
                    else if (ativetokens != null && ativetokens.Count > 0)
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
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
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
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
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
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
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
                    UserTypeId = x.UserTypeId,
                    IsActive = x.IsActive,
                    ispaid = x.ispaied,
                }).ToListAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = datas };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
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
                User user = await _Context.Users.Where(x => x.Username == username)
                        .Include(x => x.forexexperiencelevel)
                        .Include(x => x.interestforex)
                        .Include(x => x.UserType)
                        .FirstOrDefaultAsync();

                if (user == null)
                {
                    message = new SystemMessageModel() { MessageCode = -300, MessageDescription = "کاربر موجود نمی باشد", MessageData = null };
                }
                else
                {
                    Person person = await _Context.People.Where(x => x.PersonId == user.PersonId)
                            .Include(x => x.city)
                            .Include(x => x.state)
                            .Include(x => x.country)
                            .SingleOrDefaultAsync();



                    UserPersonModel userModel = null;
                    if (person != null)
                    {
                        userModel = new UserPersonModel()
                        {
                            fname = person.FName,
                            lname = person.LName,
                            username = user.Username,
                            userid = user.UserId,
                            BirthDate = person.BirthDate,
                            Companyname = person.Companyname,
                            companyregisterdate = "",
                            legalNationalCode = person.legalNationalCode,
                            Mobile = person.Mobile,
                            PersonTypeId = person.PersonTypeId,
                            taxcode = person.taxcode,
                            address = person.Address,
                            sex = person.Sex,
                            email = user.Email,
                            cityId = person.cityId,
                            countryId = person.countryId,
                            stateId = person.stateId,
                            cityname = (person.city != null) ? person.city.name : "",
                            statename = (person.state != null) ? person.state.name : "",
                            countryname = (person.country != null) ? person.country.name : "",
                            interestforexId = user.interestforexId,
                            interestforexname = (user.interestforex != null) ? user.interestforex.name : "",
                            hobbyoftradingfulltime = user.hobbyoftradingfulltime,
                            forexexperiencelevelId = user.forexexperiencelevelId,
                            forexexperiencelevelname = (user.forexexperiencelevel != null) ? user.forexexperiencelevel.name : ""
                        };
                        List<FinancialInstrumentDto> financialinstruments = null;
                        if (!string.IsNullOrEmpty(user.financialinstrumentIds))
                            financialinstruments = await _Context.FinancialInstruments.Where(x => user.financialinstrumentIds.Contains(x.Id.ToString())).Select(x=> new FinancialInstrumentDto() { Id = x.Id, name = x.name}).ToListAsync();
                        userModel.financialinstruments = financialinstruments;


                        List<TrainingMethodDto> trainingmethods = null;
                        if (!string.IsNullOrEmpty(user.financialinstrumentIds))
                            trainingmethods = await _Context.TrainingMethods.Where(x => user.trainingmethodIds.Contains(x.Id.ToString())).Select(x => new TrainingMethodDto() { Id = x.Id, name = x.name }).ToListAsync();
                        userModel.trainingmethods = trainingmethods;


                        List<TargetTrainerDto> targettrainers = null;
                        if (!string.IsNullOrEmpty(user.targettrainerIds))
                            targettrainers = await _Context.TrainingMethods.Where(x => user.targettrainerIds.Contains(x.Id.ToString())).Select(x => new TargetTrainerDto() { Id = x.Id, name = x.name }).ToListAsync();
                        userModel.targettrainers = targettrainers;
                    }
                    else
                    {
                        userModel = new UserPersonModel()
                        {
                            fname = user.Fname,
                            lname = user.Lname,
                            username = user.Username,
                            userid = user.UserId,
                            BirthDate = null,
                            Companyname = null,
                            companyregisterdate = "",
                            legalNationalCode = null,
                            Mobile = user.Mobile,
                            PersonTypeId = null,
                            taxcode = null
                        };
                    }
                    if (user.IsActive)
                        message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = userModel };
                    else
                        message = new SystemMessageModel() { MessageCode = -301, MessageDescription = "کاربر غیر فعال می باشد", MessageData = userModel };
                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        /// <summary>
        /// فعال و غیر فعال کردن کاربر  
        /// </summary>
        /// <param name="username">نام کاربری</param>
        /// <param name="password">رمز عبور</param>
        /// <returns></returns>
        public async Task<SystemMessageModel> SetUserActiveOrDisactive(UserSearchModel model, UserModel userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 121000;

            try
            {
                User user = await _Context.Users.Where(x => x.Username == model.username).FirstOrDefaultAsync();

                if (user == null)
                {
                    message = new SystemMessageModel() { MessageCode = -300, MessageDescription = "کاربر موجود نمی باشد", MessageData = null };
                }
                else
                {
                    user.IsActive = (bool)model.isactive;

                    _Context.Users.Update(user);
                    await _Context.SaveChangesAsync();

                    if (user.IsActive)
                        message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = "user actied" };
                    else
                        message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = "user disactied" };
                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
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
                    if (user.IsActive)
                        message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = user };
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
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
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
                                _Context.Users.Update(user);

                                await _Context.SaveChangesAsync();
                            }
                        }
                        message = new SystemMessageModel() { MessageCode = 200, MessageData = identityResult, MessageDescription = "Request Compeleted Successfully" };

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
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
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
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
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
                    return new SystemMessageModel() { MessageCode = -220, MessageDescription = "Error In doing Request", MessageData = "شماره موبایل ارسالی صحیح نیست" };
                }
                else
                {
                    data.ActiveCode = Base.Common.Generator.KeyGenerator.Generate(4, 4);
                    data.Password = data.ActiveCode;
                    data.ActiveCodeExpire = DateTime.Now.AddMinutes(15);
                    _Context.Users.Update(data);
                    await _Context.SaveChangesAsync();


                    //SMSDto smsmodel = new SMSDto()
                    //{
                    //    sourcenumber = string.Empty,
                    //    distinationnumber = data.Mobile,
                    //    //smsbody = " your activationcode for atlas ewallet is : " + data.ActiveCode,
                    //    smsbody = data.ActiveCode,
                    //};

                    //return await _sMSService.ActionManagment(smsmodel, 1, "", "", processId, null, "");

                    //return new SystemMessage() { returncode = 1, returndescription = "Request Compeleted Successfully", returnvalue = data.ActiveCode };
                    return null;
                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 401) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
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
                    return new SystemMessageModel() { MessageCode = -220, MessageDescription = "Error In doing Request", MessageData = "کاربر صحیح نیست" };
                }
                else
                {
                    data.ActiveCode = string.Empty;
                    data.ActiveCodeExpire = null;

                    _Context.Users.Update(data);
                    await _Context.SaveChangesAsync();


                    return new SystemMessageModel() { MessageCode = 1, MessageDescription = "Request Compeleted Successfully", MessageData = data.ActiveCode };

                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 401) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
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
                    return new SystemMessageModel() { MessageCode = -220, MessageDescription = "Error In doing Request", MessageData = "کاربر صحیح نیست" };
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


                    return new SystemMessageModel() { MessageCode = 1, MessageDescription = "Request Compeleted Successfully", MessageData = data.ActiveCode };

                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
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
                    return new SystemMessageModel() { MessageCode = -220, MessageDescription = "Error In doing Request", MessageData = "کاربر صحیح نیست" };
                }
                else
                {
                    Person _pseron = _Context.People.SingleOrDefault(x => x.PersonId == data.PersonId);
                    if (string.IsNullOrEmpty(data.Mobile) || _pseron.BirthDate == null)
                    {
                        return new SystemMessageModel() { MessageCode = -221, MessageDescription = "به علت ناقص بودن اطلاعات کاربر امکان تایید وجود ندارد", MessageData = new { username = username } };
                    }

                    data.ActiveCode = string.Empty;
                    data.IsActive = true;
                    data.IsAccepted = true;

                    _Context.Users.Update(data);
                    await _Context.SaveChangesAsync();


                    return new SystemMessageModel() { MessageCode = 1, MessageDescription = "Request Compeleted Successfully", MessageData = new { username = username } };

                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
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
                _user.Fname = person.FName = model.fname;
                _user.Lname = person.LName = model.lname;
                person.BirthDate = model.BirthDate;
                person.Sex = model.sex;
                person.postalcode = model.postalcode;
                person.BirthDate = model.BirthDate;
                person.Address = model.address;


                _Context.People.Update(person);
                _Context.Users.Update(_user);
                await _Context.SaveChangesAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = model };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> ChangeUserPassword(UserDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 11300;

            try
            {
                User data = null;
                if (!string.IsNullOrEmpty(model.username))
                {
                    data = await _Context.Users.Where(x => x.Username == model.username).SingleOrDefaultAsync();
                }
                else
                {
                    data = await _Context.Users.FindAsync(userlogin.userid);
                }
                if (data.Password == model.oldpassword)
                    data.Password = model.newpassword;
                else
                {
                    return new SystemMessageModel() { MessageCode = 403, MessageDescription = "old password is wrong", MessageData = model };
                }
                _Context.Users.Update(data);

                await _Context.SaveChangesAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = model };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> GetUserListByReferralCode(UserSearchModel filter, UserModel? userlogin, string processId, string clientip, string hosturl)
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
                {
                    User parentuser = await _Context.Users.Where(x => x.Username == filter.username).SingleOrDefaultAsync();
                    if (parentuser != null)
                        query = query.Where(x => x.ParentUserId == parentuser.UserId);
                }
                else
                    query = query.Where(x => x.ParentUserId == userlogin.userid);

                int pageIndex = (filter.pageindex == null || filter.pageindex == 0) ? 1 : (int)filter.pageindex;
                int PageRowCount = (filter.rowcount == null || filter.rowcount == 0) ? 50 : (int)filter.rowcount;

                datas = await query.Select(x => new UserModel()
                {
                    userid = x.UserId,
                    username = x.Username,
                    fname = x.Fname,
                    lname = x.Lname,
                    UserTypeId = x.UserTypeId,
                    IsActive = x.IsActive,
                    ispaid = x.ispaied,
                }).ToListAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = datas };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }
    }
}
