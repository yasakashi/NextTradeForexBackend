using Base.Common.Enums;
using DataLayers;
using Entities.DBEntities;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NextTradeAPIs.Dtos;
using System.Diagnostics;

namespace NextTradeAPIs.Services
{
    public class SiteMessageServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;
        public SiteMessageServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
        }

        public async Task<SystemMessageModel> CreateSiteMessage(SiteMessageDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                User user = await _Context.Users.Where(x=>x.Username == model.reciverusername).SingleOrDefaultAsync();
                SiteMessage data = new SiteMessage()
                {
                    Id = Guid.NewGuid(),
                    registerdatetime = DateTime.Now,
                    isvisited = false,
                    reciveruserId = user.UserId,
                    messagebody = model.messagebody,
                    messagetitle = model.messagetitle
                };

                _Context.SiteMessages.Add(data);
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


        public async Task<SystemMessageModel> GetSiteMessage(SiteMessageDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<SiteMessage> query = _Context.SiteMessages;
                if (model.fromdate != null)
                    query = query.Where(x => x.registerdatetime >= model.fromdate);

                if (model.todate != null)
                    query = query.Where(x => x.registerdatetime <= model.todate);

                if (model.Id != null)
                    query = query.Where(x => x.Id == model.Id);

                if (model.isvisited != null)
                    query = query.Where(x => x.isvisited == model.isvisited);

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

                List<SiteMessageDto> datas = await query
                                .Skip(pageIndex - 1)
                                .Take(PageRowCount)
                                .Include(x=>x.reciveruser)
                                .Select(x => new SiteMessageDto()
                                {
                                    Id = x.Id,
                                    isvisited = x.isvisited,
                                    messagebody = x.messagebody,
                                    messagetitle = x.messagetitle,
                                    reciveruserId = x.reciveruserId,
                                    registerdatetime = x.registerdatetime,
                                    reciverusername = x.reciveruser.Username,
                                    rowcount = PageRowCount,
                                    pageindex = pageIndex
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
    }
}
