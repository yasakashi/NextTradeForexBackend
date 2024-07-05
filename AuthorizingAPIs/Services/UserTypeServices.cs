using Base.Common.Enums;
using DataLayers;
using Entities.DBEntities;
using Entities.Dtos;
using Entities.Dtos;
using Entities.Systems;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NextTradeAPIs.Dtos;
using System.Diagnostics;

namespace NextTradeAPIs.Services;

public class UserTypeServices
{
    SBbContext _Context { get; set; }
    LogSBbContext _LogContext { get; set; }
    SystemLogServices _systemLogServices;
    private readonly IConfiguration _config;
    public UserTypeServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
    {
        _Context = context;
        _LogContext = logcontext;
        _config = config;
        _systemLogServices = systemLogServices;
    }

    public async Task<SystemMessageModel> GetUserTypes(UserModel? userlogin, string processId, string clientip, string hosturl)
    {
        SystemMessageModel message;
        StackTrace stackTrace = new StackTrace();
        string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
        long SerrvieCode = 129000;

        try
        {
            List<UserTypeDto> datas = await _Context.UserTypes.Select(x=> new UserTypeDto() { 
                Id = x.Id,
                Name = x.Name
            } ).ToListAsync();

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

    public async Task<SystemMessageModel> GetCategory(UserModel? userlogin, string processId, string clientip, string hosturl)
    {
        SystemMessageModel message;
        StackTrace stackTrace = new StackTrace();
        string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
        long SerrvieCode = 130000;

        try
        {
            List<CategorisDto> datas = await _Context.Categories.Select(x => new CategorisDto()
            {
                Id = x.Id,
                parentId=x.parentId,
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

    public async Task<SystemMessageModel> GetSubCategory(long? categoryid, UserModel? userlogin, string processId, string clientip, string hosturl)
    {
        SystemMessageModel message;
        StackTrace stackTrace = new StackTrace();
        string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
        long SerrvieCode = 130000;

        try
        {
            IQueryable<Category> query = _Context.Categories;
            if (categoryid != null && ((long)categoryid < 0))
                query = query.Where(x=> x.Id == categoryid);

            List<CategorisDto> datas = await query.Select(x => new CategorisDto()
            {
                Id = x.Id,
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

    public async Task<SystemMessageModel> GetSubCategoryGroup(long? subcategoryid, UserModel? userlogin, string processId, string clientip, string hosturl)
    {
        SystemMessageModel message = new SystemMessageModel();
        StackTrace stackTrace = new StackTrace();
        string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
        long SerrvieCode = 130000;

        try
        {
            //IQueryable<SubCategory> query = _Context.SubCategories;

            //if (subcategoryid != null && ((long)subcategoryid < 0))
            //    query = query.Where(x => x.Id == subcategoryid);

            //List<SubCategoryDto> datas = await query.Select(x => new SubCategoryDto()
            //{
            //    Id = x.Id,
            //    name = x.name,
            //    categotyId = x.categotyId
            //}).ToListAsync();

            //message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = datas };
        }
        catch (Exception ex)
        {
            message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
            string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
            await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
        }
        return message;
    }

    public async Task<SystemMessageModel> AddCategory(BaseInformationDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
    {
        SystemMessageModel message;
        StackTrace stackTrace = new StackTrace();
        string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
        long SerrvieCode = 130000;

        try
        {
            Category datas = new Category()
            {
                name = model.name,
                parentId=model.parentid
            };
            _Context.Categories.Add(datas);
            await _Context.SaveChangesAsync();

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

    public async Task<SystemMessageModel> GetGroupTypes(UserModel? userlogin, string processId, string clientip, string hosturl)
    {
        SystemMessageModel message;
        StackTrace stackTrace = new StackTrace();
        string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
        long SerrvieCode = 129000;

        try
        {
            List<GroupTypeDto> datas = await _Context.GroupTypes.Select(x => new GroupTypeDto()
            {
                Id = x.Id,
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

}
