using Base.Common.Enums;
using DataLayers;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using Entities.DBEntities;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NextTradeAPIs.Dtos;
using System.Diagnostics;

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
                    featuredImagecontent = model.featuredImagecontent
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
                if (CourseCategoryList.Count() > 0)
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
                if (CourseCategoryList.Count() > 0)
                    await _Context.CourseBuildeVideoPdfUrls.AddRangeAsync(videoPdfUrllist);

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


        public async Task<SystemMessageModel> GetCourses(CourseBuilderCourseDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
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
                    query = query.Where(x => x.Id == model.Id);

                if (model.authorId != null)
                    query = query.Where(x => x.authorId == model.authorId);

                if (model.allowQA != null)
                    query = query.Where(x => x.allowQA == model.allowQA);

                if (model.isPublicCourse != null)
                    query = query.Where(x => x.isPublicCourse == model.isPublicCourse);

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


                List<CourseBuilderCourseDto> data = await query.Skip((pageIndex - 1) * PageRowCount).Take(PageRowCount)
                                                  .Select(x => new CourseBuilderCourseDto()
                                                  {
                                                      Id = x.Id,
                                                      courseName = x.courseName,
                                                      courseDescription = x.courseDescription,
                                                      courseFilename = x.courseFilename,
                                                      courseFilepath = x.courseFilepath,
                                                      excerpt = x.excerpt,
                                                      authorId = x.authorId,
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
                                                      featuredImagepath = x.featuredImagepath,
                                                      registerdatetime = x.registerdatetime,
                                                      courseFilecontent = x.courseFilecontent,
                                                      featuredImagecontent = x.featuredImagecontent
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
                    topicSummary = model.topicSummary
                };
                await _Context.CourseBuilderTopics.AddAsync(data);

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


                List<CourseBuilderTopicDto> data = await query.Skip((pageIndex - 1) * PageRowCount).Take(PageRowCount)
                                                  .Select(x => new CourseBuilderTopicDto()
                                                  {
                                                      Id = x.Id,
                                                      courseId = x.courseId,
                                                      topicName = x.topicName,
                                                      topicSummary = x.topicSummary
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
                    videoPlaybackTime = model.videoPlaybackTime,
                    lessonFilepath = model.lessonFilepath,
                    videoSource = model.videoSource
                };
                await _Context.CourseBuilderLessons.AddAsync(data);


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


        public async Task<SystemMessageModel> GetCourseLessons(CourseBuilderLessonDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
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


                List<CourseBuilderLessonDto> data = await query.Skip((pageIndex - 1) * PageRowCount).Take(PageRowCount)
                                                  .Select(x => new CourseBuilderLessonDto()
                                                  {
                                                      Id = x.Id,
                                                      courseId = x.courseId,
                                                      featureImagename = x.featureImagename,
                                                      featureImagecontenttype = x.featureImagecontenttype,
                                                      featureImagepath = x.featureImagepath,
                                                      lessonDescription = x.lessonDescription,
                                                      lessonFilecontenttype = x.lessonFilecontenttype,
                                                      lessonFilename = x.lessonFilename,
                                                      lessonFilepath = x.lessonFilepath,
                                                      lessonName = x.lessonName,
                                                      topicId = x.topicId,
                                                      videoPlaybackTime = x.videoPlaybackTime,
                                                      videoSource = x.videoSource
                                                  }).ToListAsync();


                foreach (CourseBuilderLessonDto item in data)
                {
                    item.fileattachments = await _Context.CourseBuilderLessonFiles.Where(x => x.lessonId == item.Id).Select(x => new CourseBuilderLessonFileDto()
                    {
                        lessonId = x.lessonId,
                        Id = x.Id,
                        lessonFilecontenttype = x.lessonFilecontenttype,
                        lessonFilename = x.lessonFilename,
                        lessonFilepath = x.lessonFilepath
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
        public async Task<SystemMessageModel> DeleteFile(string filename, string galleryId)
        {
            try
            {
                string _filePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\gallery\\" + galleryId + "\\" + filename;

                File.Delete(filename);

                return new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = null };
            }
            catch (Exception ex) { return new SystemMessageModel() { MessageCode = -501, MessageDescription = "File saving Error", MessageData = ex.Message }; }
        }

        internal async Task<SystemMessageModel> AddNewCourseLessonQuez(CourseBuilderQuizDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
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
                    quizFeedbackModeId = (long)model.quizFeedbackModeId,
                    quizTitle = model.quizTitle ?? "",
                    timeLimit = model.timeLimit ?? 1
                };
                await _Context.CourseBuilderQuizs.AddAsync(data);

                if (model.questions != null && model.questions.Count > 0)
                {
                    List<Question> qlist = new List<Question>();
                    foreach (QuestionDto item in model.questions)
                    {
                        qlist.Add(new Question()
                        {
                            Id = Guid.NewGuid(),
                            coursebuilderquizId = data.Id,
                            displayPoints = item.displayPoints?? false,
                            isAnswerRequired = item.isAnswerRequired?? false,
                            isRandomized = item.isRandomized?? false,
                            points = (int) item.points,
                            questionDescription = item.questionDescription,
                            questionTitle = item.questionTitle,
                            questionType = (int)item.questionType
                        });
                    }
                    await _Context.Questions.AddRangeAsync(qlist);
                }

                if (model.advancedSettings != null )
                {
                    AdvancedSetting _advancedSetting= new AdvancedSetting()
                    {
                        Id = Guid.NewGuid(),
                        coursebuilderquizId = data.Id,
                        hideQuestionNumber = model.advancedSettings.hideQuestionNumber??false,
                        openEndedEssayQuestionsAnswerCharactersLimit = (int)model.advancedSettings.openEndedEssayQuestionsAnswerCharactersLimit,
                        questionsOrderId = (Guid)model.advancedSettings.questionsOrderId,
                        quizAutoStart = model.advancedSettings.quizAutoStart??false,
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
        public async Task<SystemMessageModel> GetCourseLessonQuezs(CourseBuilderQuizDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
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
                        qitem.options = await _Context.QuestionOptions.Where(x => x.questionId == qitem.Id).Select(x => new QuestionOptionDto() { 
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

        
    }
}
