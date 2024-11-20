using Base.Common.Enums;
using DataLayers;
using Entities.DBEntities;
using Entities.Dtos;
using Entities.Dtos;
using Entities.Systems;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using Newtonsoft.Json;
using NextTradeAPIs.Dtos;
using System.Collections.Generic;
using System.Diagnostics;

namespace NextTradeAPIs.Services;

public class CategoriesServices
{
    SBbContext _Context { get; set; }
    LogSBbContext _LogContext { get; set; }
    SystemLogServices _systemLogServices;
    private readonly IConfiguration _config;
    public CategoriesServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
    {
        _Context = context;
        _LogContext = logcontext;
        _config = config;
        _systemLogServices = systemLogServices;
    }


    public async Task<SystemMessageModel> GetCategory(CategorisDto filter, UserModel? userlogin, string processId, string clientip, string hosturl)
    {
        SystemMessageModel message;
        StackTrace stackTrace = new StackTrace();
        string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
        long SerrvieCode = 130000;

        try
        {
            List<CategorisDto> datas = await _Context.Categories
                                                     //.Where(x => x.categorytypeid == 14)
                                                     .Include(x => x.categorytype)
                                                     .Select(x => new CategorisDto()
                                                     {
                                                         Id = x.Id,
                                                         parentId = x.parentId,
                                                         name = x.name,
                                                         categorytypeid = x.categorytypeid,
                                                         categorytypename = (x.categorytype != null) ? x.categorytype.name : ""
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
    public async Task<SystemMessageModel> GetCategoryTree(CategorisDto filter, UserModel? userlogin, string processId, string clientip, string hosturl)
    {
        SystemMessageModel message;
        StackTrace stackTrace = new StackTrace();
        string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
        long SerrvieCode = 130000;

        try
        {
            IQueryable<Category> categories = _Context.Categories;

            List<Category> alldatalist = await _Context.Categories.ToListAsync();

            if (filter.parentId != null)
                categories = categories.Where(x => x.parentId == (long)filter.parentId);

            if (filter.categorytypeid != null)
                categories = categories.Where(x => x.categorytypeid == (int)filter.categorytypeid);

            List<CategorisTreeDto> datas = await categories.Select(x => new CategorisTreeDto()
            {
                Id = x.Id,
                parentId = x.parentId,
                name = x.name,
                categorytypeid = x.categorytypeid,
            }).ToListAsync();


            foreach (CategorisTreeDto node in datas)
            {
                node.children = CreateTree(alldatalist, (long)node.Id);
            }
            message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = datas, Meta = new { totalitem = categories.Count() } };
        }
        catch (Exception ex)
        {
            message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
            string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
            await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
        }
        return message;
    }

    private List<CategorisTreeDto> CreateTree(List<Category> datalist, long parentId)
    {
        List<Category> datas = datalist.Where(x => x.parentId == parentId && x.Id != parentId).ToList();
        if (datas != null && datas.Count > 0)
        {
            List<CategorisTreeDto> children = new List<CategorisTreeDto>();
            foreach (Category x in datas)
            {
                children.Add(new CategorisTreeDto()
                {
                    Id = x.Id,
                    parentId = x.parentId,
                    name = x.name,
                    categorytypeid = x.categorytypeid,
                    children = CreateTree(datalist, x.Id)
                });
            }
            return children;
        }
        else
            return null;

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
                query = query.Where(x => x.Id == categoryid);

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
                parentId = model.parentid
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

    public async Task<SystemMessageModel> GetCategory4MarketPulsForex(ForexFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl, bool gettop)
    {
        SystemMessageModel message;
        StackTrace stackTrace = new StackTrace();
        string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
        long SerrvieCode = 130000;
        List<CategorisDto> datas = null;
        try
        {
            List<long> categoryids = await _Context.Forexs.Select(x => x.categoryid).ToListAsync();
            if (gettop)
            {
                //List<long> topcategoryids = await _Context.Categories.Where(x => categoryids.Contains((long)x.Id)).Select(x=>(long)x.parentId ).ToListAsync();
                //datas = await _Context.Categories.Where(x => topcategoryids.Contains((long)x.Id)).Include(x => x.categorytype)
                //                                     .Select(x => new CategorisDto()
                //                                     {
                //                                         Id = x.Id,
                //                                         parentId = x.parentId,
                //                                         name = x.name,
                //                                         categorytypeid = x.categorytypeid,
                //                                         categorytypename = (x.categorytype != null) ? x.categorytype.name : ""
                //                                     }).ToListAsync();
                datas = await _Context.Categories.Where(x => x.Id == 896).Select(x => new CategorisDto()
                {
                    Id = x.Id,
                    parentId = x.parentId,
                    name = x.name,
                    categorytypeid = x.categorytypeid,
                    categorytypename = (x.categorytype != null) ? x.categorytype.name : ""
                }).ToListAsync();

            }
            else
            {
                //datas = await _Context.Categories.Where(x => categoryids.Contains((long)x.Id)).Include(x => x.categorytype)
                //                     .Select(x => new CategorisDto()
                //                     {
                //                         Id = x.Id,
                //                         parentId = x.parentId,
                //                         name = x.name,
                //                         categorytypeid = x.categorytypeid,
                //                         categorytypename = (x.categorytype != null) ? x.categorytype.name : ""
                //}).ToListAsync();
                datas = await _Context.Categories.Where(x => x.parentId == 896).Select(x => new CategorisDto()
                {
                    Id = x.Id,
                    parentId = x.parentId,
                    name = x.name,
                    categorytypeid = x.categorytypeid,
                    categorytypename = (x.categorytype != null) ? x.categorytype.name : ""
                }).ToListAsync();
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

    public async Task<SystemMessageModel> GetCategoryCurrency4MarketPulsForex(ForexFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl, bool gettop)
    {
        SystemMessageModel message;
        StackTrace stackTrace = new StackTrace();
        string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
        long SerrvieCode = 130000;
        List<CategorisDto> datas = null;
        try
        {

            datas = await _Context.Categories.Where(x => x.parentId == model.categoryid).Select(x => new CategorisDto()
            {
                Id = x.Id,
                parentId = x.parentId,
                name = x.name,
                categorytypeid = x.categorytypeid,
                categorytypename = (x.categorytype != null) ? x.categorytype.name : ""
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

    public async Task<SystemMessageModel> UpdateCategory(CategorisDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
    {
        SystemMessageModel message;
        StackTrace stackTrace = new StackTrace();
        string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
        long SerrvieCode = 130000;

        try
        {
            if (model.Id == null)
                return new SystemMessageModel() { MessageCode = -102, MessageDescription = "Id is Wrong" };

            Category data = await _Context.Categories.FindAsync(model.Id);

            if (data == null)
                return new SystemMessageModel() { MessageCode = -103, MessageDescription = "data not find" };

            data.name = model.name;
            data.parentId = model.parentId;

            _Context.Categories.Update(data);
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

    public async Task<SystemMessageModel> DeleteCategory(CategorisDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
    {
        SystemMessageModel message;
        StackTrace stackTrace = new StackTrace();
        string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
        long SerrvieCode = 130000;

        try
        {
            if (model.Id == null)
                return new SystemMessageModel() { MessageCode = -102, MessageDescription = "Id is Wrong" };

            Category data = await _Context.Categories.FindAsync(model.Id);

            if (data == null)
                return new SystemMessageModel() { MessageCode = -103, MessageDescription = "data not find" };

            List<Category> datas = await _Context.Categories.Where(x=>x.parentId == model.Id).ToListAsync();

            _Context.Categories.Remove(data);

            if(datas!= null && datas.Count() > 0)
                _Context.Categories.RemoveRange(datas);

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

    internal async Task<SystemMessageModel> GetCategory4MarketPulsComodity(ComodityFilterDto model, object value, string processId, string clientip, string hosturl, bool v)
    {
        throw new NotImplementedException();
    }

    internal async Task<SystemMessageModel> GetCategoryCurrency4MarketPulsComodity(ComodityFilterDto model, object value, string processId, string clientip, string hosturl, bool v)
    {
        throw new NotImplementedException();
    }

    internal async Task<SystemMessageModel> GetCategory4MarketPulsIndice(IndiceFilterDto model, object value, string processId, string clientip, string hosturl, bool v)
    {
        //1174
        SystemMessageModel message;
        StackTrace stackTrace = new StackTrace();
        string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
        long SerrvieCode = 130000;
        List<CategorisDto> datas = null;
        try
        {

            datas = await _Context.Categories.Where(x => x.parentId == model.categoryid).Select(x => new CategorisDto()
            {
                Id = x.Id,
                parentId = x.parentId,
                name = x.name,
                categorytypeid = x.categorytypeid,
                categorytypename = (x.categorytype != null) ? x.categorytype.name : ""
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

    internal async Task<SystemMessageModel> GetCategoryCurrency4MarketPulsIndice(IndiceFilterDto model, object value, string processId, string clientip, string hosturl, bool v)
    {
        SystemMessageModel message;
        StackTrace stackTrace = new StackTrace();
        string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
        long SerrvieCode = 130000;
        List<CategorisDto> datas = null;
        try
        {

            datas = await _Context.Categories.Where(x => x.parentId == model.categoryid).Select(x => new CategorisDto()
            {
                Id = x.Id,
                parentId = x.parentId,
                name = x.name,
                categorytypeid = x.categorytypeid,
                categorytypename = (x.categorytype != null) ? x.categorytype.name : ""
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

    internal async Task<SystemMessageModel> GetCategory4MarketPulsIndices(IndiceFilterDto model, object value, string processId, string clientip, string hosturl, bool v)
    {
        SystemMessageModel message;
        StackTrace stackTrace = new StackTrace();
        string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
        long SerrvieCode = 130000;
        List<CategorisDto> datas = null;
        try
        {
            List<long> categoryids = await _Context.Forexs.Select(x => x.categoryid).ToListAsync();

                datas = await _Context.Categories.Where(x => x.parentId == model.categoryid).Select(x => new CategorisDto()
                {
                    Id = x.Id,
                    parentId = x.parentId,
                    name = x.name,
                    categorytypeid = x.categorytypeid,
                    categorytypename = (x.categorytype != null) ? x.categorytype.name : ""
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
