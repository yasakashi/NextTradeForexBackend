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
                    tags = model.tags,
                    fundamentalandtechnicaltabsection_privatenotes = model.fundamentalandtechnicaltabsection.privatenotes,
                    fundamentalandtechnicaltabsection_instrumentname = model.fundamentalandtechnicaltabsection.instrumentname,
                    fundamentalandtechnicaltabsection_fundamentalheading = model.fundamentalandtechnicaltabsection.fundamentalheading,
                    fundamentalandtechnicaltabsection_technicalheading = model.fundamentalandtechnicaltabsection.technicalheading,
                    fundamentalandtechnicaltabsection_marketsentimentstitle = model.fundamentalandtechnicaltabsection.marketsentimentstitle,
                    fundamentalandtechnicaltabsection_marketsentimentsscript = model.fundamentalandtechnicaltabsection.marketsentimentsscript,
                    fundamentalandtechnicaltabsection_relatedresorces = model.fundamentalandtechnicaltabsection.relatedresorces,
                    discussion_allowcomments = model.discussion_allowcomments,
                    sendtrackbacks = model.sendtrackbacks,
                    discussion_allowtrackbacksandpingbacks = model.discussion_allowtrackbacksandpingbacks,
                    fundamentalandtechnicaltabsection_marketsessionscript = model.fundamentalandtechnicaltabsection.marketsentimentsscript,
                    fundamentalandtechnicaltabsection_marketsessiontitle = model.fundamentalandtechnicaltabsection.marketsentimentstitle
                };
                await _Context.ForexCharts.AddAsync(data);

                if (model.fundamentalandtechnicaltabsection != null)
                {
                    List<ForexChartFundamentalNewsSection> NewsSectioList = new List<ForexChartFundamentalNewsSection>();
                    List<ForexChartNewsMainContent> NewsMainContetList = new List<ForexChartNewsMainContent>();
                    foreach (ForexChartFundamentalNewsSectionDto news in model.fundamentalandtechnicaltabsection.fundamentalnewssectionlist)
                    {
                        ForexChartFundamentalNewsSection newssection = new ForexChartFundamentalNewsSection()
                        {
                            id = Guid.NewGuid(),
                            forexchartid = data.id,
                            maintitle = news.maintitle,
                            script = news.script
                        };

                        foreach (ForexChartNewsMainContentDto newsmaincontent in news.newsmaincontentlist)
                        {
                            NewsMainContetList.Add(new ForexChartNewsMainContent()
                            {
                                id = Guid.NewGuid(),
                                forexchartid = data.id,
                                fundamentalnewssectionid = newssection.id,
                                forexchartflexibleblockid = newssection.id,
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

                    await _Context.ForexChartFundamentalNewsSections.AddRangeAsync(NewsSectioList);
                    await _Context.ForexChartNewsMainContents.AddRangeAsync(NewsMainContetList);

                    List<ForexChartTechnicalTab> technicaltabList = new List<ForexChartTechnicalTab>();
                    List<ForexChartTechnicalBreakingNews> technicalbreakList = new List<ForexChartTechnicalBreakingNews>();
                    foreach (ForexChartTechnicalTabDto tab in model.fundamentalandtechnicaltabsection.technicaltablist)
                    {
                        ForexChartTechnicalTab newtab = new ForexChartTechnicalTab()
                        {
                            id = Guid.NewGuid(),
                            forexchartid = data.id,
                            tabtitle = tab.tabtitle,
                            script = tab.script
                        };

                        foreach (ForexChartTechnicalBreakingNewsDto newsmaincontent in tab.newsmaincontentlist)
                        {
                            technicalbreakList.Add(new ForexChartTechnicalBreakingNews()
                            {
                                id = Guid.NewGuid(),
                                forexchartid = data.id,
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
                                forexchartid = data.id,
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
                                forexchartid = data.id,
                                url = pdf.url,
                                urltitle = pdf.urltitle
                            });
                        }
                        if (UrlSectionlist.Count > 0)
                            await _Context.ForexChartURLSections.AddRangeAsync(UrlSectionlist);
                    }

                }

                if (model.forexflexibleblocks != null && model.forexflexibleblocks.Count > 0)
                {

                    List<ForexChartFlexibleBlock> flexibleBlocklist = new List<ForexChartFlexibleBlock>();
                    List<ForexChartCountriesData> countriesDatalist = new List<ForexChartCountriesData>();
                    List<ForexChartFirstCountryData> firstCountryDatalist = new List<ForexChartFirstCountryData>();
                    List<ForexChartSecondCountryData> secondCountryDatalist = new List<ForexChartSecondCountryData>();

                    foreach (ForexChartFlexibleBlockDto flex in model.forexflexibleblocks)
                    {
                        ForexChartFlexibleBlock FlexibleBlock = new ForexChartFlexibleBlock();
                        FlexibleBlock.id = Guid.NewGuid();
                        FlexibleBlock.forexchartid = data.id;
                        FlexibleBlock.maintitle = flex.maintitle;
                        FlexibleBlock.forexoneyeardescription = flex.forexoneyeardescription;
                        FlexibleBlock.forexoneyeardescriptionfilename = flex.forexoneyeardescriptionfilename;
                        FlexibleBlock.forexoneyeardescriptionfilepath = flex.forexoneyeardescriptionfilepath;
                        FlexibleBlock.forexoneyeardescriptionfileurl = flex.forexoneyeardescriptionfileurl;
                        FlexibleBlock.forexoneyeardescriptionfilecontenttype = flex.forexoneyeardescriptionfilecontenttype;
                        FlexibleBlock.forexchartdescription = flex.forexchartdescription;
                        FlexibleBlock.forexchartdescriptionfilename = flex.forexchartdescriptionfilename;
                        FlexibleBlock.forexchartdescriptionfilepath = flex.forexchartdescriptionfilepath;
                        FlexibleBlock.forexchartdescriptionfileurl = flex.forexchartdescriptionfileurl;
                        FlexibleBlock.forexchartdescriptionfilecontenttype = flex.forexchartdescriptionfilecontenttype;
                        FlexibleBlock.forexfirstcountryheading = flex.forexfirstcountryheading;
                        FlexibleBlock.forexfirstcountrydescription = flex.forexfirstcountrydescription;
                        FlexibleBlock.forexfirstcountrydescriptionfilename = flex.forexfirstcountrydescriptionfilename;
                        FlexibleBlock.forexfirstcountrydescriptionfilepath = flex.forexfirstcountrydescriptionfilepath;
                        FlexibleBlock.forexfirstcountrydescriptionfileurl = flex.forexfirstcountrydescriptionfileurl;
                        FlexibleBlock.forexfirstcountrydescriptionfilecontenttype = flex.forexfirstcountrydescriptionfilecontenttype;
                        FlexibleBlock.forexsecondcountryheading = flex.forexsecondcountryheading;
                        FlexibleBlock.forexsecondcountrydescription = flex.forexsecondcountrydescription;
                        FlexibleBlock.forexsecondcountrydescriptionfilename = flex.forexsecondcountrydescriptionfilename;
                        FlexibleBlock.forexsecondcountrydescriptionfilepath = flex.forexsecondcountrydescriptionfilepath;
                        FlexibleBlock.forexsecondcountrydescriptionfileurl = flex.forexsecondcountrydescriptionfileurl;
                        FlexibleBlock.forexsecondcountrydescriptionfilecontenttype = flex.forexsecondcountrydescriptionfilecontenttype;
                        FlexibleBlock.forexbottomdescription = flex.forexbottomdescription;
                        //FlexibleBlock.forexbottomdescriptionfilename = flex.forexbottomdescriptionfilename;
                        //FlexibleBlock.forexbottomdescriptionfilepath = flex.forexbottomdescriptionfilepath;
                        //FlexibleBlock.forexbottomdescriptionfileurl = flex.forexbottomdescriptionfileurl;
                        //FlexibleBlock.forexbottomdescriptionfilecontenttype = flex.forexbottomdescriptionfilecontenttype;
                        FlexibleBlock.forexmaindescription = flex.forexmaindescription;
                        FlexibleBlock.forexmaindescriptionfilename = flex.forexmaindescriptionfilename;
                        FlexibleBlock.forexmaindescriptionfilepath = flex.forexmaindescriptionfilepath;
                        FlexibleBlock.forexmaindescriptionfileurl = flex.forexmaindescriptionfileurl;
                        FlexibleBlock.forexmaindescriptionfilecontenttype = flex.forexmaindescriptionfilecontenttype;
                        FlexibleBlock.forexsinglepagechartimage = flex.forexsinglepagechartimage;

                        flexibleBlocklist.Add(FlexibleBlock);

                        if (flex.countriesdatalist != null && flex.countriesdatalist.Count > 0)
                        {
                            foreach (ForexChartCountriesDataDto contrydata in flex.countriesdatalist)
                            {
                                countriesDatalist.Add(new ForexChartCountriesData()
                                {
                                    id = Guid.NewGuid(),
                                    forexchartid = data.id,
                                    forexchartflexibleblockid = FlexibleBlock.id,
                                    forexcontries = contrydata.forexcontries,
                                    forexpairsthatcorrelate = contrydata.forexpairsthatcorrelate,
                                    highsandlows = contrydata.highsandlows,
                                    forexpairtype = contrydata.forexpairtype,
                                    forexdailyaveragemovementinpair = contrydata.forexdailyaveragemovementinpair
                                });
                            }
                            await _Context.ForexChartCountriesDatas.AddRangeAsync(countriesDatalist);
                        }

                        if (flex.forexfirstcountrydatalist != null && flex.forexfirstcountrydatalist.Count > 0)
                        {
                            foreach (ForexChartFirstCountryDataDto contrydata in flex.forexfirstcountrydatalist)
                            {
                                firstCountryDatalist.Add(new ForexChartFirstCountryData()
                                {
                                    id = Guid.NewGuid(),
                                    forexchartid = data.id,
                                    forexchartflexibleblockid = FlexibleBlock.id,
                                    forexlcentralbank = contrydata.forexlcentralbank,
                                    forexlcontriy = contrydata.forexlcontriy,
                                    forexlnickname = contrydata.forexlnickname,
                                    forexlofaeragedailyturnover = contrydata.forexlofaeragedailyturnover
                                });
                            }
                            await _Context.ForexChartFirstCountryDatas.AddRangeAsync(firstCountryDatalist);
                        }

                        if (flex.forexsecondcountrydatalist != null && flex.forexsecondcountrydatalist.Count > 0)
                        {
                            foreach (ForexChartSecondCountryDataDto contrydata in flex.forexsecondcountrydatalist)
                            {
                                secondCountryDatalist.Add(new ForexChartSecondCountryData()
                                {
                                    id = Guid.NewGuid(),
                                    forexchartid = data.id,
                                    forexchartflexibleblockid = FlexibleBlock.id,
                                    forexrcontriy = contrydata.forexrcontriy,
                                    forexrcentralbank = contrydata.forexrcentralbank,
                                    forexrnickname = contrydata.forexrnickname,
                                    forexrofaeragedailyturnover = contrydata.forexrofaeragedailyturnover
                                });
                            }
                            await _Context.ForexChartSecondCountryDatas.AddRangeAsync(secondCountryDatalist);
                        }
                    }
                    await _Context.ForexChartFlexibleBlocks.AddRangeAsync(flexibleBlocklist);
                }

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
                                    discussion_allowcomments = x.discussion_allowcomments,
                                    discussion_allowtrackbacksandpingbacks = x.discussion_allowtrackbacksandpingbacks,
                                    sendtrackbacks = x.sendtrackbacks,
                                    fundamentalandtechnicaltabsection = new ForexChartFundamentalAndTechnicalTabSectionDto()
                                    {
                                        privatenotes = x.fundamentalandtechnicaltabsection_privatenotes,
                                        instrumentname = x.fundamentalandtechnicaltabsection_instrumentname,
                                        fundamentalheading = x.fundamentalandtechnicaltabsection_fundamentalheading,
                                        technicalheading = x.fundamentalandtechnicaltabsection_technicalheading,
                                        marketsentimentstitle = x.fundamentalandtechnicaltabsection_marketsentimentstitle,
                                        marketsentimentsscript = x.fundamentalandtechnicaltabsection_marketsentimentsscript,
                                        relatedresorces = x.fundamentalandtechnicaltabsection_relatedresorces,
                                        marketsessionscript = x.fundamentalandtechnicaltabsection_marketsessionscript,
                                        marketsessiontitle = x.fundamentalandtechnicaltabsection_marketsessiontitle,
                                    }
                                }).ToListAsync();

                foreach (ForexChartDto data in datas)
                {
                    data.fundamentalandtechnicaltabsection.urlsectionlist = await _Context.ForexChartURLSections.Where(x => x.forexchartid == data.id).Select(x => new ForexChartURLSectionDto()
                    {
                        id = x.id,
                        forexchartid = x.forexchartid,
                        url = x.url,
                        urltitle = x.urltitle
                    }).ToListAsync();

                    data.fundamentalandtechnicaltabsection.pdfsectionlist = await _Context.ForexChartPDFSections.Where(x => x.forexchartid == data.id).Select(x => new ForexChartPDFSectionDto()
                    {
                        id = x.id,
                        forexchartid = x.forexchartid,
                        author = x.author,
                        pdfshortcodeid = x.pdfshortcodeid,
                        pdftitle = x.pdftitle,
                        shortdescription = x.shortdescription
                    }).ToListAsync();


                    data.fundamentalandtechnicaltabsection.fundamentalnewssectionlist = await _Context.ForexChartFundamentalNewsSections.Where(x => x.forexchartid == data.id).Select(x => new ForexChartFundamentalNewsSectionDto() { id = x.id, forexchartid = x.forexchartid, maintitle = x.maintitle, script = x.script }).ToListAsync();

                    foreach (ForexChartFundamentalNewsSectionDto news in data.fundamentalandtechnicaltabsection.fundamentalnewssectionlist)
                    {
                        news.newsmaincontentlist = await _Context.ForexChartNewsMainContents.Where(x => x.forexchartid == data.id && x.fundamentalnewssectionid == news.id).Select(x => new ForexChartNewsMainContentDto()
                        {
                            id = x.id,
                            forexchartid = x.forexchartid,
                            fundamentalnewssectionid = x.fundamentalnewssectionid,
                            description = x.description,
                            descriptionfilecontenttype = x.descriptionfilecontenttype,
                            descriptionfilename = x.descriptionfilename,
                            descriptionfilepath = x.descriptionfilepath,
                            descriptionfileurl = x.descriptionfileurl,
                            link = x.link,
                            title = x.title
                        }).ToListAsync();
                    }

                    data.fundamentalandtechnicaltabsection.technicaltablist = await _Context.ForexChartTechnicalTabs.Where(x => x.forexchartid == data.id).Select(x => new ForexChartTechnicalTabDto() { id = x.id, forexchartid = x.forexchartid, tabtitle = x.tabtitle, script = x.script }).ToListAsync();

                    foreach (ForexChartTechnicalTabDto tab in data.fundamentalandtechnicaltabsection.technicaltablist)
                    {
                        tab.newsmaincontentlist = await _Context.ForexChartTechnicalBreakingNewss.Where(x => x.forexchartid == data.id && x.technicaltabid == tab.id).Select(x => new ForexChartTechnicalBreakingNewsDto()
                        {
                            id = x.id,
                            forexchartid = x.forexchartid,
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


                    data.forexflexibleblocks = await _Context.ForexChartFlexibleBlocks.Where(x => x.forexchartid == data.id).Select(x => new ForexChartFlexibleBlockDto()
                    {
                        id = x.id,
                        forexchartid = x.forexchartid,
                        maintitle = x.maintitle,
                        forexoneyeardescription = x.forexoneyeardescription,
                        forexoneyeardescriptionfilename = x.forexoneyeardescriptionfilename,
                        forexoneyeardescriptionfilepath = x.forexoneyeardescriptionfilepath,
                        forexoneyeardescriptionfileurl = x.forexoneyeardescriptionfileurl,
                        forexoneyeardescriptionfilecontenttype = x.forexoneyeardescriptionfilecontenttype,
                        forexchartdescription = x.forexchartdescription,
                        forexchartdescriptionfilename = x.forexchartdescriptionfilename,
                        forexchartdescriptionfilepath = x.forexchartdescriptionfilepath,
                        forexchartdescriptionfileurl = x.forexchartdescriptionfileurl,
                        forexchartdescriptionfilecontenttype = x.forexchartdescriptionfilecontenttype,
                        forexfirstcountryheading = x.forexfirstcountryheading,
                        forexfirstcountrydescription = x.forexfirstcountrydescription,
                        forexfirstcountrydescriptionfilename = x.forexfirstcountrydescriptionfilename,
                        forexfirstcountrydescriptionfilepath = x.forexfirstcountrydescriptionfilepath,
                        forexfirstcountrydescriptionfileurl = x.forexfirstcountrydescriptionfileurl,
                        forexfirstcountrydescriptionfilecontenttype = x.forexfirstcountrydescriptionfilecontenttype,
                        forexsecondcountryheading = x.forexsecondcountryheading,
                        forexsecondcountrydescription = x.forexsecondcountrydescription,
                        forexsecondcountrydescriptionfilename = x.forexsecondcountrydescriptionfilename,
                        forexsecondcountrydescriptionfilepath = x.forexsecondcountrydescriptionfilepath,
                        forexsecondcountrydescriptionfileurl = x.forexsecondcountrydescriptionfileurl,
                        forexsecondcountrydescriptionfilecontenttype = x.forexsecondcountrydescriptionfilecontenttype,
                        forexbottomdescription = x.forexbottomdescription,
                        forexmaindescription = x.forexmaindescription,
                        forexmaindescriptionfilename = x.forexmaindescriptionfilename,
                        forexmaindescriptionfilepath = x.forexmaindescriptionfilepath,
                        forexmaindescriptionfileurl = x.forexmaindescriptionfileurl,
                        forexmaindescriptionfilecontenttype = x.forexmaindescriptionfilecontenttype,
                        forexsinglepagechartimage = x.forexsinglepagechartimage
                    }).ToListAsync();

                    foreach (ForexChartFlexibleBlockDto item in data.forexflexibleblocks)
                    {
                        item.forexsecondcountrydatalist = await _Context.ForexChartSecondCountryDatas.Where(x => x.forexchartid == data.id && x.forexchartflexibleblockid == item.id).Select(x => new ForexChartSecondCountryDataDto()
                        {
                            id = x.id,
                            forexchartflexibleblockid = x.forexchartflexibleblockid,
                            forexchartid = x.forexchartid,
                            forexrcentralbank = x.forexrcentralbank,
                            forexrcontriy = x.forexrcontriy,
                            forexrnickname = x.forexrnickname,
                            forexrofaeragedailyturnover = x.forexrofaeragedailyturnover
                        }).ToListAsync();
                        item.forexfirstcountrydatalist = await _Context.ForexChartFirstCountryDatas.Where(x => x.forexchartid == data.id && x.forexchartflexibleblockid == item.id).Select(x => new ForexChartFirstCountryDataDto()
                        {
                            id = x.id,
                            forexchartflexibleblockid = x.forexchartflexibleblockid,
                            forexchartid = x.forexchartid,
                            forexlcentralbank = x.forexlcentralbank,
                            forexlcontriy = x.forexlcontriy,
                            forexlnickname = x.forexlnickname,
                            forexlofaeragedailyturnover = x.forexlofaeragedailyturnover
                        }).ToListAsync();
                        item.countriesdatalist = await _Context.ForexChartCountriesDatas.Where(x => x.forexchartid == data.id && x.forexchartflexibleblockid == item.id).Select(x => new ForexChartCountriesDataDto()
                        {
                            id = x.id,
                            forexchartflexibleblockid = x.forexchartflexibleblockid,
                            forexchartid = x.forexchartid,
                            forexcontries = x.forexcontries,
                            forexdailyaveragemovementinpair = x.forexdailyaveragemovementinpair,
                            forexpairsthatcorrelate = x.forexpairsthatcorrelate,
                            forexpairtype = x.forexpairtype,
                            highsandlows = x.highsandlows
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


                List<ForexChartFlexibleBlock> _ForexChartFlexibleBlocks = await _Context.ForexChartFlexibleBlocks.Where(x => x.forexchartid == data.id).ToListAsync();
                _Context.ForexChartFlexibleBlocks.RemoveRange(_ForexChartFlexibleBlocks);

                List<ForexChartCountriesData> _ForexChartCountriesDatas = await _Context.ForexChartCountriesDatas.Where(x => x.forexchartid == data.id).ToListAsync();
                _Context.ForexChartCountriesDatas.RemoveRange(_ForexChartCountriesDatas);

                List<ForexChartFirstCountryData> _ForexChartFirstCountryDataCountriesDatas = await _Context.ForexChartFirstCountryDatas.Where(x => x.forexchartid == data.id).ToListAsync();
                _Context.ForexChartFirstCountryDatas.RemoveRange(_ForexChartFirstCountryDataCountriesDatas);

                List<ForexChartSecondCountryData> _ForexChartSecondCountryDataCountriesDatas = await _Context.ForexChartSecondCountryDatas.Where(x => x.forexchartid == data.id).ToListAsync();
                _Context.ForexChartSecondCountryDatas.RemoveRange(_ForexChartSecondCountryDataCountriesDatas);

                List<ForexChartFundamentalNewsSection> _ForexChart_fundamentalandtechnicaltabsection_fundamentalnewssections = await _Context.ForexChartFundamentalNewsSections.Where(x => x.forexchartid == data.id).ToListAsync();
                _Context.ForexChartFundamentalNewsSections.RemoveRange(_ForexChart_fundamentalandtechnicaltabsection_fundamentalnewssections);

                List<ForexChartNewsMainContent> _ForexChart_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents = await _Context.ForexChartNewsMainContents.Where(x => x.forexchartid == data.id).ToListAsync();
                _Context.ForexChartNewsMainContents.RemoveRange(_ForexChart_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents);

                List<ForexChartTechnicalTab> _ForexChart_Fundamentalandtechnicaltabsection_TechnicalTabses = await _Context.ForexChartTechnicalTabs.Where(x => x.forexchartid == data.id).ToListAsync();
                _Context.ForexChartTechnicalTabs.RemoveRange(_ForexChart_Fundamentalandtechnicaltabsection_TechnicalTabses);

                List<ForexChartTechnicalBreakingNews> _ForexChart_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses = await _Context.ForexChartTechnicalBreakingNewss.Where(x => x.forexchartid == data.id).ToListAsync();
                _Context.ForexChartTechnicalBreakingNewss.RemoveRange(_ForexChart_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses);

                List<ForexChartPDFSection> _ForexChart_FundamentalandTechnicalTabSection_RelatedResorces_PDFSections = await _Context.ForexChartPDFSections.Where(x => x.forexchartid == data.id).ToListAsync();
                _Context.ForexChartPDFSections.RemoveRange(_ForexChart_FundamentalandTechnicalTabSection_RelatedResorces_PDFSections);

                List<ForexChartURLSection> _ForexChart_FundamentalandTechnicalTabSection_RelatedResorces_URLSectionsawait = await _Context.ForexChartURLSections.Where(x => x.forexchartid == data.id).ToListAsync();
                _Context.ForexChartURLSections.RemoveRange(_ForexChart_FundamentalandTechnicalTabSection_RelatedResorces_URLSectionsawait);


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
                data.categoryid = model.categoryid;
                data.title = model.title;
                data.tags = model.tags;
                data.authorname = model.authorname;
                data.excerpt = model.excerpt;
                data.authorid = model.authorid;
                data.isvisible = model.isvisible;
                data.courseleveltypeId = model.courseleveltypeId;
                data.coursestatusid = model.coursestatusid;
                data.creatoruserid = model.creatoruserid;
                data.createdatetime = model.createdatetime;
                data.changestatusdate = model.changestatusdate;
                data.sendtrackbacks = model.sendtrackbacks;
                data.discussion_allowcomments = model.discussion_allowcomments;
                data.discussion_allowtrackbacksandpingbacks = model.discussion_allowtrackbacksandpingbacks;
                data.fundamentalandtechnicaltabsection_instrumentname = model.fundamentalandtechnicaltabsection.instrumentname;
                data.fundamentalandtechnicaltabsection_fundamentalheading = model.fundamentalandtechnicaltabsection.fundamentalheading;
                data.fundamentalandtechnicaltabsection_technicalheading = model.fundamentalandtechnicaltabsection.technicalheading;
                data.fundamentalandtechnicaltabsection_marketsessiontitle = model.fundamentalandtechnicaltabsection.marketsessiontitle;
                data.fundamentalandtechnicaltabsection_marketsessionscript = model.fundamentalandtechnicaltabsection.marketsessionscript;
                data.fundamentalandtechnicaltabsection_marketsentimentstitle = model.fundamentalandtechnicaltabsection.marketsentimentstitle;
                data.fundamentalandtechnicaltabsection_marketsentimentsscript = model.fundamentalandtechnicaltabsection.marketsentimentsscript;
                data.fundamentalandtechnicaltabsection_relatedresorces = model.fundamentalandtechnicaltabsection.relatedresorces;
                data.fundamentalandtechnicaltabsection_privatenotes = model.fundamentalandtechnicaltabsection.privatenotes;

                _Context.ForexCharts.Update(data);

                if (model.forexflexibleblocks != null && model.forexflexibleblocks.Count > 0)
                {
                    List<ForexChartFlexibleBlock> flexibleBlocklist = new List<ForexChartFlexibleBlock>();
                    List<ForexChartCountriesData> countriesDatalist = new List<ForexChartCountriesData>();
                    List<ForexChartFirstCountryData> firstCountryDatalist = new List<ForexChartFirstCountryData>();
                    List<ForexChartSecondCountryData> secondCountryDatalist = new List<ForexChartSecondCountryData>();


                    List<ForexChartFlexibleBlock> _ForexChartFlexibleBlocks = await _Context.ForexChartFlexibleBlocks.Where(x => x.forexchartid == data.id).ToListAsync();
                    _Context.ForexChartFlexibleBlocks.RemoveRange(_ForexChartFlexibleBlocks);

                    foreach (ForexChartFlexibleBlockDto flex in model.forexflexibleblocks)
                    {
                        ForexChartFlexibleBlock FlexibleBlock = new ForexChartFlexibleBlock();
                        FlexibleBlock.id = Guid.NewGuid();
                        FlexibleBlock.forexchartid = data.id;
                        FlexibleBlock.maintitle = flex.maintitle;
                        FlexibleBlock.forexoneyeardescription = flex.forexoneyeardescription;
                        FlexibleBlock.forexoneyeardescriptionfilename = flex.forexoneyeardescriptionfilename;
                        FlexibleBlock.forexoneyeardescriptionfilepath = flex.forexoneyeardescriptionfilepath;
                        FlexibleBlock.forexoneyeardescriptionfileurl = flex.forexoneyeardescriptionfileurl;
                        FlexibleBlock.forexoneyeardescriptionfilecontenttype = flex.forexoneyeardescriptionfilecontenttype;
                        FlexibleBlock.forexchartdescription = flex.forexchartdescription;
                        FlexibleBlock.forexchartdescriptionfilename = flex.forexchartdescriptionfilename;
                        FlexibleBlock.forexchartdescriptionfilepath = flex.forexchartdescriptionfilepath;
                        FlexibleBlock.forexchartdescriptionfileurl = flex.forexchartdescriptionfileurl;
                        FlexibleBlock.forexchartdescriptionfilecontenttype = flex.forexchartdescriptionfilecontenttype;
                        FlexibleBlock.forexfirstcountryheading = flex.forexfirstcountryheading;
                        FlexibleBlock.forexfirstcountrydescription = flex.forexfirstcountrydescription;
                        FlexibleBlock.forexfirstcountrydescriptionfilename = flex.forexfirstcountrydescriptionfilename;
                        FlexibleBlock.forexfirstcountrydescriptionfilepath = flex.forexfirstcountrydescriptionfilepath;
                        FlexibleBlock.forexfirstcountrydescriptionfileurl = flex.forexfirstcountrydescriptionfileurl;
                        FlexibleBlock.forexfirstcountrydescriptionfilecontenttype = flex.forexfirstcountrydescriptionfilecontenttype;
                        FlexibleBlock.forexsecondcountryheading = flex.forexsecondcountryheading;
                        FlexibleBlock.forexsecondcountrydescription = flex.forexsecondcountrydescription;
                        FlexibleBlock.forexsecondcountrydescriptionfilename = flex.forexsecondcountrydescriptionfilename;
                        FlexibleBlock.forexsecondcountrydescriptionfilepath = flex.forexsecondcountrydescriptionfilepath;
                        FlexibleBlock.forexsecondcountrydescriptionfileurl = flex.forexsecondcountrydescriptionfileurl;
                        FlexibleBlock.forexsecondcountrydescriptionfilecontenttype = flex.forexsecondcountrydescriptionfilecontenttype;
                        FlexibleBlock.forexbottomdescription = flex.forexbottomdescription;
                        FlexibleBlock.forexmaindescription = flex.forexmaindescription;
                        FlexibleBlock.forexmaindescriptionfilename = flex.forexmaindescriptionfilename;
                        FlexibleBlock.forexmaindescriptionfilepath = flex.forexmaindescriptionfilepath;
                        FlexibleBlock.forexmaindescriptionfileurl = flex.forexmaindescriptionfileurl;
                        FlexibleBlock.forexmaindescriptionfilecontenttype = flex.forexmaindescriptionfilecontenttype;
                        FlexibleBlock.forexsinglepagechartimage = flex.forexsinglepagechartimage;

                        flexibleBlocklist.Add(FlexibleBlock);

                        if (flex.countriesdatalist != null && flex.countriesdatalist.Count > 0)
                        {
                            List<ForexChartCountriesData> _ForexChartCountriesDatas = await _Context.ForexChartCountriesDatas.Where(x => x.forexchartid == data.id).ToListAsync();
                            _Context.ForexChartCountriesDatas.RemoveRange(_ForexChartCountriesDatas);

                            foreach (ForexChartCountriesDataDto contrydata in flex.countriesdatalist)
                            {
                                countriesDatalist.Add(new ForexChartCountriesData()
                                {
                                    id = Guid.NewGuid(),
                                    forexchartid = data.id,
                                    forexchartflexibleblockid = FlexibleBlock.id,
                                    forexcontries = contrydata.forexcontries,
                                    forexpairsthatcorrelate = contrydata.forexpairsthatcorrelate,
                                    highsandlows = contrydata.highsandlows,
                                    forexpairtype = contrydata.forexpairtype,
                                    forexdailyaveragemovementinpair = contrydata.forexdailyaveragemovementinpair
                                });
                            }
                            await _Context.ForexChartCountriesDatas.AddRangeAsync(countriesDatalist);
                        }

                        if (flex.forexfirstcountrydatalist != null && flex.forexfirstcountrydatalist.Count > 0)
                        {
                            List<ForexChartFirstCountryData> _ForexChartFirstCountryDataCountriesDatas = await _Context.ForexChartFirstCountryDatas.Where(x => x.forexchartid == data.id).ToListAsync();
                            _Context.ForexChartFirstCountryDatas.RemoveRange(_ForexChartFirstCountryDataCountriesDatas);
                            foreach (ForexChartFirstCountryDataDto contrydata in flex.forexfirstcountrydatalist)
                            {
                                firstCountryDatalist.Add(new ForexChartFirstCountryData()
                                {
                                    id = Guid.NewGuid(),
                                    forexchartid = data.id,
                                    forexchartflexibleblockid = FlexibleBlock.id,
                                    forexlcentralbank = contrydata.forexlcentralbank,
                                    forexlcontriy = contrydata.forexlcontriy,
                                    forexlnickname = contrydata.forexlnickname,
                                    forexlofaeragedailyturnover = contrydata.forexlofaeragedailyturnover
                                });
                            }
                            await _Context.ForexChartFirstCountryDatas.AddRangeAsync(firstCountryDatalist);
                        }

                        if (flex.forexsecondcountrydatalist != null && flex.forexsecondcountrydatalist.Count > 0)
                        {
                            List<ForexChartSecondCountryData> _ForexChartSecondCountryDataCountriesDatas = await _Context.ForexChartSecondCountryDatas.Where(x => x.forexchartid == data.id).ToListAsync();
                            _Context.ForexChartSecondCountryDatas.RemoveRange(_ForexChartSecondCountryDataCountriesDatas);
                            foreach (ForexChartSecondCountryDataDto contrydata in flex.forexsecondcountrydatalist)
                            {
                                secondCountryDatalist.Add(new ForexChartSecondCountryData()
                                {
                                    id = Guid.NewGuid(),
                                    forexchartid = data.id,
                                    forexchartflexibleblockid = FlexibleBlock.id,
                                    forexrcontriy = contrydata.forexrcontriy,
                                    forexrcentralbank = contrydata.forexrcentralbank,
                                    forexrnickname = contrydata.forexrnickname,
                                    forexrofaeragedailyturnover = contrydata.forexrofaeragedailyturnover
                                });
                            }
                            await _Context.ForexChartSecondCountryDatas.AddRangeAsync(secondCountryDatalist);
                        }
                    }
                    await _Context.ForexChartFlexibleBlocks.AddRangeAsync(flexibleBlocklist);
                }


                if (model.fundamentalandtechnicaltabsection != null)
                {
                    if (model.fundamentalandtechnicaltabsection.fundamentalnewssectionlist != null && model.fundamentalandtechnicaltabsection.fundamentalnewssectionlist.Count > 0)
                    {
                        List<ForexChartFundamentalNewsSection> NewsSectioList = new List<ForexChartFundamentalNewsSection>();
                        List<ForexChartNewsMainContent> NewsMainContetList = new List<ForexChartNewsMainContent>();

                        List<ForexChartFundamentalNewsSection> _ForexChart_fundamentalandtechnicaltabsection_fundamentalnewssections = await _Context.ForexChartFundamentalNewsSections.Where(x => x.forexchartid == data.id).ToListAsync();
                        _Context.ForexChartFundamentalNewsSections.RemoveRange(_ForexChart_fundamentalandtechnicaltabsection_fundamentalnewssections);

                        List<ForexChartNewsMainContent> _ForexChart_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents = await _Context.ForexChartNewsMainContents.Where(x => x.forexchartid == data.id).ToListAsync();
                        _Context.ForexChartNewsMainContents.RemoveRange(_ForexChart_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents);


                        foreach (ForexChartFundamentalNewsSectionDto news in model.fundamentalandtechnicaltabsection.fundamentalnewssectionlist)
                        {
                            ForexChartFundamentalNewsSection newssection = new ForexChartFundamentalNewsSection()
                            {
                                id = Guid.NewGuid(),
                                forexchartid = data.id,
                                maintitle = news.maintitle,
                                script = news.script
                            };

                            foreach (ForexChartNewsMainContentDto newsmaincontent in news.newsmaincontentlist)
                            {
                                NewsMainContetList.Add(new ForexChartNewsMainContent()
                                {
                                    id = Guid.NewGuid(),
                                    forexchartid = data.id,
                                    fundamentalnewssectionid = newssection.id,
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

                        await _Context.ForexChartFundamentalNewsSections.AddRangeAsync(NewsSectioList);
                        await _Context.ForexChartNewsMainContents.AddRangeAsync(NewsMainContetList);
                    }

                    if (model.fundamentalandtechnicaltabsection.technicaltablist != null && model.fundamentalandtechnicaltabsection.technicaltablist.Count > 0)
                    {
                        List<ForexChartTechnicalTab> _ForexChart_Fundamentalandtechnicaltabsection_TechnicalTabses = await _Context.ForexChartTechnicalTabs.Where(x => x.forexchartid == data.id).ToListAsync();
                        _Context.ForexChartTechnicalTabs.RemoveRange(_ForexChart_Fundamentalandtechnicaltabsection_TechnicalTabses);

                        List<ForexChartTechnicalBreakingNews> _ForexChart_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses = await _Context.ForexChartTechnicalBreakingNewss.Where(x => x.forexchartid == data.id).ToListAsync();
                        _Context.ForexChartTechnicalBreakingNewss.RemoveRange(_ForexChart_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses);


                        List<ForexChartTechnicalTab> technicaltabList = new List<ForexChartTechnicalTab>();
                        List<ForexChartTechnicalBreakingNews> technicalbreakList = new List<ForexChartTechnicalBreakingNews>();
                        foreach (ForexChartTechnicalTabDto tab in model.fundamentalandtechnicaltabsection.technicaltablist)
                        {
                            ForexChartTechnicalTab newtab = new ForexChartTechnicalTab()
                            {
                                id = Guid.NewGuid(),
                                forexchartid = data.id,
                                tabtitle = tab.tabtitle,
                                script = tab.script
                            };

                            foreach (ForexChartTechnicalBreakingNewsDto newsmaincontent in tab.newsmaincontentlist)
                            {
                                technicalbreakList.Add(new ForexChartTechnicalBreakingNews()
                                {
                                    id = Guid.NewGuid(),
                                    forexchartid = data.id,
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
                        List<ForexChartPDFSection> _ForexChart_FundamentalandTechnicalTabSection_RelatedResorces_PDFSections = await _Context.ForexChartPDFSections.Where(x => x.forexchartid == data.id).ToListAsync();
                        _Context.ForexChartPDFSections.RemoveRange(_ForexChart_FundamentalandTechnicalTabSection_RelatedResorces_PDFSections);

                        List<ForexChartPDFSection> PDFSectionlist = new List<ForexChartPDFSection>();
                        foreach (ForexChartPDFSectionDto pdf in model.fundamentalandtechnicaltabsection.pdfsectionlist)
                        {
                            PDFSectionlist.Add(new ForexChartPDFSection()
                            {
                                id = Guid.NewGuid(),
                                forexchartid = data.id,
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
                        List<ForexChartURLSection> _ForexChart_FundamentalandTechnicalTabSection_RelatedResorces_URLSectionsawait = await _Context.ForexChartURLSections.Where(x => x.forexchartid == data.id).ToListAsync();
                        _Context.ForexChartURLSections.RemoveRange(_ForexChart_FundamentalandTechnicalTabSection_RelatedResorces_URLSectionsawait);


                        List<ForexChartURLSection> UrlSectionlist = new List<ForexChartURLSection>();
                        foreach (ForexChartURLSectionDto pdf in model.fundamentalandtechnicaltabsection.urlsectionlist)
                        {
                            UrlSectionlist.Add(new ForexChartURLSection()
                            {
                                id = Guid.NewGuid(),
                                forexchartid = data.id,
                                url = pdf.url,
                                urltitle = pdf.urltitle
                            });
                        }
                        if (UrlSectionlist.Count > 0)
                            await _Context.ForexChartURLSections.AddRangeAsync(UrlSectionlist);
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
