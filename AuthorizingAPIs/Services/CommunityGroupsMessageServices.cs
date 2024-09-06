using Base.Common.Enums;
using DataLayers;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Entities.DBEntities;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NextTradeAPIs.Dtos;
using System.Diagnostics;

namespace NextTradeAPIs.Services
{
    public class CommunityGroupsMessageServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;
        public CommunityGroupsMessageServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
        }

        public async Task<SystemMessageModel> CreateCommunityGroupsMessage(CommunityGroupsMessageDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.communitygroupid != null)
                {
                    List<long> userIds = await _Context.CommunityGroupMembers.Where(x => x.communitygroupId == (Guid)model.communitygroupid).Select(x => x.userId).ToListAsync();
                    _Context.CommunityGroupsMessages.Add(new CommunityGroupsMessage()
                    {
                        Id = Guid.NewGuid(),
                        registerdatetime = DateTime.Now,
                        creatoruserid = userlogin.userid,
                        messagebody = model.messagebody,
                        messagetitle = model.messagetitle,
                        communitygroupid = (Guid)model.communitygroupid,
                        filecontenttype = model.filecontenttype,
                        fileurl = model.fileurl,
                        issignaltemplate = model.issignaltemplate
                    });

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


        public async Task<SystemMessageModel> GetCommunityGroupsMessage(CommunityGroupsMessageDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<CommunityGroupsMessage> query = _Context.CommunityGroupsMessages;
                if (model.fromdate != null)
                    query = query.Where(x => x.registerdatetime >= model.fromdate);

                if (model.todate != null)
                    query = query.Where(x => x.registerdatetime <= model.todate);

                if (model.Id != null)
                    query = query.Where(x => x.Id == model.Id);

                if (model.communitygroupid != null)
                    query = query.Where(x => x.communitygroupid == model.communitygroupid);


                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 10 : (int)model.rowcount;


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
                            };
                        }
                        else if (!(bool)item.ascending)
                        {
                            switch (item.fieldname.ToLower())
                            {
                                case "registerdatetime":
                                    query = query.OrderByDescending(x => x.registerdatetime);
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


                List<CommunityGroupsMessageDto> datas = await query
                                .Skip((pageIndex - 1) * PageRowCount)
                                .Take(PageRowCount)
                                .Include(x => x.creatoruser)
                                .Select(x => new CommunityGroupsMessageDto()
                                {
                                    Id = x.Id,
                                    messagebody = x.messagebody,
                                    messagetitle = x.messagetitle,
                                    registerdatetime = x.registerdatetime,
                                    creatoruserid = x.creatoruserid,
                                    creatorusername = x.creatoruser.Username,
                                    rowcount = PageRowCount,
                                    pageindex = pageIndex,
                                    pagecount = pagecount,
                                    communitygroupid = x.communitygroupid,
                                    filecontenttype = x.filecontenttype,
                                    fileurl = x.fileurl,
                                    issignaltemplate = x.issignaltemplate
                                }).ToListAsync();
                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = datas, Meta = new { pagecount = pagecount } };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> RemoveCommunityGroupsMessage(CommunityGroupsMessageDto model, string sitePath, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                CommunityGroupsMessage data = await _Context.CommunityGroupsMessages.FindAsync(model.Id);

                if (data != null)
                {
                    _Context.CommunityGroupsMessages.Remove(data);
                    await DeleteFile(userlogin.userid, data.communitygroupid.ToString().Replace("-", ""), sitePath);
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

        public async Task<SystemMessageModel> SaveFile(byte[] filecontent, long userid, string communitygroupId, string FileName, string sitePath)
        {
            string filegroupname = "ComminityGroupMessagesFiles";
            try
            {
                if (filecontent != null)
                {
                    string _filePath = sitePath + "\\" + filegroupname + "\\" + communitygroupId + "\\" + FileName;
                    string fileurl = AppDomain.CurrentDomain.BaseDirectory + "/" + filegroupname + "/" + communitygroupId + "/" + FileName;
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
            string filegroupname = "ComminityGroupMessagesFiles";
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
