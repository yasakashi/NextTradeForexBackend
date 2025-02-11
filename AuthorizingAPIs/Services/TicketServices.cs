using Base.Common.Enums;
using DataLayers;
using Entities.DBEntities;
using Entities.Dtos;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NextTradeAPIs.Dtos;
using System.Diagnostics;
using System.Net.Mail;

namespace NextTradeAPIs.Services
{
    public class TicketServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;
        public TicketServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
        }
        public async Task<SystemMessageModel> GetTicket(TicketDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<Ticket> query = _Context.Tickets;

                if (model.priorityId != null)
                    query = query.Where(x => x.priorityId == model.priorityId);

                if (model.creatoruserId != null)
                    query = query.Where(x => x.creatoruserId == model.creatoruserId);

                if (model.isanswerd != null)
                {
                    if ((bool)model.isanswerd)
                        query = query.Where(x => x.responsedatetime != null);
                    else
                        query = query.Where(x => x.responsedatetime == null);
                }

                if (model.Id != null)
                    query = query.Where(x => x.Id == model.Id);

                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 50 : (int)model.rowcount;


                List<TicketDto> data = await query.Skip((pageIndex - 1) * PageRowCount).Take(PageRowCount)
                                    .Include(x => x.priority)
                                    .Include(x => x.creatoruser)
                                    .Include(x => x.responseuser)
                                    .Select(x => new TicketDto()
                                    {
                                        Id = x.Id,
                                        creatoruserId = x.creatoruserId,
                                        isanswerd = (x.responseuserid != null) ? true : false,
                                        registerdatetime = x.registerdatetime,
                                        responsedatetime = x.responsedatetime,
                                        priorityId = x.priorityId,
                                        creatorusername = x.creatoruser.Username,
                                        responseusername = (x.responseuser != null) ? x.responseuser.Username : "",
                                        priorityname = x.priority.name,
                                        responseuserid = x.responseuserid,
                                        responsedescription = x.responsedescription ?? "",
                                        subject = x.subject,
                                        textbody = x.textbody,
                                        attachmentcontexttype = x.attachmentcontexttype,
                                        fileattachmentname = x.fileattachmentname,
                                        attachmentfileurl = x.attachmentfileurl,
                                        attachmentfilepath = x.attachmentfilepath,
                                        pageindex = pageIndex,
                                        rowcount = PageRowCount
                                    }).ToListAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = data, Meta = new { pageindex = pageIndex, rowcount = PageRowCount } };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> CreateTicket(TicketDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                Ticket data = new Ticket()
                {
                    Id = Guid.NewGuid(),
                    registerdatetime = DateTime.Now,
                    creatoruserId = userlogin.userid,
                    priorityId = (long)model.priorityId,
                    responsedatetime = null,
                    responseuserid = null,
                    subject = model.subject,
                    textbody = model.textbody,
                    responsedescription = null,
                    fileattachment = null
                };

                await _Context.Tickets.AddAsync(data);
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

        public async Task<SystemMessageModel> RemoveTicket(TicketDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.Id == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 601) * -1), MessageDescription = "Data is not complete", MessageData = model };

                Ticket data = await _Context.Tickets.FindAsync(model.Id);

                if (data == null)
                {
                    message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "ID is worng", MessageData = model };
                }
                else
                {
                    _Context.Tickets.RemoveRange(data);

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

        public async Task<SystemMessageModel> UploadTicketFile(AttachmentFileDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                Ticket data = await _Context.Tickets.FindAsync(new Guid(model.Id.ToString()));
                data.fileattachment = model.attachment;
                data.attachmentcontexttype = model.fileextention;


                _Context.Tickets.Update(data);
                await _Context.SaveChangesAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request success", MessageData = new { Id = data.Id } };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<Ticket> GetTicketFile(Guid Id)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                Ticket data = await _Context.Tickets.FindAsync(Id);

                return data;
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, "", "", methodpath, LogTypes.SystemError);
                return null;
            }
        }

        public async Task<SystemMessageModel> AnswerTicket(TicketAnswerDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                Ticket data = await _Context.Tickets.FindAsync(model.Id);

                data.responsedatetime = DateTime.Now;
                data.responseuserid = userlogin.userid;
                data.responsedescription = model.responsedescription;
                data.attachmentcontexttype = model.attachmentcontexttype;
                data.attachmentfilepath = model.attachmentfilepath;
                data.attachmentfileurl = model.attachmentfileurl;
                data.fileattachmentname = model.fileattachmentname;

                _Context.Tickets.Update(data);
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
        public async Task<SystemMessageModel> AnswerTicket(TicketDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                Ticket data = await _Context.Tickets.FindAsync(model.Id);

                data.responsedatetime = DateTime.Now;
                data.responseuserid = userlogin.userid;
                data.responsedescription = model.responsedescription;
                if (!string.IsNullOrEmpty(model.attachmentcontexttype))
                    data.attachmentcontexttype = model.attachmentcontexttype;
                if (!string.IsNullOrEmpty(model.attachmentfilepath))
                    data.attachmentfilepath = model.attachmentfilepath;
                if (!string.IsNullOrEmpty(model.attachmentfileurl))
                    data.attachmentfileurl = model.attachmentfileurl;
                if (!string.IsNullOrEmpty(model.fileattachmentname))
                    data.fileattachmentname = model.fileattachmentname;

                _Context.Tickets.Update(data);
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

        public async Task<SystemMessageModel> GetTicketReport(TicketDto filter, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            List<TicketReportDto> datas = null;
            long SerrvieCode = 120000;

            string username = string.Empty;
            try
            {


                string querystrnig = $"EXECUTE dbo.spGetAllTicketInfo ";
                datas = await _Context.Database.SqlQueryRaw<TicketReportDto>(querystrnig).ToListAsync();

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

        public async Task<SystemMessageModel> SaveFile(byte[] filecontent, Guid id, long userid, string FileName, string sitePath, string hosturl)
        {
            string filegroupname = "ticket";
            try
            {
                if (filecontent != null)
                {
                    string _filePath = sitePath + "\\" + filegroupname + "\\" + id.ToString().Replace("-", "") + "\\";
                    if (!Directory.Exists(_filePath))
                        Directory.CreateDirectory(_filePath);

                    _filePath += FileName;
                    string fileurl = hosturl + "/" + filegroupname + "/" + id.ToString().Replace("-", "") + "/" + FileName;

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
