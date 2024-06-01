using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DataLayers;
using System.Diagnostics;
using Entities.Dtos;

using Microsoft.Extensions.Configuration;
using Base.Common.Enums;
using System.Collections.Generic;
using NextTradeAPIs.Dtos;
using RestSharp;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Entities.DBEntities;
using System.Net.Sockets;
using DocumentFormat.OpenXml.InkML;
using Entities.Systems;
using NextTradeAPIs.Services;

namespace NextTradeAPIs.Services
{
    public class PeopleServices
    {
        SBbContext _Context { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;

        public PeopleServices(SBbContext context, IConfiguration config, SystemLogServices systemLogServices)
        {
            _Context = context;
            _config = config;
            _systemLogServices = systemLogServices;
        }

        public async Task<SystemMessageModel> GetPeople(UserSearchModel filter, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            List<Person> datas = null;
            long SerrvieCode = 201000;
            try
            {
                IQueryable<Person> query = _Context.People;

                if (!string.IsNullOrEmpty(filter.mobile))
                    query = query.Where(x => x.Mobile == filter.mobile);


                datas = await query.ToListAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = datas };
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
