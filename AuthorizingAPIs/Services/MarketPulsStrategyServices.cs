using Base.Common.Enums;
using DataLayers;
using Entities.DBEntities;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NextTradeAPIs.Dtos;
using System.Diagnostics;
using System.Security.AccessControl;

namespace NextTradeAPIs.Services
{
    public class MarketPulsStrategyServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;
        public MarketPulsStrategyServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
        }

        public async Task<SystemMessageModel> SaveStrategyItem(StrategyDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (await _Context.Strategys.Where(x => x.categoryid == model.categoryid).AnyAsync())
                {
                    return new SystemMessageModel() { MessageCode = -103, MessageDescription = "For this category save before use update service", MessageData = model };
                }

                Strategy data = new Strategy()
                {
                    id = Guid.NewGuid(),
                    categoryid = (long)model.categoryid,
                    creatoruserid = userlogin.userid,
                    isvisible = model.isvisible ?? true,
                    courseleveltypeId = model.courseleveltypeId ?? 0,
                    title = model.title,
                    excerpt = model.excerpt,
                    authorid = model.authorid,
                    authorname = model.authorname,
                    createdatetime = DateTime.Now,
                    changestatusdate = DateTime.Now,
                    coursestatusid = (int)CourseStatusList.Draft,
                    tags = model.tags,
                    privatenotes = model.privatenotes,
                };
                await _Context.Strategys.AddAsync(data);

                if (model.mainlessoncontentlist != null)
                {
                    List<StrategyMainLessonContent> mainlessoncontents = new List<StrategyMainLessonContent>();
                    foreach (StrategyMainLessonContentDto mainlessoncontent in model.mainlessoncontentlist)
                    {
                        StrategyMainLessonContent item = new StrategyMainLessonContent()
                        {
                            id = Guid.NewGuid(),
                            strategyid = data.id,
                            strategycontenttypeid = mainlessoncontent.strategycontenttypeid,
                            descritption = mainlessoncontent.descritption,
                            descritptionfilename = mainlessoncontent.descritptionfilename,
                            descritptionfilepath = mainlessoncontent.descritptionfilepath,
                            descritptionfileurl = mainlessoncontent.descritptionfileurl,
                            descritptionfilecontenttype = mainlessoncontent.descritptionfilecontenttype,
                            imagefilename = mainlessoncontent.imagefilename,
                            imagefilepath = mainlessoncontent.imagefilepath,
                            imagefileurl = mainlessoncontent.imagefileurl,
                            imagefilecontenttype = mainlessoncontent.imagefilecontenttype,
                            galleryvideofilename = mainlessoncontent.galleryvideofilename,
                            galleryvideofilepath = mainlessoncontent.galleryvideofilepath,
                            galleryvideofileurl = mainlessoncontent.galleryvideofileurl,
                            galleryvideofilecontenttype = mainlessoncontent.galleryvideofilecontenttype,
                            youtubevideo = mainlessoncontent.youtubevideo,
                            videofromanyothersource = mainlessoncontent.videofromanyothersource,
                            pdftitle = mainlessoncontent.pdftitle,
                            pdfshortcodeid = mainlessoncontent.pdfshortcodeid,
                            pdfauther = mainlessoncontent.pdfauther,
                            pdfshortdescription = mainlessoncontent.pdfshortdescription,
                            tabletitle = mainlessoncontent.tabletitle,
                            tableshortcodeid = mainlessoncontent.tableshortcodeid,
                            widgetscript = mainlessoncontent.widgetscript,
                            audiobookfilename = mainlessoncontent.audiobookfilename,
                            audiobookfilepath = mainlessoncontent.audiobookfilepath,
                            audiobookfileurl = mainlessoncontent.audiobookfileurl,
                            audiobookfilecontenttype = mainlessoncontent.audiobookfilecontenttype,
                            galleryimagefilename = mainlessoncontent.galleryimagefilename,
                            galleryimagefilepath = mainlessoncontent.galleryimagefilepath,
                            galleryimagefileurl = mainlessoncontent.galleryimagefileurl,
                            galleryimagefilecontenttype = mainlessoncontent.galleryimagefilecontenttype
                        };


                        mainlessoncontents.Add(item);
                    }

                    await _Context.StrategyMainLessonContents.AddRangeAsync(mainlessoncontents);

                }

                await _Context.SaveChangesAsync();
                model.id = data.id;

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

        public async Task<SystemMessageModel> SaveStrategyItem(StrategyMainLessonContentDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {

                StrategyMainLessonContent item = new StrategyMainLessonContent()
                {
                    id = Guid.NewGuid(),
                    strategyid = model.strategyid,
                    strategycontenttypeid = model.strategycontenttypeid,
                    descritption = model.descritption,
                    descritptionfilename = model.descritptionfilename,
                    descritptionfilepath = model.descritptionfilepath,
                    descritptionfileurl = model.descritptionfileurl,
                    descritptionfilecontenttype = model.descritptionfilecontenttype,
                    imagefilename = model.imagefilename,
                    imagefilepath = model.imagefilepath,
                    imagefileurl = model.imagefileurl,
                    imagefilecontenttype = model.imagefilecontenttype,
                    galleryvideofilename = model.galleryvideofilename,
                    galleryvideofilepath = model.galleryvideofilepath,
                    galleryvideofileurl = model.galleryvideofileurl,
                    galleryvideofilecontenttype = model.galleryvideofilecontenttype,
                    youtubevideo = model.youtubevideo,
                    videofromanyothersource = model.videofromanyothersource,
                    pdftitle = model.pdftitle,
                    pdfshortcodeid = model.pdfshortcodeid,
                    pdfauther = model.pdfauther,
                    pdfshortdescription = model.pdfshortdescription,
                    tabletitle = model.tabletitle,
                    tableshortcodeid = model.tableshortcodeid,
                    widgetscript = model.widgetscript,
                    audiobookfilename = model.audiobookfilename,
                    audiobookfilepath = model.audiobookfilepath,
                    audiobookfileurl = model.audiobookfileurl,
                    audiobookfilecontenttype = model.audiobookfilecontenttype,
                    galleryimagefilename = model.galleryimagefilename,
                    galleryimagefilepath = model.galleryimagefilepath,
                    galleryimagefileurl = model.galleryimagefileurl,
                    galleryimagefilecontenttype = model.galleryimagefilecontenttype
                };


                await _Context.StrategyMainLessonContents.AddAsync(item);


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

        public async Task<SystemMessageModel> GetStrategyItems(StrategyFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                List<StrategyDto> datas;
                IQueryable<Strategy> query = _Context.Strategys;

                if (model.id != null)
                    query = query.Where(x => x.id == model.id);

                if (model.categoryid != null)
                    query = query.Where(x => x.categoryid == model.categoryid);

                if (model.sortitem != null && model.sortitem.Count() > 0)
                    if (model.sortitem != null)
                    {
                        foreach (var item in model.sortitem)
                        {
                            if (item.ascending == null || (bool)item.ascending)
                            {
                                switch (item.fieldname.ToLower())
                                {
                                    case "createdatetime":
                                        query = query.OrderBy(x => x.createdatetime);
                                        break;
                                };
                            }
                            else if (!(bool)item.ascending)
                            {
                                switch (item.fieldname.ToLower())
                                {
                                    case "createdatetime":
                                        query = query.OrderByDescending(x => x.createdatetime);
                                        break;
                                };
                            }
                        }
                    }

                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 10 : (int)model.rowcount;
                int totaldata = query.Count();
                if (totaldata <= 0) totaldata = 1;
                decimal pagecountd = ((decimal)totaldata / (decimal)PageRowCount);
                int pagecount = (totaldata / PageRowCount);
                pagecount = (pagecount <= 0) ? 1 : pagecount;
                if (Math.Floor(pagecountd) > 0)
                    pagecount++;

                datas = await query.Skip((pageIndex - 1) * PageRowCount)
                                .Take(PageRowCount).Select(x => new StrategyDto()
                                {
                                    id = x.id,
                                    categoryid = x.categoryid,
                                    creatoruserid = x.creatoruserid,
                                    isvisible = x.isvisible,
                                    courseleveltypeId = x.courseleveltypeId,
                                    title = x.title,
                                    excerpt = x.excerpt,
                                    authorid = x.authorid,
                                    authorname = x.authorname,
                                    createdatetime = x.createdatetime,
                                    changestatusdate = x.changestatusdate,
                                    coursestatusid = x.coursestatusid,
                                    tags = x.tags,
                                    privatenotes = x.privatenotes
                                }).ToListAsync();

                foreach (StrategyDto data in datas)
                {
                    data.mainlessoncontentlist = await _Context.StrategyMainLessonContents.Where(x => x.strategyid == data.id).Select(x => new StrategyMainLessonContentDto()
                    {
                        id = x.id,
                        strategyid = x.strategyid,
                        strategycontenttypeid = x.strategycontenttypeid,
                        descritption = x.descritption,
                        descritptionfilename = x.descritptionfilename,
                        descritptionfilepath = string.Empty,
                        descritptionfileurl = x.descritptionfileurl,
                        descritptionfilecontenttype = x.descritptionfilecontenttype,
                        imagefilename = x.imagefilename,
                        imagefilepath = string.Empty,
                        imagefileurl = x.imagefileurl,
                        imagefilecontenttype = x.imagefilecontenttype,
                        galleryvideofilename = x.galleryvideofilename,
                        galleryvideofilepath = string.Empty,
                        galleryvideofileurl = x.galleryvideofileurl,
                        galleryvideofilecontenttype = x.galleryvideofilecontenttype,
                        youtubevideo = x.youtubevideo,
                        videofromanyothersource = x.videofromanyothersource,
                        pdftitle = x.pdftitle,
                        pdfshortcodeid = x.pdfshortcodeid,
                        pdfauther = x.pdfauther,
                        pdfshortdescription = x.pdfshortdescription,
                        tabletitle = x.tabletitle,
                        tableshortcodeid = x.tableshortcodeid,
                        widgetscript = x.widgetscript,
                        audiobookfilename = x.audiobookfilename,
                        audiobookfilepath = string.Empty,
                        audiobookfileurl = x.audiobookfileurl,
                        audiobookfilecontenttype = x.audiobookfilecontenttype,
                        galleryimagefilename = x.galleryimagefilename,
                        galleryimagefilepath = string.Empty,
                        galleryimagefileurl = x.galleryimagefileurl,
                        galleryimagefilecontenttype = x.galleryimagefilecontenttype
                    }).ToListAsync();


                }

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = datas, Meta = new { totaldata = datas.Count, pagecount = pagecount } };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> DeleteStrategyItem(StrategyFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                Strategy data;
                IQueryable<Strategy> query = _Context.Strategys;

                if (model.id != null)
                    query = query.Where(x => x.id == model.id);

                if (model.categoryid != null)
                    query = query.Where(x => x.categoryid == model.categoryid);

                data = await query.FirstOrDefaultAsync();

                _Context.Strategys.Remove(data);


                List<StrategyMainLessonContent> _StrategyFlexibleBlocks = await _Context.StrategyMainLessonContents.Where(x => x.strategyid == data.id).ToListAsync();
                _Context.StrategyMainLessonContents.RemoveRange(_StrategyFlexibleBlocks);

                await _Context.SaveChangesAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = model, Meta = null };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> UpdateStrategyItem(StrategyDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.id == null)
                    return new SystemMessageModel() { MessageCode = -102, MessageDescription = "Id is Wrong" };

                Strategy data = await _Context.Strategys.FindAsync(model.id);
                if (data == null)
                    return new SystemMessageModel() { MessageCode = -103, MessageDescription = "data not find" };

                if (data.categoryid != model.categoryid && await _Context.Strategys.AnyAsync(x => x.categoryid == model.categoryid))
                    return new SystemMessageModel() { MessageCode = -104, MessageDescription = "this category is exist" };

                data.categoryid = (long)model.categoryid;
                data.isvisible = model.isvisible ?? true;
                data.categoryid = model.categoryid;
                data.title = model.title;
                data.tags = model.tags;
                data.authorname = model.authorname;
                data.excerpt = model.excerpt;
                data.authorid = model.authorid;
                data.isvisible = model.isvisible;
                data.courseleveltypeId = model.courseleveltypeId;
                data.coursestatusid = model.coursestatusid;
                data.creatoruserid = model.creatoruserid;
                data.createdatetime = model.createdatetime;
                data.changestatusdate = model.changestatusdate;
                data.privatenotes = model.privatenotes;

                _Context.Strategys.Update(data);

                if (model.mainlessoncontentlist != null && model.mainlessoncontentlist.Count > 0)
                {
                    List<StrategyMainLessonContent> mainlessoncontents = new List<StrategyMainLessonContent>();
                    foreach (StrategyMainLessonContentDto mainlessoncontent in model.mainlessoncontentlist)
                    {
                        StrategyMainLessonContent item = new StrategyMainLessonContent()
                        {
                            id = Guid.NewGuid(),
                            strategyid = data.id,
                            strategycontenttypeid = mainlessoncontent.strategycontenttypeid,
                            descritption = mainlessoncontent.descritption,
                            descritptionfilename = mainlessoncontent.descritptionfilename,
                            descritptionfilepath = mainlessoncontent.descritptionfilepath,
                            descritptionfileurl = mainlessoncontent.descritptionfileurl,
                            descritptionfilecontenttype = mainlessoncontent.descritptionfilecontenttype,
                            imagefilename = mainlessoncontent.imagefilename,
                            imagefilepath = mainlessoncontent.imagefilepath,
                            imagefileurl = mainlessoncontent.imagefileurl,
                            imagefilecontenttype = mainlessoncontent.imagefilecontenttype,
                            galleryvideofilename = mainlessoncontent.galleryvideofilename,
                            galleryvideofileurl = mainlessoncontent.galleryvideofileurl,
                            galleryvideofilecontenttype = mainlessoncontent.galleryvideofilecontenttype,
                            youtubevideo = mainlessoncontent.youtubevideo,
                            videofromanyothersource = mainlessoncontent.videofromanyothersource,
                            pdftitle = mainlessoncontent.pdftitle,
                            pdfshortcodeid = mainlessoncontent.pdfshortcodeid,
                            pdfauther = mainlessoncontent.pdfauther,
                            pdfshortdescription = mainlessoncontent.pdfshortdescription,
                            tabletitle = mainlessoncontent.tabletitle,
                            tableshortcodeid = mainlessoncontent.tableshortcodeid,
                            widgetscript = mainlessoncontent.widgetscript,
                            audiobookfilename = mainlessoncontent.audiobookfilename,
                            audiobookfilepath = mainlessoncontent.audiobookfilepath,
                            audiobookfileurl = mainlessoncontent.audiobookfileurl,
                            audiobookfilecontenttype = mainlessoncontent.audiobookfilecontenttype,
                            galleryimagefilename = mainlessoncontent.galleryimagefilename,
                            galleryimagefilepath = mainlessoncontent.galleryimagefilepath,
                            galleryimagefileurl = mainlessoncontent.galleryimagefileurl,
                            galleryimagefilecontenttype = mainlessoncontent.galleryimagefilecontenttype
                        };


                        mainlessoncontents.Add(item);
                    }
                    await _Context.StrategyMainLessonContents.AddRangeAsync(mainlessoncontents);
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

        public async Task<SystemMessageModel> GetStrategyContentTypes(UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                List<StrategyContentTypeDto> datas = await _Context.StrategyContentTypes.Select(x => new StrategyContentTypeDto()
                {
                    id = x.id,
                    name = x.name
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

        public async Task<SystemMessageModel> SaveFile(byte[] filecontent, Guid strategyid, long userid, string FileName, string sitePath, string hosturl)
        {
            string filegroupname = "marketpulsefile";
            try
            {
                if (filecontent != null)
                {
                    string _filePath = sitePath + "\\" + filegroupname + "\\" + strategyid.ToString().Replace("-", "") + "\\";
                    if (!Directory.Exists(_filePath))
                    {
                        DirectoryInfo directory = new DirectoryInfo(_filePath);
                        DirectorySecurity security = directory.GetAccessControl();

                        security.AddAccessRule(new FileSystemAccessRule(@"Everyone",
                                                FileSystemRights.Modify,
                                                AccessControlType.Allow));

                        directory.SetAccessControl(security);
                        Directory.CreateDirectory(_filePath);
                    }
                    

                    _filePath += FileName;
                    string fileurl = hosturl + "/" + filegroupname + "/" + strategyid.ToString().Replace("-", "") + "/" + FileName;

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
