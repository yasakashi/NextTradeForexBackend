using System.Diagnostics;
using Base.Common.Enums;
using Base.Common.Generator;
using DataLayers;
using Entities.DBEntities;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NextTradeAPIs.Dtos;

namespace NextTradeAPIs.Services
{
    public class ADPodcastServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;
        public ADPodcastServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
        }


        public async Task<SystemMessageModel> GetADPodcasts(ADPodcastSearchDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<ADPodcast> query = _Context.ADPodcasts;

                if (model.title != null)
                    query = query.Where(x => x.title.Contains(model.title));


                if (model.shortcode != null)
                    query = query.Where(x => x.shortcode.Contains(model.shortcode));

                if (model.id != null)
                    query = query.Where(x => x.id == model.id);

                if (model.categoryid != null)
                {
                    List<Guid> PIds = await _Context.ADPodcastCategories.Where(x => x.categoryid == model.categoryid).Select(x => x.id).ToListAsync();
                    query = query.Where(x => PIds.Contains(x.id));
                }

                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 50 : (int)model.rowcount;


                List<ADPodcastDto> data = await query.Skip((pageIndex - 1) * PageRowCount).Take(PageRowCount)
                                                  .Select(x => new ADPodcastDto()
                                                  {
                                                      id = (Guid)x.id,
                                                      title = x.title,
                                                      shortcode = x.shortcode,
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

                foreach (ADPodcastDto item in data)
                {
                    item.categories = await _Context.ADPodcastCategories.Where(x => x.podcastid == item.id).Include(x => x.category).Select(x => new CategoryBaseDto() { Id = x.categoryid, name = x.category.name, categorytypeid = x.category.categorytypeid, parentId = x.category.parentId }).ToListAsync();
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

        public async Task<SystemMessageModel> AddNewADPodcast(ADPodcastDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                ADPodcast data = new ADPodcast()
                {
                    id = (Guid)model.id,
                    title = model.title,
                    shortcode = model.shortcode ?? "pflip id = ? / pflip".Replace("?", KeyGenerator.Generate(6)),
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

                await _Context.ADPodcasts.AddAsync(data);

                List<ADPodcastCategory> categorylist = new List<ADPodcastCategory>();
                foreach (long catId in model.categoryids)
                {
                    categorylist.Add(new ADPodcastCategory()
                    {
                        id = Guid.NewGuid(),
                        categoryid = catId,
                        podcastid = data.id
                    });
                }
                if (categorylist != null && categorylist.Count > 0)
                    await _Context.ADPodcastCategories.AddRangeAsync(categorylist);

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

        public async Task<SystemMessageModel> RemoveADPodcast(ADPodcastSearchDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.id == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -1), MessageDescription = "Data is wrong", MessageData = model };
                ADPodcast data = await _Context.ADPodcasts.FindAsync(model.id);

                if (data == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -2), MessageDescription = "Data is wrong", MessageData = model };

                _Context.ADPodcasts.Remove(data);

                await DeleteFile(data.audiofilepath);
                await DeleteFile(data.featuredimagepath);

                List<ADPodcastCategory> categoeris = await _Context.ADPodcastCategories.Where(x => x.podcastid == data.id).ToListAsync();
                _Context.ADPodcastCategories.RemoveRange(categoeris);

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

        public async Task<SystemMessageModel> EditADPodcast(ADPodcastDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.id == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -1), MessageDescription = "Data is wrong", MessageData = model };
                ADPodcast data = await _Context.ADPodcasts.FindAsync(model.id);

                if (data == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -2), MessageDescription = "Data is wrong", MessageData = model };


                data.title = model.title;
                data.excerpt = model.excerpt;
                data.shortcode = model.shortcode;
                data.description = model.description;
                data.audiofilename = model.audiofilename;
                data.audiofilepath = model.audiofilepath;
                data.audiofilecontenttype = model.audiofilecontenttype;
                data.audiofileurl = model.audiofileurl;
                data.featuredimagename = model.featuredimagename;
                data.featuredimagepath = model.featuredimagepath;
                data.featuredimageurl = model.featuredimageurl;
                data.featuredimagecontenttype = model.featuredimagecontenttype;

                _Context.ADPodcasts.Update(data);

                if (model.categoryids != null && model.categoryids.Count > 0)
                {
                    List<ADPodcastCategory> categoeris = await _Context.ADPodcastCategories.Where(x => x.podcastid == data.id).ToListAsync();
                    _Context.ADPodcastCategories.RemoveRange(categoeris);

                    List<ADPodcastCategory> categorylist = new List<ADPodcastCategory>();
                    foreach (long catId in model.categoryids)
                    {
                        categorylist.Add(new ADPodcastCategory()
                        {
                            id = Guid.NewGuid(),
                            categoryid = catId,
                            podcastid = data.id
                        });
                    }
                    if (categorylist != null && categorylist.Count > 0)
                        await _Context.ADPodcastCategories.AddRangeAsync(categorylist);
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

        public async Task<SystemMessageModel> SaveFile(byte[] filecontent, ADPodcastDto model, long userid, string FileName, string sitePath, string hosturl)
        {
            string filegroupname = "ADPodcastfile";
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
