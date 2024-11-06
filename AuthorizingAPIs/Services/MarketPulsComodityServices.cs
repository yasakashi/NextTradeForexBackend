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


        public async Task<SystemMessageModel> SaveComodityItem(ComodityDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (await _Context.Comodities.Where(x => x.categoryid == model.categoryid).AnyAsync())
                    await DeleteComodityItem(new ComodityFilterDto() { id = model.id, categoryid = model.categoryid }, userlogin, processId, clientip, hosturl);

                Comodity data = new Comodity()
                {
                    id = Guid.NewGuid(),
                    categoryid = (long)model.categoryid,
                    creatoruserid = userlogin.userid,
                    isvisible = model.isvisible ?? true,
                    courseleveltypeId = model.courseleveltypeId ?? 0,
                    title = model.title,
                    oneyeardescription = model.oneyeardescription,
                    chartdescription = model.chartdescription,
                    firstcountryheading = model.firstcountryheading,
                    firstcountrydescription = model.firstcountrydescription,
                    secondcountryheading = model.secondcountryheading,
                    secondcountrydescription = model.secondcountrydescription,
                    bottomdescription = model.bottomdescription,
                    maindescription = model.maindescription,
                    singlepagechartimage = model.singlepagechartimage,
                    excerpt = model.excerpt,
                    authorid = model.authorid,
                    authorname =model.authorname,
                    createdatetime = DateTime.Now,
                    bottomdescriptionfilecontenttype = model.bottomdescriptionfilecontenttype,  
                    bottomdescriptionfilename= model.bottomdescriptionfilename,
                    bottomdescriptionfilepath= model.bottomdescriptionfilepath,
                    changestatusdate = DateTime.Now,
                    chartdescriptionfilecontenttype = model.chartdescriptionfilecontenttype,
                    chartdescriptionfilename = model.chartdescriptionfilename,
                    chartdescriptionfilepath = model.chartdescriptionfilepath,
                    comodities_maintitle = model.comodities_maintitle,
                    coursestatusid= (int)CourseStatusList.Draft,
                    firstcountrydescriptionfilecontenttype = model.firstcountrydescriptionfilecontenttype,
                    firstcountrydescriptionfilename = model.firstcountrydescriptionfilename,
                    firstcountrydescriptionfilepath = model.firstcountrydescriptionfilepath,
                    fundamentalandtechnicaltabsection_fundamentalheading = model.fundamentalandtechnicaltabsection_fundamentalheading,
                    fundamentalandtechnicaltabsection_instrumentname= model.fundamentalandtechnicaltabsection_instrumentname,
                    fundamentalandtechnicaltabsection_marketsentimentsscript = model.fundamentalandtechnicaltabsection_marketsentimentsscript,
                    fundamentalandtechnicaltabsection_marketsentimentstitle = model.fundamentalandtechnicaltabsection_marketsentimentstitle,
                    fundamentalandtechnicaltabsection_marketsessionscript = model.fundamentalandtechnicaltabsection_marketsessionscript,
                    fundamentalandtechnicaltabsection_marketsessiontitle = model.fundamentalandtechnicaltabsection_marketsessiontitle,
                    fundamentalandtechnicaltabsection_privatenotes = model.fundamentalandtechnicaltabsection_privatenotes,
                    fundamentalandtechnicaltabsection_relatedresorces = model.fundamentalandtechnicaltabsection_relatedresorces,
                    fundamentalandtechnicaltabsection_technicalheading = model.fundamentalandtechnicaltabsection_technicalheading,
                    maindescriptionfilecontenttype = model.maindescriptionfilecontenttype,  
                    maindescriptionfilename = model.maindescriptionfilename,
                    maindescriptionfilepath = model.maindescriptionfilepath,
                    oneyeardescriptionfilecontenttype = model.oneyeardescriptionfilecontenttype,
                    oneyeardescriptionfilename = model.oneyeardescriptionfilename,
                    oneyeardescriptionfilepath = model.oneyeardescriptionfilepath,
                    secondcountrydescriptionfilecontenttype = model.secondcountrydescriptionfilecontenttype,
                    secondcountrydescriptionfilename = model.secondcountrydescriptionfilename,
                    secondcountrydescriptionfilepath = model.secondcountrydescriptionfilepath
                };
                await _Context.Comodities.AddAsync(data);
/*

                if (model.Comodities_fundamentalandtechnicaltabsection_fundamentalnewssectionlist != null && model.Comodities_fundamentalandtechnicaltabsection_fundamentalnewssectionlist.Count() > 0)
                {
                    List<Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection> list = new List<Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection>();
                    foreach (Comodities_fundamentalandtechnicaltabsection_fundamentalnewssectionDto x in model.Comodities_fundamentalandtechnicaltabsection_fundamentalnewssectionlist)
                    {
                        list.Add(new Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection()
                        {
                            url = x.url,
                            urltitle = x.urltitle,
                            id = Guid.NewGuid(),
                            marketpulsforexid = data.id
                        });
                    }
                    await _Context.Comodities_fundamentalandtechnicaltabsection_fundamentalnewssections.AddRangeAsync(list);
                }
                if (model.TechnicalTabslist != null && model.TechnicalTabslist.Count() > 0)
                {
                    List<TechnicalTabs> TechnicalTabslist = new List<TechnicalTabs>();
                    foreach (TechnicalTabsDto x in model.TechnicalTabslist)
                    {
                        TechnicalTabs _technicalTabs = new TechnicalTabs()
                        {
                            maintitle = x.maintitle,
                            script = x.script,
                            id = Guid.NewGuid(),
                            marketpulsforexid = data.id
                        };

                        if (x.TechnicalBreakingNewslist != null && x.TechnicalBreakingNewslist.Count() > 0)
                        {
                            List<TechnicalBreakingNews> TechnicalBreakingNewslist = new List<TechnicalBreakingNews>();
                            foreach (TechnicalBreakingNewsDto xn in x.TechnicalBreakingNewslist)
                            {
                                TechnicalBreakingNewslist.Add(new TechnicalBreakingNews()
                                {
                                    maintitle = xn.maintitle,
                                    script = xn.script,
                                    id = Guid.NewGuid(),
                                    technicaltabid = _technicalTabs.id,
                                    marketpulsforexid = data.id
                                });
                            }
                            await _Context.TechnicalBreakingNewss.AddRangeAsync(TechnicalBreakingNewslist);
                        }
                        TechnicalTabslist.Add(_technicalTabs);

                    }
                    await _Context.TechnicalTabss.AddRangeAsync(TechnicalTabslist);
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


                if (model.FundamentalNewsSectionlist != null && model.FundamentalNewsSectionlist.Count() > 0)
                {
                    List<FundamentalNewsSection> FundamentalNewsSectionlist = new List<FundamentalNewsSection>();
                    foreach (FundamentalNewsSectionDto x in model.FundamentalNewsSectionlist)
                    {
                        FundamentalNewsSection _fundamentalNewsSection = new FundamentalNewsSection()
                        {
                            maintitle = x.maintitle,
                            script = x.script,
                            id = Guid.NewGuid(),
                            marketpulsforexid = data.id
                        };


                        if (x.NewsMainContentlist != null && x.NewsMainContentlist.Count() > 0)
                        {
                            List<NewsMainContent> NewsMainContentlist = new List<NewsMainContent>();
                            foreach (NewsMainContentDto xn in x.NewsMainContentlist)
                            {
                                NewsMainContentlist.Add(new NewsMainContent()
                                {
                                    maintitle = xn.maintitle,
                                    script = xn.script,
                                    id = Guid.NewGuid(),
                                    marketpulsforexid = data.id,
                                    fundamentalnewssectionid = _fundamentalNewsSection.id
                                });
                            }
                            await _Context.NewsMainContents.AddRangeAsync(NewsMainContentlist);
                        }
                        FundamentalNewsSectionlist.Add(_fundamentalNewsSection);
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
                */
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
