using System.Diagnostics;
using Base.Common.Enums;
using DataLayers;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Entities.DBEntities;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NextTradeAPIs.Dtos;

namespace NextTradeAPIs.Services
{
    public class MarketPulsStockServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;
        public MarketPulsStockServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
        }

        public async Task<SystemMessageModel> SaveStockItem(StockDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (await _Context.Stocks.Where(x => x.categoryid == model.categoryid).AnyAsync())
                {
                    return new SystemMessageModel() { MessageCode = -103, MessageDescription = "For this category save before use update service", MessageData = model };
                }

                Stock data = new Stock()
                {
                    id = Guid.NewGuid(),
                    categoryid = (long)model.categoryid,
                    creatoruserid = userlogin.userid,
                    isvisible = model.isvisible ?? true,
                    courseleveltypeId = model.courseleveltypeId ?? 0,
                    title = model.title,
                    excerpt = model.excerpt,
                    authorid = model.authorid,
                    authorname = model.authorname,
                    createdatetime = DateTime.Now,
                    changestatusdate = DateTime.Now,
                    coursestatusid = (int)CourseStatusList.Draft,
                    fundamentalandtechnicaltabsection_privatenotes = model.fundamentalandtechnicaltabsection.privatenotes,
                    fundamentalandtechnicaltabsection_instrumentname = model.fundamentalandtechnicaltabsection.instrumentname,
                    fundamentalandtechnicaltabsection_fundamentalheading = model.fundamentalandtechnicaltabsection.fundamentalheading,
                    fundamentalandtechnicaltabsection_technicalheading = model.fundamentalandtechnicaltabsection.technicalheading,
                    fundamentalandtechnicaltabsection_marketsesstiontite = model.fundamentalandtechnicaltabsection.marketsesstiontite,
                    fundamentalandtechnicaltabsection_marketsesstionscript = model.fundamentalandtechnicaltabsection.marketsesstionscript,
                    fundamentalandtechnicaltabsection_marketsentimentstitle = model.fundamentalandtechnicaltabsection.marketsentimentstitle,
                    fundamentalandtechnicaltabsection_marketsentimentsscript = model.fundamentalandtechnicaltabsection.marketsentimentsscript,
                    fundamentalandtechnicaltabsection_relatedresorces = model.fundamentalandtechnicaltabsection.relatedresorces,
                    stocksection_newstickernew = model.stock.newstickernew,
                    stocksection_newstickerupdate = model.stock.newstickerupdate,
                    stocksection_newstickerimportant = model.stock.newstickerimportant,
                    stocksection_established = model.stock.established,
                    stocksection_exchange = model.stock.exchange,
                    stocksection_companytype = model.stock.companytype,
                    stocksection_ownership = model.stock.ownership,
                    stocksection_mainofficecountr = model.stock.mainofficecountr,
                    stocksection_url = model.stock.url,
                    stocksection_totalbranches = model.stock.totalbranches,
                    stocksection_otherimportantlocation = model.stock.otherimportantlocation,
                    stocksection_overalllocations = model.stock.overalllocations,
                    stocksection_servicesoffered = model.stock.servicesoffered,
                    stocksection_marketfocus = model.stock.marketfocus,
                    stocksection_briefdescriptionofcompany = model.stock.briefdescriptionofcompany,
                    stocksection_importantresearchnotes = model.stock.importantresearchnotes,
                    stocksection_chart = model.stock.chart,
                    stocksection_briefdescriptionofratio = model.stock.briefdescriptionofratio,
                    financialdata_estturnoverus_year1 = model.stock.financialdata_estturnoverus.year1,
                    financialdata_estturnoverus_year2 = model.stock.financialdata_estturnoverus.year2,
                    financialdata_estturnoverus_year3 = model.stock.financialdata_estturnoverus.year3,
                    financialdata_estturnoverus_year4 = model.stock.financialdata_estturnoverus.year4,
                    financialdata_estturnoverus_year5 = model.stock.financialdata_estturnoverus.year5,
                    financialdata_estgrossprofit_year1 = model.stock.financialdata_estgrossprofit.year1,
                    financialdata_estgrossprofit_year2 = model.stock.financialdata_estgrossprofit.year2,
                    financialdata_estgrossprofit_year3 = model.stock.financialdata_estgrossprofit.year3,
                    financialdata_estgrossprofit_year4 = model.stock.financialdata_estgrossprofit.year4,
                    financialdata_estgrossprofit_year5 = model.stock.financialdata_estgrossprofit.year5,
                    financialdata_estnetprofit_year1 = model.stock.financialdata_estnetprofit.year1,
                    financialdata_estnetprofit_year2 = model.stock.financialdata_estnetprofit.year2,
                    financialdata_estnetprofit_year3 = model.stock.financialdata_estnetprofit.year3,
                    financialdata_estnetprofit_year4 = model.stock.financialdata_estnetprofit.year4,
                    financialdata_estnetprofit_year5 = model.stock.financialdata_estnetprofit.year5,
                    currentfinancial_estturnoverus_q1 = model.stock.currentfinancial_estturnoverus.q1,
                    currentfinancial_estturnoverus_q2 = model.stock.currentfinancial_estturnoverus.q2,
                    currentfinancial_estturnoverus_q3 = model.stock.currentfinancial_estturnoverus.q3,
                    currentfinancial_estturnoverus_q4 = model.stock.currentfinancial_estturnoverus.q4,
                    currentfinancial_estgrossprofit_q1 = model.stock.currentfinancial_estgrossprofit.q1,
                    currentfinancial_estgrossprofit_q2 = model.stock.currentfinancial_estgrossprofit.q2,
                    currentfinancial_estgrossprofit_q3 = model.stock.currentfinancial_estgrossprofit.q3,
                    currentfinancial_estgrossprofit_q4 = model.stock.currentfinancial_estgrossprofit.q4,
                    currentfinancial_estnetprofit_q1 = model.stock.currentfinancial_estnetprofit.q1,
                    currentfinancial_estnetprofit_q2 = model.stock.currentfinancial_estnetprofit.q2,
                    currentfinancial_estnetprofit_q3 = model.stock.currentfinancial_estnetprofit.q3,
                    currentfinancial_estnetprofit_q4 = model.stock.currentfinancial_estnetprofit.q4,
                    workingcapotalratio_ratio = model.stock.workingcapotalratio.ratio,
                    workingcapotalratio_analysisisgood = model.stock.workingcapotalratio.analysis_isgood,
                    quickratio_ratio = model.stock.quickratio.ratio,
                    quickratio_analysisisgood = model.stock.quickratio.analysis_isgood,
                    earningpershareratio_ratio = model.stock.earningpershareratio.ratio,
                    earningpershareratio_analysisisgood = model.stock.earningpershareratio.analysis_isgood,
                    priceearninsratio_ratio = model.stock.priceearninsratio.ratio,
                    priceearninsratio_analysisisgood = model.stock.priceearninsratio.analysis_isgood,
                    earningpersdebttoequityratio_ratio = model.stock.earningpersdebttoequityratio.ratio,
                    earningpersdebttoequityratio_analysisisgood = model.stock.earningpersdebttoequityratio.analysis_isgood,
                    returnonequityratio_ratio = model.stock.returnonequityratio.ratio,
                    returnonequityratio_analysisisgood = model.stock.returnonequityratio.analysis_isgood,

                    tags = model.tags
                };
                await _Context.Stocks.AddAsync(data);
                if (model.fundamentalandtechnicaltabsection != null)
                {
                    List<StockFundametalNewsSection> NewsSectioList = new List<StockFundametalNewsSection>();
                    List<StockNewsMainContent> NewsMainContetList = new List<StockNewsMainContent>();
                    foreach (StockFundametalNewsSectionDto news in model.fundamentalandtechnicaltabsection.fndamentalnewssectionlist)
                    {
                        StockFundametalNewsSection newssection = new StockFundametalNewsSection()
                        {
                            id = Guid.NewGuid(),
                            stockid = data.id,
                            maintitle = news.maintitle,
                            script = news.script
                        };

                        foreach (StockNewsMainContentDto newsmaincontent in news.newsmaincontentlist)
                        {
                            NewsMainContetList.Add(new StockNewsMainContent()
                            {
                                id = Guid.NewGuid(),
                                stockid = data.id,
                                fundamentalandnewssectionid = newssection.id,
                                description = newsmaincontent.description,
                                descriptionfilecontenttype = newsmaincontent.descriptionfilecontenttype,
                                descriptionfilename = newsmaincontent.descriptionfilename,
                                descriptionfilepath = newsmaincontent.descriptionfilepath,
                                descriptionfileurl = newsmaincontent.descriptionfileurl,
                                link = newsmaincontent.link,
                                title = newsmaincontent.title
                            });
                        }
                        NewsSectioList.Add(newssection);
                    }

                    await _Context.StockFundametalNewsSections.AddRangeAsync(NewsSectioList);
                    await _Context.StockNewsMainContents.AddRangeAsync(NewsMainContetList);

                    List<StockTechnicalTab> technicaltabList = new List<StockTechnicalTab>();
                    List<StockTechnicalBreakingNews> technicalbreakList = new List<StockTechnicalBreakingNews>();
                    foreach (StockTechnicalTabsDto tab in model.fundamentalandtechnicaltabsection.technicaltablist)
                    {
                        StockTechnicalTab newtab = new StockTechnicalTab()
                        {
                            id = Guid.NewGuid(),
                            stockid = data.id,
                            tabtitle = tab.tabtitle,
                            script = tab.script
                        };

                        foreach (StockTechnicalBreakingNewsDto newsmaincontent in tab.newsmaincontentlist)
                        {
                            technicalbreakList.Add(new StockTechnicalBreakingNews()
                            {
                                id = Guid.NewGuid(),
                                stockid = data.id,
                                technicaltabid = (Guid)newtab.id,
                                description = newsmaincontent.description,
                                descriptionfilecontenttype = newsmaincontent.descriptionfilecontenttype,
                                descriptionfilename = newsmaincontent.descriptionfilename,
                                descriptionfilepath = newsmaincontent.descriptionfilepath,
                                descriptionfileurl = newsmaincontent.descriptionfileurl,
                                link = newsmaincontent.link,
                                title = newsmaincontent.title
                            });
                        }
                        technicaltabList.Add(newtab);
                    }

                    await _Context.StockTechnicalTabs.AddRangeAsync(technicaltabList);
                    await _Context.StockTechnicalBreakingNewss.AddRangeAsync(technicalbreakList);

                    List<StockPDFSection> PDFSectionlist = new List<StockPDFSection>();
                    if (model.fundamentalandtechnicaltabsection.pdfsectionlist != null && model.fundamentalandtechnicaltabsection.pdfsectionlist.Count > 0)
                    {
                        foreach (StockPDFSectionDto pdf in model.fundamentalandtechnicaltabsection.pdfsectionlist)
                        {
                            PDFSectionlist.Add(new StockPDFSection()
                            {
                                id = Guid.NewGuid(),
                                stockid = data.id,
                                author = pdf.author,
                                pdfshortcodeid = pdf.pdfshortcodeid,
                                pdftitle = pdf.pdftitle,
                                shortdescription = pdf.shortdescription
                            });
                        }
                        if (PDFSectionlist.Count > 0)
                            await _Context.StockPDFSections.AddRangeAsync(PDFSectionlist);
                    }
                    List<StockURLSection> UrlSectionlist = new List<StockURLSection>();
                    if (model.fundamentalandtechnicaltabsection.urlsectionlist != null && model.fundamentalandtechnicaltabsection.urlsectionlist.Count > 0)
                    {
                        foreach (StockURLSectionDto pdf in model.fundamentalandtechnicaltabsection.urlsectionlist)
                        {
                            UrlSectionlist.Add(new StockURLSection()
                            {
                                id = Guid.NewGuid(),
                                stockid = data.id,
                                url = pdf.url,
                                urltitle = pdf.urltitle
                            });
                        }
                        if (UrlSectionlist.Count > 0)
                            await _Context.StockURLSections.AddRangeAsync(UrlSectionlist);
                    }

                }

                if (model.stock != null)
                {
                    if (model.stock.productsservices != null && model.stock.productsservices.Count > 0)
                    {
                        List<StockProductAndService> StockProductAndServiceList = new List<StockProductAndService>();
                        foreach (StockProductAndServiceDto item in model.stock.productsservices)
                        {
                            StockProductAndServiceList.Add(new StockProductAndService()
                            {
                                id = Guid.NewGuid(),
                                stockid = data.id,
                                productserviceratio = item.productserviceratio,
                                revenueetimate = item.revenueetimate
                            });
                        }
                        if (StockProductAndServiceList.Count > 0)
                            await _Context.StockProductAndServices.AddRangeAsync(StockProductAndServiceList);
                    }
                    if (model.stock.industryfocuslist != null && model.stock.industryfocuslist.Count > 0)
                    {
                        List<StockInductryFocus> StockInductryFocusList = new List<StockInductryFocus>();
                        foreach (StockInductryFocusDto item in model.stock.industryfocuslist)
                        {
                            StockInductryFocusList.Add(new StockInductryFocus()
                            {
                                id = Guid.NewGuid(),
                                stockid = data.id,
                                clientnameifapplicable = item.clientnameifapplicable,
                                industryfocus = item.industryfocus,
                                revenueshare = item.revenueshare
                            });
                        }
                        if (StockInductryFocusList.Count > 0)
                            await _Context.StockInductryFocuss.AddRangeAsync(StockInductryFocusList);
                    }
                    if (model.stock.managementteam != null && model.stock.managementteam.Count > 0)
                    {
                        List<StockManagementTeam> StockManagementTeamList = new List<StockManagementTeam>();
                        foreach (StockManagementTeamDto item in model.stock.managementteam)
                        {
                            StockManagementTeamList.Add(new StockManagementTeam()
                            {
                                id = Guid.NewGuid(),
                                stockid = data.id,
                                designation = item.designation,
                                name = item.name
                            });
                        }
                        if (StockManagementTeamList.Count > 0)
                            await _Context.StockManagementTeams.AddRangeAsync(StockManagementTeamList);
                    }
                }

                List<StockFlexibleBlock> flexibleBlocklist = new List<StockFlexibleBlock>();
                List<StockCountriesData> countriesDatalist = new List<StockCountriesData>();
                List<StockFirstCountryData> firstCountryDatalist = new List<StockFirstCountryData>();
                List<StockSecondCountryData> secondCountryDatalist = new List<StockSecondCountryData>();

                foreach (StockFlexibleBlockDto flex in model.stacks_old)
                {
                    StockFlexibleBlock FlexibleBlock = new StockFlexibleBlock();
                    FlexibleBlock.id = Guid.NewGuid();
                    FlexibleBlock.stockid = data.id;
                    FlexibleBlock.maintitle = flex.maintitle;
                    FlexibleBlock.oneyeardescription = flex.oneyeardescription;
                    FlexibleBlock.oneyeardescriptionfilename = flex.oneyeardescriptionfilename;
                    FlexibleBlock.oneyeardescriptionfilepath = flex.oneyeardescriptionfilepath;
                    FlexibleBlock.oneyeardescriptionfileurl = flex.oneyeardescriptionfileurl;
                    FlexibleBlock.oneyeardescriptionfilecontenttype = flex.oneyeardescriptionfilecontenttype;
                    FlexibleBlock.chartdescription = flex.chartdescription;
                    FlexibleBlock.chartdescriptionfilename = flex.chartdescriptionfilename;
                    FlexibleBlock.chartdescriptionfilepath = flex.chartdescriptionfilepath;
                    FlexibleBlock.chartdescriptionfileurl = flex.chartdescriptionfileurl;
                    FlexibleBlock.chartdescriptionfilecontenttype = flex.chartdescriptionfilecontenttype;
                    FlexibleBlock.firstcountryheading = flex.firstcountryheading;
                    FlexibleBlock.firstcountrydescription = flex.firstcountrydescription;
                    FlexibleBlock.firstcountrydescriptionfilename = flex.firstcountrydescriptionfilename;
                    FlexibleBlock.firstcountrydescriptionfilepath = flex.firstcountrydescriptionfilepath;
                    FlexibleBlock.firstcountrydescriptionfileurl = flex.firstcountrydescriptionfileurl;
                    FlexibleBlock.firstcountrydescriptionfilecontenttype = flex.firstcountrydescriptionfilecontenttype;
                    FlexibleBlock.secondcountryheading = flex.secondcountryheading;
                    FlexibleBlock.secondcountrydescription = flex.secondcountrydescription;
                    FlexibleBlock.secondcountrydescriptionfilename = flex.secondcountrydescriptionfilename;
                    FlexibleBlock.secondcountrydescriptionfilepath = flex.secondcountrydescriptionfilepath;
                    FlexibleBlock.secondcountrydescriptionfileurl = flex.secondcountrydescriptionfileurl;
                    FlexibleBlock.secondcountrydescriptionfilecontenttype = flex.secondcountrydescriptionfilecontenttype;
                    FlexibleBlock.bottomdescription = flex.bottomdescription;
                    FlexibleBlock.bottomdescriptionfilename = flex.bottomdescriptionfilename;
                    FlexibleBlock.bottomdescriptionfilepath = flex.bottomdescriptionfilepath;
                    FlexibleBlock.bottomdescriptionfileurl = flex.bottomdescriptionfileurl;
                    FlexibleBlock.bottomdescriptionfilecontenttype = flex.bottomdescriptionfilecontenttype;
                    FlexibleBlock.maindescription = flex.maindescription;
                    FlexibleBlock.maindescriptionfilename = flex.maindescriptionfilename;
                    FlexibleBlock.maindescriptionfilepath = flex.maindescriptionfilepath;
                    FlexibleBlock.maindescriptionfileurl = flex.maindescriptionfileurl;
                    FlexibleBlock.maindescriptionfilecontenttype = flex.maindescriptionfilecontenttype;
                    FlexibleBlock.singlepagechartimage = flex.singlepagechartimage;

                    flexibleBlocklist.Add(FlexibleBlock);

                    if (flex.countriesdatalist != null && flex.countriesdatalist.Count > 0)
                    {
                        foreach (StockCountriesDataDto contrydata in flex.countriesdatalist)
                        {
                            countriesDatalist.Add(new StockCountriesData()
                            {
                                id = Guid.NewGuid(),
                                stockid = data.id,
                                stockflexibleblockid = FlexibleBlock.id,
                                contry = contrydata.contry,
                                centeralbank = contrydata.centeralbank,
                                nickname = contrydata.nickname,
                                ofaveragedailyturnover = contrydata.ofaveragedailyturnover
                            });
                        }
                        await _Context.StockCountriesDatas.AddRangeAsync(countriesDatalist);
                    }

                    if (flex.firstcountrydatalist != null && flex.firstcountrydatalist.Count > 0)
                    {
                        foreach (StockFirstCountryDataDto contrydata in flex.firstcountrydatalist)
                        {
                            firstCountryDatalist.Add(new StockFirstCountryData()
                            {
                                id = Guid.NewGuid(),
                                stockid = data.id,
                                stockflexibleblockid = FlexibleBlock.id,
                                centeralbank = contrydata.centeralbank,
                                contry = contrydata.contry,
                                nickname = contrydata.nickname,
                                ofaveragedailyturnover = contrydata.ofaveragedailyturnover
                            });
                        }
                        await _Context.StockFirstCountryDatas.AddRangeAsync(firstCountryDatalist);
                    }

                    if (flex.secondcountrydatalist != null && flex.secondcountrydatalist.Count > 0)
                    {
                        foreach (StockSecondCountryDataDto contrydata in flex.secondcountrydatalist)
                        {
                            secondCountryDatalist.Add(new StockSecondCountryData()
                            {
                                id = Guid.NewGuid(),
                                stockid = data.id,
                                stockflexibleblockid = FlexibleBlock.id,
                                centeralbank = contrydata.centeralbank,
                                contry = contrydata.contry,
                                nickname = contrydata.nickname,
                                ofaveragedailyturnover = contrydata.ofaveragedailyturnover
                            });
                        }
                        await _Context.StockSecondCountryDatas.AddRangeAsync(secondCountryDatalist);
                    }
                }

                await _Context.StockFlexibleBlocks.AddRangeAsync(flexibleBlocklist);

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

        public async Task<SystemMessageModel> GetStockItems(StockFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                List<StockDto> datas;
                IQueryable<Stock> query = _Context.Stocks;

                if (model.id != null)
                    query = query.Where(x => x.id == model.id);

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

                datas = await query.Skip((pageIndex - 1) * PageRowCount)
                                .Take(PageRowCount).Select(x => new StockDto()
                                {
                                    id = x.id,
                                    categoryid = x.categoryid,
                                    creatoruserid = x.creatoruserid,
                                    isvisible = x.isvisible,
                                    courseleveltypeId = x.courseleveltypeId,
                                    title = x.title,
                                    excerpt = x.excerpt,
                                    authorid = x.authorid,
                                    authorname = x.authorname,
                                    createdatetime = x.createdatetime,
                                    changestatusdate = x.changestatusdate,
                                    coursestatusid = x.coursestatusid,
                                    tags = x.tags,
                                    fundamentalandtechnicaltabsection = new StockFundamentalAndTechnicalTabSectionDto()
                                    {
                                        privatenotes = x.fundamentalandtechnicaltabsection_privatenotes,
                                        instrumentname = x.fundamentalandtechnicaltabsection_instrumentname,
                                        fundamentalheading = x.fundamentalandtechnicaltabsection_fundamentalheading,
                                        technicalheading = x.fundamentalandtechnicaltabsection_technicalheading,
                                        marketsesstiontite = x.fundamentalandtechnicaltabsection_marketsesstiontite,
                                        marketsesstionscript = x.fundamentalandtechnicaltabsection_marketsesstionscript,
                                        marketsentimentstitle = x.fundamentalandtechnicaltabsection_marketsentimentstitle,
                                        marketsentimentsscript = x.fundamentalandtechnicaltabsection_marketsentimentsscript,
                                        relatedresorces = x.fundamentalandtechnicaltabsection_relatedresorces,
                                    },
                                    stock = new StockSectionDto()
                                    {
                                        newstickernew = x.stocksection_newstickernew,
                                        newstickerupdate = x.stocksection_newstickerupdate,
                                        newstickerimportant = x.stocksection_newstickerimportant,
                                        established = x.stocksection_established,
                                        exchange = x.stocksection_exchange,
                                        companytype = x.stocksection_companytype,
                                        ownership = x.stocksection_ownership,
                                        mainofficecountr = x.stocksection_mainofficecountr,
                                        url = x.stocksection_url,
                                        totalbranches = x.stocksection_totalbranches,
                                        otherimportantlocation = x.stocksection_otherimportantlocation,
                                        overalllocations = x.stocksection_overalllocations,
                                        servicesoffered = x.stocksection_servicesoffered,
                                        marketfocus = x.stocksection_marketfocus,
                                        briefdescriptionofcompany = x.stocksection_briefdescriptionofcompany,
                                        importantresearchnotes = x.stocksection_importantresearchnotes,
                                        chart = x.stocksection_chart,
                                        briefdescriptionofratio = x.stocksection_briefdescriptionofratio,
                                        financialdata_estturnoverus = new StockFinancialDataDto()
                                        {
                                            year1 = x.financialdata_estturnoverus_year1,
                                            year2 = x.financialdata_estturnoverus_year2,
                                            year3 = x.financialdata_estturnoverus_year3,
                                            year4 = x.financialdata_estturnoverus_year4,
                                            year5 = x.financialdata_estturnoverus_year5,
                                        },
                                        financialdata_estgrossprofit = new StockFinancialDataDto()
                                        {
                                            year1 = x.financialdata_estgrossprofit_year1,
                                            year2 = x.financialdata_estgrossprofit_year2,
                                            year3 = x.financialdata_estgrossprofit_year3,
                                            year4 = x.financialdata_estgrossprofit_year4,
                                            year5 = x.financialdata_estgrossprofit_year5,
                                        },
                                        financialdata_estnetprofit = new StockFinancialDataDto()
                                        {
                                            year1 = x.financialdata_estnetprofit_year1,
                                            year2 = x.financialdata_estnetprofit_year2,
                                            year3 = x.financialdata_estnetprofit_year3,
                                            year4 = x.financialdata_estnetprofit_year4,
                                            year5 = x.financialdata_estnetprofit_year5
                                        },
                                        currentfinancial_estturnoverus = new StockCurrentFinancialYearDto()
                                        {
                                            q1 = x.currentfinancial_estturnoverus_q1,
                                            q2 = x.currentfinancial_estturnoverus_q2,
                                            q3 = x.currentfinancial_estturnoverus_q3,
                                            q4 = x.currentfinancial_estturnoverus_q4
                                        },
                                        currentfinancial_estgrossprofit = new StockCurrentFinancialYearDto()
                                        {
                                            q1 = x.currentfinancial_estgrossprofit_q1,
                                            q2 = x.currentfinancial_estgrossprofit_q2,
                                            q3 = x.currentfinancial_estgrossprofit_q3,
                                            q4 = x.currentfinancial_estgrossprofit_q4
                                        },
                                        currentfinancial_estnetprofit = new StockCurrentFinancialYearDto()
                                        {
                                            q1 = x.currentfinancial_estnetprofit_q1,
                                            q2 = x.currentfinancial_estnetprofit_q2,
                                            q3 = x.currentfinancial_estnetprofit_q3,
                                            q4 = x.currentfinancial_estnetprofit_q4
                                        },
                                        workingcapotalratio = new StockFinancialRatiosDto()
                                        {
                                            ratio = x.workingcapotalratio_ratio,
                                            analysis_isgood = x.workingcapotalratio_analysisisgood
                                        },
                                        quickratio = new StockFinancialRatiosDto()
                                        {
                                            ratio = x.quickratio_ratio,
                                            analysis_isgood = x.quickratio_analysisisgood
                                        },
                                        earningpershareratio = new StockFinancialRatiosDto()
                                        {
                                            ratio = x.earningpershareratio_ratio,
                                            analysis_isgood = x.earningpershareratio_analysisisgood
                                        },
                                        priceearninsratio = new StockFinancialRatiosDto()
                                        {
                                            ratio = x.priceearninsratio_ratio,
                                            analysis_isgood = x.priceearninsratio_analysisisgood
                                        },
                                        earningpersdebttoequityratio = new StockFinancialRatiosDto()
                                        {
                                            ratio = x.earningpersdebttoequityratio_ratio,
                                            analysis_isgood = x.earningpersdebttoequityratio_analysisisgood
                                        },
                                        returnonequityratio = new StockFinancialRatiosDto()
                                        {
                                            ratio = x.returnonequityratio_ratio,
                                            analysis_isgood = x.returnonequityratio_analysisisgood
                                        }
                                    }
                                }).ToListAsync();

                foreach (StockDto data in datas)
                {
                    data.fundamentalandtechnicaltabsection.urlsectionlist = await _Context.StockURLSections.Where(x => x.stockid == data.id).Select(x => new StockURLSectionDto()
                    {
                        id = x.id,
                        stockid = x.stockid,
                        url = x.url,
                        urltitle = x.urltitle
                    }).ToListAsync();

                    data.fundamentalandtechnicaltabsection.pdfsectionlist = await _Context.StockPDFSections.Where(x => x.stockid == data.id).Select(x => new StockPDFSectionDto()
                    {
                        id = x.id,
                        stockid = x.stockid,
                        author = x.author,
                        pdfshortcodeid = x.pdfshortcodeid,
                        pdftitle = x.pdftitle,
                        shortdescription = x.shortdescription
                    }).ToListAsync();

                    data.fundamentalandtechnicaltabsection.fndamentalnewssectionlist = await _Context.StockFundametalNewsSections.Where(x => x.stockid == data.id).Select(x => new StockFundametalNewsSectionDto() { id = x.id, stockid = x.stockid, maintitle = x.maintitle, script = x.script }).ToListAsync();

                    foreach (StockFundametalNewsSectionDto news in data.fundamentalandtechnicaltabsection.fndamentalnewssectionlist)
                    {
                        news.newsmaincontentlist = await _Context.StockNewsMainContents.Where(x => x.stockid == data.id && x.fundamentalandnewssectionid == news.id).Select(x => new StockNewsMainContentDto()
                        {
                            id = x.id,
                            stockid = x.stockid,
                            fundamentalandnewssectionid = x.fundamentalandnewssectionid,
                            description = x.description,
                            descriptionfilecontenttype = x.descriptionfilecontenttype,
                            descriptionfilename = x.descriptionfilename,
                            descriptionfilepath = x.descriptionfilepath,
                            descriptionfileurl = x.descriptionfileurl,
                            link = x.link,
                            title = x.title
                        }).ToListAsync();
                    }

                    data.fundamentalandtechnicaltabsection.technicaltablist = await _Context.StockTechnicalTabs.Where(x => x.stockid == data.id).Select(x => new StockTechnicalTabsDto() { id = x.id, stockid = x.stockid, tabtitle = x.tabtitle, script = x.script }).ToListAsync();

                    foreach (StockTechnicalTabsDto tab in data.fundamentalandtechnicaltabsection.technicaltablist)
                    {
                        tab.newsmaincontentlist = await _Context.StockNewsMainContents.Where(x => x.stockid == data.id && x.fundamentalandnewssectionid == tab.id).Select(x => new StockTechnicalBreakingNewsDto()
                        {
                            id = x.id,
                            stockid = x.stockid,
                            technicaltabid = x.fundamentalandnewssectionid,
                            description = x.description,
                            descriptionfilecontenttype = x.descriptionfilecontenttype,
                            descriptionfilename = x.descriptionfilename,
                            descriptionfilepath = x.descriptionfilepath,
                            descriptionfileurl = x.descriptionfileurl,
                            link = x.link,
                            title = x.title
                        }).ToListAsync();
                    }

                    data.stock.productsservices = await _Context.StockProductAndServices.Where(x => x.stockid == data.id).Select(x => new StockProductAndServiceDto()
                    {
                        id = x.id,
                        stockid = x.stockid,
                        productserviceratio = x.productserviceratio,
                        revenueetimate = x.revenueetimate
                    }).ToListAsync();

                    data.stock.industryfocuslist = await _Context.StockInductryFocuss.Where(x => x.stockid == data.id).Select(x => new StockInductryFocusDto()
                    {
                        id = x.id,
                        stockid = x.stockid,
                        clientnameifapplicable = x.clientnameifapplicable,
                        industryfocus = x.industryfocus,
                        revenueshare = x.revenueshare
                    }).ToListAsync();

                    data.stock.managementteam = await _Context.StockManagementTeams.Where(x => x.stockid == data.id).Select(x => new StockManagementTeamDto()
                    {
                        id = x.id,
                        stockid = x.stockid,
                        designation = x.designation,
                        name = x.name
                    }).ToListAsync();

                    data.stacks_old = await _Context.StockFlexibleBlocks.Where(x => x.stockid == data.id).Select(x => new StockFlexibleBlockDto()
                    {
                        id = x.id,
                        stockid = x.stockid,
                        maintitle = x.maintitle,
                        oneyeardescription = x.oneyeardescription,
                        oneyeardescriptionfilename = x.oneyeardescriptionfilename,
                        oneyeardescriptionfilepath = x.oneyeardescriptionfilepath,
                        oneyeardescriptionfileurl = x.oneyeardescriptionfileurl,
                        oneyeardescriptionfilecontenttype = x.oneyeardescriptionfilecontenttype,
                        chartdescription = x.chartdescription,
                        chartdescriptionfilename = x.chartdescriptionfilename,
                        chartdescriptionfilepath = x.chartdescriptionfilepath,
                        chartdescriptionfileurl = x.chartdescriptionfileurl,
                        chartdescriptionfilecontenttype = x.chartdescriptionfilecontenttype,
                        firstcountryheading = x.firstcountryheading,
                        firstcountrydescription = x.firstcountrydescription,
                        firstcountrydescriptionfilename = x.firstcountrydescriptionfilename,
                        firstcountrydescriptionfilepath = x.firstcountrydescriptionfilepath,
                        firstcountrydescriptionfileurl = x.firstcountrydescriptionfileurl,
                        firstcountrydescriptionfilecontenttype = x.firstcountrydescriptionfilecontenttype,
                        secondcountryheading = x.secondcountryheading,
                        secondcountrydescription = x.secondcountrydescription,
                        secondcountrydescriptionfilename = x.secondcountrydescriptionfilename,
                        secondcountrydescriptionfilepath = x.secondcountrydescriptionfilepath,
                        secondcountrydescriptionfileurl = x.secondcountrydescriptionfileurl,
                        secondcountrydescriptionfilecontenttype = x.secondcountrydescriptionfilecontenttype,
                        bottomdescription = x.bottomdescription,
                        bottomdescriptionfilename = x.bottomdescriptionfilename,
                        bottomdescriptionfilepath = x.bottomdescriptionfilepath,
                        bottomdescriptionfileurl = x.bottomdescriptionfileurl,
                        bottomdescriptionfilecontenttype = x.bottomdescriptionfilecontenttype,
                        maindescription = x.maindescription,
                        maindescriptionfilename = x.maindescriptionfilename,
                        maindescriptionfilepath = x.maindescriptionfilepath,
                        maindescriptionfileurl = x.maindescriptionfileurl,
                        maindescriptionfilecontenttype = x.maindescriptionfilecontenttype,
                        singlepagechartimage = x.singlepagechartimage,
                    }).ToListAsync();

                    foreach (StockFlexibleBlockDto FlexibleBlockitem in data.stacks_old)
                    {
                        FlexibleBlockitem.countriesdatalist = await _Context.StockCountriesDatas.Where(x => x.stockid == data.id && x.stockflexibleblockid == FlexibleBlockitem.id).Select(x => new StockCountriesDataDto()
                        {
                            id = x.id,
                            stockflexibleblockid = x.stockflexibleblockid,
                            stockid = x.stockid,
                            contry = x.contry,
                            centeralbank = x.centeralbank,
                            nickname = x.nickname,
                            ofaveragedailyturnover = x.ofaveragedailyturnover
                        }).ToListAsync();

                        FlexibleBlockitem.firstcountrydatalist = await _Context.StockFirstCountryDatas.Where(x => x.stockid == data.id && x.stockflexibleblockid == FlexibleBlockitem.id).Select(x => new StockFirstCountryDataDto()
                        {
                            id = x.id,
                            stockflexibleblockid = x.stockflexibleblockid,
                            stockid = x.stockid,
                            contry = x.contry,
                            centeralbank = x.centeralbank,
                            nickname = x.nickname,
                            ofaveragedailyturnover = x.ofaveragedailyturnover
                        }).ToListAsync();

                        FlexibleBlockitem.secondcountrydatalist = await _Context.StockSecondCountryDatas.Where(x => x.stockid == data.id && x.stockflexibleblockid == FlexibleBlockitem.id).Select(x => new StockSecondCountryDataDto()
                        {
                            id = x.id,
                            stockflexibleblockid = x.stockflexibleblockid,
                            stockid = x.stockid,
                            centeralbank = x.centeralbank,
                            nickname = x.nickname,
                            ofaveragedailyturnover = x.ofaveragedailyturnover,
                            contry = x.contry
                        }).ToListAsync();
                    }
                }

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = datas, Meta = new { totaldata = datas.Count, pagecount = pagecount } };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> DeleteStockItem(StockFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                Stock data;
                IQueryable<Stock> query = _Context.Stocks;

                if (model.id != null)
                    query = query.Where(x => x.id == model.id);

                if (model.categoryid != null)
                    query = query.Where(x => x.categoryid == model.categoryid);

                data = await query.FirstOrDefaultAsync();

                _Context.Stocks.Remove(data);


                List<StockFlexibleBlock> _StockFlexibleBlocks = await _Context.StockFlexibleBlocks.Where(x => x.stockid == data.id).ToListAsync();
                _Context.StockFlexibleBlocks.RemoveRange(_StockFlexibleBlocks);

                List<StockCountriesData> _StockCountriesDatas = await _Context.StockCountriesDatas.Where(x => x.stockid == data.id).ToListAsync();
                _Context.StockCountriesDatas.RemoveRange(_StockCountriesDatas);

                List<StockFirstCountryData> _StockFirstCountryDataCountriesDatas = await _Context.StockFirstCountryDatas.Where(x => x.stockid == data.id).ToListAsync();
                _Context.StockFirstCountryDatas.RemoveRange(_StockFirstCountryDataCountriesDatas);

                List<StockSecondCountryData> _StockSecondCountryDataCountriesDatas = await _Context.StockSecondCountryDatas.Where(x => x.stockid == data.id).ToListAsync();
                _Context.StockSecondCountryDatas.RemoveRange(_StockSecondCountryDataCountriesDatas);

                List<StockFundametalNewsSection> _Stock_fundamentalandtechnicaltabsection_fundamentalnewssections = await _Context.StockFundametalNewsSections.Where(x => x.stockid == data.id).ToListAsync();
                _Context.StockFundametalNewsSections.RemoveRange(_Stock_fundamentalandtechnicaltabsection_fundamentalnewssections);

                List<StockNewsMainContent> _Stock_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents = await _Context.StockNewsMainContents.Where(x => x.stockid == data.id).ToListAsync();
                _Context.StockNewsMainContents.RemoveRange(_Stock_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents);

                List<StockTechnicalTab> _Stock_Fundamentalandtechnicaltabsection_TechnicalTabses = await _Context.StockTechnicalTabs.Where(x => x.stockid == data.id).ToListAsync();
                _Context.StockTechnicalTabs.RemoveRange(_Stock_Fundamentalandtechnicaltabsection_TechnicalTabses);

                List<StockTechnicalBreakingNews> _Stock_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses = await _Context.StockTechnicalBreakingNewss.Where(x => x.stockid == data.id).ToListAsync();
                _Context.StockTechnicalBreakingNewss.RemoveRange(_Stock_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses);

                List<StockPDFSection> _Stock_FundamentalandTechnicalTabSection_RelatedResorces_PDFSections = await _Context.StockPDFSections.Where(x => x.stockid == data.id).ToListAsync();
                _Context.StockPDFSections.RemoveRange(_Stock_FundamentalandTechnicalTabSection_RelatedResorces_PDFSections);

                List<StockURLSection> _Stock_FundamentalandTechnicalTabSection_RelatedResorces_URLSectionsawait = await _Context.StockURLSections.Where(x => x.stockid == data.id).ToListAsync();
                _Context.StockURLSections.RemoveRange(_Stock_FundamentalandTechnicalTabSection_RelatedResorces_URLSectionsawait);

                List<StockManagementTeam> StockManagementTeamslist = await _Context.StockManagementTeams.Where(x => x.stockid == data.id).ToListAsync();
                _Context.StockManagementTeams.RemoveRange(StockManagementTeamslist);

                List<StockInductryFocus> StockInductryFocusslist = await _Context.StockInductryFocuss.Where(x => x.stockid == data.id).ToListAsync();
                _Context.StockInductryFocuss.RemoveRange(StockInductryFocusslist);

                List<StockProductAndService> StockProductAndServiceslist = await _Context.StockProductAndServices.Where(x => x.stockid == data.id).ToListAsync();
                _Context.StockProductAndServices.RemoveRange(StockProductAndServiceslist);

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


        public async Task<SystemMessageModel> UpdateStockItem(StockDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.id == null)
                    return new SystemMessageModel() { MessageCode = -102, MessageDescription = "Id is Wrong" };

                Stock data = await _Context.Stocks.FindAsync(model.id);
                if (data == null)
                    return new SystemMessageModel() { MessageCode = -103, MessageDescription = "data not find" };

                if (data.categoryid != model.categoryid && await _Context.Stocks.AnyAsync(x => x.categoryid == model.categoryid))
                    return new SystemMessageModel() { MessageCode = -104, MessageDescription = "this category is exist" };

                data.categoryid = (long)model.categoryid;
                data.isvisible = model.isvisible ?? true;
                data.courseleveltypeId = model.courseleveltypeId ?? 0;
                data.title = model.title;
                data.excerpt = model.excerpt;
                data.authorid = model.authorid;
                data.authorname = model.authorname;
                data.changestatusdate = DateTime.Now;
                data.coursestatusid = model.coursestatusid;
                data.tags = model.tags;
                data.fundamentalandtechnicaltabsection_privatenotes = model.fundamentalandtechnicaltabsection.privatenotes;
                data.fundamentalandtechnicaltabsection_instrumentname = model.fundamentalandtechnicaltabsection.instrumentname;
                data.fundamentalandtechnicaltabsection_fundamentalheading = model.fundamentalandtechnicaltabsection.fundamentalheading;
                data.fundamentalandtechnicaltabsection_technicalheading = model.fundamentalandtechnicaltabsection.technicalheading;
                data.fundamentalandtechnicaltabsection_marketsesstiontite = model.fundamentalandtechnicaltabsection.marketsesstiontite;
                data.fundamentalandtechnicaltabsection_marketsesstionscript = model.fundamentalandtechnicaltabsection.marketsesstionscript;
                data.fundamentalandtechnicaltabsection_marketsentimentstitle = model.fundamentalandtechnicaltabsection.marketsentimentstitle;
                data.fundamentalandtechnicaltabsection_marketsentimentsscript = model.fundamentalandtechnicaltabsection.marketsentimentsscript;
                data.fundamentalandtechnicaltabsection_relatedresorces = model.fundamentalandtechnicaltabsection.relatedresorces;
                data.stocksection_newstickernew = model.stock.newstickernew;
                data.stocksection_newstickerupdate = model.stock.newstickerupdate;
                data.stocksection_newstickerimportant = model.stock.newstickerimportant;
                data.stocksection_established = model.stock.established;
                data.stocksection_exchange = model.stock.exchange;
                data.stocksection_companytype = model.stock.companytype;
                data.stocksection_ownership = model.stock.ownership;
                data.stocksection_mainofficecountr = model.stock.mainofficecountr;
                data.stocksection_url = model.stock.url;
                data.stocksection_totalbranches = model.stock.totalbranches;
                data.stocksection_otherimportantlocation = model.stock.otherimportantlocation;
                data.stocksection_overalllocations = model.stock.overalllocations;
                data.stocksection_servicesoffered = model.stock.servicesoffered;
                data.stocksection_marketfocus = model.stock.marketfocus;
                data.stocksection_briefdescriptionofcompany = model.stock.briefdescriptionofcompany;
                data.stocksection_importantresearchnotes = model.stock.importantresearchnotes;
                data.stocksection_chart = model.stock.chart;
                data.stocksection_briefdescriptionofratio = model.stock.briefdescriptionofratio;
                data.financialdata_estturnoverus_year1 = model.stock.financialdata_estturnoverus.year1;
                data.financialdata_estturnoverus_year2 = model.stock.financialdata_estturnoverus.year2;
                data.financialdata_estturnoverus_year3 = model.stock.financialdata_estturnoverus.year3;
                data.financialdata_estturnoverus_year4 = model.stock.financialdata_estturnoverus.year4;
                data.financialdata_estturnoverus_year5 = model.stock.financialdata_estturnoverus.year5;
                data.financialdata_estgrossprofit_year1 = model.stock.financialdata_estgrossprofit.year1;
                data.financialdata_estgrossprofit_year2 = model.stock.financialdata_estgrossprofit.year2;
                data.financialdata_estgrossprofit_year3 = model.stock.financialdata_estgrossprofit.year3;
                data.financialdata_estgrossprofit_year4 = model.stock.financialdata_estgrossprofit.year4;
                data.financialdata_estgrossprofit_year5 = model.stock.financialdata_estgrossprofit.year5;
                data.financialdata_estnetprofit_year1 = model.stock.financialdata_estnetprofit.year1;
                data.financialdata_estnetprofit_year2 = model.stock.financialdata_estnetprofit.year2;
                data.financialdata_estnetprofit_year3 = model.stock.financialdata_estnetprofit.year3;
                data.financialdata_estnetprofit_year4 = model.stock.financialdata_estnetprofit.year4;
                data.financialdata_estnetprofit_year5 = model.stock.financialdata_estnetprofit.year5;
                data.currentfinancial_estturnoverus_q1 = model.stock.currentfinancial_estturnoverus.q1;
                data.currentfinancial_estturnoverus_q2 = model.stock.currentfinancial_estturnoverus.q2;
                data.currentfinancial_estturnoverus_q3 = model.stock.currentfinancial_estturnoverus.q3;
                data.currentfinancial_estturnoverus_q4 = model.stock.currentfinancial_estturnoverus.q4;
                data.currentfinancial_estgrossprofit_q1 = model.stock.currentfinancial_estgrossprofit.q1;
                data.currentfinancial_estgrossprofit_q2 = model.stock.currentfinancial_estgrossprofit.q2;
                data.currentfinancial_estgrossprofit_q3 = model.stock.currentfinancial_estgrossprofit.q3;
                data.currentfinancial_estgrossprofit_q4 = model.stock.currentfinancial_estgrossprofit.q4;
                data.currentfinancial_estnetprofit_q1 = model.stock.currentfinancial_estnetprofit.q1;
                data.currentfinancial_estnetprofit_q2 = model.stock.currentfinancial_estnetprofit.q2;
                data.currentfinancial_estnetprofit_q3 = model.stock.currentfinancial_estnetprofit.q3;
                data.currentfinancial_estnetprofit_q4 = model.stock.currentfinancial_estnetprofit.q4;
                data.workingcapotalratio_ratio = model.stock.workingcapotalratio.ratio;
                data.workingcapotalratio_analysisisgood = model.stock.workingcapotalratio.analysis_isgood;
                data.quickratio_ratio = model.stock.quickratio.ratio;
                data.quickratio_analysisisgood = model.stock.quickratio.analysis_isgood;
                data.earningpershareratio_ratio = model.stock.earningpershareratio.ratio;
                data.earningpershareratio_analysisisgood = model.stock.earningpershareratio.analysis_isgood;
                data.priceearninsratio_ratio = model.stock.priceearninsratio.ratio;
                data.priceearninsratio_analysisisgood = model.stock.priceearninsratio.analysis_isgood;
                data.earningpersdebttoequityratio_ratio = model.stock.earningpersdebttoequityratio.ratio;
                data.earningpersdebttoequityratio_analysisisgood = model.stock.earningpersdebttoequityratio.analysis_isgood;
                data.returnonequityratio_ratio = model.stock.returnonequityratio.ratio;
                data.returnonequityratio_analysisisgood = model.stock.returnonequityratio.analysis_isgood;

                _Context.Stocks.Update(data);


                if (model.stacks_old != null && model.stacks_old.Count > 0)
                {
                    List<StockFlexibleBlock> flexibleBlocklist = new List<StockFlexibleBlock>();
                    List<StockCountriesData> countriesDatalist = new List<StockCountriesData>();
                    List<StockFirstCountryData> firstCountryDatalist = new List<StockFirstCountryData>();
                    List<StockSecondCountryData> secondCountryDatalist = new List<StockSecondCountryData>();


                    List<StockFlexibleBlock> _StockFlexibleBlocks = await _Context.StockFlexibleBlocks.Where(x => x.stockid == data.id).ToListAsync();
                    _Context.StockFlexibleBlocks.RemoveRange(_StockFlexibleBlocks);

                    foreach (StockFlexibleBlockDto flex in model.stacks_old)
                    {
                        StockFlexibleBlock FlexibleBlock = new StockFlexibleBlock();
                        FlexibleBlock.id = Guid.NewGuid();
                        FlexibleBlock.stockid = data.id;
                        FlexibleBlock.maintitle = flex.maintitle;
                        FlexibleBlock.oneyeardescription = flex.oneyeardescription;
                        FlexibleBlock.oneyeardescriptionfilename = flex.oneyeardescriptionfilename;
                        FlexibleBlock.oneyeardescriptionfilepath = flex.oneyeardescriptionfilepath;
                        FlexibleBlock.oneyeardescriptionfileurl = flex.oneyeardescriptionfileurl;
                        FlexibleBlock.oneyeardescriptionfilecontenttype = flex.oneyeardescriptionfilecontenttype;
                        FlexibleBlock.chartdescription = flex.chartdescription;
                        FlexibleBlock.chartdescriptionfilename = flex.chartdescriptionfilename;
                        FlexibleBlock.chartdescriptionfilepath = flex.chartdescriptionfilepath;
                        FlexibleBlock.chartdescriptionfileurl = flex.chartdescriptionfileurl;
                        FlexibleBlock.chartdescriptionfilecontenttype = flex.chartdescriptionfilecontenttype;
                        FlexibleBlock.firstcountryheading = flex.firstcountryheading;
                        FlexibleBlock.firstcountrydescription = flex.firstcountrydescription;
                        FlexibleBlock.firstcountrydescriptionfilename = flex.firstcountrydescriptionfilename;
                        FlexibleBlock.firstcountrydescriptionfilepath = flex.firstcountrydescriptionfilepath;
                        FlexibleBlock.firstcountrydescriptionfileurl = flex.firstcountrydescriptionfileurl;
                        FlexibleBlock.firstcountrydescriptionfilecontenttype = flex.firstcountrydescriptionfilecontenttype;
                        FlexibleBlock.secondcountryheading = flex.secondcountryheading;
                        FlexibleBlock.secondcountrydescription = flex.secondcountrydescription;
                        FlexibleBlock.secondcountrydescriptionfilename = flex.secondcountrydescriptionfilename;
                        FlexibleBlock.secondcountrydescriptionfilepath = flex.secondcountrydescriptionfilepath;
                        FlexibleBlock.secondcountrydescriptionfileurl = flex.secondcountrydescriptionfileurl;
                        FlexibleBlock.secondcountrydescriptionfilecontenttype = flex.secondcountrydescriptionfilecontenttype;
                        FlexibleBlock.bottomdescription = flex.bottomdescription;
                        FlexibleBlock.bottomdescriptionfilename = flex.bottomdescriptionfilename;
                        FlexibleBlock.bottomdescriptionfilepath = flex.bottomdescriptionfilepath;
                        FlexibleBlock.bottomdescriptionfileurl = flex.bottomdescriptionfileurl;
                        FlexibleBlock.bottomdescriptionfilecontenttype = flex.bottomdescriptionfilecontenttype;
                        FlexibleBlock.maindescription = flex.maindescription;
                        FlexibleBlock.maindescriptionfilename = flex.maindescriptionfilename;
                        FlexibleBlock.maindescriptionfilepath = flex.maindescriptionfilepath;
                        FlexibleBlock.maindescriptionfileurl = flex.maindescriptionfileurl;
                        FlexibleBlock.maindescriptionfilecontenttype = flex.maindescriptionfilecontenttype;
                        FlexibleBlock.singlepagechartimage = flex.singlepagechartimage;

                        flexibleBlocklist.Add(FlexibleBlock);
                    }
                    await _Context.StockFlexibleBlocks.AddRangeAsync(flexibleBlocklist);
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

    }
}
