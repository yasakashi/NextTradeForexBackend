using Base.Common.Enums;
using System.Diagnostics;
using DataLayers;
using Entities.DBEntities;
using Entities.Dtos;
using Newtonsoft.Json;
using NextTradeAPIs.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace NextTradeAPIs.Services
{
    public class LearnToTradeVideoServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;
        public LearnToTradeVideoServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
        }
        public async Task<SystemMessageModel> GetLearnToTradeVideos(VideoSearchDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<Video> query = _Context.Videos;

                if (model.title != null)
                    query = query.Where(x => x.title.Contains(model.title));

                if (model.id != null)
                    query = query.Where(x => x.id == model.id);

                if (model.lessonCategoryLevelId != null)
                    query = query.Where(x => x.lessonCategoryLevelId == model.lessonCategoryLevelId);

                if (model.downloadable != null)
                    query = query.Where(x => x.downloadable == model.downloadable);

                if (model.categoryId != null)
                {
                    List<Guid> Ids = await _Context.VideoCategories.Where(x => x.categoryid == model.categoryId).Select(x => x.id).ToListAsync();
                    if (Ids != null && Ids.Count > 0)
                        query = query.Where(x => Ids.Contains(x.id));
                }
                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 50 : (int)model.rowcount;


                List<VideoDto> data = await query.Skip((pageIndex - 1) * PageRowCount).Take(PageRowCount)
                                                .Select(x => new VideoDto()
                                                {
                                                    id = (Guid)x.id,
                                                    title = x.title,
                                                    description = x.description,
                                                    downloadable = x.downloadable,
                                                    videofilename = x.videofilename,
                                                    videofilecontenttype = x.videofilecontenttype,
                                                    videofileurl = x.videofileurl,
                                                    featuredimagename = x.featuredimagename,
                                                    featuredimagecontenttype = x.featuredimagecontenttype,
                                                    featuredimageurl = x.featuredimageurl,
                                                    excerpt = x.excerpt,
                                                    lessonCategoryLevelId = x.lessonCategoryLevelId,
                                                }).ToListAsync();

                foreach (VideoDto item in data)
                {
                    item.categories = await _Context.VideoCategories.Where(x => x.videoid == item.id).Include(x => x.category).Select(x => new CategoryBaseDto() { Id = x.categoryid, categorytypeid = x.category.categorytypeid, name = x.category.name, parentId = x.category.parentId }).ToListAsync();
                    item.videosubtitles = await _Context.VideoSubtitles.Where(x => x.videoid == item.id).Select(x => new VideoSubtitleDto()
                    {
                        id = x.id,
                        videoid = x.videoid,
                        lang = x.lang,
                        subtitlefilecontenttype = x.subtitlefilecontenttype,
                        subtitlefilename = x.subtitlefilename,
                        subtitlefileurl = x.subtitlefileurl
                    }).ToListAsync();
                }

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

        public async Task<SystemMessageModel> AddNewLearnToTradeVideo(VideoDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                Video data = new Video()
                {
                    id = (Guid)model.id,
                    title = model.title,
                    description = model.description,
                    downloadable = model.downloadable,
                    videofilename = model.videofilename,
                    videofilecontenttype = model.videofilecontenttype,
                    videofileurl = model.videofileurl,
                    videofilepath = model.videofilepath,
                    featuredimagename = model.featuredimagename,
                    featuredimagecontenttype = model.featuredimagecontenttype,
                    featuredimageurl = model.featuredimageurl,
                    featuredimagepath = model.featuredimagepath,
                    excerpt = model.excerpt,
                    lessonCategoryLevelId = model.lessonCategoryLevelId
                };
                await _Context.Videos.AddAsync(data);

                if (model.categoriesIds != null && model.categoriesIds.Count > 0)
                {
                    List<VideoCategory> categories = new List<VideoCategory>();
                    foreach (long cId in model.categoriesIds)
                    {
                        categories.Add(new VideoCategory() { id = Guid.NewGuid(), categoryid = cId, videoid = data.id });
                    }
                    await _Context.VideoCategories.AddRangeAsync(categories);
                }

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

        public async Task<SystemMessageModel> RemoveLearnToTradeVideo(VideoSearchDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.id == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -1), MessageDescription = "Data is wrong", MessageData = model };
                Video data = await _Context.Videos.FindAsync(model.id);

                if (data == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -2), MessageDescription = "Data is wrong", MessageData = model };

                _Context.Videos.Remove(data);

                List<VideoCategory> categories = await _Context.VideoCategories.Where(x => x.videoid == model.id).ToListAsync();
                _Context.VideoCategories.RemoveRange(categories);

                List<VideoSubtitle> videosubtitles = await _Context.VideoSubtitles.Where(x => x.videoid == model.id).ToListAsync();
                _Context.VideoSubtitles.RemoveRange(videosubtitles);

                foreach (VideoSubtitle item in videosubtitles)
                { await DeleteFile(item.subtitlefilepath); }

                await _Context.SaveChangesAsync();

                await DeleteFile(data.videofilepath);
                await DeleteFile(data.featuredimagepath);
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

        public async Task<SystemMessageModel> EditLearnToTradeVideo(VideoDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.id == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -1), MessageDescription = "Data is wrong", MessageData = model };
                Video data = await _Context.Videos.FindAsync(model.id);

                if (data == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -2), MessageDescription = "Data is wrong", MessageData = model };



                data.title = model.title;
                data.description = model.description;
                data.downloadable = model.downloadable;
                data.excerpt = model.excerpt;
                data.lessonCategoryLevelId = model.lessonCategoryLevelId;

                if (!string.IsNullOrEmpty(model.videofilename))
                    data.videofilename = model.videofilename;
                if (!string.IsNullOrEmpty(model.videofilecontenttype))
                    data.videofilecontenttype = model.videofilecontenttype;
                if (!string.IsNullOrEmpty(model.videofileurl))
                    data.videofileurl = model.videofileurl;
                if (!string.IsNullOrEmpty(model.videofilepath))
                    data.videofilepath = model.videofilepath;
                if (!string.IsNullOrEmpty(model.featuredimagename))
                    data.featuredimagename = model.featuredimagename;
                if (!string.IsNullOrEmpty(model.featuredimagecontenttype))
                    data.featuredimagecontenttype = model.featuredimagecontenttype;
                if (!string.IsNullOrEmpty(model.featuredimageurl))
                    data.featuredimageurl = model.featuredimageurl;
                if (!string.IsNullOrEmpty(model.featuredimagepath))
                    data.featuredimagepath = model.featuredimagepath;

                _Context.Videos.Update(data);

                if (model.categoriesIds != null && model.categoriesIds.Count > 0)
                {
                    List<VideoCategory> recategories = await _Context.VideoCategories.Where(x => x.videoid == model.id).ToListAsync();
                    _Context.VideoCategories.RemoveRange(recategories);

                    List<VideoCategory> categories = new List<VideoCategory>();
                    foreach (long cId in model.categoriesIds)
                    {
                        categories.Add(new VideoCategory() { id = Guid.NewGuid(), categoryid = cId, videoid = data.id });
                    }
                    await _Context.VideoCategories.AddRangeAsync(categories);
                }

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

        public async Task<SystemMessageModel> AddNewVideoSuntitle(VideoSubtitleDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                VideoSubtitle data = new VideoSubtitle()
                {
                    id = (Guid)model.id,
                    videoid = (Guid)model.videoid,
                    lang = model.lang,
                    subtitlefilecontenttype = model.subtitlefilecontenttype,
                    subtitlefilename = model.subtitlefilename,
                    subtitlefilepath = model.subtitlefilepath,
                    subtitlefileurl = model.subtitlefileurl
                };

                await _Context.VideoSubtitles.AddAsync(data);


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

        public async Task<SystemMessageModel> RemoveVideoSubtitle(VideoSubtitleSearchDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.id == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -1), MessageDescription = "Data is wrong", MessageData = model };
                VideoSubtitle data = await _Context.VideoSubtitles.FindAsync(model.id);

                if (data == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -2), MessageDescription = "Data is wrong", MessageData = model };

                _Context.VideoSubtitles.Remove(data);


                await _Context.SaveChangesAsync();

                await DeleteFile(data.subtitlefilepath);
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


        public async Task<SystemMessageModel> SaveFile(byte[] filecontent, VideoDto model, long userid, string FileName, string sitePath, string hosturl)
        {
            string filegroupname = "LearnToTradeVideo";
            try
            {
                if (filecontent != null)
                {
                    string _filePath = sitePath + "\\" + filegroupname + "\\" + model.id.ToString().Replace("-", "") + "\\";
                    if (!Directory.Exists(_filePath))
                        Directory.CreateDirectory(_filePath);

                    _filePath += FileName;
                    string fileurl = hosturl + "/" + filegroupname + "/" + model.id.ToString().Replace("-", "") + "/" + FileName;

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

        public async Task<SystemMessageModel> SaveSubtitleFile(byte[] filecontent, Guid videoid, long userid, string FileName, string sitePath, string hosturl)
        {
            string filegroupname = "LearnToTradeVideo";
            try
            {
                if (filecontent != null)
                {
                    string _filePath = sitePath + "\\" + filegroupname + "\\" + videoid.ToString().Replace("-", "") + "\\";
                    if (!Directory.Exists(_filePath))
                        Directory.CreateDirectory(_filePath);

                    _filePath += FileName;
                    string fileurl = hosturl + "/" + filegroupname + "/" + videoid.ToString().Replace("-", "") + "/" + FileName;

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

    }
}
