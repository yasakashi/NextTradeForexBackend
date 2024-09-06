using Base.Common.Enums;
using DataLayers;
using DocumentFormat.OpenXml.Office2021.Excel.RichDataWebImage;
using DocumentFormat.OpenXml.Spreadsheet;
using Entities.DBEntities;
using Entities.Dtos;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NextTradeAPIs.Dtos;
using System.Diagnostics;

namespace NextTradeAPIs.Services
{
    public class CourseServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;

        private WalletServices _walletService;

        public CourseServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices, WalletServices walletService)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
            _walletService = walletService;
        }


        public async Task<SystemMessageModel> GetCourses(CourseDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<Course> query = _Context.Courses;

                if (model.courseStatusId != null)
                    query = query.Where(x => x.courseStatusId == model.courseStatusId);
                if (model.Id != null)
                    query = query.Where(x => x.Id == model.Id);

                if (model.owneruserId != null)
                    query = query.Where(x => x.owneruserId == model.owneruserId);

                if (model.isadminaccepted != null)
                    query = query.Where(x => x.isadminaccepted == model.isadminaccepted);

                if (model.isprelesson != null)
                    query = query.Where(x => x.isprelesson == model.isprelesson);

                if (model.issitecourse != null)
                    query = query.Where(x => x.issitecourse == model.issitecourse);

                if (model.coursetypeId != null && model.coursetypeId > 0)
                    query = query.Where(x => x.coursetypeId == model.coursetypeId);

                if (model.grouptypeId != null)
                    query = query.Where(x => x.grouptypeId == model.grouptypeId);

                if (model.courseleveltypeId != null)
                    query = query.Where(x => x.courseleveltypeId == model.courseleveltypeId);

                if (!string.IsNullOrEmpty(model.coursetgas))
                    query = query.Where(x => x.coursetgas.Contains(model.coursetgas.Trim()));

                if (!string.IsNullOrEmpty(model.coursename))
                    query = query.Where(x => x.coursename.Contains(model.coursename.Trim()));

                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 50 : (int)model.rowcount;


                List<CourseDto> data = await query.Skip((pageIndex - 1) * PageRowCount).Take(PageRowCount)
                                                  .Include(x => x.owneruser)
                                                  .Include(x => x.courseleveltype)
                                                  .Include(x => x.coursetype)
                                                  .Select(x => new CourseDto()
                                                  {
                                                      Id = x.Id,
                                                      allowdownload = x.allowdownload,
                                                      coursecoverimage = x.coursecoverimage,
                                                      coursedescription = x.coursedescription,
                                                      courseduringtime = x.courseduringtime,
                                                      coursename = x.coursename,
                                                      courseleveltypeId = x.courseleveltypeId,
                                                      courseleveltypename = x.courseleveltype.name,
                                                      courseprice = (x.courseprice == null || x.courseprice == 0) ? 0 : x.courseprice,
                                                      allowQA = (model.allowQA == null) ? false : (bool)model.allowQA,
                                                      coursetypeId = x.coursetypeId,
                                                      coursetypename = x.coursetype.name,
                                                      enddate = x.enddate,
                                                      lessencount = x.lessencount,
                                                      owneruserId = x.owneruserId,
                                                      ownerusername = x.owneruser.Username,
                                                      pageindex = pageIndex,
                                                      registerdatetime = x.registerdatetime,
                                                      rowcount = PageRowCount,
                                                      isprelesson = x.isprelesson,
                                                      isadminaccepted = x.isadminaccepted,
                                                      startdate = x.startdate,
                                                      courseintrofileurl = x.courseintrofileurl,
                                                      coursetgas = x.coursetgas,
                                                      grouptypeId = x.grouptypeId,
                                                      grouptypename = (x.grouptype != null) ? x.grouptype.name : "",
                                                      issitecourse = x.issitecourse,
                                                      courseintrofilecontenttype = x.courseintrofilecontenttype,
                                                      courseintrofilefileextention = x.courseintrofilefileextention,
                                                      whatlearn = x.whatlearn,
                                                      targetedaudience = x.targetedaudience,
                                                      materialsincluded = x.materialsincluded,
                                                      requirementsinstructions = x.requirementsinstructions
                                                  }).ToListAsync();

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

        public async Task<SystemMessageModel> AddNewCourses(CourseDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                Course data = new Course()
                {
                    Id = Guid.NewGuid(),
                    allowdownload = (bool)model.allowdownload,
                    coursecoverimage = model.coursecoverimage,
                    coursedescription = model.coursedescription,
                    courseduringtime = (int)model.courseduringtime,
                    coursename = model.coursename,
                    courseleveltypeId = (int)model.courseleveltypeId,
                    courseprice = (model.courseprice == null || model.courseprice == 0) ? 0 : (decimal)model.courseprice,
                    coursetypeId = (int)model.coursetypeId,
                    enddate = (DateTime)model.enddate,
                    lessencount = (int)model.lessencount,
                    owneruserId = userlogin.userid,
                    registerdatetime = DateTime.Now,
                    isadminaccepted = model.isadminaccepted ?? false,
                    isprelesson = model.isprelesson ?? false,
                    startdate = (DateTime)model.startdate,
                    grouptypeId = model.grouptypeId,
                    issitecourse = model.issitecourse,
                    coursetgas = model.coursetgas,
                    allowQA = (model.allowQA == null) ? false : (bool)model.allowQA,
                    whatlearn = model.whatlearn,
                    targetedaudience = model.targetedaudience,
                    materialsincluded = model.materialsincluded,
                    requirementsinstructions = model.requirementsinstructions,
                    courseStatusId = 0,
                };

                await _Context.Courses.AddAsync(data);
                await _Context.SaveChangesAsync();


                model.Id = data.Id;
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

        public async Task<SystemMessageModel> RemoveCourse(CourseDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.Id == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -1), MessageDescription = "Data is wrong", MessageData = model };
                Course data = await _Context.Courses.FindAsync(model.Id);

                if (data == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -2), MessageDescription = "Data is wrong", MessageData = model };

                _Context.Courses.Remove(data);
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

        public async Task<SystemMessageModel> EditCourses(CourseDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.Id == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -1), MessageDescription = "Data is wrong", MessageData = model };
                Course data = await _Context.Courses.FindAsync(model.Id);

                if (data == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -2), MessageDescription = "Data is wrong", MessageData = model };


                data.allowdownload = (bool)model.allowdownload;
                data.coursecoverimage = model.coursecoverimage;
                data.coursedescription = model.coursedescription;
                data.courseduringtime = (int)model.courseduringtime;
                data.coursename = model.coursename;
                data.courseleveltypeId = (int)model.courseleveltypeId;
                data.courseprice = (model.courseprice == null || model.courseprice == 0) ? 0 : (decimal)model.courseprice;
                data.coursetypeId = (int)model.coursetypeId;
                data.enddate = (DateTime)model.enddate;
                data.lessencount = (int)model.lessencount;
                data.isprelesson = model.isprelesson ?? false;
                data.startdate = (DateTime)model.startdate;
                data.isadminaccepted = (model.isadminaccepted == null) ? false : (bool)model.isadminaccepted;
                data.allowQA = (model.allowQA == null) ? false : (bool)model.allowQA;
                data.grouptypeId = model.grouptypeId;
                data.whatlearn = model.whatlearn;
                data.targetedaudience = model.targetedaudience;
                data.materialsincluded = model.materialsincluded;
                data.requirementsinstructions = model.requirementsinstructions;
                data.courseStatusId = model.courseStatusId;
                _Context.Courses.Update(data);
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

        public async Task<SystemMessageModel> UploadCourseIntroFile(CourseDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.Id == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -1), MessageDescription = "Data is wrong", MessageData = model };
                Course data = await _Context.Courses.FindAsync(model.Id);

                data.courseintrofileurl = model.courseintrofileurl;
                data.courseintrofilefileextention = model.courseintrofilefileextention;
                data.courseintrofilecontenttype = model.courseintrofilecontenttype;

                _Context.Courses.Update(data);
                await _Context.SaveChangesAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request success", MessageData = new { Id = data.Id } };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<Course> GetCourseIntoFile(Guid Id)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                Course data = await _Context.Courses.FindAsync(Id);

                return data;
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, "", "", methodpath, LogTypes.SystemError);
                return null;
            }
        }

        public async Task<SystemMessageModel> AcceptCoursesByAdmin(CourseDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.Id == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -1), MessageDescription = "Data is wrong", MessageData = model };
                Course data = await _Context.Courses.FindAsync(model.Id);

                if (data == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -2), MessageDescription = "Data is wrong", MessageData = model };


                data.isadminaccepted = (bool)model.isadminaccepted;

                _Context.Courses.Update(data);
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

        public async Task<SystemMessageModel> UpdateCourseImage(CourseDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                Course data = await _Context.Courses.FindAsync(model.Id);
                if (data != null)
                {
                    data.coursecoverimage = model.coursecoverimage;

                    _Context.Courses.Update(data);
                    await _Context.SaveChangesAsync();
                }

                CourseDto data2 = new CourseDto()
                {
                    Id = Guid.NewGuid(),
                    allowdownload = (bool)data.allowdownload,
                    coursecoverimage = data.coursecoverimage,
                    coursedescription = data.coursedescription,
                    courseduringtime = (int)data.courseduringtime,
                    coursename = data.coursename,
                    courseleveltypeId = (int)data.courseleveltypeId,
                    courseprice = (data.courseprice == null || data.courseprice == 0) ? 0 : (decimal)data.courseprice,
                    coursetypeId = (int)data.coursetypeId,
                    enddate = (DateTime)data.enddate,
                    lessencount = (int)data.lessencount,
                    owneruserId = userlogin.userid,
                    registerdatetime = DateTime.Now,
                    isadminaccepted = data.isadminaccepted ?? false,
                    isprelesson = data.isprelesson,
                    startdate = (DateTime)data.startdate,
                    grouptypeId = data.grouptypeId,
                    issitecourse = data.issitecourse,
                    coursetgas = data.coursetgas,
                    whatlearn = data.whatlearn,
                    allowQA = data.allowQA,
                    targetedaudience = data.targetedaudience,
                    materialsincluded = data.materialsincluded,
                    requirementsinstructions = data.requirementsinstructions
                };
                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request success", MessageData = data2 };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<byte[]> GetCourseImage(Guid Id)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                Course data = await _Context.Courses.FindAsync(Id);

                if (data != null && data.coursecoverimage != null)
                {
                    return data.coursecoverimage;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, "", "", methodpath, LogTypes.SystemError);
            }
            return null;
        }

        public async Task<SystemMessageModel> AddCourseLesson(CourseLessonDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                CourseLesson data = new CourseLesson()
                {
                    Id = Guid.NewGuid(),
                    registerdatetime = DateTime.Now,
                    author = model.author ?? "",
                    courseId = (Guid)model.courseId,
                    lessondescription = model.lessondescription,
                    endtime = (DateTime)model.endtime,
                    starttime = (DateTime)model.starttime,
                    lessontime = model.lessontime ?? 60,
                    lessonname = model.lessonname,
                    aoutoruserid = (string.IsNullOrEmpty(model.author)) ? model.aoutoruserid : null,
                };

                model.Id = data.Id;
                await _Context.CourseLessons.AddAsync(data);
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

        public async Task<SystemMessageModel> GetCourseLessons(CourseLessonDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<CourseLesson> query = _Context.CourseLessons;

                if (model.Id != null)
                    query = query.Where(x => x.Id == model.Id);

                if (model.courseId != null)
                    query = query.Where(x => x.courseId == model.courseId);

                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 50 : (int)model.rowcount;


                List<CourseLessonDto> data = await query.Skip((pageIndex - 1) * PageRowCount).Take(PageRowCount)
                                                  .Include(x => x.course)
                                                  .Include(x => x.aoutoruser)
                                                  .Select(x => new CourseLessonDto()
                                                  {
                                                      Id = x.Id,
                                                      aoutoruserid = x.aoutoruserid,
                                                      aoutorusername = (x.aoutoruser != null) ? x.aoutoruser.Username : "",
                                                      courseId = x.courseId,
                                                      author = x.author,
                                                      coursename = x.course.coursename,
                                                      endtime = x.endtime,
                                                      starttime = x.starttime,
                                                      lessonname = x.lessonname,
                                                      lessondescription = x.lessondescription,
                                                      lessontime = x.lessontime,
                                                      pageindex = pageIndex,
                                                      registerdatetime = x.registerdatetime,
                                                      rowcount = PageRowCount
                                                  }).ToListAsync();

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


        public async Task<SystemMessageModel> UploadCourseLessonFile(CourseLessonFileDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                CourseLessonFile data = new CourseLessonFile()
                {
                    Id = Guid.NewGuid(),
                    courselessonId = model.courselessonId,
                    attachment = model.attachment,
                    fileextention = model.fileextention
                };

                await _Context.CourseLessonFiles.AddAsync(data);
                await _Context.SaveChangesAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request success", MessageData = new { Id = data.Id } };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        internal async Task<SystemMessageModel> GetCourseLessonFileList(CourseLessonFileDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<CourseLessonFile> query = _Context.CourseLessonFiles;

                if (model.Id != null)
                    query = query.Where(x => x.Id == model.Id);

                if (model.courselessonId != null)
                    query = query.Where(x => x.courselessonId == model.courselessonId);

                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 50 : (int)model.rowcount;


                List<CourseLessonFileDto> data = await query.Skip((pageIndex - 1) * PageRowCount).Take(PageRowCount)
                                                  .Include(x => x.courselesson)
                                                  .Select(x => new CourseLessonFileDto()
                                                  {
                                                      Id = x.Id,
                                                      courselessonId = x.courselessonId,
                                                      contenttype = x.contenttype,
                                                      fileextention = x.fileextention,
                                                      filename = x.filename,
                                                      courselessonname = x.courselesson.lessonname
                                                  }).ToListAsync();

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

        public async Task<CourseLessonFile> GetCourseLessonFile(Guid Id)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                CourseLessonFile data = await _Context.CourseLessonFiles.FindAsync(Id);

                return data;
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, "", "", methodpath, LogTypes.SystemError);
                return null;
            }
        }

        public async Task<SystemMessageModel> AddCourseRequest(CourseLessonDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (await _Context.CourseMemebers.Where(x => x.courseId == model.courseId && x.userId == userlogin.userid).AnyAsync())
                {
                    return new SystemMessageModel() { MessageCode = 200, MessageDescription = "you bought this course before ", MessageData = model };
                }

                WalletTransactionDto transactionmodel = new WalletTransactionDto();
                Course course = await _Context.Courses.FindAsync(model.courseId);

                Wallet sourcewallet = await _Context.Wallets.Where(x => x.userId == userlogin.userid).SingleOrDefaultAsync();

                transactionmodel.sourcewalletId = sourcewallet.Id;

                if ((bool)course.issitecourse)
                {
                    transactionmodel.destiationwalletId = SitWallet.SitWalletId;
                }
                else
                {
                    Wallet distinationwallet = await _Context.Wallets.Where(x => x.userId == course.owneruserId).SingleOrDefaultAsync();
                    transactionmodel.destiationwalletId = distinationwallet.Id;
                }


                transactionmodel.transactionamount = course.courseprice;

                if (transactionmodel.transactionamount > 0)
                {
                    message = await _walletService.Transfer(transactionmodel, userlogin, processId, clientip, hosturl);
                    if (message.MessageCode < 0)
                        return message;
                }

                CourseMemeber data = new CourseMemeber()
                {
                    Id = Guid.NewGuid(),
                    registerdatetime = DateTime.Now,
                    courseId = (Guid)model.courseId,
                    userId = userlogin.userid
                };

                model.Id = data.Id;
                await _Context.CourseMemebers.AddAsync(data);
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

        public async Task<SystemMessageModel> GetUserCourses(CourseDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                List<Guid> CourseIds = null;
                IQueryable<Course> query = _Context.Courses;

                if (model.Id != null)
                    query = query.Where(x => x.Id == model.Id);

                if (model.owneruserId != null)
                    query = query.Where(x => x.owneruserId == model.owneruserId);

                if (model.isadminaccepted != null)
                    query = query.Where(x => x.isadminaccepted == model.isadminaccepted);

                if (model.isprelesson != null)
                    query = query.Where(x => x.isprelesson == model.isprelesson);

                if (model.coursetypeId != null)
                    query = query.Where(x => x.coursetypeId == model.coursetypeId);

                if (model.courseleveltypeId != null)
                    query = query.Where(x => x.courseleveltypeId == model.courseleveltypeId);

                if (!string.IsNullOrEmpty(model.username))
                {
                    User user = await _Context.Users.Where(x => x.Username == model.username).SingleOrDefaultAsync();
                    CourseIds = await _Context.CourseMemebers.Where(x => x.userId == user.UserId).Select(x => x.courseId).ToListAsync();
                }
                else
                {
                    CourseIds = await _Context.CourseMemebers.Where(x => x.userId == userlogin.userid).Select(x => x.courseId).ToListAsync();
                }

                if (CourseIds != null && CourseIds.Count > 0)
                {
                    query = query.Where(x => CourseIds.Contains(x.Id));
                }


                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 50 : (int)model.rowcount;


                List<CourseDto> data = await query.Skip((pageIndex - 1) * PageRowCount).Take(PageRowCount)
                                                  .Include(x => x.owneruser)
                                                  .Include(x => x.courseleveltype)
                                                  .Include(x => x.coursetype)
                                                  .Select(x => new CourseDto()
                                                  {
                                                      Id = x.Id,
                                                      allowdownload = x.allowdownload,
                                                      coursecoverimage = x.coursecoverimage,
                                                      coursedescription = x.coursedescription,
                                                      courseduringtime = x.courseduringtime,
                                                      coursename = x.coursename,
                                                      courseleveltypeId = x.courseleveltypeId,
                                                      courseleveltypename = x.courseleveltype.name,
                                                      courseprice = (x.courseprice == null || x.courseprice == 0) ? 0 : x.courseprice,
                                                      coursetypeId = x.coursetypeId,
                                                      coursetypename = x.coursetype.name,
                                                      enddate = x.enddate,
                                                      lessencount = x.lessencount,
                                                      owneruserId = x.owneruserId,
                                                      ownerusername = x.owneruser.Username,
                                                      pageindex = pageIndex,
                                                      registerdatetime = x.registerdatetime,
                                                      rowcount = PageRowCount,
                                                      isprelesson = x.isprelesson,
                                                      isadminaccepted = x.isadminaccepted,
                                                      startdate = x.startdate
                                                  }).ToListAsync();

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
    }
}
