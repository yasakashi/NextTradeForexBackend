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

        public async Task<SystemMessageModel> CreateForumMessage(ForumMessageFilterDto model,UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if(userlogin.UserTypeId == (long)UserTypes.Student && model.communitygroupid == null)
                    return new SystemMessageModel() { MessageCode = -220, MessageDescription = "you not allowed to create message", MessageData = model };

                ForumMessage data  = new ForumMessage()
                { 
                    Id = Guid.NewGuid(),
                    parentId= model.parentId,
                    categoryid = (long)model.categoryid,
                    creatoruserid = userlogin.userid,
                    registerdatetime = DateTime.Now,    
                    messagebody = model.messagebody,    
                    communitygroupid = model.communitygroupid,  
                    title = model.title
                };

                _Context.ForumMessages.Add(data);
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


        public async Task<SystemMessageModel> GetForumMessage(ForumMessageFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable <ForumMessage> query = _Context.ForumMessages;
                if (model.parentId != null)
                    query = query.Where(x => x.parentId == model.parentId);

                if (model.categoryid != null)
                    query = query.Where(x => x.categoryid == model.categoryid);

                if (model.communitygroupid != null)
                    query = query.Where(x => x.communitygroupid == model.communitygroupid);

                if (model.fromregisterdatetime != null)
                    query = query.Where(x => x.registerdatetime >= model.fromregisterdatetime);

                if (model.toregisterdatetime != null)
                    query = query.Where(x => x.registerdatetime <= model.toregisterdatetime);

                List<ForumMessageDto> datas = await query.Include(x=> x.category).Select(x=> new ForumMessageDto() { 
                    Id = x.Id,
                    parentId = x.parentId,
                    categoryid = x.categoryid,
                    creatoruserid = x.creatoruserid,
                    registerdatetime = x.registerdatetime,
                    messagebody = x.messagebody,
                    communitygroupid = x.communitygroupid,
                    title = x.title,
                    isneedpaid = x.isneedpaid,
                    allowtoshow = (userlogin.ispaid || !x.isneedpaid) ? true : false,
                    categoryname = x.category.name
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
