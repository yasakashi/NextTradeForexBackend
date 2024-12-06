using Base.Common.Enums;
using System.Diagnostics;
using DataLayers;
using Entities.DBEntities;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NextTradeAPIs.Dtos;
using Microsoft.IdentityModel.Tokens;

namespace NextTradeAPIs.Services
{
    public class WebinarServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;
        public WebinarServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
        }


        public async Task<SystemMessageModel> GetWebinars(WebinarSearchDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<Webinar> query = _Context.Webinars;

                if (model.title != null)
                    query = query.Where(x => x.title.Contains(model.title));

                if (model.id != null)
                    query = query.Where(x => x.id == model.id);

                if (model.categoryid != null)
                {
                    List<Guid> PIds = await _Context.WebinarCategories.Where(x => x.categoryid == model.categoryid).Select(x => x.id).ToListAsync();
                    query = query.Where(x => PIds.Contains(x.id));
                }
                if (model.fromdateAndTime != null)
                    query = query.Where(x => x.dateAndTime >= model.fromdateAndTime);

                if (model.todateAndTime != null)
                    query = query.Where(x => x.dateAndTime <= model.todateAndTime);

                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 50 : (int)model.rowcount;


                List<WebinarDto> data = await query.Skip((pageIndex - 1) * PageRowCount).Take(PageRowCount)
                                                  .Select(x => new WebinarDto()
                                                  {
                                                      id = x.id,
                                                      title = x.title,
                                                      excerpt = x.excerpt,
                                                      description = x.description,
                                                      videofilename = x.videofilename,
                                                      videofilepath = x.videofilepath,
                                                      videofilecontenttype = x.videofilecontenttype,
                                                      videofileurl = x.videofileurl,
                                                      featuredimagename = x.featuredimagename,
                                                      featuredimagepath = x.featuredimagepath,
                                                      featuredimageurl = x.featuredimageurl,
                                                      featuredimagecontenttype = x.featuredimagecontenttype,
                                                      meetingLink = x.meetingLink,
                                                      dateAndTime = x.dateAndTime
                                                  }).ToListAsync();

                foreach (WebinarDto item in data)
                {
                    item.categories = await _Context.WebinarCategories.Where(x => x.webinarid == item.id).Include(x => x.category).Select(x => new CategoryBaseDto() { Id = x.categoryid, name = x.category.name, categorytypeid = x.category.categorytypeid, parentId = x.category.parentId }).ToListAsync();
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

        public async Task<SystemMessageModel> AddNewWebinar(WebinarDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                Webinar data = new Webinar()
                {
                    id = (Guid)model.id,
                    title = model.title,
                    excerpt = model.excerpt,
                    description = model.description,
                    videofilename = model.videofilename,
                    videofilepath = model.videofilepath,
                    videofilecontenttype = model.videofilecontenttype,
                    videofileurl = model.videofileurl,
                    featuredimagename = model.featuredimagename,
                    featuredimagepath = model.featuredimagepath,
                    featuredimageurl = model.featuredimageurl,
                    featuredimagecontenttype = model.featuredimagecontenttype,
                    meetingLink = model.meetingLink,
                    dateAndTime = model.dateAndTime
                };

                await _Context.Webinars.AddAsync(data);

                List<WebinarCategory> webinarcategorylist = new List<WebinarCategory>();
                foreach (long catId in model.categoryids)
                {
                    webinarcategorylist.Add(new WebinarCategory()
                    {
                        id = Guid.NewGuid(),
                        categoryid = catId,
                        webinarid = data.id
                    });
                }
                if (webinarcategorylist != null && webinarcategorylist.Count > 0)
                    await _Context.WebinarCategories.AddRangeAsync(webinarcategorylist);

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

        public async Task<SystemMessageModel> RemoveWebinar(WebinarSearchDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.id == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -1), MessageDescription = "Data is wrong", MessageData = model };
                Webinar data = await _Context.Webinars.FindAsync(model.id);

                if (data == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -2), MessageDescription = "Data is wrong", MessageData = model };

                _Context.Webinars.Remove(data);

                await DeleteFile(data.videofilepath);
                await DeleteFile(data.featuredimagepath);


                List<WebinarCategory> categoeris = await _Context.WebinarCategories.Where(x => x.webinarid == data.id).ToListAsync();
                _Context.WebinarCategories.RemoveRange(categoeris);

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

        public async Task<SystemMessageModel> EditWebinar(WebinarDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.id == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -1), MessageDescription = "Data is wrong", MessageData = model };
                Webinar data = await _Context.Webinars.FindAsync(model.id);

                if (data == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -2), MessageDescription = "Data is wrong", MessageData = model };

                data.title = model.title;
                data.excerpt = model.excerpt;
                data.description = model.description;
                data.meetingLink = model.meetingLink;
                data.dateAndTime = model.dateAndTime;

                if (!string.IsNullOrEmpty(model.featuredimagename))
                    data.featuredimagename = model.featuredimagename;
                if (!string.IsNullOrEmpty(model.featuredimagepath))
                    data.featuredimagepath = model.featuredimagepath;
                if (!string.IsNullOrEmpty(model.featuredimageurl))
                    data.featuredimageurl = model.featuredimageurl;
                if (!string.IsNullOrEmpty(model.featuredimagecontenttype))
                    data.featuredimagecontenttype = model.featuredimagecontenttype;

                if (!string.IsNullOrEmpty(model.videofilename))
                    data.videofilename = model.videofilename;
                if (!string.IsNullOrEmpty(model.videofilepath))
                    data.videofilepath = model.videofilepath;
                if (!string.IsNullOrEmpty(model.videofileurl))
                    data.videofileurl = model.videofileurl;
                if (!string.IsNullOrEmpty(model.videofilecontenttype))
                    data.videofilecontenttype = model.videofilecontenttype;

                _Context.Webinars.Update(data);

                if (model.categoryids != null && model.categoryids.Count > 0)
                {
                    List<WebinarCategory> categoeris = await _Context.WebinarCategories.Where(x => x.webinarid == data.id).ToListAsync();
                    _Context.WebinarCategories.RemoveRange(categoeris);

                    List<WebinarCategory> categorylist = new List<WebinarCategory>();
                    foreach (long catId in model.categoryids)
                    {
                        categorylist.Add(new WebinarCategory()
                        {
                            id = Guid.NewGuid(),
                            categoryid = catId,
                            webinarid = data.id
                        });
                    }
                    if (categorylist != null && categorylist.Count > 0)
                        await _Context.WebinarCategories.AddRangeAsync(categorylist);
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

        public async Task<SystemMessageModel> SaveFile(byte[] filecontent, WebinarDto model, long userid, string FileName, string sitePath, string hosturl)
        {
            string filegroupname = "webinarsfile";
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
