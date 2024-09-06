using Base.Common.Enums;
using DataLayers;
using DocumentFormat.OpenXml.Math;
using Entities.DBEntities;
using Entities.Dtos;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NextTradeAPIs.Dtos;
using System.Diagnostics;

namespace NextTradeAPIs.Services
{
    public class ForumsServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;
        public ForumsServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
        }

        public async Task<SystemMessageModel> CreateForumMessage(ForumMessageFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (userlogin.UserTypeId == (long)UserTypes.Student && model.communitygroupid == null)
                    return new SystemMessageModel() { MessageCode = -220, MessageDescription = "you not allowed to create message", MessageData = model };

                ForumMessage data = new ForumMessage()
                {
                    Id = Guid.NewGuid(),
                    parentId = model.parentId,
                    //categoryid = (long)model.categoryid,
                    creatoruserid = userlogin.userid,
                    registerdatetime = DateTime.Now,
                    messagebody = model.messagebody,
                    communitygroupid = model.communitygroupid,
                    issignaltemplate  = model.issignaltemplate,
                    title = model.title
                };

                _Context.ForumMessages.Add(data);
                await _Context.SaveChangesAsync();

                model.Id = data.Id;

                foreach (long catid in (List<long>)model.categoryids)
                {
                    _Context.ForumMessageCategorys.Add(new ForumMessageCategory() { Id = Guid.NewGuid(), categoryid = catid, forummessageId = data.Id });
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

        public async Task<SystemMessageModel> EditForumMessage(ForumMessageFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.Id == null)
                    return new SystemMessageModel() { MessageCode = -220, MessageDescription = "you not allowed to create message", MessageData = model };

                ForumMessage data = await _Context.ForumMessages.FindAsync(model.Id);

                if (data != null)
                {
                    data.messagebody = model.messagebody;
                    data.communitygroupid = model.communitygroupid;
                    data.title = model.title;
                    data.issignaltemplate = model.issignaltemplate;


                    _Context.ForumMessages.Update(data);
                    await _Context.SaveChangesAsync();
                }

                if (model.categoryids != null && model.categoryids.Count > 0)
                {
                    List<ForumMessageCategory> categories = await _Context.ForumMessageCategorys.Where(x => x.forummessageId == data.Id).ToListAsync();

                    _Context.ForumMessageCategorys.RemoveRange(categories);

                    foreach (long catid in (List<long>)model.categoryids)
                    {
                        _Context.ForumMessageCategorys.Add(new ForumMessageCategory() { Id = Guid.NewGuid(), categoryid = catid, forummessageId = data.Id });
                    }
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


        public async Task<SystemMessageModel> GetForumMessage(ForumMessageFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<ForumMessage> query = _Context.ForumMessages;
                if (model.Id != null)
                    query = query.Where(x => x.Id == model.Id);

                if (model.parentId != null)
                    query = query.Where(x => x.parentId == model.parentId);

                if (model.categoryid != null)
                    query = query.Where(x => x.categoryid == model.categoryid);

                if (model.categoryids != null && model.categoryids.Count() > 0)
                {
                    List<Guid> ForumMessageIds = await _Context.ForumMessageCategorys.Where(x => model.categoryids.Contains(x.categoryid)).Select(x => x.forummessageId).ToListAsync();
                    if (ForumMessageIds != null && ForumMessageIds.Count() > 0)
                        query = query.Where(x => ForumMessageIds.Contains(x.Id));
                }
                if (model.communitygroupid != null)
                    query = query.Where(x => x.communitygroupid == model.communitygroupid);

                if (model.fromregisterdatetime != null)
                    query = query.Where(x => x.registerdatetime >= model.fromregisterdatetime);

                if (model.toregisterdatetime != null)
                    query = query.Where(x => x.registerdatetime <= model.toregisterdatetime);

                if (model.creatoruserid != null)
                    query = query.Where(x => x.creatoruserid == model.creatoruserid);

                if (model.showpost != null)
                {
                    if (model.showpost == true)
                    {
                        query = query.Where(x => x.parentId == null);
                    }
                    else
                    {
                        query = query.Where(x => x.parentId != null);
                    }
                }


                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 50 : (int)model.rowcount;


                int totaldata = query.Count();
                if (totaldata <= 0) totaldata = 1;
                decimal pagecountd = ((decimal)totaldata /(decimal) PageRowCount);
                int pagecount = (totaldata / PageRowCount);
                pagecount = (pagecount <= 0) ? 1 : pagecount;
                if (Math.Floor(pagecountd) > 0)
                    pagecount++;

                List<ForumMessageDto> datas = await query.Skip((pageIndex - 1) * PageRowCount).Take(PageRowCount)
                    .Include(x => x.creatoruser)
                    .Select(x => new ForumMessageDto()
                    {
                        Id = x.Id,
                        parentId = x.parentId,
                        categoryid = x.categoryid,
                        creatoruserid = x.creatoruserid,
                        registerdatetime = x.registerdatetime,
                        messagebody = x.messagebody,
                        communitygroupid = x.communitygroupid,
                        title = x.title,
                        issignaltemplate = x.issignaltemplate??false,
                        isneedpaid = x.isneedpaid,
                        allowtoshow = (userlogin.ispaid || !x.isneedpaid) ? true : false,
                        creatorusername = x.creatoruser.Username,
                        pagecount = pagecount
                    }).ToListAsync();



                foreach (ForumMessageDto data in datas)
                {
                    data.commentcount = await _Context.ForumMessages.Where(x => x.parentId == data.Id).CountAsync();
                    data.categories = await _Context.ForumMessageCategorys.Where(x => x.forummessageId == data.Id).Include(x => x.category).Select(x => new ForumMessageCategoryDto() { Id = x.Id, forummessageId = data.Id, categoryid = x.categoryid, categoryname = x.category.name }).ToListAsync();
                    MessageAttachement attachement = await _Context.MessageAttachements.Where(x => x.ForumMessageId == data.Id).FirstOrDefaultAsync();
                    if (attachement != null)
                    {
                        data.photofileurl = attachement.photofileurl;
                        data.videofileurl = attachement.videofileurl;
                        data.audiofileurl = attachement.audiofileurl;
                        data.photofilecontenttype = attachement.photofilecontenttype;
                        data.videofilecontenttype = attachement.videofilecontenttype;
                        data.audiofilecontenttype = attachement.audiofilecontenttype;
                    }
                    //try
                    //{
                    //    data.reactions = await _Context.ForumMessageReactions
                    //                                         .Where(x => x.forummessageId == model.Id)
                    //                                         .GroupBy(x => x.reactiontypeId)
                    //                                         .Select(x => new ForumMessageReactionModel() { reactiontypeId = x.Key, reactioncount = x.Count() })
                    //                                         .ToListAsync();
                    //}
                    //catch { }
                }

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = datas, Meta = new {pagecount  = pagecount } };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> GetTopLatestForumMessage(ForumMessageFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {

                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 10 : (int)model.rowcount;

                int pagecount = 1;

                List<ForumMessageDto> datas = await _Context.ForumMessages.Where(x=>x.isneedpaid == true&& x.parentId == null).OrderByDescending(x=>x.registerdatetime).Take(PageRowCount)
                    .Include(x => x.creatoruser)
                    .Select(x => new ForumMessageDto()
                    {
                        Id = x.Id,
                        parentId = x.parentId,
                        categoryid = x.categoryid,
                        creatoruserid = x.creatoruserid,
                        registerdatetime = x.registerdatetime,
                        messagebody = x.messagebody,
                        communitygroupid = x.communitygroupid,
                        title = x.title,
                        issignaltemplate = x.issignaltemplate,
                        isneedpaid = x.isneedpaid,
                        allowtoshow = (x.isneedpaid) ? false : true,
                        creatorusername = x.creatoruser.Username,
                        pagecount = pagecount
                    }).ToListAsync();



                foreach (ForumMessageDto data in datas)
                {
                    data.categories = await _Context.ForumMessageCategorys.Where(x => x.forummessageId == data.Id).Include(x => x.category).Select(x => new ForumMessageCategoryDto() { Id = x.Id, forummessageId = data.Id, categoryid = x.categoryid, categoryname = x.category.name }).ToListAsync();
                }

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

        public async Task<SystemMessageModel> DeleteForumMessage(ForumMessageFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                ForumMessage forummessage = await _Context.ForumMessages.FindAsync(model.Id);
                if (forummessage != null)
                {
                    _Context.ForumMessages.Remove(forummessage);

                    List<ForumMessage> listdata = await _Context.ForumMessages.Where(x => x.parentId == forummessage.Id).ToListAsync();
                    _Context.ForumMessages.RemoveRange(listdata);

                    List<ForumMessageCategory> categories = await _Context.ForumMessageCategorys.Where(x => x.forummessageId == forummessage.Id).ToListAsync();
                    _Context.ForumMessageCategorys.RemoveRange(categories);

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

        public async Task<SystemMessageModel> SaveForumMessageImage(MessageAttachement model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                MessageAttachement data = await _Context.MessageAttachements.Where(x => x.ForumMessageId == model.ForumMessageId).FirstOrDefaultAsync();

                if (data == null)
                {
                    data = new MessageAttachement() { Id = model.Id, ForumMessageId = model.ForumMessageId};
                    if (!string.IsNullOrEmpty(model.photofileurl))
                    {
                        data.photofileurl = model.photofileurl;
                        data.photofilecontenttype = model.photofilecontenttype;
                    }
                    if (!string.IsNullOrEmpty(model.videofileurl))
                    {
                        data.videofileurl = model.videofileurl;
                        data.videofilecontenttype = model.videofilecontenttype;
                    }
                    if (!string.IsNullOrEmpty(model.audiofileurl))
                    {
                        data.audiofileurl = model.audiofileurl;
                        data.audiofilecontenttype = model.audiofilecontenttype;
                    }
                    await _Context.MessageAttachements.AddAsync(model);
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.photofileurl))
                    {
                        data.photofileurl = model.photofileurl;
                        data.photofilecontenttype = model.photofilecontenttype;
                    }
                    if (!string.IsNullOrEmpty(model.videofileurl))
                    {
                        data.videofileurl = model.videofileurl;
                        data.videofilecontenttype = model.videofilecontenttype;
                    }
                    if (!string.IsNullOrEmpty(model.audiofileurl))
                    {
                        data.audiofileurl = model.audiofileurl;
                        data.audiofilecontenttype = model.audiofilecontenttype;
                    }
                    _Context.MessageAttachements.Update(data);
                }
                await _Context.SaveChangesAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request success", MessageData = new { Id = model.Id } };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> GetForumMessageImage(Guid Id)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                MessageAttachement data = await _Context.MessageAttachements.Where(x=>x.ForumMessageId == Id).FirstOrDefaultAsync();

                if (data != null)
                {
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = data }; ;
                }
                else
                {
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = null }; 
                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, "", "", methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> CreateForumMessageReaction(ForumMessageReactionDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.forummessageId == null)
                    return new SystemMessageModel() { MessageCode = -220, MessageDescription = "you not allowed to create message", MessageData = model };

                ForumMessageReaction data = new ForumMessageReaction();

                data.Id = Guid.NewGuid();
                data.reactiontypeId = (int)model.reactiontypeId;
                data.userId = userlogin.userid;
                data.forummessageId = (Guid) model.forummessageId;

                _Context.ForumMessageReactions.Add(data);
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


        public async Task<SystemMessageModel> GetForumMessageReaction(ForumMessageReactionDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                //string stringquery = $"SELECT count(*) as reactioncount,reactiontypeId AS Id, tblReactionTypes.name AS reactiontypename FROM tblForumMessageReactions LEFT OUTER JOIN tblReactionTypes ON tblForumMessageReactions.reactiontypeId  = tblReactionTypes.Id  WHERE  tblForumMessageReactions.forummessageId = '{model.forummessageId}' group by tblForumMessageReactions.reactiontypeId,tblReactionTypes.name";
                //List<ForumMessageReactionModel> datas = await _Context.Database.SqlQueryRaw<ForumMessageReactionModel>(stringquery).ToListAsync();

                List<ForumMessageReactionModel> datas = await _Context.ForumMessageReactions
                                                         .Where(x=>x.forummessageId == model.forummessageId)
                                                         .GroupBy(x=>x.reactiontypeId)
                                                         .Select(x=> new ForumMessageReactionModel() { reactiontypeId = x.Key, reactioncount = x.Count() })
                                                         .ToListAsync();


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

        public async Task<SystemMessageModel> SaveFile(byte[] filecontent, long userid, string communitygroupId, string FileName, string sitePath)
        {
            string filegroupname = "ComminityGroupForumMessageFiles";
            try
            {
                if (filecontent != null)
                {
                    string _filePath = sitePath + "\\" + filegroupname + "\\" + communitygroupId+ "\\"  ;
                    if (!Directory.Exists(_filePath))
                        Directory.CreateDirectory(_filePath);

                    _filePath +=  FileName;

                    string fileurl = "/" + filegroupname + "/" + communitygroupId + "/" + FileName;
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

        public async Task<SystemMessageModel> DeleteFile(long userid, string communitygroupId, string sitePath)
        {
            string filegroupname = "ComminityGroupForumMessageFiles";
            try
            {
                string _filePath = sitePath + "\\" + filegroupname + "\\" + communitygroupId;

                foreach (string dfile in Directory.GetFiles(_filePath))
                    File.Delete(_filePath);
                return new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = null };
            }
            catch (Exception ex) { return new SystemMessageModel() { MessageCode = -501, MessageDescription = "File saving Error", MessageData = ex.Message }; }
        }
    }
}
