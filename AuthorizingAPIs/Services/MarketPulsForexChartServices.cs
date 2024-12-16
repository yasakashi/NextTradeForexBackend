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
    public class MarketPulsForexChartServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;
        public MarketPulsForexChartServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
        }

        public async Task<SystemMessageModel> SaveForexChartItem(ForexChartDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (await _Context.ForexCharts.Where(x => x.categoryid == model.categoryid).AnyAsync())
                {
                    return new SystemMessageModel() { MessageCode = -103, MessageDescription = "For this category save before use update service", MessageData = model };
                }

                ForexChart data = new ForexChart()
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
                    ForexChartsection_newstickernew = model.ForexChart.newstickernew,
                    ForexChartsection_newstickerupdate = model.ForexChart.newstickerupdate,
                    ForexChartsection_newstickerimportant = model.ForexChart.newstickerimportant,
                    ForexChartsection_established = model.ForexChart.established,
                    ForexChartsection_exchange = model.ForexChart.exchange,
                    ForexChartsection_companytype = model.ForexChart.companytype,
                    ForexChartsection_ownership = model.ForexChart.ownership,
                    ForexChartsection_mainofficecountr = model.ForexChart.mainofficecountr,
                    ForexChartsection_url = model.ForexChart.url,
                    ForexChartsection_totalbranches = model.ForexChart.totalbranches,
                    ForexChartsection_otherimportantlocation = model.ForexChart.otherimportantlocation,
                    ForexChartsection_overalllocations = model.ForexChart.overalllocations,
                    ForexChartsection_servicesoffered = model.ForexChart.servicesoffered,
                    ForexChartsection_marketfocus = model.ForexChart.marketfocus,
                    ForexChartsection_briefdescriptionofcompany = model.ForexChart.briefdescriptionofcompany,
                    ForexChartsection_importantresearchnotes = model.ForexChart.importantresearchnotes,
                    ForexChartsection_chart = model.ForexChart.chart,
                    ForexChartsection_briefdescriptionofratio = model.ForexChart.briefdescriptionofratio,
                    financialdata_estturnoverus_year1 = model.ForexChart.financialdata_estturnoverus.year1,
                    financialdata_estturnoverus_year2 = model.ForexChart.financialdata_estturnoverus.year2,
                    financialdata_estturnoverus_year3 = model.ForexChart.financialdata_estturnoverus.year3,
                    financialdata_estturnoverus_year4 = model.ForexChart.financialdata_estturnoverus.year4,
                    financialdata_estturnoverus_year5 = model.ForexChart.financialdata_estturnoverus.year5,
                    financialdata_estgrossprofit_year1 = model.ForexChart.financialdata_estgrossprofit.year1,
                    financialdata_estgrossprofit_year2 = model.ForexChart.financialdata_estgrossprofit.year2,
                    financialdata_estgrossprofit_year3 = model.ForexChart.financialdata_estgrossprofit.year3,
                    financialdata_estgrossprofit_year4 = model.ForexChart.financialdata_estgrossprofit.year4,
                    financialdata_estgrossprofit_year5 = model.ForexChart.financialdata_estgrossprofit.year5,
                    financialdata_estnetprofit_year1 = model.ForexChart.financialdata_estnetprofit.year1,
                    financialdata_estnetprofit_year2 = model.ForexChart.financialdata_estnetprofit.year2,
                    financialdata_estnetprofit_year3 = model.ForexChart.financialdata_estnetprofit.year3,
                    financialdata_estnetprofit_year4 = model.ForexChart.financialdata_estnetprofit.year4,
                    financialdata_estnetprofit_year5 = model.ForexChart.financialdata_estnetprofit.year5,
                    currentfinancial_estturnoverus_q1 = model.ForexChart.currentfinancial_estturnoverus.q1,
                    currentfinancial_estturnoverus_q2 = model.ForexChart.currentfinancial_estturnoverus.q2,
                    currentfinancial_estturnoverus_q3 = model.ForexChart.currentfinancial_estturnoverus.q3,
                    currentfinancial_estturnoverus_q4 = model.ForexChart.currentfinancial_estturnoverus.q4,
                    currentfinancial_estgrossprofit_q1 = model.ForexChart.currentfinancial_estgrossprofit.q1,
                    currentfinancial_estgrossprofit_q2 = model.ForexChart.currentfinancial_estgrossprofit.q2,
                    currentfinancial_estgrossprofit_q3 = model.ForexChart.currentfinancial_estgrossprofit.q3,
                    currentfinancial_estgrossprofit_q4 = model.ForexChart.currentfinancial_estgrossprofit.q4,
                    currentfinancial_estnetprofit_q1 = model.ForexChart.currentfinancial_estnetprofit.q1,
                    currentfinancial_estnetprofit_q2 = model.ForexChart.currentfinancial_estnetprofit.q2,
                    currentfinancial_estnetprofit_q3 = model.ForexChart.currentfinancial_estnetprofit.q3,
                    currentfinancial_estnetprofit_q4 = model.ForexChart.currentfinancial_estnetprofit.q4,
                    workingcapotalratio_ratio = model.ForexChart.workingcapotalratio.ratio,
                    workingcapotalratio_analysisisgood = model.ForexChart.workingcapotalratio.analysis_isgood,
                    quickratio_ratio = model.ForexChart.quickratio.ratio,
                    quickratio_analysisisgood = model.ForexChart.quickratio.analysis_isgood,
                    earningpershareratio_ratio = model.ForexChart.earningpershareratio.ratio,
                    earningpershareratio_analysisisgood = model.ForexChart.earningpershareratio.analysis_isgood,
                    priceearninsratio_ratio = model.ForexChart.priceearninsratio.ratio,
                    priceearninsratio_analysisisgood = model.ForexChart.priceearninsratio.analysis_isgood,
                    earningpersdebttoequityratio_ratio = model.ForexChart.earningpersdebttoequityratio.ratio,
                    earningpersdebttoequityratio_analysisisgood = model.ForexChart.earningpersdebttoequityratio.analysis_isgood,
                    returnonequityratio_ratio = model.ForexChart.returnonequityratio.ratio,
                    returnonequityratio_analysisisgood = model.ForexChart.returnonequityratio.analysis_isgood,

                    tags = model.tags
                };
                await _Context.ForexCharts.AddAsync(data);
                if (model.fundamentalandtechnicaltabsection != null)
                {
                    List<ForexChartFundametalNewsSection> NewsSectioList = new List<ForexChartFundametalNewsSection>();
                    List<ForexChartNewsMainContent> NewsMainContetList = new List<ForexChartNewsMainContent>();
                    foreach (ForexChartFundametalNewsSectionDto news in model.fundamentalandtechnicaltabsection.fndamentalnewssectionlist)
                    {
                        ForexChartFundametalNewsSection newssection = new ForexChartFundametalNewsSection()
                        {
                            id = Guid.NewGuid(),
                            ForexChartid = data.id,
                            maintitle = news.maintitle,
                            script = news.script
                        };

                        foreach (ForexChartNewsMainContentDto newsmaincontent in news.newsmaincontentlist)
                        {
                            NewsMainContetList.Add(new ForexChartNewsMainContent()
                            {
                                id = Guid.NewGuid(),
                                ForexChartid = data.id,
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

                    await _Context.ForexChartFundametalNewsSections.AddRangeAsync(NewsSectioList);
                    await _Context.ForexChartNewsMainContents.AddRangeAsync(NewsMainContetList);

                    List<ForexChartTechnicalTab> technicaltabList = new List<ForexChartTechnicalTab>();
                    List<ForexChartTechnicalBreakingNews> technicalbreakList = new List<ForexChartTechnicalBreakingNews>();
                    foreach (ForexChartTechnicalTabsDto tab in model.fundamentalandtechnicaltabsection.technicaltablist)
                    {
                        ForexChartTechnicalTab newtab = new ForexChartTechnicalTab()
                        {
                            id = Guid.NewGuid(),
                            ForexChartid = data.id,
                            tabtitle = tab.tabtitle,
                            script = tab.script
                        };

                        foreach (ForexChartTechnicalBreakingNewsDto newsmaincontent in tab.newsmaincontentlist)
                        {
                            technicalbreakList.Add(new ForexChartTechnicalBreakingNews()
                            {
                                id = Guid.NewGuid(),
                                ForexChartid = data.id,
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

                    await _Context.ForexChartTechnicalTabs.AddRangeAsync(technicaltabList);
                    await _Context.ForexChartTechnicalBreakingNewss.AddRangeAsync(technicalbreakList);

                    List<ForexChartPDFSection> PDFSectionlist = new List<ForexChartPDFSection>();
                    if (model.fundamentalandtechnicaltabsection.pdfsectionlist != null && model.fundamentalandtechnicaltabsection.pdfsectionlist.Count > 0)
                    {
                        foreach (ForexChartPDFSectionDto pdf in model.fundamentalandtechnicaltabsection.pdfsectionlist)
                        {
                            PDFSectionlist.Add(new ForexChartPDFSection()
                            {
                                id = Guid.NewGuid(),
                                ForexChartid = data.id,
                                author = pdf.author,
                                pdfshortcodeid = pdf.pdfshortcodeid,
                                pdftitle = pdf.pdftitle,
                                shortdescription = pdf.shortdescription
                            });
                        }
                        if (PDFSectionlist.Count > 0)
                            await _Context.ForexChartPDFSections.AddRangeAsync(PDFSectionlist);
                    }
                    List<ForexChartURLSection> UrlSectionlist = new List<ForexChartURLSection>();
                    if (model.fundamentalandtechnicaltabsection.urlsectionlist != null && model.fundamentalandtechnicaltabsection.urlsectionlist.Count > 0)
                    {
                        foreach (ForexChartURLSectionDto pdf in model.fundamentalandtechnicaltabsection.urlsectionlist)
                        {
                            UrlSectionlist.Add(new ForexChartURLSection()
                            {
                                id = Guid.NewGuid(),
                                ForexChartid = data.id,
                                url = pdf.url,
                                urltitle = pdf.urltitle
                            });
                        }
                        if (UrlSectionlist.Count > 0)
                            await _Context.ForexChartURLSections.AddRangeAsync(UrlSectionlist);
                    }

                }

                if (model.ForexChart != null)
                {
                    if (model.ForexChart.productsservices != null && model.ForexChart.productsservices.Count > 0)
                    {
                        List<ForexChartProductAndService> ForexChartProductAndServiceList = new List<ForexChartProductAndService>();
                        foreach (ForexChartProductAndServiceDto item in model.ForexChart.productsservices)
                        {
                            ForexChartProductAndServiceList.Add(new ForexChartProductAndService()
                            {
                                id = Guid.NewGuid(),
                                ForexChartid = data.id,
                                productserviceratio = item.productserviceratio,
                                revenueetimate = item.revenueetimate
                            });
                        }
                        if (ForexChartProductAndServiceList.Count > 0)
                            await _Context.ForexChartProductAndServices.AddRangeAsync(ForexChartProductAndServiceList);
                    }
                    if (model.ForexChart.industryfocuslist != null && model.ForexChart.industryfocuslist.Count > 0)
                    {
                        List<ForexChartInductryFocus> ForexChartInductryFocusList = new List<ForexChartInductryFocus>();
                        foreach (ForexChartInductryFocusDto item in model.ForexChart.industryfocuslist)
                        {
                            ForexChartInductryFocusList.Add(new ForexChartInductryFocus()
                            {
                                id = Guid.NewGuid(),
                                ForexChartid = data.id,
                                clientnameifapplicable = item.clientnameifapplicable,
                                industryfocus = item.industryfocus,
                                revenueshare = item.revenueshare
                            });
                        }
                        if (ForexChartInductryFocusList.Count > 0)
                            await _Context.ForexChartInductryFocuss.AddRangeAsync(ForexChartInductryFocusList);
                    }
                    if (model.ForexChart.managementteam != null && model.ForexChart.managementteam.Count > 0)
                    {
                        List<ForexChartManagementTeam> ForexChartManagementTeamList = new List<ForexChartManagementTeam>();
                        foreach (ForexChartManagementTeamDto item in model.ForexChart.managementteam)
                        {
                            ForexChartManagementTeamList.Add(new ForexChartManagementTeam()
                            {
                                id = Guid.NewGuid(),
                                ForexChartid = data.id,
                                designation = item.designation,
                                name = item.name
                            });
                        }
                        if (ForexChartManagementTeamList.Count > 0)
                            await _Context.ForexChartManagementTeams.AddRangeAsync(ForexChartManagementTeamList);
                    }
                }

                List<ForexChartFlexibleBlock> flexibleBlocklist = new List<ForexChartFlexibleBlock>();
                List<ForexChartCountriesData> countriesDatalist = new List<ForexChartCountriesData>();
                List<ForexChartFirstCountryData> firstCountryDatalist = new List<ForexChartFirstCountryData>();
                List<ForexChartSecondCountryData> secondCountryDatalist = new List<ForexChartSecondCountryData>();

                foreach (ForexChartFlexibleBlockDto flex in model.stacks_old)
                {
                    ForexChartFlexibleBlock FlexibleBlock = new ForexChartFlexibleBlock();
                    FlexibleBlock.id = Guid.NewGuid();
                    FlexibleBlock.ForexChartid = data.id;
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
                        foreach (ForexChartCountriesDataDto contrydata in flex.countriesdatalist)
                        {
                            countriesDatalist.Add(new ForexChartCountriesData()
                            {
                                id = Guid.NewGuid(),
                                ForexChartid = data.id,
                                ForexChartflexibleblockid = FlexibleBlock.id,
                                countries = contrydata.countries,
                                pairthatcorrelate = contrydata.pairthatcorrelate,
                                highsandlows = contrydata.highsandlows,
                                pairtype = contrydata.pairtype,
                                dailyaveragmovementinpips = contrydata.dailyaveragmovementinpips
                            });
                        }
                        await _Context.ForexChartCountriesDatas.AddRangeAsync(countriesDatalist);
                    }

                    if (flex.firstcountrydatalist != null && flex.firstcountrydatalist.Count > 0)
                    {
                        foreach (ForexChartFirstCountryDataDto contrydata in flex.firstcountrydatalist)
                        {
                            firstCountryDatalist.Add(new ForexChartFirstCountryData()
                            {
                                id = Guid.NewGuid(),
                                ForexChartid = data.id,
                                ForexChartflexibleblockid = FlexibleBlock.id,
                                centeralbank = contrydata.centeralbank,
                                contry = contrydata.contry,
                                nickname = contrydata.nickname,
                                ofaveragedailyturnover = contrydata.ofaveragedailyturnover
                            });
                        }
                        await _Context.ForexChartFirstCountryDatas.AddRangeAsync(firstCountryDatalist);
                    }

                    if (flex.secondcountrydatalist != null && flex.secondcountrydatalist.Count > 0)
                    {
                        foreach (ForexChartSecondCountryDataDto contrydata in flex.secondcountrydatalist)
                        {
                            secondCountryDatalist.Add(new ForexChartSecondCountryData()
                            {
                                id = Guid.NewGuid(),
                                ForexChartid = data.id,
                                ForexChartflexibleblockid = FlexibleBlock.id,
                                centeralbank = contrydata.centeralbank,
                                contry = contrydata.contry,
                                nickname = contrydata.nickname,
                                ofaveragedailyturnover = contrydata.ofaveragedailyturnover
                            });
                        }
                        await _Context.ForexChartSecondCountryDatas.AddRangeAsync(secondCountryDatalist);
                    }
                }

                await _Context.ForexChartFlexibleBlocks.AddRangeAsync(flexibleBlocklist);

                await _Context.SaveChangesAsync();
                model.id = data.id;

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

        public async Task<SystemMessageModel> GetForexChartItems(ForexChartFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                List<ForexChartDto> datas;
                IQueryable<ForexChart> query = _Context.ForexCharts;

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
                                .Take(PageRowCount).Select(x => new ForexChartDto()
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
                                    fundamentalandtechnicaltabsection = new ForexChartFundamentalAndTechnicalTabSectionDto()
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
                                    ForexChart = new ForexChartSectionDto()
                                    {
                                        newstickernew = x.ForexChartsection_newstickernew,
                                        newstickerupdate = x.ForexChartsection_newstickerupdate,
                                        newstickerimportant = x.ForexChartsection_newstickerimportant,
                                        established = x.ForexChartsection_established,
                                        exchange = x.ForexChartsection_exchange,
                                        companytype = x.ForexChartsection_companytype,
                                        ownership = x.ForexChartsection_ownership,
                                        mainofficecountr = x.ForexChartsection_mainofficecountr,
                                        url = x.ForexChartsection_url,
                                        totalbranches = x.ForexChartsection_totalbranches,
                                        otherimportantlocation = x.ForexChartsection_otherimportantlocation,
                                        overalllocations = x.ForexChartsection_overalllocations,
                                        servicesoffered = x.ForexChartsection_servicesoffered,
                                        marketfocus = x.ForexChartsection_marketfocus,
                                        briefdescriptionofcompany = x.ForexChartsection_briefdescriptionofcompany,
                                        importantresearchnotes = x.ForexChartsection_importantresearchnotes,
                                        chart = x.ForexChartsection_chart,
                                        briefdescriptionofratio = x.ForexChartsection_briefdescriptionofratio,
                                        financialdata_estturnoverus = new ForexChartFinancialDataDto()
                                        {
                                            year1 = x.financialdata_estturnoverus_year1,
                                            year2 = x.financialdata_estturnoverus_year2,
                                            year3 = x.financialdata_estturnoverus_year3,
                                            year4 = x.financialdata_estturnoverus_year4,
                                            year5 = x.financialdata_estturnoverus_year5,
                                        },
                                        financialdata_estgrossprofit = new ForexChartFinancialDataDto()
                                        {
                                            year1 = x.financialdata_estgrossprofit_year1,
                                            year2 = x.financialdata_estgrossprofit_year2,
                                            year3 = x.financialdata_estgrossprofit_year3,
                                            year4 = x.financialdata_estgrossprofit_year4,
                                            year5 = x.financialdata_estgrossprofit_year5,
                                        },
                                        financialdata_estnetprofit = new ForexChartFinancialDataDto()
                                        {
                                            year1 = x.financialdata_estnetprofit_year1,
                                            year2 = x.financialdata_estnetprofit_year2,
                                            year3 = x.financialdata_estnetprofit_year3,
                                            year4 = x.financialdata_estnetprofit_year4,
                                            year5 = x.financialdata_estnetprofit_year5
                                        },
                                        currentfinancial_estturnoverus = new ForexChartCurrentFinancialYearDto()
                                        {
                                            q1 = x.currentfinancial_estturnoverus_q1,
                                            q2 = x.currentfinancial_estturnoverus_q2,
                                            q3 = x.currentfinancial_estturnoverus_q3,
                                            q4 = x.currentfinancial_estturnoverus_q4
                                        },
                                        currentfinancial_estgrossprofit = new ForexChartCurrentFinancialYearDto()
                                        {
                                            q1 = x.currentfinancial_estgrossprofit_q1,
                                            q2 = x.currentfinancial_estgrossprofit_q2,
                                            q3 = x.currentfinancial_estgrossprofit_q3,
                                            q4 = x.currentfinancial_estgrossprofit_q4
                                        },
                                        currentfinancial_estnetprofit = new ForexChartCurrentFinancialYearDto()
                                        {
                                            q1 = x.currentfinancial_estnetprofit_q1,
                                            q2 = x.currentfinancial_estnetprofit_q2,
                                            q3 = x.currentfinancial_estnetprofit_q3,
                                            q4 = x.currentfinancial_estnetprofit_q4
                                        },
                                        workingcapotalratio = new ForexChartFinancialRatiosDto()
                                        {
                                            ratio = x.workingcapotalratio_ratio,
                                            analysis_isgood = x.workingcapotalratio_analysisisgood
                                        },
                                        quickratio = new ForexChartFinancialRatiosDto()
                                        {
                                            ratio = x.quickratio_ratio,
                                            analysis_isgood = x.quickratio_analysisisgood
                                        },
                                        earningpershareratio = new ForexChartFinancialRatiosDto()
                                        {
                                            ratio = x.earningpershareratio_ratio,
                                            analysis_isgood = x.earningpershareratio_analysisisgood
                                        },
                                        priceearninsratio = new ForexChartFinancialRatiosDto()
                                        {
                                            ratio = x.priceearninsratio_ratio,
                                            analysis_isgood = x.priceearninsratio_analysisisgood
                                        },
                                        earningpersdebttoequityratio = new ForexChartFinancialRatiosDto()
                                        {
                                            ratio = x.earningpersdebttoequityratio_ratio,
                                            analysis_isgood = x.earningpersdebttoequityratio_analysisisgood
                                        },
                                        returnonequityratio = new ForexChartFinancialRatiosDto()
                                        {
                                            ratio = x.returnonequityratio_ratio,
                                            analysis_isgood = x.returnonequityratio_analysisisgood
                                        }
                                    }
                                }).ToListAsync();

                foreach (ForexChartDto data in datas)
                {
                    data.fundamentalandtechnicaltabsection.urlsectionlist = await _Context.ForexChartURLSections.Where(x => x.ForexChartid == data.id).Select(x => new ForexChartURLSectionDto()
                    {
                        id = x.id,
                        ForexChartid = x.ForexChartid,
                        url = x.url,
                        urltitle = x.urltitle
                    }).ToListAsync();

                    data.fundamentalandtechnicaltabsection.pdfsectionlist = await _Context.ForexChartPDFSections.Where(x => x.ForexChartid == data.id).Select(x => new ForexChartPDFSectionDto()
                    {
                        id = x.id,
                        ForexChartid = x.ForexChartid,
                        author = x.author,
                        pdfshortcodeid = x.pdfshortcodeid,
                        pdftitle = x.pdftitle,
                        shortdescription = x.shortdescription
                    }).ToListAsync();

                    data.fundamentalandtechnicaltabsection.fndamentalnewssectionlist = await _Context.ForexChartFundametalNewsSections.Where(x => x.ForexChartid == data.id).Select(x => new ForexChartFundametalNewsSectionDto() { id = x.id, ForexChartid = x.ForexChartid, maintitle = x.maintitle, script = x.script }).ToListAsync();

                    foreach (ForexChartFundametalNewsSectionDto news in data.fundamentalandtechnicaltabsection.fndamentalnewssectionlist)
                    {
                        news.newsmaincontentlist = await _Context.ForexChartNewsMainContents.Where(x => x.ForexChartid == data.id && x.fundamentalandnewssectionid == news.id).Select(x => new ForexChartNewsMainContentDto()
                        {
                            id = x.id,
                            ForexChartid = x.ForexChartid,
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

                    data.fundamentalandtechnicaltabsection.technicaltablist = await _Context.ForexChartTechnicalTabs.Where(x => x.ForexChartid == data.id).Select(x => new ForexChartTechnicalTabsDto() { id = x.id, ForexChartid = x.ForexChartid, tabtitle = x.tabtitle, script = x.script }).ToListAsync();

                    foreach (ForexChartTechnicalTabsDto tab in data.fundamentalandtechnicaltabsection.technicaltablist)
                    {
                        tab.newsmaincontentlist = await _Context.ForexChartTechnicalBreakingNewss.Where(x => x.ForexChartid == data.id && x.technicaltabid == tab.id).Select(x => new ForexChartTechnicalBreakingNewsDto()
                        {
                            id = x.id,
                            ForexChartid = x.ForexChartid,
                            technicaltabid = x.technicaltabid,
                            description = x.description,
                            descriptionfilecontenttype = x.descriptionfilecontenttype,
                            descriptionfilename = x.descriptionfilename,
                            descriptionfilepath = x.descriptionfilepath,
                            descriptionfileurl = x.descriptionfileurl,
                            link = x.link,
                            title = x.title
                        }).ToListAsync();
                    }

                    data.ForexChart.productsservices = await _Context.ForexChartProductAndServices.Where(x => x.ForexChartid == data.id).Select(x => new ForexChartProductAndServiceDto()
                    {
                        id = x.id,
                        ForexChartid = x.ForexChartid,
                        productserviceratio = x.productserviceratio,
                        revenueetimate = x.revenueetimate
                    }).ToListAsync();

                    data.ForexChart.industryfocuslist = await _Context.ForexChartInductryFocuss.Where(x => x.ForexChartid == data.id).Select(x => new ForexChartInductryFocusDto()
                    {
                        id = x.id,
                        ForexChartid = x.ForexChartid,
                        clientnameifapplicable = x.clientnameifapplicable,
                        industryfocus = x.industryfocus,
                        revenueshare = x.revenueshare
                    }).ToListAsync();

                    data.ForexChart.managementteam = await _Context.ForexChartManagementTeams.Where(x => x.ForexChartid == data.id).Select(x => new ForexChartManagementTeamDto()
                    {
                        id = x.id,
                        ForexChartid = x.ForexChartid,
                        designation = x.designation,
                        name = x.name
                    }).ToListAsync();

                    data.stacks_old = await _Context.ForexChartFlexibleBlocks.Where(x => x.ForexChartid == data.id).Select(x => new ForexChartFlexibleBlockDto()
                    {
                        id = x.id,
                        ForexChartid = x.ForexChartid,
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

                    foreach (ForexChartFlexibleBlockDto FlexibleBlockitem in data.stacks_old)
                    {
                        FlexibleBlockitem.countriesdatalist = await _Context.ForexChartCountriesDatas.Where(x => x.ForexChartid == data.id && x.ForexChartflexibleblockid == FlexibleBlockitem.id).Select(x => new ForexChartCountriesDataDto()
                        {
                            id = x.id,
                            ForexChartflexibleblockid = x.ForexChartflexibleblockid,
                            ForexChartid = x.ForexChartid,
                            pairthatcorrelate = x.pairthatcorrelate,
                            countries = x.countries,
                            highsandlows = x.highsandlows,
                            pairtype = x.pairtype,
                            dailyaveragmovementinpips = x.dailyaveragmovementinpips
                        }).ToListAsync();

                        FlexibleBlockitem.firstcountrydatalist = await _Context.ForexChartFirstCountryDatas.Where(x => x.ForexChartid == data.id && x.ForexChartflexibleblockid == FlexibleBlockitem.id).Select(x => new ForexChartFirstCountryDataDto()
                        {
                            id = x.id,
                            ForexChartflexibleblockid = x.ForexChartflexibleblockid,
                            ForexChartid = x.ForexChartid,
                            contry = x.contry,
                            centeralbank = x.centeralbank,
                            nickname = x.nickname,
                            ofaveragedailyturnover = x.ofaveragedailyturnover
                        }).ToListAsync();

                        FlexibleBlockitem.secondcountrydatalist = await _Context.ForexChartSecondCountryDatas.Where(x => x.ForexChartid == data.id && x.ForexChartflexibleblockid == FlexibleBlockitem.id).Select(x => new ForexChartSecondCountryDataDto()
                        {
                            id = x.id,
                            ForexChartflexibleblockid = x.ForexChartflexibleblockid,
                            ForexChartid = x.ForexChartid,
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

        public async Task<SystemMessageModel> DeleteForexChartItem(ForexChartFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                ForexChart data;
                IQueryable<ForexChart> query = _Context.ForexCharts;

                if (model.id != null)
                    query = query.Where(x => x.id == model.id);

                if (model.categoryid != null)
                    query = query.Where(x => x.categoryid == model.categoryid);

                data = await query.FirstOrDefaultAsync();

                _Context.ForexCharts.Remove(data);


                List<ForexChartFlexibleBlock> _ForexChartFlexibleBlocks = await _Context.ForexChartFlexibleBlocks.Where(x => x.ForexChartid == data.id).ToListAsync();
                _Context.ForexChartFlexibleBlocks.RemoveRange(_ForexChartFlexibleBlocks);

                List<ForexChartCountriesData> _ForexChartCountriesDatas = await _Context.ForexChartCountriesDatas.Where(x => x.ForexChartid == data.id).ToListAsync();
                _Context.ForexChartCountriesDatas.RemoveRange(_ForexChartCountriesDatas);

                List<ForexChartFirstCountryData> _ForexChartFirstCountryDataCountriesDatas = await _Context.ForexChartFirstCountryDatas.Where(x => x.ForexChartid == data.id).ToListAsync();
                _Context.ForexChartFirstCountryDatas.RemoveRange(_ForexChartFirstCountryDataCountriesDatas);

                List<ForexChartSecondCountryData> _ForexChartSecondCountryDataCountriesDatas = await _Context.ForexChartSecondCountryDatas.Where(x => x.ForexChartid == data.id).ToListAsync();
                _Context.ForexChartSecondCountryDatas.RemoveRange(_ForexChartSecondCountryDataCountriesDatas);

                List<ForexChartFundametalNewsSection> _ForexChart_fundamentalandtechnicaltabsection_fundamentalnewssections = await _Context.ForexChartFundametalNewsSections.Where(x => x.ForexChartid == data.id).ToListAsync();
                _Context.ForexChartFundametalNewsSections.RemoveRange(_ForexChart_fundamentalandtechnicaltabsection_fundamentalnewssections);

                List<ForexChartNewsMainContent> _ForexChart_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents = await _Context.ForexChartNewsMainContents.Where(x => x.ForexChartid == data.id).ToListAsync();
                _Context.ForexChartNewsMainContents.RemoveRange(_ForexChart_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents);

                List<ForexChartTechnicalTab> _ForexChart_Fundamentalandtechnicaltabsection_TechnicalTabses = await _Context.ForexChartTechnicalTabs.Where(x => x.ForexChartid == data.id).ToListAsync();
                _Context.ForexChartTechnicalTabs.RemoveRange(_ForexChart_Fundamentalandtechnicaltabsection_TechnicalTabses);

                List<ForexChartTechnicalBreakingNews> _ForexChart_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses = await _Context.ForexChartTechnicalBreakingNewss.Where(x => x.ForexChartid == data.id).ToListAsync();
                _Context.ForexChartTechnicalBreakingNewss.RemoveRange(_ForexChart_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses);

                List<ForexChartPDFSection> _ForexChart_FundamentalandTechnicalTabSection_RelatedResorces_PDFSections = await _Context.ForexChartPDFSections.Where(x => x.ForexChartid == data.id).ToListAsync();
                _Context.ForexChartPDFSections.RemoveRange(_ForexChart_FundamentalandTechnicalTabSection_RelatedResorces_PDFSections);

                List<ForexChartURLSection> _ForexChart_FundamentalandTechnicalTabSection_RelatedResorces_URLSectionsawait = await _Context.ForexChartURLSections.Where(x => x.ForexChartid == data.id).ToListAsync();
                _Context.ForexChartURLSections.RemoveRange(_ForexChart_FundamentalandTechnicalTabSection_RelatedResorces_URLSectionsawait);

                List<ForexChartManagementTeam> ForexChartManagementTeamslist = await _Context.ForexChartManagementTeams.Where(x => x.ForexChartid == data.id).ToListAsync();
                _Context.ForexChartManagementTeams.RemoveRange(ForexChartManagementTeamslist);

                List<ForexChartInductryFocus> ForexChartInductryFocusslist = await _Context.ForexChartInductryFocuss.Where(x => x.ForexChartid == data.id).ToListAsync();
                _Context.ForexChartInductryFocuss.RemoveRange(ForexChartInductryFocusslist);

                List<ForexChartProductAndService> ForexChartProductAndServiceslist = await _Context.ForexChartProductAndServices.Where(x => x.ForexChartid == data.id).ToListAsync();
                _Context.ForexChartProductAndServices.RemoveRange(ForexChartProductAndServiceslist);

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


        public async Task<SystemMessageModel> UpdateForexChartItem(ForexChartDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.id == null)
                    return new SystemMessageModel() { MessageCode = -102, MessageDescription = "Id is Wrong" };

                ForexChart data = await _Context.ForexCharts.FindAsync(model.id);
                if (data == null)
                    return new SystemMessageModel() { MessageCode = -103, MessageDescription = "data not find" };

                if (data.categoryid != model.categoryid && await _Context.ForexCharts.AnyAsync(x => x.categoryid == model.categoryid))
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
                data.ForexChartsection_newstickernew = model.ForexChart.newstickernew;
                data.ForexChartsection_newstickerupdate = model.ForexChart.newstickerupdate;
                data.ForexChartsection_newstickerimportant = model.ForexChart.newstickerimportant;
                data.ForexChartsection_established = model.ForexChart.established;
                data.ForexChartsection_exchange = model.ForexChart.exchange;
                data.ForexChartsection_companytype = model.ForexChart.companytype;
                data.ForexChartsection_ownership = model.ForexChart.ownership;
                data.ForexChartsection_mainofficecountr = model.ForexChart.mainofficecountr;
                data.ForexChartsection_url = model.ForexChart.url;
                data.ForexChartsection_totalbranches = model.ForexChart.totalbranches;
                data.ForexChartsection_otherimportantlocation = model.ForexChart.otherimportantlocation;
                data.ForexChartsection_overalllocations = model.ForexChart.overalllocations;
                data.ForexChartsection_servicesoffered = model.ForexChart.servicesoffered;
                data.ForexChartsection_marketfocus = model.ForexChart.marketfocus;
                data.ForexChartsection_briefdescriptionofcompany = model.ForexChart.briefdescriptionofcompany;
                data.ForexChartsection_importantresearchnotes = model.ForexChart.importantresearchnotes;
                data.ForexChartsection_chart = model.ForexChart.chart;
                data.ForexChartsection_briefdescriptionofratio = model.ForexChart.briefdescriptionofratio;
                data.financialdata_estturnoverus_year1 = model.ForexChart.financialdata_estturnoverus.year1;
                data.financialdata_estturnoverus_year2 = model.ForexChart.financialdata_estturnoverus.year2;
                data.financialdata_estturnoverus_year3 = model.ForexChart.financialdata_estturnoverus.year3;
                data.financialdata_estturnoverus_year4 = model.ForexChart.financialdata_estturnoverus.year4;
                data.financialdata_estturnoverus_year5 = model.ForexChart.financialdata_estturnoverus.year5;
                data.financialdata_estgrossprofit_year1 = model.ForexChart.financialdata_estgrossprofit.year1;
                data.financialdata_estgrossprofit_year2 = model.ForexChart.financialdata_estgrossprofit.year2;
                data.financialdata_estgrossprofit_year3 = model.ForexChart.financialdata_estgrossprofit.year3;
                data.financialdata_estgrossprofit_year4 = model.ForexChart.financialdata_estgrossprofit.year4;
                data.financialdata_estgrossprofit_year5 = model.ForexChart.financialdata_estgrossprofit.year5;
                data.financialdata_estnetprofit_year1 = model.ForexChart.financialdata_estnetprofit.year1;
                data.financialdata_estnetprofit_year2 = model.ForexChart.financialdata_estnetprofit.year2;
                data.financialdata_estnetprofit_year3 = model.ForexChart.financialdata_estnetprofit.year3;
                data.financialdata_estnetprofit_year4 = model.ForexChart.financialdata_estnetprofit.year4;
                data.financialdata_estnetprofit_year5 = model.ForexChart.financialdata_estnetprofit.year5;
                data.currentfinancial_estturnoverus_q1 = model.ForexChart.currentfinancial_estturnoverus.q1;
                data.currentfinancial_estturnoverus_q2 = model.ForexChart.currentfinancial_estturnoverus.q2;
                data.currentfinancial_estturnoverus_q3 = model.ForexChart.currentfinancial_estturnoverus.q3;
                data.currentfinancial_estturnoverus_q4 = model.ForexChart.currentfinancial_estturnoverus.q4;
                data.currentfinancial_estgrossprofit_q1 = model.ForexChart.currentfinancial_estgrossprofit.q1;
                data.currentfinancial_estgrossprofit_q2 = model.ForexChart.currentfinancial_estgrossprofit.q2;
                data.currentfinancial_estgrossprofit_q3 = model.ForexChart.currentfinancial_estgrossprofit.q3;
                data.currentfinancial_estgrossprofit_q4 = model.ForexChart.currentfinancial_estgrossprofit.q4;
                data.currentfinancial_estnetprofit_q1 = model.ForexChart.currentfinancial_estnetprofit.q1;
                data.currentfinancial_estnetprofit_q2 = model.ForexChart.currentfinancial_estnetprofit.q2;
                data.currentfinancial_estnetprofit_q3 = model.ForexChart.currentfinancial_estnetprofit.q3;
                data.currentfinancial_estnetprofit_q4 = model.ForexChart.currentfinancial_estnetprofit.q4;
                data.workingcapotalratio_ratio = model.ForexChart.workingcapotalratio.ratio;
                data.workingcapotalratio_analysisisgood = model.ForexChart.workingcapotalratio.analysis_isgood;
                data.quickratio_ratio = model.ForexChart.quickratio.ratio;
                data.quickratio_analysisisgood = model.ForexChart.quickratio.analysis_isgood;
                data.earningpershareratio_ratio = model.ForexChart.earningpershareratio.ratio;
                data.earningpershareratio_analysisisgood = model.ForexChart.earningpershareratio.analysis_isgood;
                data.priceearninsratio_ratio = model.ForexChart.priceearninsratio.ratio;
                data.priceearninsratio_analysisisgood = model.ForexChart.priceearninsratio.analysis_isgood;
                data.earningpersdebttoequityratio_ratio = model.ForexChart.earningpersdebttoequityratio.ratio;
                data.earningpersdebttoequityratio_analysisisgood = model.ForexChart.earningpersdebttoequityratio.analysis_isgood;
                data.returnonequityratio_ratio = model.ForexChart.returnonequityratio.ratio;
                data.returnonequityratio_analysisisgood = model.ForexChart.returnonequityratio.analysis_isgood;

                _Context.ForexCharts.Update(data);
                if (model.stacks_old != null && model.stacks_old.Count > 0)
                {
                    List<ForexChartFlexibleBlock> flexibleBlocklist = new List<ForexChartFlexibleBlock>();
                    List<ForexChartCountriesData> countriesDatalist = new List<ForexChartCountriesData>();
                    List<ForexChartFirstCountryData> firstCountryDatalist = new List<ForexChartFirstCountryData>();
                    List<ForexChartSecondCountryData> secondCountryDatalist = new List<ForexChartSecondCountryData>();


                    List<ForexChartFlexibleBlock> _ForexChartFlexibleBlocks = await _Context.ForexChartFlexibleBlocks.Where(x => x.ForexChartid == data.id).ToListAsync();
                    _Context.ForexChartFlexibleBlocks.RemoveRange(_ForexChartFlexibleBlocks);

                    foreach (ForexChartFlexibleBlockDto flex in model.stacks_old)
                    {
                        ForexChartFlexibleBlock FlexibleBlock = new ForexChartFlexibleBlock();
                        FlexibleBlock.id = Guid.NewGuid();
                        FlexibleBlock.ForexChartid = data.id;
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
                            List<ForexChartCountriesData> _ForexChartCountriesDatas = await _Context.ForexChartCountriesDatas.Where(x => x.ForexChartid == data.id).ToListAsync();
                            _Context.ForexChartCountriesDatas.RemoveRange(_ForexChartCountriesDatas);

                            foreach (ForexChartCountriesDataDto contrydata in flex.countriesdatalist)
                            {
                                countriesDatalist.Add(new ForexChartCountriesData()
                                {
                                    id = Guid.NewGuid(),
                                    ForexChartid = data.id,
                                    ForexChartflexibleblockid = FlexibleBlock.id,
                                    countries = contrydata.countries,
                                    pairthatcorrelate = contrydata.pairthatcorrelate,
                                    highsandlows = contrydata.highsandlows,
                                    pairtype = contrydata.pairtype,
                                    dailyaveragmovementinpips = contrydata.dailyaveragmovementinpips
                                });
                            }
                            await _Context.ForexChartCountriesDatas.AddRangeAsync(countriesDatalist);
                        }

                        if (flex.firstcountrydatalist != null && flex.firstcountrydatalist.Count > 0)
                        {
                            List<ForexChartFirstCountryData> _ForexChartFirstCountryDataCountriesDatas = await _Context.ForexChartFirstCountryDatas.Where(x => x.ForexChartid == data.id).ToListAsync();
                            _Context.ForexChartFirstCountryDatas.RemoveRange(_ForexChartFirstCountryDataCountriesDatas);
                            foreach (ForexChartFirstCountryDataDto contrydata in flex.firstcountrydatalist)
                            {
                                firstCountryDatalist.Add(new ForexChartFirstCountryData()
                                {
                                    id = Guid.NewGuid(),
                                    ForexChartid = data.id,
                                    ForexChartflexibleblockid = FlexibleBlock.id,
                                    centeralbank = contrydata.centeralbank,
                                    contry = contrydata.contry,
                                    nickname = contrydata.nickname,
                                    ofaveragedailyturnover = contrydata.ofaveragedailyturnover
                                });
                            }
                            await _Context.ForexChartFirstCountryDatas.AddRangeAsync(firstCountryDatalist);
                        }

                        if (flex.secondcountrydatalist != null && flex.secondcountrydatalist.Count > 0)
                        {
                            List<ForexChartSecondCountryData> _ForexChartSecondCountryDataCountriesDatas = await _Context.ForexChartSecondCountryDatas.Where(x => x.ForexChartid == data.id).ToListAsync();
                            _Context.ForexChartSecondCountryDatas.RemoveRange(_ForexChartSecondCountryDataCountriesDatas);
                            foreach (ForexChartSecondCountryDataDto contrydata in flex.secondcountrydatalist)
                            {
                                secondCountryDatalist.Add(new ForexChartSecondCountryData()
                                {
                                    id = Guid.NewGuid(),
                                    ForexChartid = data.id,
                                    ForexChartflexibleblockid = FlexibleBlock.id,
                                    centeralbank = contrydata.centeralbank,
                                    contry = contrydata.contry,
                                    nickname = contrydata.nickname,
                                    ofaveragedailyturnover = contrydata.ofaveragedailyturnover
                                });
                            }
                            await _Context.ForexChartSecondCountryDatas.AddRangeAsync(secondCountryDatalist);
                        }
                    }
                    await _Context.ForexChartFlexibleBlocks.AddRangeAsync(flexibleBlocklist);
                }


                if (model.fundamentalandtechnicaltabsection != null)
                {
                    if (model.fundamentalandtechnicaltabsection.fndamentalnewssectionlist != null && model.fundamentalandtechnicaltabsection.fndamentalnewssectionlist.Count > 0)
                    {
                        List<ForexChartFundametalNewsSection> NewsSectioList = new List<ForexChartFundametalNewsSection>();
                        List<ForexChartNewsMainContent> NewsMainContetList = new List<ForexChartNewsMainContent>();

                        List<ForexChartFundametalNewsSection> _ForexChart_fundamentalandtechnicaltabsection_fundamentalnewssections = await _Context.ForexChartFundametalNewsSections.Where(x => x.ForexChartid == data.id).ToListAsync();
                        _Context.ForexChartFundametalNewsSections.RemoveRange(_ForexChart_fundamentalandtechnicaltabsection_fundamentalnewssections);

                        List<ForexChartNewsMainContent> _ForexChart_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents = await _Context.ForexChartNewsMainContents.Where(x => x.ForexChartid == data.id).ToListAsync();
                        _Context.ForexChartNewsMainContents.RemoveRange(_ForexChart_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents);


                        foreach (ForexChartFundametalNewsSectionDto news in model.fundamentalandtechnicaltabsection.fndamentalnewssectionlist)
                        {
                            ForexChartFundametalNewsSection newssection = new ForexChartFundametalNewsSection()
                            {
                                id = Guid.NewGuid(),
                                ForexChartid = data.id,
                                maintitle = news.maintitle,
                                script = news.script
                            };

                            foreach (ForexChartNewsMainContentDto newsmaincontent in news.newsmaincontentlist)
                            {
                                NewsMainContetList.Add(new ForexChartNewsMainContent()
                                {
                                    id = Guid.NewGuid(),
                                    ForexChartid = data.id,
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

                        await _Context.ForexChartFundametalNewsSections.AddRangeAsync(NewsSectioList);
                        await _Context.ForexChartNewsMainContents.AddRangeAsync(NewsMainContetList);
                    }

                    if (model.fundamentalandtechnicaltabsection.technicaltablist != null && model.fundamentalandtechnicaltabsection.technicaltablist.Count > 0)
                    {
                        List<ForexChartTechnicalTab> _ForexChart_Fundamentalandtechnicaltabsection_TechnicalTabses = await _Context.ForexChartTechnicalTabs.Where(x => x.ForexChartid == data.id).ToListAsync();
                        _Context.ForexChartTechnicalTabs.RemoveRange(_ForexChart_Fundamentalandtechnicaltabsection_TechnicalTabses);

                        List<ForexChartTechnicalBreakingNews> _ForexChart_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses = await _Context.ForexChartTechnicalBreakingNewss.Where(x => x.ForexChartid == data.id).ToListAsync();
                        _Context.ForexChartTechnicalBreakingNewss.RemoveRange(_ForexChart_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses);


                        List<ForexChartTechnicalTab> technicaltabList = new List<ForexChartTechnicalTab>();
                        List<ForexChartTechnicalBreakingNews> technicalbreakList = new List<ForexChartTechnicalBreakingNews>();
                        foreach (ForexChartTechnicalTabsDto tab in model.fundamentalandtechnicaltabsection.technicaltablist)
                        {
                            ForexChartTechnicalTab newtab = new ForexChartTechnicalTab()
                            {
                                id = Guid.NewGuid(),
                                ForexChartid = data.id,
                                tabtitle = tab.tabtitle,
                                script = tab.script
                            };

                            foreach (ForexChartTechnicalBreakingNewsDto newsmaincontent in tab.newsmaincontentlist)
                            {
                                technicalbreakList.Add(new ForexChartTechnicalBreakingNews()
                                {
                                    id = Guid.NewGuid(),
                                    ForexChartid = data.id,
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

                        await _Context.ForexChartTechnicalTabs.AddRangeAsync(technicaltabList);
                        await _Context.ForexChartTechnicalBreakingNewss.AddRangeAsync(technicalbreakList);
                    }


                    if (model.fundamentalandtechnicaltabsection.pdfsectionlist != null && model.fundamentalandtechnicaltabsection.pdfsectionlist.Count > 0)
                    {
                        List<ForexChartPDFSection> _ForexChart_FundamentalandTechnicalTabSection_RelatedResorces_PDFSections = await _Context.ForexChartPDFSections.Where(x => x.ForexChartid == data.id).ToListAsync();
                        _Context.ForexChartPDFSections.RemoveRange(_ForexChart_FundamentalandTechnicalTabSection_RelatedResorces_PDFSections);

                        List<ForexChartPDFSection> PDFSectionlist = new List<ForexChartPDFSection>();
                        foreach (ForexChartPDFSectionDto pdf in model.fundamentalandtechnicaltabsection.pdfsectionlist)
                        {
                            PDFSectionlist.Add(new ForexChartPDFSection()
                            {
                                id = Guid.NewGuid(),
                                ForexChartid = data.id,
                                author = pdf.author,
                                pdfshortcodeid = pdf.pdfshortcodeid,
                                pdftitle = pdf.pdftitle,
                                shortdescription = pdf.shortdescription
                            });
                        }
                        if (PDFSectionlist.Count > 0)
                            await _Context.ForexChartPDFSections.AddRangeAsync(PDFSectionlist);
                    }

                    if (model.fundamentalandtechnicaltabsection.urlsectionlist != null && model.fundamentalandtechnicaltabsection.urlsectionlist.Count > 0)
                    {
                        List<ForexChartURLSection> _ForexChart_FundamentalandTechnicalTabSection_RelatedResorces_URLSectionsawait = await _Context.ForexChartURLSections.Where(x => x.ForexChartid == data.id).ToListAsync();
                        _Context.ForexChartURLSections.RemoveRange(_ForexChart_FundamentalandTechnicalTabSection_RelatedResorces_URLSectionsawait);


                        List<ForexChartURLSection> UrlSectionlist = new List<ForexChartURLSection>();
                        foreach (ForexChartURLSectionDto pdf in model.fundamentalandtechnicaltabsection.urlsectionlist)
                        {
                            UrlSectionlist.Add(new ForexChartURLSection()
                            {
                                id = Guid.NewGuid(),
                                ForexChartid = data.id,
                                url = pdf.url,
                                urltitle = pdf.urltitle
                            });
                        }
                        if (UrlSectionlist.Count > 0)
                            await _Context.ForexChartURLSections.AddRangeAsync(UrlSectionlist);
                    }

                }

                if (model.ForexChart != null)
                {
                    if (model.ForexChart.productsservices != null && model.ForexChart.productsservices.Count > 0)
                    {
                        List<ForexChartProductAndService> ForexChartProductAndServiceslist = await _Context.ForexChartProductAndServices.Where(x => x.ForexChartid == data.id).ToListAsync();
                        _Context.ForexChartProductAndServices.RemoveRange(ForexChartProductAndServiceslist);

                        List<ForexChartProductAndService> ForexChartProductAndServiceList = new List<ForexChartProductAndService>();
                        foreach (ForexChartProductAndServiceDto item in model.ForexChart.productsservices)
                        {
                            ForexChartProductAndServiceList.Add(new ForexChartProductAndService()
                            {
                                id = Guid.NewGuid(),
                                ForexChartid = data.id,
                                productserviceratio = item.productserviceratio,
                                revenueetimate = item.revenueetimate
                            });
                        }
                        if (ForexChartProductAndServiceList.Count > 0)
                            await _Context.ForexChartProductAndServices.AddRangeAsync(ForexChartProductAndServiceList);
                    }


                    if (model.ForexChart.industryfocuslist != null && model.ForexChart.industryfocuslist.Count > 0)
                    {
                        List<ForexChartInductryFocus> ForexChartInductryFocusslist = await _Context.ForexChartInductryFocuss.Where(x => x.ForexChartid == data.id).ToListAsync();
                        _Context.ForexChartInductryFocuss.RemoveRange(ForexChartInductryFocusslist);


                        List<ForexChartInductryFocus> ForexChartInductryFocusList = new List<ForexChartInductryFocus>();
                        foreach (ForexChartInductryFocusDto item in model.ForexChart.industryfocuslist)
                        {
                            ForexChartInductryFocusList.Add(new ForexChartInductryFocus()
                            {
                                id = Guid.NewGuid(),
                                ForexChartid = data.id,
                                clientnameifapplicable = item.clientnameifapplicable,
                                industryfocus = item.industryfocus,
                                revenueshare = item.revenueshare
                            });
                        }
                        if (ForexChartInductryFocusList.Count > 0)
                            await _Context.ForexChartInductryFocuss.AddRangeAsync(ForexChartInductryFocusList);
                    }

                    if (model.ForexChart.managementteam != null && model.ForexChart.managementteam.Count > 0)
                    {
                        List<ForexChartManagementTeam> ForexChartManagementTeamslist = await _Context.ForexChartManagementTeams.Where(x => x.ForexChartid == data.id).ToListAsync();
                        _Context.ForexChartManagementTeams.RemoveRange(ForexChartManagementTeamslist);

                        List<ForexChartManagementTeam> ForexChartManagementTeamList = new List<ForexChartManagementTeam>();
                        foreach (ForexChartManagementTeamDto item in model.ForexChart.managementteam)
                        {
                            ForexChartManagementTeamList.Add(new ForexChartManagementTeam()
                            {
                                id = Guid.NewGuid(),
                                ForexChartid = data.id,
                                designation = item.designation,
                                name = item.name
                            });
                        }
                        if (ForexChartManagementTeamList.Count > 0)
                            await _Context.ForexChartManagementTeams.AddRangeAsync(ForexChartManagementTeamList);
                    }
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
