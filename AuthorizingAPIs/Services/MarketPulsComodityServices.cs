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
    public class MarketPulsComodityServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;
        public MarketPulsComodityServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
        }
        public async Task<SystemMessageModel> DeleteComodityItem(ComodityFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                Comodity data;
                IQueryable<Comodity> query = _Context.Comodities;

                if (model.id != null)
                    query = query.Where(x => x.id == model.id);

                if (model.categoryid != null)
                    query = query.Where(x => x.categoryid == model.categoryid);

                data = await query.FirstOrDefaultAsync();

                _Context.Comodities.Remove(data);

                //List<URLSection> URLSectionlist = await _Context.URLSections.Where(x => x.marketpulsforexid == data.id).ToListAsync();
                //_Context.URLSections.RemoveRange(URLSectionlist);

                //List<TechnicalTabs> TechnicalTabslist = await _Context.TechnicalTabss.Where(x => x.marketpulsforexid == data.id).ToListAsync();
                //_Context.TechnicalTabss.RemoveRange(TechnicalTabslist);


                //List<TechnicalBreakingNews> TechnicalBreakingNewslist = await _Context.TechnicalBreakingNewss.Where(x => x.marketpulsforexid == data.id).ToListAsync();
                //_Context.TechnicalBreakingNewss.RemoveRange(TechnicalBreakingNewslist);


                //List<SecondCountryData> SecondCountryDatalist = await _Context.SecondCountryDatas.Where(x => x.marketpulsforexid == data.id).ToListAsync();
                //_Context.SecondCountryDatas.RemoveRange(SecondCountryDatalist);


                //List<PDFSection> PDFSectionlist = await _Context.PDFSections.Where(x => x.marketpulsforexid == data.id).ToListAsync();
                //_Context.PDFSections.RemoveRange(PDFSectionlist);

                //List<NewsMainContent> NewsMainContentlist = await _Context.NewsMainContents.Where(x => x.marketpulsforexid == data.id).ToListAsync();
                //_Context.NewsMainContents.RemoveRange(NewsMainContentlist);

                //List<FundamentalNewsSection> FundamentalNewsSectionlist = await _Context.FundamentalNewsSections.Where(x => x.marketpulsforexid == data.id).ToListAsync();
                //_Context.FundamentalNewsSections.RemoveRange(FundamentalNewsSectionlist);


                //List<FlexibleBlock> FlexibleBlocklist = await _Context.FlexibleBlocks.Where(x => x.marketpulsforexid == data.id).ToListAsync();
                //_Context.FlexibleBlocks.RemoveRange(FlexibleBlocklist);

                //List<FirstCountryData> FirstCountryDatalist = await _Context.FirstCountryDatas.Where(x => x.marketpulsforexid == data.id).ToListAsync();
                //_Context.FirstCountryDatas.RemoveRange(FirstCountryDatalist);

                await _Context.SaveChangesAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = model, Meta = null };
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
