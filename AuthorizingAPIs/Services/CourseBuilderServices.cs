using Base.Common.Enums;
using DataLayers;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using Entities.DBEntities;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NextTradeAPIs.Dtos;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace NextTradeAPIs.Services
{
    public class CourseBuilderServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;

        private WalletServices _walletService;

        public CourseBuilderServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices, WalletServices walletService)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
            _walletService = walletService;
        }


        public async Task<SystemMessageModel> AddNewCourses(CourseBuilderCourseDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                CourseBuilderCourse data = new CourseBuilderCourse()
                {
                    Id = (Guid)model.Id,
                    courseName = model.courseName,
                    courseDescription = model.courseDescription,
                    courseFilename = model.courseFilename,
                    courseFilepath = model.courseFilepath,
                    excerpt = model.excerpt,
                    authorId = model.authorId ?? userlogin.userid,
                    maximumStudents = model.maximumStudents ?? 1,
                    courseleveltypeId = model.difficultyLevelId,
                    isPublicCourse = model.isPublicCourse ?? false,
                    allowQA = model.allowQA ?? false,
                    coursePrice = model.coursePrice ?? 0,
                    whatWillILearn = model.whatWillILearn,
                    targetedAudience = model.targetedAudience,
                    courseDuration = model.courseDuration ?? 1,
                    materialsIncluded = model.materialsIncluded,
                    requirementsInstructions = model.requirementsInstructions,
                    courseIntroVideo = model.courseIntroVideo,
                    courseTags = model.courseTags,
                    featuredImagename = model.featuredImagename,
                    featuredImagepath = model.featuredImagepath,
                    registerdatetime = DateTime.Now,
                    courseFilecontent = model.courseFilecontent,
                    featuredImagecontent = model.featuredImagecontent,
                    coursestatusid = (int)CourseStatusList.Draft,
                    changestatusdate = DateTime.Now,
                    isvisibledropdown = model.isvisibledropdown,
                    isvisible = model.isvisible
                };
                await _Context.CourseBuilderCourses.AddAsync(data);

                List<CourseCategory> CourseCategoryList = new List<CourseCategory>();
                foreach (long cId in model.courseCategoryIds)
                {
                    CourseCategoryList.Add(new CourseCategory()
                    {
                        Id = Guid.NewGuid(),
                        categoryId = cId,
                        courseId = data.Id
                    });
                }
                if (CourseCategoryList.Count() > 0)
                    await _Context.CourseCategories.AddRangeAsync(CourseCategoryList);
                await _Context.SaveChangesAsync();

                List<CourseBuilderMeeting> meetingList = new List<CourseBuilderMeeting>();
                foreach (CourseBuilderMeetingDto item in model.meetings)
                {
                    meetingList.Add(new CourseBuilderMeeting()
                    {
                        Id = Guid.NewGuid(),
                        courseId = data.Id,
                        meetingDateTime = item.meetingDateTime,
                        meetingDescription = item.meetingDescription,
                        meetingFilename = item.meetingFilename,
                        meetingFilepath = item.meetingFilepath,
                        meetingTitle = item.meetingTitle,
                        meetingURL = item.meetingURL
                    });
                }
                if (meetingList.Count() > 0)
                    await _Context.CourseBuilderMeetings.AddRangeAsync(meetingList);

                List<CourseBuildeVideoPdfUrl> videoPdfUrllist = new List<CourseBuildeVideoPdfUrl>();

                foreach (CourseBuildeVideoPdfUrlDto item in model.videoPdfUrls)
                {
                    videoPdfUrllist.Add(new CourseBuildeVideoPdfUrl()
                    {
                        Id = Guid.NewGuid(),
                        courseId = data.Id,
                        downloadable = item.downloadable ?? false,
                        pdfDescription = item.pdfDescription,
                        pdfFilename = item.pdfFilename,
                        pdfFilepath = item.pdfFilepath,
                        pdfTitle = item.pdfTitle
                    });
                }
                if (videoPdfUrllist.Count() > 0)
                    await _Context.CourseBuildeVideoPdfUrls.AddRangeAsync(videoPdfUrllist);

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

        public async Task<SystemMessageModel> AddNewCourseMeetings(CourseBuilderMeetingDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {

                CourseBuilderMeeting data = new CourseBuilderMeeting()
                {
                    Id = Guid.NewGuid(),
                    courseId = (Guid)model.courseId,
                    meetingDateTime = model.meetingDateTime,
                    meetingDescription = model.meetingDescription,
                    meetingFilename = model.meetingFilename,
                    meetingFilepath = model.meetingFilepath,
                    meetingfilecontetnttype = model.meetingfilecontetnttype,
                    meetingTitle = model.meetingTitle,
                    meetingURL = model.meetingURL
                };
                await _Context.CourseBuilderMeetings.AddAsync(data);
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

        public async Task<SystemMessageModel> DeleteCourseMeeting(CourseBuilderMeetingDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.Id == null)
                    return new SystemMessageModel() { MessageCode = -102, MessageDescription = "Id is Wrong" };

                CourseBuilderMeeting data = await _Context.CourseBuilderMeetings.FindAsync(model.Id);

                if (data == null)
                    return new SystemMessageModel() { MessageCode = -103, MessageDescription = "data not find" };

                await DeleteFile(data.meetingFilepath);

                _Context.CourseBuilderMeetings.Remove(data);
                await _Context.SaveChangesAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = model.Id };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> GetCourseMeetings(CourseBuilderMeetingFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;
            List<CourseBuilderMeetingDto> datas = null;
            try
            {
                IQueryable<CourseBuilderMeeting> query = _Context.CourseBuilderMeetings;
                if (model.Id != null)
                    query = query.Where(x => x.Id == model.Id);

                if (model.frommeetingdatetime != null)
                    query = query.Where(x => x.meetingDateTime >= model.frommeetingdatetime);

                if (model.tomeetingdatetime != null)
                    query = query.Where(x => x.meetingDateTime <= model.tomeetingdatetime);

                if (model.courseId != null)
                    query = query.Where(x => x.courseId == model.courseId);

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
                                case "meetingdatetime":
                                    query = query.OrderBy(x => x.meetingDateTime);
                                    break;
                                case "meetingtitle":
                                    query = query.OrderBy(x => x.meetingTitle);
                                    break;
                            };
                        }
                        else if (!(bool)item.ascending)
                        {
                            switch (item.fieldname.ToLower())
                            {
                                case "meetingdatetime":
                                    query = query.OrderByDescending(x => x.meetingDateTime);
                                    break;
                                case "meetingtitle":
                                    query = query.OrderByDescending(x => x.meetingTitle);
                                    break;
                            };
                        }
                    }
                }

                datas =  await query.Skip((pageIndex - 1) * PageRowCount).Take(PageRowCount).Select(x => new CourseBuilderMeetingDto()
                {
                    Id = x.Id,
                    courseId = x.courseId,
                    meetingDateTime = x.meetingDateTime,
                    meetingDescription = x.meetingDescription,
                    meetingFilename = x.meetingFilename,
                    meetingTitle = x.meetingTitle,
                    meetingURL = x.meetingURL
                }).ToListAsync();
                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = datas, Meta = new { pageIndex = pageIndex, PageRowCount = PageRowCount, totaldata = totaldata, pagecount = pagecount } };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }


        public async Task<SystemMessageModel> AddNewCourseVideoPdfUrls(CourseBuildeVideoPdfUrlDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {

                CourseBuildeVideoPdfUrl videoPdfUrllist = new CourseBuildeVideoPdfUrl()
                {
                    Id = Guid.NewGuid(),
                    courseId = (Guid)model.courseId,
                    downloadable = model.downloadable ?? false,
                    pdfDescription = model.pdfDescription,
                    pdfFilename = model.pdfFilename,
                    pdfFilepath = model.pdfFilepath,
                    pdfFilecontenttype = model.pdfFilecontenttype,
                    pdfTitle = model.pdfTitle
                };
                await _Context.CourseBuildeVideoPdfUrls.AddAsync(videoPdfUrllist);

                await _Context.SaveChangesAsync();


                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = videoPdfUrllist };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> DeleteCourseVideoPdfUrl(CourseBuildeVideoPdfUrlDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.Id == null)
                    return new SystemMessageModel() { MessageCode = -102, MessageDescription = "Id is Wrong" };

                CourseBuildeVideoPdfUrl data = await _Context.CourseBuildeVideoPdfUrls.FindAsync(model.Id);

                if (data == null)
                    return new SystemMessageModel() { MessageCode = -103, MessageDescription = "data not find" };

                await DeleteFile(data.pdfFilepath);

                _Context.CourseBuildeVideoPdfUrls.Remove(data);
                await _Context.SaveChangesAsync();



                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = model.Id };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }


        public async Task<SystemMessageModel> GetCourses(CourseBuilderCourseFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl, bool sendfilepath = true)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<CourseBuilderCourse> query = _Context.CourseBuilderCourses;

                if (model.courseCategoryIds != null && model.courseCategoryIds.Count > 0)
                {
                    List<Guid> Ids = await _Context.CourseCategories.Where(x => model.courseCategoryIds.Contains(x.categoryId)).Select(x => x.courseId).ToListAsync();
                    if (Ids != null && Ids.Count > 0)
                        query = query.Where(x => Ids.Contains(x.Id));
                }
                if (model.Id != null)
                {
                    query = query.Where(x => x.Id == model.Id);
                    sendfilepath = true;
                }
                if (model.authorId != null)
                    query = query.Where(x => x.authorId == model.authorId);

                if (model.allowQA != null)
                    query = query.Where(x => x.allowQA == model.allowQA);

                if (model.isfree != null)
                    query = query.Where(x => x.coursePrice == 0);

                if (model.ispaid != null)
                    query = query.Where(x => x.coursePrice > 0);

                if (model.allowQA != null)
                    query = query.Where(x => x.allowQA == model.allowQA);

                if (model.isPublicCourse != null)
                    query = query.Where(x => x.isPublicCourse == model.isPublicCourse);

                if (model.coursestatusid != null)
                    query = query.Where(x => x.coursestatusid == model.coursestatusid);

                if (model.difficultyLevelId != null)
                    query = query.Where(x => x.courseleveltypeId == model.difficultyLevelId);

                if (!string.IsNullOrEmpty(model.courseTags))
                    query = query.Where(x => x.courseTags.Contains(model.courseTags.Trim()));

                if (!string.IsNullOrEmpty(model.courseName))
                    query = query.Where(x => x.courseName.Contains(model.courseName.Trim()));

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

                List<CourseBuilderCourseDto> data = await query.Skip((pageIndex - 1) * PageRowCount).Take(PageRowCount)
                                                  .Include(x => x.coursestatus)
                                                  .Include(x => x.author)
                                                  .Select(x => new CourseBuilderCourseDto()
                                                  {
                                                      Id = x.Id,
                                                      courseName = x.courseName,
                                                      courseDescription = x.courseDescription,
                                                      courseFilename = x.courseFilename,
                                                      courseFilepath = (string.IsNullOrEmpty(x.courseFilepath))?"": ((sendfilepath == true) ? x.courseFilepath : hosturl + x.courseFilepath.Substring(x.courseFilepath.IndexOf("wwwroot\\")).Replace ("wwwroot", "").Replace("\\", "/") ),
                                                      excerpt = x.excerpt,
                                                      authorId = x.authorId,
                                                      authorname = (x.author.Fname ?? "") + " " + (x.author.Lname ?? ""),
                                                      authorusername = x.author.Username,
                                                      maximumStudents = x.maximumStudents ?? 1,
                                                      difficultyLevelId = x.courseleveltypeId,
                                                      isPublicCourse = x.isPublicCourse ?? false,
                                                      allowQA = x.allowQA ?? false,
                                                      coursePrice = x.coursePrice ?? 0,
                                                      whatWillILearn = x.whatWillILearn,
                                                      targetedAudience = x.targetedAudience,
                                                      courseDuration = x.courseDuration ?? 1,
                                                      materialsIncluded = x.materialsIncluded,
                                                      requirementsInstructions = x.requirementsInstructions,
                                                      courseIntroVideo = x.courseIntroVideo,
                                                      courseTags = x.courseTags,
                                                      featuredImagename = x.featuredImagename,
                                                      featuredImagepath = string.IsNullOrEmpty(x.featuredImagepath)?"":((sendfilepath == true) ? x.featuredImagepath:(hosturl + x.featuredImagepath.Substring                                     (x.featuredImagepath.IndexOf("wwwroot\\")).Replace("wwwroot", "").Replace("\\", "/"))),
                                                      registerdatetime = x.registerdatetime,
                                                      courseFilecontent = x.courseFilecontent,
                                                      featuredImagecontent = x.featuredImagecontent,
                                                      coursestatusid = x.coursestatusid,
                                                      coursestatusname = x.coursestatus.name,
                                                      changestatusdate = x.changestatusdate,
                                                      isvisible = x.isvisible,
                                                      isvisibledropdown = x.isvisibledropdown
                                                  }).ToListAsync();


                foreach (CourseBuilderCourseDto item in data)
                {
                    item.courseCategorys = await _Context.CourseCategories.Where(x => x.courseId == item.Id).Include(x => x.category).Select(x => new CourseCategoryDto()
                    {
                        categoryId = x.categoryId,
                        courseId = x.courseId,
                        Id = x.Id,
                        categoryname = x.category.name
                    }).ToListAsync();

                    item.meetings = await _Context.CourseBuilderMeetings.Where(x => x.courseId == item.Id).Select(x => new CourseBuilderMeetingDto()
                    {
                        Id = x.Id,
                        courseId = x.courseId,
                        meetingDateTime = x.meetingDateTime,
                        meetingDescription = x.meetingDescription,
                        meetingFilename = x.meetingFilename,
                        meetingTitle = x.meetingTitle,
                        meetingURL = x.meetingURL
                    }).ToListAsync();
                    item.meetingcount = item.meetings.Count();
                    item.videoPdfUrls = await _Context.CourseBuildeVideoPdfUrls.Where(x => x.courseId == item.Id).Select(x => new CourseBuildeVideoPdfUrlDto()
                    {
                        Id = x.Id,
                        courseId = x.courseId,
                        downloadable = x.downloadable,
                        pdfDescription = x.pdfDescription,
                        pdfFilename = x.pdfFilename,
                        pdfTitle = x.pdfTitle,
                        viewPdfFile = x.viewPdfFile
                    }).ToListAsync();
                    item.videoPdfcount = item.videoPdfUrls.Count();

                    if (!string.IsNullOrEmpty(item.featuredImagepath)&& !item.featuredImagepath.StartsWith("http"))
                        item.featuredImagepath = hosturl + item.featuredImagepath.Substring(item.featuredImagepath.IndexOf("wwwroot\\")).Replace("wwwroot", "").Replace("\\", "/");

                    if (!string.IsNullOrEmpty(item.courseFilepath) && !item.courseFilepath.StartsWith("http"))
                        item.courseFilepath = hosturl + item.courseFilepath.Substring(item.courseFilepath.IndexOf("wwwroot\\")).Replace("wwwroot", "").Replace("\\", "/");

                    item.lessoncount = await _Context.CourseBuilderLessons.Where(x => x.courseId == item.Id).CountAsync();
                    item.topiccount = await _Context.CourseBuilderTopics.Where(x => x.courseId == item.Id).CountAsync();
                    item.quizcount = await _Context.CourseBuilderQuizs.Where(x => x.courseId == item.Id).CountAsync();
                    //Uri physicalUri = new Uri(data.courseFilepath);
                    //Uri baseUri = new Uri(sitePath);
                    //Uri relativeUri = physicalUri.MakeRelativeUri(baseUri);
                    //data.courseFilepath = relativePath;


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

        public async Task<SystemMessageModel> ChangeCourseStatus(CourseBuilderCourseFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.Id == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -1), MessageDescription = "Error In doing Request", MessageData = "data not complete" };

                CourseBuilderCourse data = await _Context.CourseBuilderCourses.FindAsync(model.Id);
                if (data == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -1), MessageDescription = "Error In doing Request", MessageData = "Id is wrong" };

                data.coursestatusid = (int)model.coursestatusid;
                data.changestatusdate = DateTime.Now;

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


        public async Task<SystemMessageModel> DeleteCourses(CourseBuilderCourseFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.Id == null)
                    return new SystemMessageModel() { MessageCode = -102, MessageDescription = "Id is Wrong" };

                CourseBuilderCourse data = await _Context.CourseBuilderCourses.FindAsync(model.Id);
                if (data == null)
                    return new SystemMessageModel() { MessageCode = -103, MessageDescription = "data not find" };

                await DeleteFile(data.courseFilepath);
                await DeleteFile(data.featuredImagepath);

                _Context.CourseBuilderCourses.Remove(data);

                List<CourseCategory> courseCategorys = await _Context.CourseCategories.Where(x => x.courseId == data.Id).ToListAsync();
                if (courseCategorys != null && courseCategorys.Count > 0)
                    _Context.CourseCategories.RemoveRange(courseCategorys);

                List<CourseBuilderMeeting> meetings = await _Context.CourseBuilderMeetings.Where(x => x.courseId == data.Id).ToListAsync();
                if (meetings != null && meetings.Count > 0)
                {
                    foreach (CourseBuilderMeeting meeting in meetings)
                    {
                        await DeleteFile(meeting.meetingFilepath);
                    }
                    _Context.CourseBuilderMeetings.RemoveRange(meetings);
                }

                List<CourseBuildeVideoPdfUrl> videoPdfUrls = await _Context.CourseBuildeVideoPdfUrls.Where(x => x.courseId == data.Id).ToListAsync();
                if (videoPdfUrls != null && videoPdfUrls.Count > 0)
                {
                    foreach (CourseBuildeVideoPdfUrl videoPdfUrl in videoPdfUrls)
                    {
                        await DeleteFile(videoPdfUrl.pdfFilepath);
                    }
                    _Context.CourseBuildeVideoPdfUrls.RemoveRange(videoPdfUrls);
                }

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


        public async Task<SystemMessageModel> AddCourseTopics(CourseBuilderTopicDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                CourseBuilderTopic data = new CourseBuilderTopic()
                {
                    Id = (Guid)model.Id,
                    courseId = (Guid)model.courseId,
                    topicName = model.topicName,
                    topicSummary = model.topicSummary,
                    topicorder = model.topicorder??1
                };
                await _Context.CourseBuilderTopics.AddAsync(data);

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


        public async Task<SystemMessageModel> GetCourseTopics(CourseBuilderTopicDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<CourseBuilderTopic> query = _Context.CourseBuilderTopics;

                if (model.Id != null)
                    query = query.Where(x => x.Id == model.Id);

                if (model.courseId != null)
                    query = query.Where(x => x.courseId == model.courseId);

                if (!string.IsNullOrEmpty(model.topicName))
                    query = query.Where(x => x.topicName.Contains(model.topicName.Trim()));

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
                                case "topicorder":
                                    query = query.OrderBy(x => x.topicorder);
                                    break;
                                case "topicname":
                                    query = query.OrderBy(x => x.topicName);
                                    break;
                            };
                        }
                        else if (!(bool)item.ascending)
                        {
                            switch (item.fieldname.ToLower())
                            {
                                case "topicorder":
                                    query = query.OrderByDescending(x => x.topicorder);
                                    break;
                                case "topicname":
                                    query = query.OrderByDescending(x => x.topicName);
                                    break;
                            };
                        }
                    }
                }


                List<CourseBuilderTopicDto> data = await query.Skip((pageIndex - 1) * PageRowCount).Take(PageRowCount)
                                                  .Select(x => new CourseBuilderTopicDto()
                                                  {
                                                      Id = x.Id,
                                                      courseId = x.courseId,
                                                      topicName = x.topicName,
                                                      topicSummary = x.topicSummary,
                                                      topicorder = x.topicorder
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

        public async Task<SystemMessageModel> UpdateCourseTopic(CourseBuilderTopicDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.Id == null)
                    return new SystemMessageModel() { MessageCode = -102, MessageDescription = "Id is Wrong" };

                CourseBuilderTopic data = await _Context.CourseBuilderTopics.FindAsync(model.Id);
                if (data == null)
                    return new SystemMessageModel() { MessageCode = -103, MessageDescription = "data not find" };

                data.topicName = model.topicName;
                data.topicSummary = model.topicSummary;


                _Context.CourseBuilderTopics.Update(data);
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

        public async Task<SystemMessageModel> DeleteCourseTopics(CourseBuilderTopicDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.Id == null)
                    return new SystemMessageModel() { MessageCode = -102, MessageDescription = "Id is Wrong" };

                CourseBuilderTopic data = await _Context.CourseBuilderTopics.FindAsync(model.Id);

                if (data == null)
                    return new SystemMessageModel() { MessageCode = -103, MessageDescription = "data not find" };

                _Context.CourseBuilderTopics.Remove(data);
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


        public async Task<SystemMessageModel> AddNewCourseLesson(CourseBuilderLessonDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                CourseBuilderLesson data = new CourseBuilderLesson()
                {
                    Id = (Guid)model.Id,
                    topicId = (Guid)model.topicId,
                    courseId = (Guid)model.courseId,
                    featureImagecontenttype = model.featureImagecontenttype,
                    featureImagename = model.featureImagename,
                    featureImagepath = model.featureImagepath,
                    lessonDescription = model.lessonDescription,
                    lessonFilecontenttype = model.lessonFilecontenttype,
                    lessonFilename = model.lessonFilename,
                    lessonName = model.lessonName,
                    videoPlaybackTime = model.videoPlaybackTime ?? 1,
                    lessonFilepath = model.lessonFilepath,
                    videoSource = model.videoSource
                };
                await _Context.CourseBuilderLessons.AddAsync(data);

                if (model.fileattachments != null && model.fileattachments.Count() > 0)
                {
                    List<CourseBuilderLessonFile> lessonfileattachmentList = new List<CourseBuilderLessonFile>();
                    foreach (CourseBuilderLessonFileDto attch in model.fileattachments)
                    {
                        lessonfileattachmentList.Add(new CourseBuilderLessonFile()
                        {
                            Id = Guid.NewGuid(),
                            lessonId = data.Id,
                            lessonFilecontenttype = attch.lessonFilecontenttype,
                            lessonFilename = attch.lessonFilename,
                            lessonFilepath = attch.lessonFilepath
                        });
                    }
                    if (lessonfileattachmentList.Count() > 0)
                        await _Context.CourseBuilderLessonFiles.AddRangeAsync(lessonfileattachmentList);
                }

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

        public async Task<SystemMessageModel> UpdateCourseLesson(CourseBuilderLessonDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                CourseBuilderLesson data = await _Context.CourseBuilderLessons.FindAsync(model.Id);

                if (!string.IsNullOrEmpty(model.featureImagecontenttype))
                    data.featureImagecontenttype = model.featureImagecontenttype;
                if (!string.IsNullOrEmpty(model.featureImagename)) data.featureImagename = model.featureImagename;
                if (!string.IsNullOrEmpty(model.featureImagepath)) data.featureImagepath = model.featureImagepath;
                if (!string.IsNullOrEmpty(model.lessonDescription)) data.lessonDescription = model.lessonDescription;
                if (!string.IsNullOrEmpty(model.lessonFilecontenttype)) data.lessonFilecontenttype = model.lessonFilecontenttype;
                if (!string.IsNullOrEmpty(model.lessonFilename)) data.lessonFilename = model.lessonFilename;
                if (!string.IsNullOrEmpty(model.lessonName)) data.lessonName = model.lessonName;
                if (model.videoPlaybackTime != null) data.videoPlaybackTime = model.videoPlaybackTime ?? 1;
                if (!string.IsNullOrEmpty(model.lessonFilepath)) data.lessonFilepath = model.lessonFilepath;
                if (!string.IsNullOrEmpty(model.videoSource)) data.videoSource = model.videoSource;

                _Context.CourseBuilderLessons.Update(data);


                List<CourseBuilderLessonFile> lessonfileattachmentList = new List<CourseBuilderLessonFile>();
                foreach (CourseBuilderLessonFileDto attch in model.fileattachments)
                {
                    lessonfileattachmentList.Add(new CourseBuilderLessonFile()
                    {
                        Id = Guid.NewGuid(),
                        lessonId = data.Id,
                        lessonFilecontenttype = attch.lessonFilecontenttype,
                        lessonFilename = attch.lessonFilename,
                        lessonFilepath = attch.lessonFilepath
                    });
                }
                if (lessonfileattachmentList.Count() > 0)
                    await _Context.CourseBuilderLessonFiles.AddRangeAsync(lessonfileattachmentList);


                await _Context.SaveChangesAsync();


                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = model.Id };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }


        public async Task<SystemMessageModel> GetCourseLessons(CourseBuilderLessonFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl, bool showfilepath = true)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<CourseBuilderLesson> query = _Context.CourseBuilderLessons;

                if (model.Id != null)
                    query = query.Where(x => x.Id == model.Id);

                if (model.courseId != null)
                    query = query.Where(x => x.courseId == model.courseId);

                if (!string.IsNullOrEmpty(model.lessonName))
                    query = query.Where(x => x.lessonName.Contains(model.lessonName.Trim()));

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
                                case "lessonorder":
                                    query = query.OrderBy(x => x.lessonorder);
                                    break;
                                case "lessonname":
                                    query = query.OrderBy(x => x.lessonName);
                                    break;
                            };
                        }
                        else if (!(bool)item.ascending)
                        {
                            switch (item.fieldname.ToLower())
                            {
                                case "lessonorder":
                                    query = query.OrderByDescending(x => x.lessonorder);
                                    break;
                                case "lessonname":
                                    query = query.OrderByDescending(x => x.lessonName);
                                    break;
                            };
                        }
                    }
                }

                List<CourseBuilderLessonDto> data = await query.Skip((pageIndex - 1) * PageRowCount).Take(PageRowCount)
                                                  .Select(x => new CourseBuilderLessonDto()
                                                  {
                                                      Id = x.Id,
                                                      courseId = x.courseId,
                                                      featureImagename = x.featureImagename,
                                                      featureImagecontenttype = x.featureImagecontenttype,
                                                      featureImagepath = (string.IsNullOrEmpty(x.featureImagepath))?"":( (showfilepath == true) ? x.featureImagepath :  hosturl + x.featureImagepath.Substring                                (x.featureImagepath.IndexOf("wwwroot\\")).Replace("wwwroot", "").Replace("\\", "/")),
                                                      lessonDescription = x.lessonDescription,
                                                      lessonFilecontenttype = x.lessonFilecontenttype,
                                                      lessonFilename = x.lessonFilename,
                                                      lessonFilepath = (string.IsNullOrEmpty(x.featureImagepath)) ? "" : ((showfilepath == true) ? x.lessonFilepath : hosturl + x.lessonFilepath.Substring(x.lessonFilepath.IndexOf("wwwroot\\")).Replace("wwwroot", "").Replace("\\", "/")),
                                                      lessonName = x.lessonName,
                                                      topicId = x.topicId,
                                                      videoPlaybackTime = x.videoPlaybackTime,
                                                      videoSource = x.videoSource,
                                                      lessonorder = x.lessonorder
                                                  }).ToListAsync();


                foreach (CourseBuilderLessonDto item in data)
                {
                    item.fileattachments = await _Context.CourseBuilderLessonFiles.Where(x => x.lessonId == item.Id).Select(x => new CourseBuilderLessonFileDto()
                    {
                        lessonId = x.lessonId,
                        Id = x.Id,
                        lessonFilecontenttype = x.lessonFilecontenttype,
                        lessonFilename = x.lessonFilename,
                        lessonFilepath = (string.IsNullOrEmpty(x.lessonFilepath))?"":((showfilepath == true)? x.lessonFilepath : hosturl + x.lessonFilepath.Substring(x.lessonFilepath.IndexOf("wwwroot\\")).Replace("wwwroot", "").Replace("\\", "/"))
                    }).ToListAsync();
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


        public async Task<SystemMessageModel> DeleteCourseLessons(CourseBuilderLessonFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.Id == null)
                    return new SystemMessageModel() { MessageCode = -102, MessageDescription = "Id is Wrong" };

                CourseBuilderLesson data = await _Context.CourseBuilderLessons.FindAsync(model.Id);

                if (data == null)
                    return new SystemMessageModel() { MessageCode = -103, MessageDescription = "data not find" };

                _Context.CourseBuilderLessons.Remove(data);

                List<CourseBuilderLessonFile> fileattachments = await _Context.CourseBuilderLessonFiles.Where(x => x.lessonId == data.Id).ToListAsync();

                foreach (CourseBuilderLessonFile item in fileattachments)
                {
                    await DeleteFile(item.lessonFilepath);
                }

                _Context.CourseBuilderLessonFiles.RemoveRange(fileattachments);

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


        public async Task<SystemMessageModel> GetCourseLessonAttachedFiles(CourseBuilderLessonFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl, bool showfilepath = true)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<CourseBuilderLesson> query = _Context.CourseBuilderLessons;

                if (model.Id != null)
                    query = query.Where(x => x.Id == model.Id);

                if (model.courseId != null)
                    query = query.Where(x => x.courseId == model.courseId);

                if (!string.IsNullOrEmpty(model.lessonName))
                    query = query.Where(x => x.lessonName.Contains(model.lessonName.Trim()));

                CourseBuilderLesson lesson = await query.FirstOrDefaultAsync();

                if (lesson != null)
                {

                    List<CourseBuilderLessonFileDto> data = await _Context.CourseBuilderLessonFiles.Where(x => x.lessonId == lesson.Id).Select(x => new CourseBuilderLessonFileDto()
                    {
                        lessonId = x.lessonId,
                        Id = x.Id,
                        lessonFilecontenttype = x.lessonFilecontenttype,
                        lessonFilename = x.lessonFilename,
                        lessonFilepath = (showfilepath == true) ? x.lessonFilepath : ""
                    }).ToListAsync();

                    message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = data, Meta = new { pageIndex = 1, PageRowCount = data.Count(), totaldata = data.Count(), pagecount = 1 } };
                }
                else
                {
                    message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = null, Meta = new { pageIndex = 1, PageRowCount = 0, totaldata = 0, pagecount = 1 } };
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

        public async Task<SystemMessageModel> DeleteCourseLessonAttachedFile(CourseBuilderLessonFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.Id == null)
                    return new SystemMessageModel() { MessageCode = -102, MessageDescription = "Id is Wrong" };

                CourseBuilderLessonFile data = await _Context.CourseBuilderLessonFiles.FindAsync(model.Id);

                if (data == null)
                    return new SystemMessageModel() { MessageCode = -103, MessageDescription = "data not find" };

                await DeleteFile(data.lessonFilepath);

                _Context.CourseBuilderLessonFiles.Remove(data);

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

        public async Task<SystemMessageModel> GetCourseFileURL(Guid Id, string sitePath)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                CourseBuilderCourse data = await _Context.CourseBuilderCourses.FindAsync(Id);
                if (data != null)
                {
                    return new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = data };
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

        public async Task<SystemMessageModel> GetLessonFileURL(Guid Id, string sitePath)
        {
            try
            {
                CourseBuilderLesson data = await _Context.CourseBuilderLessons.FindAsync(Id);
                if (data != null)
                {
                    return new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = data };
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null; //new SystemMessageModel() { MessageCode = -501, MessageDescription = "File saving Error", MessageData = ex.Message };
            }
        }

        public async Task<SystemMessageModel> GetLessonAttachmentFileURL(Guid Id, string sitePath)
        {
            try
            {
                CourseBuilderLessonFile data = await _Context.CourseBuilderLessonFiles.FindAsync(Id);
                if (data != null)
                {
                    return new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = data };
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null; //new SystemMessageModel() { MessageCode = -501, MessageDescription = "File saving Error", MessageData = ex.Message };
            }
        }


        public async Task<SystemMessageModel> SaveCourseFile(byte[] filecontent, CourseBuilderCourseDto model, long userid, string FileName, string sitePath)
        {
            string filegroupname = "coursesfile";
            try
            {
                if (filecontent != null)
                {
                    string _filePath = sitePath + "\\" + filegroupname + "\\" + model.Id.ToString().Replace("-", "") + "\\";
                    if (!Directory.Exists(_filePath))
                        Directory.CreateDirectory(_filePath);

                    _filePath += FileName;
                    string fileurl = AppDomain.CurrentDomain.BaseDirectory + "/" + filegroupname + "/" + model.Id.ToString().Replace("-", "") + "/" + FileName;
                    Uri uri = new Uri(fileurl, UriKind.Relative);

                    if (!File.Exists(_filePath))
                    {
                        File.WriteAllBytes(_filePath, filecontent);
                    }
                    FileActionDto dto = new FileActionDto()
                    {
                        filepath = _filePath,
                        fileurl = fileurl
                    };
                    return new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = dto };
                }
                else
                {
                    return new SystemMessageModel() { MessageCode = -501, MessageDescription = "File Error", MessageData = null };
                }
            }
            catch (Exception ex) { return new SystemMessageModel() { MessageCode = -501, MessageDescription = "File saving Error", MessageData = ex.Message }; }
        }

        public async Task<SystemMessageModel> SaveCourseLessonFile(byte[] filecontent, CourseBuilderLessonDto model, long userid, string FileName, string sitePath)
        {
            string filegroupname = "coursesfile";
            try
            {
                if (filecontent != null)
                {
                    string _filePath = sitePath + "\\" + filegroupname + "\\" + model.Id.ToString().Replace("-", "") + "\\";
                    if (!Directory.Exists(_filePath))
                        Directory.CreateDirectory(_filePath);

                    _filePath += FileName;
                    string fileurl = AppDomain.CurrentDomain.BaseDirectory + "/" + filegroupname + "/" + model.Id.ToString().Replace("-", "") + "/" + FileName;
                    Uri uri = new Uri(fileurl, UriKind.Relative);

                    if (!File.Exists(_filePath))
                    {
                        File.WriteAllBytes(_filePath, filecontent);
                    }
                    FileActionDto dto = new FileActionDto()
                    {
                        filepath = _filePath,
                        fileurl = fileurl
                    };
                    return new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = dto };
                }
                else
                {
                    return new SystemMessageModel() { MessageCode = -501, MessageDescription = "File Error", MessageData = null };
                }
            }
            catch (Exception ex) { return new SystemMessageModel() { MessageCode = -501, MessageDescription = "File saving Error", MessageData = ex.Message }; }
        }

        public async Task<SystemMessageModel> SaveCourseLessonAttachFile(byte[] filecontent, CourseBuilderLessonDto model, long userid, string FileName, string sitePath)
        {
            string filegroupname = "coursesfile";
            string filegroupname2 = "courseslessonattachfiles";
            try
            {
                if (filecontent != null)
                {
                    string _filePath = sitePath + "\\" + filegroupname + "\\" + model.Id.ToString().Replace("-", "") + "\\" + filegroupname2 + "\\";
                    if (!Directory.Exists(_filePath))
                        Directory.CreateDirectory(_filePath);

                    _filePath += FileName;
                    string fileurl = AppDomain.CurrentDomain.BaseDirectory + "/" + filegroupname + "/" + model.Id.ToString().Replace("-", "") + "/" + filegroupname2 + "/" + FileName;
                    Uri uri = new Uri(fileurl, UriKind.Relative);

                    if (!File.Exists(_filePath))
                    {
                        File.WriteAllBytes(_filePath, filecontent);
                    }
                    FileActionDto dto = new FileActionDto()
                    {
                        filepath = _filePath,
                        fileurl = fileurl
                    };
                    return new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = dto };
                }
                else
                {
                    return new SystemMessageModel() { MessageCode = -501, MessageDescription = "File Error", MessageData = null };
                }
            }
            catch (Exception ex) { return new SystemMessageModel() { MessageCode = -501, MessageDescription = "File saving Error", MessageData = ex.Message }; }
        }

        public async Task<SystemMessageModel> DeleteFiles(long userid, string galleryId)
        {
            try
            {
                string _filePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\gallery\\" + galleryId + "\\";

                foreach (string filename in Directory.GetFiles(_filePath))
                {
                    File.Delete(filename);
                }
                return new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = null };
            }
            catch (Exception ex) { return new SystemMessageModel() { MessageCode = -501, MessageDescription = "File saving Error", MessageData = ex.Message }; }
        }
        public async Task<SystemMessageModel> DeleteFile(string filename)
        {
            try
            {
                if (File.Exists(filename))
                    File.Delete(filename);

                return new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = null };
            }
            catch (Exception ex) { return new SystemMessageModel() { MessageCode = -501, MessageDescription = "File saving Error", MessageData = ex.Message }; }
        }

        public async Task<SystemMessageModel> AddNewCourseLessonQuez(CourseBuilderQuizDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                CourseBuilderQuiz data = new CourseBuilderQuiz()
                {
                    Id = (Guid)model.Id,
                    attemptsAllowed = model.attemptsAllowed,
                    courseId = (Guid)model.courseId,
                    topicId = (Guid)model.topicId,
                    displayQuizTime = model.displayQuizTime ?? false,
                    passingGrade = (decimal)model.passingGrade,
                    quizDescription = model.quizDescription ?? "",
                    quizFeedbackModeId = (int)model.quizFeedbackModeId,
                    quizTitle = model.quizTitle ?? "",
                    timeLimit = model.timeLimit ?? 1
                };
                await _Context.CourseBuilderQuizs.AddAsync(data);

                if (model.questions != null && model.questions.Count > 0)
                {
                    List<Question> qlist = new List<Question>();
                    List<QuestionOption> qoplist = new List<QuestionOption>();
                    foreach (QuestionDto item in model.questions)
                    {
                        Question _question = new Question()
                        {
                            Id = Guid.NewGuid(),
                            coursebuilderquizId = data.Id,
                            displayPoints = item.displayPoints ?? false,
                            isAnswerRequired = item.isAnswerRequired ?? false,
                            isRandomized = item.isRandomized ?? false,
                            points = (int)item.points,
                            questionDescription = item.questionDescription,
                            questionTitle = item.questionTitle,
                            questionType = (int)item.questionType
                        };

                        foreach (QuestionOptionDto questionOption in item.qoptions)
                        {
                            qoplist.Add(new QuestionOption()
                            {
                                Id = Guid.NewGuid(),
                                isAnswer = questionOption.isAnswer,
                                questionId = _question.Id,
                                option = questionOption.option
                            });
                        }
                        qlist.Add(_question);
                    }
                    await _Context.QuestionOptions.AddRangeAsync(qoplist);
                    await _Context.Questions.AddRangeAsync(qlist);
                }
                await _Context.SaveChangesAsync();

                if (model.advancedSettings != null)
                {
                    AdvancedSetting _advancedSetting = new AdvancedSetting()
                    {
                        Id = Guid.NewGuid(),
                        coursebuilderquizId = data.Id,
                        hideQuestionNumber = model.advancedSettings.hideQuestionNumber ?? false,
                        openEndedEssayQuestionsAnswerCharactersLimit = (int)model.advancedSettings.openEndedEssayQuestionsAnswerCharactersLimit,
                        questionsOrderId = (Guid)model.advancedSettings.questionsOrderId,
                        quizAutoStart = model.advancedSettings.quizAutoStart ?? false,
                        quizLayoutId = (Guid)model.advancedSettings.quizLayoutId,
                        shortAnswerCharactersLimit = (int)model.advancedSettings.shortAnswerCharactersLimit
                    };

                    await _Context.AdvancedSettings.AddRangeAsync(_advancedSetting);
                }



                await _Context.SaveChangesAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = model.Id };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;

        }
        public async Task<SystemMessageModel> GetCourseLessonQuezs(CourseBuilderQuizFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<CourseBuilderQuiz> query = _Context.CourseBuilderQuizs;

                if (model.Id != null)
                    query = query.Where(x => x.Id == model.Id);

                if (model.courseId != null)
                    query = query.Where(x => x.courseId == model.courseId);

                if (model.topicId != null)
                    query = query.Where(x => x.topicId == model.topicId);


                if (!string.IsNullOrEmpty(model.quizTitle))
                    query = query.Where(x => x.quizTitle.Contains(model.quizTitle.Trim()));

                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 50 : (int)model.rowcount;

                int totaldata = query.Count();
                if (totaldata <= 0) totaldata = 1;
                decimal pagecountd = ((decimal)totaldata / (decimal)PageRowCount);
                int pagecount = (totaldata / PageRowCount);
                pagecount = (pagecount <= 0) ? 1 : pagecount;
                if (Math.Floor(pagecountd) > 0)
                    pagecount++;


                List<CourseBuilderQuizDto> data = await query.Skip((pageIndex - 1) * PageRowCount).Take(PageRowCount)
                                                  .Select(x => new CourseBuilderQuizDto()
                                                  {
                                                      Id = x.Id,
                                                      courseId = x.courseId,
                                                      maxQuestionsAllowedToAnswer = x.maxQuestionsAllowedToAnswer,
                                                      quizTitle = x.quizTitle,
                                                      timeLimit = x.timeLimit,
                                                      quizDescription = x.quizDescription,
                                                      attemptsAllowed = x.attemptsAllowed,
                                                      displayQuizTime = x.displayQuizTime,
                                                      passingGrade = x.passingGrade,
                                                      quizFeedbackModeId = x.quizFeedbackModeId,
                                                      topicId = x.topicId
                                                  }).ToListAsync();


                foreach (CourseBuilderQuizDto item in data)
                {
                    item.questions = await _Context.Questions.Where(x => x.coursebuilderquizId == item.Id).Select(x => new QuestionDto()
                    {
                        QuizId = x.coursebuilderquizId,
                        Id = x.Id,
                        displayPoints = x.displayPoints,
                        isAnswerRequired = x.isAnswerRequired,
                        isRandomized = x.isRandomized,
                        points = x.points,
                        questionDescription = x.questionDescription,
                        questionTitle = x.questionTitle,
                        questionType = x.questionType
                    }).ToListAsync();

                    item.advancedSettings = await _Context.AdvancedSettings.Where(x => x.coursebuilderquizId == item.Id).Select(x => new AdvancedSettingDto()
                    {
                        Id = x.Id,
                        hideQuestionNumber = x.hideQuestionNumber,
                        openEndedEssayQuestionsAnswerCharactersLimit = x.openEndedEssayQuestionsAnswerCharactersLimit,
                        questionsOrderId = x.questionsOrderId,
                        quizAutoStart = x.quizAutoStart,
                        quizLayoutId = x.quizLayoutId,
                        shortAnswerCharactersLimit = x.shortAnswerCharactersLimit
                    }).SingleOrDefaultAsync();

                    foreach (QuestionDto qitem in item.questions)
                    {
                        qitem.qoptions = await _Context.QuestionOptions.Where(x => x.questionId == qitem.Id).Select(x => new QuestionOptionDto()
                        {
                            Id = x.Id,
                            isAnswer = x.isAnswer,
                            questionId = x.questionId,
                            lesssonId = x.lesssonId,
                            option = x.option
                        }).ToListAsync();
                    }
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

        public async Task<SystemMessageModel> SaveCourseMeetingFile(byte[] filecontent, CourseBuilderMeetingDto model, long userid, string FileName, string sitePath)
        {
            string filegroupname = "coursesmeetingfile";
            try
            {
                if (filecontent != null)
                {
                    string _filePath = sitePath + "\\" + filegroupname + "\\" + model.Id.ToString().Replace("-", "") + "\\";
                    if (!Directory.Exists(_filePath))
                        Directory.CreateDirectory(_filePath);

                    _filePath += FileName;
                    string fileurl = AppDomain.CurrentDomain.BaseDirectory + "/" + filegroupname + "/" + model.Id.ToString().Replace("-", "") + "/" + FileName;
                    Uri uri = new Uri(fileurl, UriKind.Relative);

                    if (!File.Exists(_filePath))
                    {
                        File.WriteAllBytes(_filePath, filecontent);
                    }
                    FileActionDto dto = new FileActionDto()
                    {
                        filepath = _filePath,
                        fileurl = fileurl
                    };
                    return new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = dto };
                }
                else
                {
                    return new SystemMessageModel() { MessageCode = -501, MessageDescription = "File Error", MessageData = null };
                }
            }
            catch (Exception ex) { return new SystemMessageModel() { MessageCode = -501, MessageDescription = "File saving Error", MessageData = ex.Message }; }
        }

        public async Task<SystemMessageModel> SaveCourseVideoPdfUrlFile(byte[] filecontent, CourseBuildeVideoPdfUrlDto model, long userid, string FileName, string sitePath)
        {
            string filegroupname = "coursesvideopdffile";
            try
            {
                if (filecontent != null)
                {
                    string _filePath = sitePath + "\\" + filegroupname + "\\" + model.Id.ToString().Replace("-", "") + "\\";
                    if (!Directory.Exists(_filePath))
                        Directory.CreateDirectory(_filePath);

                    _filePath += FileName;
                    string fileurl = AppDomain.CurrentDomain.BaseDirectory + "/" + filegroupname + "/" + model.Id.ToString().Replace("-", "") + "/" + FileName;
                    Uri uri = new Uri(fileurl, UriKind.Relative);

                    if (!File.Exists(_filePath))
                    {
                        File.WriteAllBytes(_filePath, filecontent);
                    }
                    FileActionDto dto = new FileActionDto()
                    {
                        filepath = _filePath,
                        fileurl = fileurl
                    };
                    return new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = dto };
                }
                else
                {
                    return new SystemMessageModel() { MessageCode = -501, MessageDescription = "File Error", MessageData = null };
                }
            }
            catch (Exception ex) { return new SystemMessageModel() { MessageCode = -501, MessageDescription = "File saving Error", MessageData = ex.Message }; }
        }

        public async Task<SystemMessageModel> GetFileByteArray(string filepath)
        {
            try
            {
                if (!string.IsNullOrEmpty(filepath))
                {
                    byte[] file = await File.ReadAllBytesAsync(filepath);
                    return new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = file };
                }
                else
                {
                    return new SystemMessageModel() { MessageCode = -501, MessageDescription = "File Error", MessageData = null };
                }
            }
            catch (Exception ex) { return new SystemMessageModel() { MessageCode = -501, MessageDescription = "File saving Error", MessageData = ex.Message }; }
        }

        internal async Task<SystemMessageModel> UpdateCourse(CourseBuilderCourseDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.Id == null)
                    return new SystemMessageModel() { MessageCode = -102, MessageDescription = "Id is Wrong" };

                CourseBuilderCourse data = await _Context.CourseBuilderCourses.FindAsync(model.Id);

                if (data == null)
                    return new SystemMessageModel() { MessageCode = -103, MessageDescription = "data not find" };

                if (!string.IsNullOrEmpty(model.courseName))
                    data.courseName = model.courseName;
                if (!string.IsNullOrEmpty(model.courseDescription))
                    data.courseDescription = model.courseDescription;
                if (!string.IsNullOrEmpty(model.excerpt))
                    data.excerpt = model.excerpt;
                //data.authorId = model.authorId;
                //data.authorname = (model.author.Fname ?? "") + " " + (model.author.Lname ?? "");
                //data.authorusername = model.author.Username;
                if (model.maximumStudents != null)
                    data.maximumStudents = model.maximumStudents ?? 1;

                if (model.difficultyLevelId != null)
                    data.courseleveltypeId = model.difficultyLevelId;

                if (model.isPublicCourse != null)
                    data.isPublicCourse = model.isPublicCourse ?? false;

                if (model.allowQA != null)
                    data.allowQA = model.allowQA ?? false;

                if (model.isvisibledropdown != null)
                    data.isvisibledropdown = model.isvisibledropdown ?? false;

                if (model.isvisible != null)
                    data.isvisible = model.isvisible ?? false;

                if (model.coursePrice != null)
                    data.coursePrice = model.coursePrice ?? 0;

                if (!string.IsNullOrEmpty(model.whatWillILearn))
                    data.whatWillILearn = model.whatWillILearn;

                if (!string.IsNullOrEmpty(model.targetedAudience))
                    data.targetedAudience = model.targetedAudience;

                if (model.maximumStudents != null)
                    data.courseDuration = model.courseDuration ?? 1;

                if (!string.IsNullOrEmpty(model.materialsIncluded))
                    data.materialsIncluded = model.materialsIncluded;

                if (!string.IsNullOrEmpty(model.requirementsInstructions))
                    data.requirementsInstructions = model.requirementsInstructions;

                if (!string.IsNullOrEmpty(model.courseIntroVideo))
                    data.courseIntroVideo = model.courseIntroVideo;

                if (!string.IsNullOrEmpty(model.courseTags))
                    data.courseTags = model.courseTags;
                //data.featuredImagename = model.featuredImagename;
                //data.featuredImagepath = (sendfilepath == true) ? model.featuredImagepath : "";
                //data.featuredImagecontent = model.featuredImagecontent;
                //data.courseFilecontent = model.courseFilecontent;
                //data.courseFilename = model.courseFilename;
                //data.courseFilepath = (sendfilepath == true) ? model.courseFilepath : "";

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

