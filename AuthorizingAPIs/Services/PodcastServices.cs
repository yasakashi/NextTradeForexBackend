using System.Diagnostics;
using Base.Common.Enums;
using DataLayers;
using Entities.DBEntities;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NextTradeAPIs.Dtos;

namespace NextTradeAPIs.Services
{
    public class PodcastServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;
        public PodcastServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
        }


        public async Task<SystemMessageModel> GetPadcasts(PodcastSearchDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<Podcast> query = _Context.Podcasts;

                if (model.title != null)
                    query = query.Where(x => x.title.Contains(model.title));

                if (model.id != null)
                    query = query.Where(x => x.id == model.id);

                if (model.categoryid != null)
                {
                    List<Guid> PIds = await _Context.PodcastCategories.Where(x => x.categoryid == model.categoryid).Select(x => x.id).ToListAsync();
                    query = query.Where(x => PIds.Contains(x.id));
                }

                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 50 : (int)model.rowcount;


                List<PodcastDto> data = await query.Skip((pageIndex - 1) * PageRowCount).Take(PageRowCount)
                                                  .Select(x => new PodcastDto()
                                                  {
                                                      id = (Guid)x.id,
                                                      title = x.title,
                                                      excerpt = x.excerpt,
                                                      description = x.description,
                                                      audiofilename = x.audiofilename,
                                                      audiofilepath = string.Empty,
                                                      audiofilecontenttype = x.audiofilecontenttype,
                                                      audiofileurl = x.audiofileurl,
                                                      featuredimagename = x.featuredimagename,
                                                      featuredimagepath = string.Empty,
                                                      featuredimageurl = x.featuredimageurl,
                                                      featuredimagecontenttype = x.featuredimagecontenttype

                                                  }).ToListAsync();

                foreach (PodcastDto item in data)
                {
                    item.categories = await _Context.PodcastCategories.Where(x => x.podcastid == item.id).Include(x => x.category).Select(x => new CategoryBaseDto() { Id = x.categoryid, name = x.category.name, categorytypeid = x.category.categorytypeid, parentId = x.category.parentId }).ToListAsync();
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

        public async Task<SystemMessageModel> AddNewPadcast(PodcastDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                Podcast data = new Podcast()
                {
                    id = (Guid)model.id,
                    title = model.title,
                    excerpt = model.excerpt,
                    description = model.description,
                    audiofilename = model.audiofilename,
                    audiofilepath = model.audiofilepath,
                    audiofilecontenttype = model.audiofilecontenttype,
                    audiofileurl = model.audiofileurl,
                    featuredimagename = model.featuredimagename,
                    featuredimagepath = model.featuredimagepath,
                    featuredimageurl = model.featuredimageurl,
                    featuredimagecontenttype = model.featuredimagecontenttype

                };

                await _Context.Podcasts.AddAsync(data);

                List<PodcastCategory> categorylist = new List<PodcastCategory>();
                foreach (long catId in model.categoryids)
                {
                    categorylist.Add(new PodcastCategory()
                    {
                        id = Guid.NewGuid(),
                        categoryid = catId,
                        podcastid = data.id
                    });
                }
                if (categorylist != null && categorylist.Count > 0)
                    await _Context.PodcastCategories.AddRangeAsync(categorylist);

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

        public async Task<SystemMessageModel> RemovePodcast(PodcastSearchDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.id == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -1), MessageDescription = "Data is wrong", MessageData = model };
                Podcast data = await _Context.Podcasts.FindAsync(model.id);

                if (data == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -2), MessageDescription = "Data is wrong", MessageData = model };

                _Context.Podcasts.Remove(data);

                await DeleteFile(data.audiofilepath);
                await DeleteFile(data.featuredimagepath);

                List<PodcastCategory> categoeris = await _Context.PodcastCategories.Where(x => x.podcastid == data.id).ToListAsync();
                _Context.PodcastCategories.RemoveRange(categoeris);

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

        public async Task<SystemMessageModel> EditPodcast(PodcastDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.id == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -1), MessageDescription = "Data is wrong", MessageData = model };
                Podcast data = await _Context.Podcasts.FindAsync(model.id);

                if (data == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -2), MessageDescription = "Data is wrong", MessageData = model };


                data.title = model.title;
                data.excerpt = model.excerpt;
                data.description = model.description;
                data.audiofilename = model.audiofilename;
                data.audiofilepath = model.audiofilepath;
                data.audiofilecontenttype = model.audiofilecontenttype;
                data.audiofileurl = model.audiofileurl;
                data.featuredimagename = model.featuredimagename;
                data.featuredimagepath = model.featuredimagepath;
                data.featuredimageurl = model.featuredimageurl;
                data.featuredimagecontenttype = model.featuredimagecontenttype;

                _Context.Podcasts.Update(data);

                if (model.categoryids != null && model.categoryids.Count > 0)
                {
                    List<PodcastCategory> categoeris = await _Context.PodcastCategories.Where(x => x.podcastid == data.id).ToListAsync();
                    _Context.PodcastCategories.RemoveRange(categoeris);

                    List<PodcastCategory> categorylist = new List<PodcastCategory>();
                    foreach (long catId in model.categoryids)
                    {
                        categorylist.Add(new PodcastCategory()
                        {
                            id = Guid.NewGuid(),
                            categoryid = catId,
                            podcastid = data.id
                        });
                    }
                    if (categorylist != null && categorylist.Count > 0)
                        await _Context.PodcastCategories.AddRangeAsync(categorylist);
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

        public async Task<SystemMessageModel> SaveFile(byte[] filecontent, PodcastDto model, long userid, string FileName, string sitePath, string hosturl)
        {
            string filegroupname = "podcastfile";
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
