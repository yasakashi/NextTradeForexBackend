using Base.Common.Enums;
using DataLayers;
using Entities.DBEntities;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NextTradeAPIs.Dtos;
using System;
using System.Diagnostics;

namespace NextTradeAPIs.Services
{
    public class CommunityGroupGalleryServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;
        public CommunityGroupGalleryServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
        }

        public async Task<SystemMessageModel> CreateCommunityGallery(CommunityGroupGalleryDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                CommunityGroupGallery data = new CommunityGroupGallery()
                {
                    Id = Guid.NewGuid(),
                    communitygroupId = (Guid)model.communitygroupId,
                    userId = userlogin.userid,
                    description = model.description,
                    galleryname = model.galleryname,
                    gallerytypeId = model.gallerytypeId,
                    galleryaccesslevelId = model.galleryaccesslevelId,
                };

                _Context.CommunityGroupGalleries.Add(data);
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

        public async Task<SystemMessageModel> EditCommunityGallery(CommunityGroupGalleryDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                CommunityGroupGallery data = await _Context.CommunityGroupGalleries.FindAsync((Guid)model.Id);

                if (data != null)
                {
                    data.galleryname = model.galleryname;
                    data.description = model.description;
                    data.gallerytypeId = model.gallerytypeId;
                    data.galleryaccesslevelId = model.galleryaccesslevelId;

                    _Context.CommunityGroupGalleries.Update(data);
                    await _Context.SaveChangesAsync();

                }

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

        public async Task<SystemMessageModel> DeleteCommunityGallery(CommunityGroupGalleryDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                CommunityGroupGallery data = await _Context.CommunityGroupGalleries.FindAsync((Guid)model.Id);

                if (data != null)
                {
                    List<CommunityGroupGalleryFile> datafiles = await _Context.CommunityGroupGalleryFiles.Where(x => x.galleryId == (Guid)model.Id).ToListAsync();

                    _Context.CommunityGroupGalleryFiles.RemoveRange(datafiles);
                    _Context.CommunityGroupGalleries.Remove(data);

                    await _Context.SaveChangesAsync();


                    await DeleteFiles(data.userId, data.Id.ToString().Replace("-", ""));
                }

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

        public async Task<SystemMessageModel> GetCommunityGallery(CommunityGroupGalleryDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<CommunityGroupGallery> query = _Context.CommunityGroupGalleries;

                if (model.Id != null)
                    query = query.Where(x => x.Id == model.Id);

                if (model.communitygroupId != null)
                    query = query.Where(x => x.communitygroupId == model.communitygroupId);

                if (model.userId != null)
                    query = query.Where(x => x.userId == model.userId);

                if (model.gallerytypeId != null)
                    query = query.Where(x => x.gallerytypeId == model.gallerytypeId);

                if (model.galleryaccesslevelId != null)
                    query = query.Where(x => x.galleryaccesslevelId == model.galleryaccesslevelId);

                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 50 : (int)model.rowcount;


                if (model.sortitem != null)
                {
                    foreach (var item in model.sortitem)
                    {
                        if (item.ascending == null || (bool)item.ascending)
                        {
                            switch (item.fieldname.ToLower())
                            {
                                case "galleryname":
                                    query = query.OrderBy(x => x.galleryname);
                                    break;
                            };
                        }
                        else if (!(bool)item.ascending)
                        {
                            switch (item.fieldname.ToLower())
                            {
                                case "galleryname":
                                    query = query.OrderByDescending(x => x.galleryname);
                                    break;
                            };
                        }
                    }
                }
                int totaldata = query.Count();
                if (totaldata <= 0) totaldata = 1;
                decimal pagecountd = ((decimal)totaldata / (decimal)PageRowCount);
                int pagecount = (totaldata / PageRowCount);
                pagecount = (pagecount <= 0) ? 1 : pagecount;
                if (Math.Floor(pagecountd) > 0)
                    pagecount++;

                List<CommunityGroupGalleryDto> data = await query
                                    .Skip((pageIndex - 1) * PageRowCount)
                                    .Take(PageRowCount)
                                    .Include(x => x.communitygroup)
                                    .Include(x => x.user)
                                    .Include(x=>x.galleryaccesslevel)
                                    .Include (x=>x.gallerytype)
                                    .Select(x => new CommunityGroupGalleryDto()
                                    {
                                        Id = x.Id,
                                        userId = x.userId,
                                        communitygroupId = x.communitygroupId,
                                        galleryname = x.galleryname,
                                        communitygroupname = x.communitygroup.title,
                                        description = x.description,
                                        userfullname = x.user.Fname + " " + x.user.Lname,
                                        username = x.user.Username,
                                        galleryaccesslevelId = x.galleryaccesslevelId,
                                        gallerytypeId = x.gallerytypeId,
                                        galleryaccesslevelname = (x.galleryaccesslevel != null) ? x.galleryaccesslevel.name : "",
                                        gallerytypename = (x.gallerytype != null) ? x.gallerytype.name : "",
                                        pagecount = pagecount
                                    }).ToListAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = data, Meta = new { pagecount = pagecount } };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> GetCommunityGalleryFileList(CommunityGroupGalleryDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<CommunityGroupGalleryFile> query = _Context.CommunityGroupGalleryFiles;

                if (model.Id != null)
                    query = query.Where(x => x.Id == model.Id);

                if (model.communitygroupId != null)
                    query = query.Where(x => x.galleryId == model.Id);

                if (model.userId != null)
                    query = query.Where(x => x.userId == model.userId);

                if (!string.IsNullOrEmpty( model.username ))
                {
                    User user = await _Context.Users.Where(x => x.Username == model.username).SingleOrDefaultAsync();
                    if(user != null)
                    query = query.Where(x => x.userId == user.UserId);
                }

                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 50 : (int)model.rowcount;


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
                                case "title":
                                    query = query.OrderBy(x => x.title);
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
                                case "title":
                                    query = query.OrderByDescending(x => x.title);
                                    break;
                            };
                        }
                    }
                }
                int totaldata = query.Count();
                if (totaldata <= 0) totaldata = 1;
                decimal pagecountd = ((decimal)totaldata / (decimal)PageRowCount);
                int pagecount = (totaldata / PageRowCount);
                pagecount = (pagecount <= 0) ? 1 : pagecount;
                if (Math.Floor(pagecountd) > 0)
                    pagecount++;

                List<CommunityGroupGalleryFileDto> data = await query
                                    .Skip((pageIndex - 1) * PageRowCount)
                                    .Take(PageRowCount)
                                    .Include(x => x.gallery)
                                    .Include(x => x.user)
                                    .Select(x => new CommunityGroupGalleryFileDto()
                                    {
                                        Id = x.Id,
                                        contenttype = x.contenttype,
                                        userId = x.userId,
                                        createdatetime = x.createdatetime,
                                        galleryId = x.galleryId,
                                        fileextention = x.fileextention,
                                        title = x.title,
                                        galleryname = x.gallery.galleryname,
                                        filename = x.filename,
                                        description = x.description,
                                        fileurl = x.fileurl,
                                        userfulname = x.user.Fname + " " + x.user.Lname,
                                        username = x.user.Username,
                                        pagecount = pagecount
                                    }).ToListAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = data, Meta = new { pagecount = pagecount } };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> SaveCommunityGalleryFile(CommunityGroupGalleryFileDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                CommunityGroupGalleryFile data = new CommunityGroupGalleryFile()
                {
                    Id = (Guid)model.Id,
                    galleryId = (Guid)model.galleryId,
                    userId = userlogin.userid,
                    description = model.description,
                    title = model.title,
                    fileurl = model.fileurl,
                    fileextention = model.fileextention,
                    contenttype = model.contenttype,
                    filename = model.filename
                };

                _Context.CommunityGroupGalleryFiles.Add(data);
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

        public async Task<SystemMessageModel> DeleteCommunityGalleryFile(CommunityGroupGalleryFileDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                CommunityGroupGalleryFile data = await _Context.CommunityGroupGalleryFiles.FindAsync((Guid)model.Id);

                if (data != null)
                {
                    _Context.CommunityGroupGalleryFiles.Remove(data);
                    await _Context.SaveChangesAsync();

                }

                await DeleteFile(data.filename, data.galleryId.ToString().Replace("-",""));

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

        public async Task<SystemMessageModel> GetCommunityGalleryFileURL(Guid Id, string sitePath)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {

                CommunityGroupGallery data = await _Context.CommunityGroupGalleries.FindAsync(Id);
                if (data != null)
                    return new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = data };
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

        public async Task<SystemMessageModel> SaveFile(byte[] filecontent, long userid, string galleryId, string FileName, string sitePath)
        {
            string filegroupname = "gallery";
            try
            {
                if (filecontent != null)
                {
                    string _filePath = sitePath + "\\"+ filegroupname + "\\" + galleryId + "\\" + FileName;
                    string fileurl = AppDomain.CurrentDomain.BaseDirectory + "/"+ filegroupname + "/" + galleryId + "/" + FileName;
                    Uri uri = new Uri(fileurl, UriKind.Relative);

                    if (!File.Exists(_filePath))
                    {
                        File.WriteAllBytes(_filePath, filecontent);

                    }
                    return new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = uri.ToString() };
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
    }
}