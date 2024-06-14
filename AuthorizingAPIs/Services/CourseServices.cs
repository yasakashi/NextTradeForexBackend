using Base.Common.Enums;
using DataLayers;
using Entities.DBEntities;
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
        public CourseServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
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

                if (model.Id != null)
                    query = query.Where(x => x.Id == model.Id);

                if (model.owneruserId != null)
                    query = query.Where(x => x.owneruserId == model.owneruserId);

                if (model.siteisowner != null)
                    query = query.Where(x => x.siteisowner == model.siteisowner);

                if (model.coursetypeId != null)
                    query = query.Where(x => x.coursetypeId == model.coursetypeId);

                if (model.courseleveltypeId != null)
                    query = query.Where(x => x.courseleveltypeId == model.courseleveltypeId);

                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 50 : (int)model.rowcount;


                List<CourseDto> data = await query.Skip(pageIndex - 1).Take(PageRowCount)
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
                                                      siteisowner = x.siteisowner,
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
                    allowdownload = model.allowdownload,
                    coursecoverimage = model.coursecoverimage,
                    coursedescription = model.coursedescription,
                    courseduringtime = model.courseduringtime,
                    coursename = model.coursename,
                    courseleveltypeId = model.courseleveltypeId,
                    courseprice = (model.courseprice == null || model.courseprice == 0) ? 0 : model.courseprice,
                    coursetypeId = model.coursetypeId,
                    enddate = model.enddate,
                    lessencount = model.lessencount,
                    owneruserId = userlogin.userid,
                    registerdatetime = DateTime.Now,
                    siteisowner = model.siteisowner,
                    startdate = model.startdate
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


                data.allowdownload = model.allowdownload;
                data.coursecoverimage = model.coursecoverimage;
                data.coursedescription = model.coursedescription;
                data.courseduringtime = model.courseduringtime;
                data.coursename = model.coursename;
                data.courseleveltypeId = model.courseleveltypeId;
                data.courseprice = (model.courseprice == null || model.courseprice == 0) ? 0 : model.courseprice;
                data.coursetypeId = model.coursetypeId;
                data.enddate = model.enddate;
                data.lessencount = model.lessencount;
                data.siteisowner = model.siteisowner;
                data.startdate = model.startdate;

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
                    //allowdownload = model.allowdownload,
                    //coursecoverimage = model.coursecoverimage,
                    //coursedescription = model.coursedescription,
                    //courseduringtime = model.courseduringtime,
                    //coursename = model.coursename,
                    //courseleveltypeId = model.courseleveltypeId,
                    //courseprice = (model.courseprice == null || model.courseprice == 0) ? 0 : model.courseprice,
                    //coursetypeId = model.coursetypeId,
                    //enddate = model.enddate,
                    //lessencount = model.lessencount,
                    //owneruserId = userlogin.userid,
                    //registerdatetime = DateTime.Now,
                    //siteisowner = model.siteisowner,
                    //startdate = model.startdate
                };

                //model.Id = data.Id;
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
    }
}
