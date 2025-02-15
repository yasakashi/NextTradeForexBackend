using Base.Common.Enums;
using DataLayers;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Entities.DBEntities;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NextTradeAPIs.Dtos;
using System.Diagnostics;

namespace NextTradeAPIs.Services
{
    public class CourseBuilderUserServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;

        private WalletServices _walletService;

        public CourseBuilderUserServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices, WalletServices walletService)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
            _walletService = walletService;
        }


        public async Task<SystemMessageModel> AddUserCourses(UserCourseMemberDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                UserCourse data = new UserCourse()
                {
                    id = (Guid)model.id,
                    courseid = (Guid)model.courseid,
                    ispaid = model.ispaid ?? false,
                    ispassed = model.ispassed ?? false,
                    isrequested = model.isrequested ?? false,
                    userid = (model.userid == null) ? userlogin.userid : (long)model.userid,
                    registerdatetime = DateTime.Now,
                };
                await _Context.UserCourses.AddAsync(data);

                await _Context.SaveChangesAsync();


                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = model.id };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> GetUserCourses(UserCourseFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<UserCourse> query = _Context.UserCourses;

                if (model.id != null)
                {
                    query = query.Where(x => x.id == model.id);
                }

                if (model.userid != null)
                    query = query.Where(x => x.userid == model.userid);
                else
                    query = query.Where(x => x.userid == userlogin.userid);


                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 50 : (int)model.rowcount;

                int totaldata = query.Count();
                if (totaldata <= 0) totaldata = 1;
                decimal pagecountd = ((decimal)totaldata / (decimal)PageRowCount);
                int pagecount = (totaldata / PageRowCount);
                pagecount = (pagecount <= 0) ? 1 : pagecount;
                if (Math.Floor(pagecountd) > 0)
                    pagecount++;

                if (model.sortitem != null)
                {
                    foreach (var item in model.sortitem)
                    {
                        if (item.ascending == null || (bool)item.ascending)
                        {
                            switch (item.fieldname.ToLower())
                            {
                                case "registerdatetime":
                                    query = query.OrderBy(x => x.registerdatetime);
                                    break;
                            };
                        }
                        else if (!(bool)item.ascending)
                        {
                            switch (item.fieldname.ToLower())
                            {
                                case "registerdatetime":
                                    query = query.OrderByDescending(x => x.registerdatetime);
                                    break;
                            };
                        }
                    }
                }

                List<UserCourseMemberDto> data = await query.Skip((pageIndex - 1) * PageRowCount).Take(PageRowCount)
                                                  .Include(x => x.course)
                                                  .Include(x => x.user)
                                                  .Select(x => new UserCourseMemberDto()
                                                  {
                                                      id = x.id,
                                                      courseid = x.courseid,
                                                      ispaid = x.ispaid,
                                                      userid = x.userid,
                                                      ispassed = x.ispassed,
                                                      isrequested = x.ispassed,
                                                      courseName = x.course.courseName,
                                                      username = x.user.Username,
                                                      userfullname = x.user.Fname ?? "" + " " + x.user.Lname ?? ""
                                                  }).ToListAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = data, Meta = new { pageIndex = pageIndex, PageRowCount = PageRowCount, totaldata = totaldata, pagecount = pagecount } };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> DeleteUserCourse(UserCourseFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.id == null)
                    return new SystemMessageModel() { MessageCode = -102, MessageDescription = "Id is Wrong" };

                UserCourse data = await _Context.UserCourses.FindAsync(model.id);

                if (data == null)
                    return new SystemMessageModel() { MessageCode = -103, MessageDescription = "data not find" };


                _Context.UserCourses.Remove(data);

                await _Context.SaveChangesAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = data };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> UpdateUserCourse(UserCourseMemberDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.id == null)
                    return new SystemMessageModel() { MessageCode = -102, MessageDescription = "Id is Wrong" };

                UserCourse data = await _Context.UserCourses.FindAsync(model.id);

                if (data == null)
                    return new SystemMessageModel() { MessageCode = -103, MessageDescription = "data not find" };

                if (model.ispaid != null)
                    data.ispaid = (bool)model.ispaid;
                if (model.isrequested != null)
                    data.isrequested = (bool)model.isrequested;
                if (model.ispassed != null)
                    data.isrequested = (bool)model.ispassed;

                _Context.UserCourses.Update(data);
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

        internal async Task<SystemMessageModel> AddUserLessenPassed(UserLessonDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            throw new NotImplementedException();
        }

        public async Task<SystemMessageModel> GetCourseStudents(CourseBuilderMemberFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl, bool v)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<CourseBuilderCourse> query = _Context.CourseBuilderCourses;
                IQueryable<UserCourse> querymemeber = _Context.UserCourses;

                if (model.Id != null)
                {
                    query = query.Where(x => x.Id == model.Id);
                }

                if (model.courseId != null)
                {
                    query = query.Where(x => x.Id == model.courseId);
                    querymemeber = querymemeber.Where(x => x.courseid == model.courseId);
                }
                if (model.fromregisterdatetime != null)
                    query = query.Where(x => x.registerdatetime >= model.fromregisterdatetime);

                if (model.toregisterdatetime != null)
                    query = query.Where(x => x.registerdatetime <= model.toregisterdatetime);

                if (!string.IsNullOrEmpty(model.courseName))
                    query = query.Where(x => x.courseName.Contains(model.courseName.Trim()));

                List<Guid> courseIds = await query.Select(x => x.Id).ToListAsync();
                querymemeber = querymemeber.Where(x => courseIds.Contains(x.courseid));

                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 50 : (int)model.rowcount;

                int totaldata = querymemeber.Count();
                if (totaldata <= 0) totaldata = 1;
                decimal pagecountd = ((decimal)totaldata / (decimal)PageRowCount);
                int pagecount = (totaldata / PageRowCount);
                pagecount = (pagecount <= 0) ? 1 : pagecount;
                if (Math.Floor(pagecountd) > 0)
                    pagecount++;

                if (model.sortitem != null)
                {
                    foreach (var item in model.sortitem)
                    {
                        if (item.ascending == null || (bool)item.ascending)
                        {
                            switch (item.fieldname.ToLower())
                            {
                                case "registerdatetime":
                                    query = query.OrderBy(x => x.registerdatetime);
                                    break;
                                case "coursename":
                                    query = query.OrderBy(x => x.courseName);
                                    break;
                            };
                        }
                        else if (!(bool)item.ascending)
                        {
                            switch (item.fieldname.ToLower())
                            {
                                case "registerdatetime":
                                    query = query.OrderByDescending(x => x.registerdatetime);
                                    break;
                                case "coursename":
                                    query = query.OrderByDescending(x => x.courseName);
                                    break;
                            };
                        }
                    }
                }

                List<UserCourseMemberDto> data = await querymemeber.Skip((pageIndex - 1) * PageRowCount).Take(PageRowCount)
                                                  .Include(x => x.user)
                                                  .Select(x => new UserCourseMemberDto()
                                                  {
                                                      id = x.id,
                                                      username = x.user.Username,
                                                      userid = x.userid,
                                                      courseid = x.courseid,
                                                      courseName = x.course.courseName,
                                                      fname = x.user.Fname,
                                                      lname = x.user.Lname,
                                                      emial = x.user.Email,
                                                      registerdatetime = x.registerdatetime,
                                                      profilePic = x.user.userpicurl
                                                  }).ToListAsync();


                foreach (UserCourseMemberDto item in data)
                {
                    item.courseTakenCount = _Context.CourseMemebers.Where(x => x.userId == item.userid).Count();
                }
                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = data, Meta = new { pageIndex = pageIndex, PageRowCount = PageRowCount, totaldata = totaldata, pagecount = pagecount } };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;

        }

        public async Task<SystemMessageModel> GetCourseInstractorReport( UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            List<AdminPaneCourselInstractorReportDto> datas = null;
            long SerrvieCode = 120000;

            string username = string.Empty;
            try
            {


                string querystrnig = $"EXECUTE dbo.spGetAdminPanel_CourseInstractorReport ";
                datas = await _Context.Database.SqlQueryRaw<AdminPaneCourselInstractorReportDto>(querystrnig).ToListAsync();

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


        public async Task<SystemMessageModel> GetCourseInstructors(CourseBuilderMemberFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl, bool v)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                var query = from user in _Context.Users
                            where user.UserTypeId == (int) UserTypes.Instructor
                            join course in _Context.CourseBuilderCourses
                                on user.UserId equals course.authorId
                            where
                                (model.fromregisterdatetime == null || course.registerdatetime >= model.fromregisterdatetime) &&
                                (model.toregisterdatetime == null || course.registerdatetime <= model.toregisterdatetime) &&
                                (string.IsNullOrEmpty( model.courseName)||   course.courseName.Contains(model.courseName))
                            select new { user, course } ;

                var result = query.ToList();



                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 50 : (int)model.rowcount;

                int totaldata = query.Count();
                if (totaldata <= 0) totaldata = 1;
                decimal pagecountd = ((decimal)totaldata / (decimal)PageRowCount);
                int pagecount = (totaldata / PageRowCount);
                pagecount = (pagecount <= 0) ? 1 : pagecount;
                if (Math.Floor(pagecountd) > 0)
                    pagecount++;

                if (model.sortitem != null)
                {
                    foreach (var item in model.sortitem)
                    {
                        if (item.ascending == null || (bool)item.ascending)
                        {
                            switch (item.fieldname.ToLower())
                            {
                                case "registerdatetime":
                                    query = query.OrderBy(x => x.course.registerdatetime);
                                    break;
                                case "coursename":
                                    query = query.OrderBy(x => x.course.courseName);
                                    break;
                            };
                        }
                        else if (!(bool)item.ascending)
                        {
                            switch (item.fieldname.ToLower())
                            {
                                case "registerdatetime":
                                    query = query.OrderByDescending(x => x.course.registerdatetime);
                                    break;
                                case "coursename":
                                    query = query.OrderByDescending(x => x.course.courseName);
                                    break;
                            };
                        }
                    }
                }

                List<UserCourseMemberDto> data = await query.Skip((pageIndex - 1) * PageRowCount).Take(PageRowCount)
                                                  .Select(x => new UserCourseMemberDto()
                                                  {
                                                      username = x.user.Username,
                                                      userid = x.user.UserId,
                                                      courseid = x.course.Id,
                                                      courseName = x.course.courseName,
                                                      fname = x.user.Fname,
                                                      lname = x.user.Lname,
                                                      emial = x.user.Email,
                                                      registerdatetime = x.course.registerdatetime,
                                                      profilePic = x.user.userpicurl,
                                                  }).ToListAsync();


                foreach (UserCourseMemberDto item in data)
                {
                    item.totalCourses = _Context.CourseMemebers.Where(x => x.userId == item.userid).Count();
                    CourseBuilderCourse _course = await _Context.CourseBuilderCourses.Where(x => x.Id == item.courseid).Include(x => x.coursestatus).FirstOrDefaultAsync();
                    item.coursestatusid = _course.coursestatusid;
                    item.coursestatusname = _course.coursestatus.name;
                }
                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = data, Meta = new { pageIndex = pageIndex, PageRowCount = PageRowCount, totaldata = totaldata, pagecount = pagecount } };
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
