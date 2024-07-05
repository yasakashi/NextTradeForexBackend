using Base.Common.Enums;
using DataLayers;
using Entities.DBEntities;
using Entities.Dtos;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NextTradeAPIs.Dtos;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;

namespace NextTradeAPIs.Services
{
    public class SignalChannelServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;
        public SignalChannelServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
        }


        public async Task<SystemMessageModel> GetSignalChannel(SignalChannelDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<SignalChannel> query = _Context.SignalChannels;

                if (model.isneedpaid != null)
                    query = query.Where(x => x.isneedpaid == model.isneedpaid);

                if (model.grouptypeId != null)
                    query = query.Where(x => x.grouptypeId == model.grouptypeId);

                if (model.owneruserid != null)
                    query = query.Where(x => x.owneruserid == model.owneruserid);

                if (model.Id != null)
                    query = query.Where(x => x.Id == model.Id);

                if (model.communitygroupId != null)
                    query = query.Where(x => x.communitygroupId == model.communitygroupId);

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
                                case "grouptypeId":
                                    query = query.OrderBy(x => x.grouptypeId);
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
                                case "grouptypeId":
                                    query = query.OrderByDescending(x => x.grouptypeId);
                                    break;
                            };
                        }
                    }
                }
                List<SignalChannelDto> data = await query
                                    .Skip(pageIndex - 1)
                                    .Take(PageRowCount)
                                    .Include(x => x.communitygroup)
                                    .Include(x => x.grouptype)
                                    .Select(x => new SignalChannelDto()
                                    {
                                        Id = x.Id,
                                        isneedpaid = x.isneedpaid,
                                        owneruserid = x.owneruserid,
                                        createdatetime = x.createdatetime,
                                        communitygroupId = x.communitygroupId,
                                        grouptypeId = x.grouptypeId,
                                        title = x.title,
                                        communitygroupname = x.communitygroup.title,
                                        grouptypename = x.grouptype.name
                                    }).ToListAsync();

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


        public async Task<SystemMessageModel> CreateSignalChannel(SignalChannelDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                SignalChannel data = new SignalChannel()
                {
                    Id = Guid.NewGuid(),
                    isneedpaid = (bool)model.isneedpaid,
                    owneruserid = userlogin.userid,
                    createdatetime = DateTime.Now,
                    communitygroupId = model.communitygroupId,
                    grouptypeId = (long)model.grouptypeId,
                    title = model.title
                };

                await _Context.SignalChannels.AddAsync(data);
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

        public async Task<SystemMessageModel> RemoveSignalChannel(SignalChannelDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.Id == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 601) * -1), MessageDescription = "Data is not complete", MessageData = model };

                SignalChannel data = await _Context.SignalChannels.FindAsync(model.Id);

                if (data == null)
                {
                    message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "ID is worng", MessageData = model };
                }
                else
                {
                    List<Signal> signallist = await _Context.Signals.Where(x => x.signalchannelId == model.Id).ToListAsync();

                    _Context.Signals.RemoveRange(signallist);
                    _Context.SignalChannels.Remove(data);

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

        public async Task<SystemMessageModel> EditSignalChannel(SignalChannelDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.Id == null)
                    return new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 601) * -1), MessageDescription = "Data is not complete", MessageData = model };

                SignalChannel data = await _Context.SignalChannels.FindAsync(model.Id);
                data.isneedpaid = (bool)model.isneedpaid;
                data.title = model.title;

                _Context.SignalChannels.Update(data);
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


        public async Task<SystemMessageModel> GetSignals(SignalDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<Signal> query = _Context.Signals;

                if (model.signalchannelId != null)
                    query = query.Where(x => x.signalchannelId == model.signalchannelId);

                if (model.creatoruserId != null)
                    query = query.Where(x => x.creatoruserId == model.creatoruserId);

                if (model.fromcreatedatetime != null)
                    query = query.Where(x => x.createdatetime >= model.fromcreatedatetime);

                if (model.tocreatedatetime != null)
                    query = query.Where(x => x.createdatetime >= model.tocreatedatetime);
                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 50 : (int)model.rowcount;


                List<SignalDto> data = await query.Skip(pageIndex - 1).Take(PageRowCount)
                    .Include(x => x.analysistype)
                    .Include(x => x.creatoruser)
                    .Include(x => x.entrypointtype)
                    .Include(x => x.instrumenttype)
                    .Include(x => x.marketsycle)
                    .Include(x => x.positiontype)
                    .Select(x => new SignalDto()
                    {
                        Id = x.Id,
                        analysistypeId = x.analysistypeId,
                        analysistypename = (x.analysistype != null) ? x.analysistype.name : "",
                        entrypointtypeId = x.entrypointtypeId,
                        createdatetime = x.createdatetime,
                        creatoruserId = x.creatoruserId,
                        creatorusername = (x.creatoruser != null) ? x.creatoruser.Username : "",
                        description = x.description,
                        entrypoint = x.entrypoint,
                        entrypointtypename = (x.entrypointtype != null) ? x.entrypointtype.name : "",
                        entrypointtypevalue = x.entrypointtypevalue,
                        instrumenttypeid = x.instrumenttypeid,
                        instrumenttypename = (x.instrumenttype != null) ? x.instrumenttype.name : "",
                        marketsycleid = x.marketsycleid,
                        marketsyclename = (x.marketsycle != null) ? x.marketsycle.name : "",
                        positiontypeId = x.positiontypeId,
                        positiontypename = (x.positiontype != null) ? x.positiontype.name : "",
                        resistance1 = x.resistance1,
                        resistance2 = x.resistance2,
                        resistance3 = x.resistance3,
                        signalchannelId = x.signalchannelId,
                        sl = x.sl,
                        support1 = x.support1,
                        support2 = x.support2,
                        support3 = x.support3,
                        timeframe_15min = x.timeframe_15min,
                        timeframe_1day = x.timeframe_1day,
                        timeframe_1houre = x.timeframe_1houre,
                        timeframe_1min = x.timeframe_1min,
                        timeframe_1month = x.timeframe_1month,
                        timeframe_1week = x.timeframe_1week,
                        timeframe_30min = x.timeframe_30min,
                        timeframe_4houre = x.timeframe_4houre,
                        timeframe_5min = x.timeframe_5min,
                        timeframe_8houre = x.timeframe_8houre,
                        tp1 = x.tp1,
                        tp2 = x.tp2,
                        tp3 = x.tp3,
                        pageindex = pageIndex,
                        rowcount = PageRowCount
                    }).ToListAsync();

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

        public async Task<SystemMessageModel> CreateSignal(SignalDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                Signal data = new Signal()
                {
                    Id = Guid.NewGuid(),
                    analysistypeId = (int)model.analysistypeId,
                    creatoruserId = userlogin.userid,
                    description = model.description ?? "",
                    createdatetime = DateTime.Now,
                    entrypointtypeId = model.entrypointtypeId,
                    instrumenttypeid = model.instrumenttypeid,
                    tp3 = model.tp3,
                    tp2 = model.tp2,
                    tp1 = model.tp1,
                    timeframe_8houre = model.timeframe_8houre,
                    timeframe_5min = model.timeframe_5min,
                    timeframe_4houre = model.timeframe_4houre,
                    timeframe_30min = model.timeframe_30min,
                    timeframe_1week = model.timeframe_1week,
                    timeframe_1month = model.timeframe_1month,
                    timeframe_1min = model.timeframe_1min,
                    timeframe_1houre = model.timeframe_1houre,
                    timeframe_1day = model.timeframe_1day,
                    timeframe_15min = model.timeframe_15min,
                    support3 = model.support3,
                    support2 = model.support2,
                    support1 = model.support1,
                    sl = model.sl,
                    signalchannelId = model.signalchannelId,
                    resistance3 = model.resistance3,
                    resistance2 = model.resistance2,
                    resistance1 = model.resistance1,
                    positiontypeId = model.positiontypeId,
                    marketsycleid = model.marketsycleid,
                    entrypointtypevalue = model.entrypointtypevalue,
                    entrypoint = model.entrypoint
                };

                await _Context.Signals.AddAsync(data);
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

        public async Task<SystemMessageModel> DeleteSignal(SignalDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                Signal data = await _Context.Signals.FindAsync(model.Id);

                _Context.Signals.Remove(data);

                SignalFileAttachment attachment = await _Context.SignalFileAttachments.Where(x => x.signalId == model.Id).SingleOrDefaultAsync();

                _Context.SignalFileAttachments.Remove(attachment);

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

        public async Task<byte[]> AddSignalImage(Guid signalId)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                SignalFileAttachment data = await _Context.SignalFileAttachments.Where(x => x.signalId == signalId).SingleOrDefaultAsync();

                if (data != null && data.attachment != null)
                {
                    return data.attachment;
                }
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

        public async Task<SystemMessageModel> AddImageToSigal(SignalFileAttachmentDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                Signal data = await _Context.Signals.FindAsync(model.Id);
                if (data != null)
                {
                    SignalFileAttachment attachment = new SignalFileAttachment()
                    {
                        Id = Guid.NewGuid(),
                        signalId = data.Id,
                        attachment = model.attachment
                    };
                    await _Context.SignalFileAttachments.AddAsync(attachment);
                    await _Context.SaveChangesAsync();
                }


                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request success", MessageData = model };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }
    }
}
