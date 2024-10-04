using Base.Common.Enums;
using DataLayers;
using DocumentFormat.OpenXml.Office2010.Excel;
using Entities.DBEntities;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NextTradeAPIs.Dtos;
using System.Diagnostics;

namespace NextTradeAPIs.Services
{

    public class MarketPulsForexServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;
        public MarketPulsForexServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
        }

        public async Task<SystemMessageModel> SaveForexItem(ForexDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                Forex data = new Forex()
                {
                    id = Guid.NewGuid(),
                    categoryid = model.categoryid,
                    creatoruserid = userlogin.userid,
                    price = 0,
                    isvisible = model.isvisible ?? true,
                    courseleveltypeId = model.courseleveltypeId ?? 0,
                    coursetitle = model.coursetitle,
                    oneyeardescription = model.oneyeardescription,
                    chartdescription = model.chartdescription,
                    firstcountryheading = model.firstcountryheading,
                    firstcountrydescription = model.firstcountrydescription,
                    secondcountryheading = model.secondcountryheading,
                    secondcountrydescription = model.secondcountrydescription,
                    bottomdescription = model.bottomdescription,
                    maindescription = model.maindescription,
                    singlepagechartimage = model.singlepagechartimage,
                    instrumentname = model.instrumentname,
                    fundamentalheading = model.fundamentalheading,
                    technicalheading = model.technicalheading,
                    marketsessiontitle = model.marketsessiontitle,
                    marketsessionscript = model.marketsessionscript,
                    marketsentimentstitle = model.marketsentimentstitle,
                    marketsentimentsscript = model.marketsentimentsscript,
                    privatenotes = model.privatenotes,
                    excerpt = model.excerpt,
                    author = model.author
                };
                await _Context.Forexs.AddAsync(data);

                if (model.URLSectionlist != null && model.URLSectionlist.Count() > 0)
                {
                    List<URLSection> URLSectionlist = new List<URLSection>();
                    foreach (URLSectionDto x in model.URLSectionlist)
                    {
                        URLSectionlist.Add(new URLSection()
                        {
                            url = x.url,
                            urltitle = x.urltitle,
                            id = Guid.NewGuid(),
                            marketpulsforexid = data.id
                        });
                    }
                    await _Context.URLSections.AddRangeAsync(URLSectionlist);
                }
                if (model.TechnicalTabslist != null && model.TechnicalTabslist.Count() > 0)
                {
                    List<TechnicalTabs> TechnicalTabslist = new List<TechnicalTabs>();
                    foreach (TechnicalTabsDto x in model.TechnicalTabslist)
                    {
                        TechnicalTabslist.Add(new TechnicalTabs()
                        {
                            maintitle = x.maintitle,
                            script = x.script,
                            id = Guid.NewGuid(),
                            marketpulsforexid = data.id
                        });
                    }
                    await _Context.TechnicalTabss.AddRangeAsync(TechnicalTabslist);
                }


                if (model.TechnicalBreakingNewslist != null && model.TechnicalBreakingNewslist.Count() > 0)
                {
                    List<TechnicalBreakingNews> TechnicalBreakingNewslist = new List<TechnicalBreakingNews>();
                    foreach (TechnicalBreakingNewsDto x in model.TechnicalBreakingNewslist)
                    {
                        TechnicalBreakingNewslist.Add(new TechnicalBreakingNews()
                        {
                            maintitle = x.maintitle,
                            script = x.script,
                            id = Guid.NewGuid(),
                            marketpulsforexid = data.id
                        });
                    }
                    await _Context.TechnicalBreakingNewss.AddRangeAsync(TechnicalBreakingNewslist);
                }



                if (model.SecondCountryDatalist != null && model.SecondCountryDatalist.Count() > 0)
                {
                    List<SecondCountryData> SecondCountryDatalist = new List<SecondCountryData>();
                    foreach (SecondCountryDataDto x in model.SecondCountryDatalist)
                    {
                        SecondCountryDatalist.Add(new SecondCountryData()
                        {
                            id = Guid.NewGuid(),
                            marketpulsforexid = data.id,
                            avragedaily = x.avragedaily,
                            centralbank = x.centralbank,
                            countries = x.countries,
                            nickname = x.nickname
                        });
                    }
                    await _Context.SecondCountryDatas.AddRangeAsync(SecondCountryDatalist);
                }


                if (model.PDFSectionlist != null && model.PDFSectionlist.Count() > 0)
                {
                    List<PDFSection> PDFSectionlist = new List<PDFSection>();
                    foreach (PDFSectionDto x in model.PDFSectionlist)
                    {
                        PDFSectionlist.Add(new PDFSection()
                        {
                            id = Guid.NewGuid(),
                            marketpulsforexid = data.id,
                            author = x.author,
                            pdfshortcodeid = x.pdfshortcodeid,
                            pdftitle = x.pdftitle,
                            shortdescription = x.shortdescription
                        });
                    }
                    await _Context.PDFSections.AddRangeAsync(PDFSectionlist);
                }


                if (model.NewsMainContentlist != null && model.NewsMainContentlist.Count() > 0)
                {
                    List<NewsMainContent> NewsMainContentlist = new List<NewsMainContent>();
                    foreach (NewsMainContentDto x in model.NewsMainContentlist)
                    {
                        NewsMainContentlist.Add(new NewsMainContent()
                        {
                            maintitle = x.maintitle,
                            script = x.script,
                            id = Guid.NewGuid(),
                            marketpulsforexid = data.id
                        });
                    }
                    await _Context.NewsMainContents.AddRangeAsync(NewsMainContentlist);
                }


                if (model.FundamentalNewsSectionlist != null && model.FundamentalNewsSectionlist.Count() > 0)
                {
                    List<FundamentalNewsSection> FundamentalNewsSectionlist = new List<FundamentalNewsSection>();
                    foreach (FundamentalNewsSectionDto x in model.FundamentalNewsSectionlist)
                    {
                        FundamentalNewsSectionlist.Add(new FundamentalNewsSection()
                        {
                            maintitle = x.maintitle,
                            script = x.script,
                            id = Guid.NewGuid(),
                            marketpulsforexid = data.id
                        });
                    }
                    await _Context.FundamentalNewsSections.AddRangeAsync(FundamentalNewsSectionlist);
                }


                if (model.FlexibleBlocklist != null && model.FlexibleBlocklist.Count() > 0)
                {
                    List<FlexibleBlock> FlexibleBlocklist = new List<FlexibleBlock>();
                    foreach (FlexibleBlockDto x in model.FlexibleBlocklist)
                    {
                        FlexibleBlocklist.Add(new FlexibleBlock()
                        {
                            id = Guid.NewGuid(),
                            marketpulsforexid = data.id,
                            countries = x.countries,
                            dailyavrage = x.dailyavrage,
                            highslows = x.highslows,
                            MainTitle = x.MainTitle,
                            pairsthatcorrelate = x.pairsthatcorrelate,
                            pairtype = x.pairtype
                        });
                    }
                    await _Context.FlexibleBlocks.AddRangeAsync(FlexibleBlocklist);
                }


                if (model.FirstCountryDatalist != null && model.FirstCountryDatalist.Count() > 0)
                {
                    List<FirstCountryData> FirstCountryDatalist = new List<FirstCountryData>();
                    foreach (FirstCountryDataDto x in model.FirstCountryDatalist)
                    {
                        FirstCountryDatalist.Add(new FirstCountryData()
                        {
                            id = Guid.NewGuid(),
                            marketpulsforexid = data.id,
                            countries = x.countries,
                            avragedaily = x.avragedaily,
                            centralbank = x.centralbank,
                            nickname = x.nickname
                        });
                    }
                    await _Context.FirstCountryDatas.AddRangeAsync(FirstCountryDatalist);
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
        public async Task<SystemMessageModel> GetForexItem(ForexDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                List<ForexDto> datas;
                IQueryable<Forex> query = _Context.Forexs;

                if (model.id != null)
                    query = query.Where(x=>x.id == model.id);

                if (model.categoryid != null)
                    query = query.Where(x => x.categoryid == model.categoryid);

                if (model.sortitem != null && model.sortitem.Count() > 0)
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
                                };
                            }
                            else if (!(bool)item.ascending)
                            {
                                switch (item.fieldname.ToLower())
                                {
                                    case "createdatetime":
                                        query = query.OrderByDescending(x => x.createdatetime);
                                        break;
                                };
                            }
                        }
                    }

                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 10 : (int)model.rowcount;
                int totaldata = query.Count();
                if (totaldata <= 0) totaldata = 1;
                decimal pagecountd = ((decimal)totaldata / (decimal)PageRowCount);
                int pagecount = (totaldata / PageRowCount);
                pagecount = (pagecount <= 0) ? 1 : pagecount;
                if (Math.Floor(pagecountd) > 0)
                    pagecount++;


                datas = await query.Select(x => new ForexDto()
                {
                    id = x.id,
                    categoryid = x.categoryid,
                    createdatetime = x.createdatetime,
                    creatoruserid = x.creatoruserid,
                    price = x.price,
                    isvisible = x.isvisible,
                    courseleveltypeId = x.courseleveltypeId,
                    coursetitle = x.coursetitle,
                    oneyeardescription = x.oneyeardescription,
                    chartdescription = x.chartdescription,
                    firstcountryheading = x.firstcountryheading,
                    firstcountrydescription = x.firstcountrydescription,
                    secondcountryheading = x.secondcountryheading,
                    secondcountrydescription = x.secondcountrydescription,
                    bottomdescription = x.bottomdescription,
                    maindescription = x.maindescription,
                    singlepagechartimage = x.singlepagechartimage,
                    instrumentname = x.instrumentname,
                    fundamentalheading = x.fundamentalheading,
                    technicalheading = x.technicalheading,
                    marketsessiontitle = x.marketsessiontitle,
                    marketsessionscript = x.marketsessionscript,
                    marketsentimentstitle = x.marketsentimentstitle,
                    marketsentimentsscript = x.marketsentimentsscript,
                    privatenotes = x.privatenotes,
                    excerpt = x.excerpt,
                    author = x.author

                }).ToListAsync();

                foreach (ForexDto data in datas)
                {
                    data.URLSectionlist = await _Context.URLSections.Where(x => x.marketpulsforexid == data.id).Select(x => new URLSectionDto() { url = x.url, urltitle = x.urltitle, id = x.id, marketpulsforexid = x.marketpulsforexid }).ToListAsync();

                    data.TechnicalTabslist = await _Context.TechnicalTabss.Where(x => x.marketpulsforexid == data.id).Select(x => new TechnicalTabsDto() { maintitle = x.maintitle, script = x.script, id = x.id, marketpulsforexid = x.marketpulsforexid }).ToListAsync();

                    data.TechnicalBreakingNewslist = await _Context.TechnicalBreakingNewss.Where(x => x.marketpulsforexid == data.id).Select(x => new TechnicalBreakingNewsDto() { maintitle = x.maintitle, script = x.script, id = x.id, marketpulsforexid = x.marketpulsforexid }).ToListAsync();

                    data.SecondCountryDatalist = await _Context.SecondCountryDatas.Where(x => x.marketpulsforexid == data.id).Select(x => new SecondCountryDataDto() { avragedaily = x.avragedaily, centralbank = x.centralbank, countries = x.countries, nickname = x.nickname, id = x.id, marketpulsforexid = x.marketpulsforexid }).ToListAsync();

                    data.PDFSectionlist = await _Context.PDFSections.Where(x => x.marketpulsforexid == data.id).Select(x => new PDFSectionDto() { author = x.author, pdfshortcodeid = x.pdfshortcodeid, pdftitle = x.pdftitle, shortdescription = x.shortdescription, id = x.id, marketpulsforexid = x.marketpulsforexid }).ToListAsync();

                    data.NewsMainContentlist = await _Context.NewsMainContents.Where(x => x.marketpulsforexid == data.id).Select(x => new NewsMainContentDto() { maintitle = x.maintitle, script = x.script, id = x.id, marketpulsforexid = x.marketpulsforexid }).ToListAsync();

                    data.FundamentalNewsSectionlist = await _Context.FundamentalNewsSections.Where(x => x.marketpulsforexid == data.id).Select(x => new FundamentalNewsSectionDto() { maintitle = x.maintitle, script = x.script, id = x.id, marketpulsforexid = x.marketpulsforexid }).ToListAsync();

                    data.FlexibleBlocklist = await _Context.FlexibleBlocks.Where(x => x.marketpulsforexid == data.id).Select(x => new FlexibleBlockDto() { countries = x.countries, dailyavrage = x.dailyavrage, highslows = x.highslows, MainTitle = x.MainTitle, pairsthatcorrelate = x.pairsthatcorrelate, pairtype = x.pairtype, id = x.id, marketpulsforexid = x.marketpulsforexid }).ToListAsync();

                    data.FirstCountryDatalist = await _Context.FirstCountryDatas.Where(x => x.marketpulsforexid == data.id).Select(x => new FirstCountryDataDto() { avragedaily = x.avragedaily, centralbank = x.centralbank, countries = x.countries, nickname = x.nickname, id = x.id, marketpulsforexid = x.marketpulsforexid }).ToListAsync();

                }
                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = datas, Meta = new { pagecount = pagecount, rowcount = PageRowCount,pageindex = pageIndex } };
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
