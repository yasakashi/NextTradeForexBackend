using Base.Common.Enums;
using DataLayers;
using DocumentFormat.OpenXml.InkML;
using Entities.DBEntities;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NextTradeAPIs.Dtos;
using System.Diagnostics;

namespace NextTradeAPIs.Services
{
    public class CommunityGroupServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;
        public CommunityGroupServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
        }

        public async Task<SystemMessageModel> CreateCommunityGroup(CommunityGroupDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                CommunityGroup data = new CommunityGroup()
                {
                    Id = Guid.NewGuid(),
                    categoryid = (long)model.categoryid,
                    owneruserid = userlogin.userid,
                    createdatetime = DateTime.Now,
                    title = model.title
                };

                _Context.CommunityGroups.Add(data);
                await _Context.SaveChangesAsync();

                model.Id = data.Id;

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "درخواست با موفقیت انجام شد", MessageData = model };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "خطا در انجام درخواست", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }


        public async Task<SystemMessageModel> GetCommunityGroup(GroupSearchFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<CommunityGroup> query = _Context.CommunityGroups;
                if (model.owneruserid != null)
                    query = query.Where(x => x.owneruserid == model.owneruserid);

                if (model.categoryid != null)
                    query = query.Where(x => x.categoryid == model.categoryid);

                List<CommunityGroupDto> datas = await query.Include(x => x.category).Select(x => new CommunityGroupDto()
                {
                    Id = x.Id,
                    owneruserid = x.owneruserid,
                    categoryid = x.categoryid,
                    createdatetime = x.createdatetime,
                    title = x.title,
                    categoryname = x.category.name,
                }).ToListAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "درخواست با موفقیت انجام شد", MessageData = datas };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "خطا در انجام درخواست", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> UpdateCommunityGroupImage(CommunityGroupDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                CommunityGroup data = await _Context.CommunityGroups.FindAsync(model.Id);
                if(data!=  null)
                {
                    data.coverimage = model.coverimage;
                }

                _Context.CommunityGroups.Update(data);
                await _Context.SaveChangesAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request success", MessageData = model };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "خطا در انجام درخواست", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }
    }
}
