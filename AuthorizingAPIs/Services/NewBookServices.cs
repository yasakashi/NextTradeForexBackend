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
    public class NewBookServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;
        public NewBookServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
        }


        public async Task<SystemMessageModel> GetNewBooks(NewBookSearchDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<Book> query = _Context.NewBooks;

                if (model.title != null)
                    query = query.Where(x => x.title.Contains(model.title));

                if (model.id != null)
                    query = query.Where(x => x.id == model.id);

                if (model.categoryid != null)
                {
                    List<Guid> PIds = await _Context.NewBookCategories.Where(x => x.categoryid == model.categoryid).Select(x => x.id).ToListAsync();
                    query = query.Where(x => PIds.Contains(x.id));
                }

                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 50 : (int)model.rowcount;


                List<NewBookDto> data = await query.Skip((pageIndex - 1) * PageRowCount).Take(PageRowCount)
                                                  .Select(x => new NewBookDto()
                                                  {
                                                      id = (Guid)x.id,
                                                      title = x.title,
                                                      author = x.author,
                                                      description = x.description,
                                                      bgColor = x.bgColor,
                                                      flipDuration = x.flipDuration,
                                                      containerHeight = x.containerHeight,
                                                      autoplayDuration = x.autoplayDuration,
                                                      forcePageFit = x.forcePageFit,
                                                      autoEnableOutline = x.autoEnableOutline,
                                                      autoEnableThumbnail = x.autoEnableThumbnail,
                                                      overwritePdfOutline = x.overwritePdfOutline,
                                                      bookSourceTypeId = x.bookSourceTypeId,
                                                      displayMode = x.displayMode,
                                                      hardPageId = x.hardPageId,
                                                      pdfPageRenderSize = x.pdfPageRenderSize,
                                                      autoEnableSound = x.autoEnableSound,
                                                      enableDownload = x.enableDownload,
                                                      pageMode = x.pageMode,
                                                      singlePageMode = x.singlePageMode,
                                                      controlsPosition = x.controlsPosition,
                                                      direction = x.direction,
                                                      enableAutoplay = x.enableAutoplay,
                                                      enableAutoplayAutomatically = x.enableAutoplayAutomatically,
                                                      pageSize = x.pageSize,
                                                      lessonCategoryLevelId = x.lessonCategoryLevelId,
                                                      featuredimagename = x.featuredimagename,
                                                      featuredimageurl = x.featuredimageurl,
                                                      featuredimagecontenttype = x.featuredimagecontenttype,
                                                      bgimagename = x.bgimagename,
                                                      bgimageurl = x.bgimageurl,
                                                      bgimagecontenttype = x.bgimagecontenttype,
                                                      pdffilename = x.pdffilename,
                                                      pdffileurl = x.pdffileurl,
                                                      pdffilecontenttype = x.pdffilecontenttype,
                                                      pdfthumbnailimagename = x.pdfthumbnailimagename,
                                                      pdfthumbnailimageurl = x.pdfthumbnailimageurl,
                                                      pdfthumbnailimagecontenttype = x.pdfthumbnailimagecontenttype

                                                  }).ToListAsync();

                foreach (NewBookDto item in data)
                {
                    item.categories = await _Context.NewBookCategories.Where(x => x.newbookid == item.id).Include(x => x.category).Select(x => new CategoryBaseDto() { Id = x.categoryid, name = x.category.name, categorytypeid = x.category.categorytypeid, parentId = x.category.parentId }).ToListAsync();
                    item.pageimagelist = await _Context.NewBookPageImages.Where(x => x.newbookid == item.id).Select(x => new NewBookPageImageDto() { id = x.id, newbookid = x.newbookid, pageimageurl = x.pageimageurl, pageimagename = x.pageimagename, pageimagecontenttype = x.pageimagecontenttype }).ToListAsync();
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

        public async Task<SystemMessageModel> GetNewBookPageImages(List<NewBookPageImage> model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                await _Context.NewBookPageImages.AddRangeAsync(model);
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


        public async Task<SystemMessageModel> AddNewNewBook(ADBookDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                Book data = new Book()
                {
                    id = (Guid)model.id,
                    title = model.title,
                    author = model.author,
                    description = model.description,
                    bgColor = model.bgColor,
                    flipDuration = model.flipDuration,
                    containerHeight = model.containerHeight,
                    autoplayDuration = model.autoplayDuration,
                    forcePageFit = model.forcePageFit,
                    autoEnableOutline = model.autoEnableOutline,
                    autoEnableThumbnail = model.autoEnableThumbnail,
                    overwritePdfOutline = model.overwritePdfOutline,
                    bookSourceTypeId = model.bookSourceTypeId,
                    displayMode = model.displayMode,
                    hardPageId = model.hardPageId,
                    pdfPageRenderSize = model.pdfPageRenderSize,
                    autoEnableSound = model.autoEnableSound,
                    enableDownload = model.enableDownload,
                    pageMode = model.pageMode,
                    singlePageMode = model.singlePageMode,
                    controlsPosition = model.controlsPosition,
                    direction = model.direction,
                    enableAutoplay = model.enableAutoplay,
                    enableAutoplayAutomatically = model.enableAutoplayAutomatically,
                    pageSize = model.pageSize,
                    lessonCategoryLevelId = model.lessonCategoryLevelId,
                    featuredimagename = model.featuredimagename,
                    featuredimagepath = model.featuredimagepath,
                    featuredimageurl = model.featuredimageurl,
                    featuredimagecontenttype = model.featuredimagecontenttype,
                    bgimagename = model.bgimagename,
                    bgimagepath = model.bgimagepath,
                    bgimageurl = model.bgimageurl,
                    bgimagecontenttype = model.bgimagecontenttype,
                    pdffilename = model.pdffilename,
                    pdffilepath = model.pdffilepath,
                    pdffileurl = model.pdffileurl,
                    pdffilecontenttype = model.pdffilecontenttype,
                    pdfthumbnailimagename = model.pdfthumbnailimagename,
                    pdfthumbnailimagepath = model.pdfthumbnailimagepath,
                    pdfthumbnailimageurl = model.pdfthumbnailimageurl,
                    pdfthumbnailimagecontenttype = model.pdfthumbnailimagecontenttype
                };

                await _Context.NewBooks.AddAsync(data);

                List<NewBookCategory> categorylist = new List<NewBookCategory>();
                foreach (long catId in model.categoryIds)
                {
                    categorylist.Add(new NewBookCategory()
                    {
                        id = Guid.NewGuid(),
                        categoryid = catId,
                        newbookid = data.id
                    });
                }
                if (categorylist != null && categorylist.Count > 0)
                    await _Context.NewBookCategories.AddRangeAsync(categorylist);

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

        public async Task<SystemMessageModel> RemoveNewBook(NewBookSearchDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.id == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -1), MessageDescription = "Data is wrong", MessageData = model };
                Book data = await _Context.NewBooks.FindAsync(model.id);

                if (data == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -2), MessageDescription = "Data is wrong", MessageData = model };

                _Context.NewBooks.Remove(data);

                await DeleteFile(data.pdffilepath);
                await DeleteFile(data.featuredimagepath);
                await DeleteFile(data.pdfthumbnailimagepath);
                await DeleteFile(data.bgimagepath);

                List<NewBookCategory> categoeris = await _Context.NewBookCategories.Where(x => x.newbookid == data.id).ToListAsync();
                _Context.NewBookCategories.RemoveRange(categoeris);

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

        public async Task<SystemMessageModel> EditNewBook(ADBookDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.id == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -1), MessageDescription = "Data is wrong", MessageData = model };
                Book data = await _Context.NewBooks.FindAsync(model.id);

                if (data == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 101) * -2), MessageDescription = "Data is wrong", MessageData = model };


                data.title = model.title;
                data.author = model.author;
                data.description = model.description;
                data.bgColor = model.bgColor;
                data.flipDuration = model.flipDuration;
                data.containerHeight = model.containerHeight;
                data.autoplayDuration = model.autoplayDuration;
                data.forcePageFit = model.forcePageFit;
                data.autoEnableOutline = model.autoEnableOutline;
                data.autoEnableThumbnail = model.autoEnableThumbnail;
                data.overwritePdfOutline = model.overwritePdfOutline;
                data.bookSourceTypeId = model.bookSourceTypeId;
                data.displayMode = model.displayMode;
                data.hardPageId = model.hardPageId;
                data.pdfPageRenderSize = model.pdfPageRenderSize;
                data.autoEnableSound = model.autoEnableSound;
                data.enableDownload = model.enableDownload;
                data.pageMode = model.pageMode;
                data.singlePageMode = model.singlePageMode;
                data.controlsPosition = model.controlsPosition;
                data.direction = model.direction;
                data.enableAutoplay = model.enableAutoplay;
                data.enableAutoplayAutomatically = model.enableAutoplayAutomatically;
                data.pageSize = model.pageSize;
                data.lessonCategoryLevelId = model.lessonCategoryLevelId;
                if (!string.IsNullOrEmpty(model.featuredimagename))
                    data.featuredimagename = model.featuredimagename;
                if (!string.IsNullOrEmpty(model.featuredimagepath))
                    data.featuredimagepath = model.featuredimagepath;
                if (!string.IsNullOrEmpty(model.featuredimageurl))
                    data.featuredimageurl = model.featuredimageurl;
                if (!string.IsNullOrEmpty(model.featuredimagecontenttype))
                    data.featuredimagecontenttype = model.featuredimagecontenttype;

                if (!string.IsNullOrEmpty(model.bgimagename))
                    data.bgimagename = model.bgimagename;
                if (!string.IsNullOrEmpty(model.bgimagepath))
                    data.bgimagepath = model.bgimagepath;
                if (!string.IsNullOrEmpty(model.bgimageurl))
                    data.bgimageurl = model.bgimageurl;
                if (!string.IsNullOrEmpty(model.bgimagecontenttype))
                    data.bgimagecontenttype = model.bgimagecontenttype;

                if (!string.IsNullOrEmpty(model.pdffilename))
                    data.pdffilename = model.pdffilename;
                if (!string.IsNullOrEmpty(model.pdffilepath))
                    data.pdffilepath = model.pdffilepath;
                if (!string.IsNullOrEmpty(model.pdffileurl))
                    data.pdffileurl = model.pdffileurl;
                if (!string.IsNullOrEmpty(model.pdffilecontenttype))
                    data.pdffilecontenttype = model.pdffilecontenttype;

                if (!string.IsNullOrEmpty(model.pdfthumbnailimagename))
                    data.pdfthumbnailimagename = model.pdfthumbnailimagename;
                if (!string.IsNullOrEmpty(model.pdfthumbnailimagepath))
                    data.pdfthumbnailimagepath = model.pdfthumbnailimagepath;
                if (!string.IsNullOrEmpty(model.pdfthumbnailimageurl))
                    data.pdfthumbnailimageurl = model.pdfthumbnailimageurl;
                if (!string.IsNullOrEmpty(model.pdfthumbnailimagecontenttype))
                    data.pdfthumbnailimagecontenttype = model.pdfthumbnailimagecontenttype;
                _Context.NewBooks.Update(data);

                if (model.categoryIds != null && model.categoryIds.Count > 0)
                {
                    List<NewBookCategory> categoeris = await _Context.NewBookCategories.Where(x => x.newbookid == data.id).ToListAsync();
                    _Context.NewBookCategories.RemoveRange(categoeris);

                    List<NewBookCategory> categorylist = new List<NewBookCategory>();
                    foreach (long catId in model.categoryIds)
                    {
                        categorylist.Add(new NewBookCategory()
                        {
                            id = Guid.NewGuid(),
                            categoryid = catId,
                            newbookid = data.id
                        });
                    }
                    if (categorylist != null && categorylist.Count > 0)
                        await _Context.NewBookCategories.AddRangeAsync(categorylist);
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

        public async Task<SystemMessageModel> SaveFile(byte[] filecontent, ADBookDto model, long userid, string FileName, string sitePath, string hosturl)
        {
            string filegroupname = "NewBookfile";
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
