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


        public async Task<SystemMessageModel> SaveComodityItem(ComodityModel model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (await _Context.Comodities.Where(x => x.categoryid == model.categoryid).AnyAsync())
                {
                    return new SystemMessageModel() { MessageCode = -103, MessageDescription = "For this category save before use update service", MessageData = model };
                }

                Comodity data = new Comodity()
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
                    fundamentalandtechnicaltabsection_instrumentname = model.fundamentalandtechnicaltabsection.instrumentname,
                    fundamentalandtechnicaltabsection_fundamentalheading = model.fundamentalandtechnicaltabsection.fundamentalheading,
                    fundamentalandtechnicaltabsection_technicalheading = model.fundamentalandtechnicaltabsection.technicalheading,
                    fundamentalandtechnicaltabsection_marketsentimentstitle = model.fundamentalandtechnicaltabsection.marketsentimentstitle,
                    fundamentalandtechnicaltabsection_marketsentimentsscript = model.fundamentalandtechnicaltabsection.marketsentimentsscript,
                    fundamentalandtechnicaltabsection_marketsessiontitle = model.fundamentalandtechnicaltabsection.marketsessiontitle,
                    fundamentalandtechnicaltabsection_marketsessionscript = model.fundamentalandtechnicaltabsection.marketsessionscript,
                    fundamentalandtechnicaltabsection_relatedresorces = model.fundamentalandtechnicaltabsection.relatedresorces,
                    fundamentalandtechnicaltabsection_privatenotes = model.fundamentalandtechnicaltabsection.privatenotes,
                    tags = model.tags
                };
                await _Context.Comodities.AddAsync(data);

                List<Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSection> PDFSectionlist = new List<Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSection>();
                List<Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSection> UrlSectionlist = new List<Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSection>();
                if (model.fundamentalandtechnicaltabsection.pdfsectionlist != null && model.fundamentalandtechnicaltabsection.pdfsectionlist.Count > 0)
                {
                    foreach (Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSectionDto pdf in model.fundamentalandtechnicaltabsection.pdfsectionlist)
                    {
                        PDFSectionlist.Add(new Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSection()
                        {
                            id = Guid.NewGuid(),
                            comodityid = data.id,
                            author = pdf.author,
                            pdfshortcodeid = pdf.pdfshortcodeid,
                            pdftitle = pdf.pdftitle,
                            shortdescription = pdf.shortdescription
                        });
                    }
                    if (PDFSectionlist.Count > 0)
                        await _Context.Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSections.AddRangeAsync(PDFSectionlist);
                }
                if (model.fundamentalandtechnicaltabsection.urlsectionlist != null && model.fundamentalandtechnicaltabsection.urlsectionlist.Count > 0)
                {
                    foreach (Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSectionDto pdf in model.fundamentalandtechnicaltabsection.urlsectionlist)
                    {
                        UrlSectionlist.Add(new Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSection()
                        {
                            id = Guid.NewGuid(),
                            comodityid = data.id,
                            url = pdf.url,
                            urltitle = pdf.urltitle
                        });
                    }
                    if (UrlSectionlist.Count > 0)
                        await _Context.Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSections.AddRangeAsync(UrlSectionlist);
                }
                if (model.fundamentalandtechnicaltabsection.fundamentalnewssections != null && model.fundamentalandtechnicaltabsection.fundamentalnewssections.Count > 0)
                {
                    List<Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection> newssectionlist = new List<Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection>();
                    List<Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontent> newsmaincontentlist = new List<Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontent>();

                    foreach (Comodities_fundamentalandtechnicaltabsection_fundamentalnewssectionDto nitem in model.fundamentalandtechnicaltabsection.fundamentalnewssections)
                    {
                        Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection news = new Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection()
                        {
                            id = Guid.NewGuid(),
                            comodityid = data.id,
                            maintitle = nitem.maintitle,
                            script = nitem.script
                        };
                        if (nitem.newsmaincontentlist != null && nitem.newsmaincontentlist.Count > 0)
                        {
                            foreach (Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontentDto item in nitem.newsmaincontentlist)
                                newsmaincontentlist.Add(new Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontent()
                                {
                                    id = Guid.NewGuid(),
                                    comodityid = data.id,
                                    comodities_fundamentalandtechnicaltabsection_fundamentalnewssectionid = news.id,
                                    description = item.description,
                                    title = item.title,
                                    link = item.link,
                                    filecontenttype = item.filecontenttype,
                                    filename = item.filename,
                                    filepath = item.filepath
                                });
                        }
                        newssectionlist.Add(news);

                    }
                    if (newssectionlist.Count > 0)
                        await _Context.Comodities_fundamentalandtechnicaltabsection_fundamentalnewssections.AddRangeAsync(newssectionlist);
                    if (newsmaincontentlist.Count > 0)
                        await _Context.Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents.AddRangeAsync(newsmaincontentlist);
                }
                if (model.fundamentalandtechnicaltabsection.technicaltabs != null && model.fundamentalandtechnicaltabsection.technicaltabs.Count > 0)
                {
                    List<Comodities_Fundamentalandtechnicaltabsection_TechnicalTabs> technicaltablist = new List<Comodities_Fundamentalandtechnicaltabsection_TechnicalTabs>();
                    List<Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNews> technicalbreakingnewslist = new List<Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNews>();
                    foreach (Comodities_Fundamentalandtechnicaltabsection_TechnicalTabsDto tab in model.fundamentalandtechnicaltabsection.technicaltabs)
                    {
                        Comodities_Fundamentalandtechnicaltabsection_TechnicalTabs newtab = new Comodities_Fundamentalandtechnicaltabsection_TechnicalTabs()
                        {
                            id =Guid.NewGuid(),
                            comodityid = data.id,
                            script =tab.script,
                            tabtitle =tab.tabtitle
                        };

                        if (tab.technicalbreakingnewslist != null && tab.technicalbreakingnewslist.Count > 0)
                        {
                            foreach (Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewsDto breakingnews in  tab.technicalbreakingnewslist)
                            {
                                technicalbreakingnewslist.Add(new Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNews() {
                                    id = Guid.NewGuid(),
                                    comodityid = data.id,
                                    comoditiesfundamentalandtechnicaltabsectiontechnicaltabsid = newtab.id,
                                    description = breakingnews.description,
                                    filecontenttype = breakingnews.filecontenttype,
                                    filename = breakingnews.filename,
                                    filepath = breakingnews.filepath,
                                    link = breakingnews.link,
                                    title = breakingnews.title
                                });
                            }
                        }
                        technicaltablist.Add(newtab);
                    }

                    if (technicaltablist.Count > 0)
                        await _Context.Comodities_Fundamentalandtechnicaltabsection_TechnicalTabses.AddRangeAsync(technicaltablist);
                    if (technicalbreakingnewslist.Count > 0)
                        await _Context.Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses.AddRangeAsync(technicalbreakingnewslist);
                }


                List<Comodities_FlexibleBlock> flexibleBlocklist = new List<Comodities_FlexibleBlock>();
                List<ComoditiesCountriesData> countriesDatalist = new List<ComoditiesCountriesData>();
                List<ComoditiesFirstCountryDataCountriesData> firstcountriesDatalist = new List<ComoditiesFirstCountryDataCountriesData>();
                List<ComoditiesSecondCountryDataCountriesData> SecondcountriesDatalist = new List<ComoditiesSecondCountryDataCountriesData>();
                foreach (Comodities_FlexibleBlockDto flex in model.comodities)
                {
                    Comodities_FlexibleBlock FlexibleBlock = new Comodities_FlexibleBlock();
                    FlexibleBlock.id = Guid.NewGuid();
                    FlexibleBlock.bottomdescription = flex.bottomdescription;
                    FlexibleBlock.bottomdescriptionfilecontenttype = flex.bottomdescriptionfilecontenttype;
                    FlexibleBlock.bottomdescriptionfilename = flex.bottomdescriptionfilename;
                    FlexibleBlock.bottomdescriptionfilepath = flex.bottomdescriptionfilepath;
                    FlexibleBlock.bottomdescriptionfileurl = flex.bottomdescriptionfileurl;
                    FlexibleBlock.chartdescription = flex.chartdescription;
                    FlexibleBlock.chartdescriptionfilecontenttype = flex.chartdescriptionfilecontenttype;
                    FlexibleBlock.chartdescriptionfilename = flex.chartdescriptionfilename;
                    FlexibleBlock.chartdescriptionfilepath = flex.chartdescriptionfilepath;
                    FlexibleBlock.chartdescriptionfileurl = flex.chartdescriptionfileurl;
                    FlexibleBlock.comodityid = data.id;
                    FlexibleBlock.firstcontryheading = flex.firstcontryheading;
                    FlexibleBlock.firstcontrydescriptionfilename = flex.firstcontrydescriptionfilename;
                    FlexibleBlock.firstcontrydescriptionfilecontentype = flex.firstcontrydescriptionfilecontentype;
                    FlexibleBlock.firstcontrydescription = flex.firstcontrydescription;
                    FlexibleBlock.firstcontrydescriptionfilepath = flex.firstcontrydescriptionfilepath;
                    FlexibleBlock.firstcontrydescriptionfileurl = flex.firstcontrydescriptionfileurl;
                    FlexibleBlock.maindescrition = flex.maindescrition;
                    FlexibleBlock.maindescritionfilecontenttype = flex.maindescritionfilecontenttype;
                    FlexibleBlock.maindescritionfilename = flex.maindescritionfilename;
                    FlexibleBlock.maindescritionfilepath = flex.maindescritionfilepath;
                    FlexibleBlock.maindescritionfileurl = flex.maindescritionfileurl;
                    FlexibleBlock.oneyeardescription = flex.oneyeardescription;
                    FlexibleBlock.oneyeardescriptionfilecontenttype = flex.oneyeardescriptionfilecontenttype;
                    FlexibleBlock.oneyeardescriptionfilename = flex.oneyeardescriptionfilename;
                    FlexibleBlock.oneyeardescriptionfilepath = flex.oneyeardescriptionfilepath;
                    FlexibleBlock.oneyeardescriptionfileurl = flex.oneyeardescriptionfileurl;
                    FlexibleBlock.secoundcontrydescription = flex.secoundcontrydescription;
                    FlexibleBlock.maintitle = flex.maintitle;
                    FlexibleBlock.secondcountryheading = flex.secondcountryheading;
                    FlexibleBlock.secoundcontrydescriptionfilecontenttype = flex.secoundcontrydescriptionfilecontenttype;
                    FlexibleBlock.secoundcontrydescriptionfilename = flex.secoundcontrydescriptionfilename;
                    FlexibleBlock.secoundcontrydescriptionfilepath = flex.secoundcontrydescriptionfilepath;
                    FlexibleBlock.secoundcontrydescriptionfileurl = flex.secoundcontrydescriptionfileurl;
                    FlexibleBlock.singlepagechartimage = flex.singlepagechartimage;


                    if (flex.comoditiescountriesdatalist != null && flex.comoditiescountriesdatalist.Count > 0)
                    {
                        foreach (ComoditiesCountriesDataDto contrydata in flex.comoditiescountriesdatalist)
                        {
                            countriesDatalist.Add(new ComoditiesCountriesData()
                            {
                                id = Guid.NewGuid(),
                                comodityid = data.id,
                                comodityflexibleblockid = FlexibleBlock.id,
                                contries = contrydata.contries,
                                highslows = contrydata.highslows,
                                pairsthatcorrelate = contrydata.pairsthatcorrelate,
                                pairtype = contrydata.pairtype,
                                dailyaveragemovementinpips = contrydata.dailyaveragemovementinpips
                            });
                        }
                        await _Context.ComoditiesCountriesDatas.AddRangeAsync(countriesDatalist);
                    }
                    if (flex.comoditiesfirstcountrydatacountriesdatalist != null && flex.comoditiesfirstcountrydatacountriesdatalist.Count > 0)
                    {
                        foreach (ComoditiesFirstCountryDataCountriesDataDto contrydata in flex.comoditiesfirstcountrydatacountriesdatalist)
                        {
                            firstcountriesDatalist.Add(new ComoditiesFirstCountryDataCountriesData()
                            {
                                id = Guid.NewGuid(),
                                comodityid = data.id,
                                comodityflexibleblockid = FlexibleBlock.id,
                                centeralbank = contrydata.centeralbank,
                                contries = contrydata.contries,
                                nickname = contrydata.nickname,
                                ofaveragedailyturnover = contrydata.ofaveragedailyturnover
                            });
                        }
                        await _Context.ComoditiesFirstCountryDataCountriesDatas.AddRangeAsync(firstcountriesDatalist);
                    }
                    if (flex.comoditiessecondcountrydatacountriesdatalist != null && flex.comoditiessecondcountrydatacountriesdatalist.Count > 0)
                    {
                        foreach (ComoditiesSecondCountryDataCountriesDataDto contrydata in flex.comoditiessecondcountrydatacountriesdatalist)
                        {
                            SecondcountriesDatalist.Add(new ComoditiesSecondCountryDataCountriesData()
                            {
                                id = Guid.NewGuid(),
                                comodityid = data.id,
                                comodityflexibleblockid = FlexibleBlock.id,
                                centeralbank = contrydata.centeralbank,
                                contries = contrydata.contries,
                                nickname = contrydata.nickname,
                                ofaveragedailyturnover = contrydata.ofaveragedailyturnover
                            });
                        }
                        await _Context.ComoditiesSecondCountryDataCountriesDatas.AddRangeAsync(SecondcountriesDatalist);
                    }
                    //public List<>?  { get; set; }
                    flexibleBlocklist.Add(FlexibleBlock);
                }

                await _Context.ComoditiyFlexibleBlocks.AddRangeAsync(flexibleBlocklist);



                /*

                    bottomdescription = model.fundamentalandtechnicaltabsection.bottomdescription

                                    comodities_maintitle = model.comodities_maintitle,
                    oneyeardescription = model.oneyeardescription,
                    oneyeardescriptionfilename = model.oneyeardescriptionfilename,
                    oneyeardescriptionfilecontenttype = model.oneyeardescriptionfilecontenttype,
                    oneyeardescriptionfilepath = model.oneyeardescriptionfilepath,
                    chartdescription = model.chartdescription,
                    chartdescriptionfilename = model.chartdescriptionfilename,
                    chartdescriptionfilepath = model.chartdescriptionfilepath,
                    chartdescriptionfilecontenttype = model.chartdescriptionfilecontenttype,
                    firstcountryheading = model.firstcountryheading,
                    firstcountrydescription = model.firstcountrydescription,
                    firstcountrydescriptionfilename = model.firstcountrydescriptionfilename,
                    firstcountrydescriptionfilepath = model.firstcountrydescriptionfilepath,
                    firstcountrydescriptionfilecontenttype = model.firstcountrydescriptionfilecontenttype,
                    secondcountryheading = model.secondcountryheading,
                    secondcountrydescription = model.secondcountrydescription,
                    secondcountrydescriptionfilename = model.secondcountrydescriptionfilename,
                    secondcountrydescriptionfilepath = model.secondcountrydescriptionfilepath,
                    secondcountrydescriptionfilecontenttype = model.secondcountrydescriptionfilecontenttype,
                    bottomdescription = model.bottomdescription,
                    bottomdescriptionfilename = model.bottomdescriptionfilename,
                    bottomdescriptionfilepath = model.bottomdescriptionfilepath,
                    bottomdescriptionfilecontenttype = model.bottomdescriptionfilecontenttype,
                    maindescription = model.maindescription,
                    maindescriptionfilecontenttype = model.maindescriptionfilecontenttype,
                    maindescriptionfilename = model.maindescriptionfilename,
                    maindescriptionfilepath = model.maindescriptionfilepath,
                    singlepagechartimage = model.singlepagechartimage,



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

        public async Task<SystemMessageModel> GetComodityItems(ComodityFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                List<ComodityModel> datas;
                IQueryable<Comodity> query = _Context.Comodities;

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
                                .Take(PageRowCount).Select(x => new ComodityModel()
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
                                    fundamentalandtechnicaltabsection = new FundamentalAndTechnicalTabSection()
                                    {
                                        instrumentname = x.fundamentalandtechnicaltabsection_instrumentname,
                                        fundamentalheading = x.fundamentalandtechnicaltabsection_fundamentalheading,
                                        marketsentimentsscript = x.fundamentalandtechnicaltabsection_marketsentimentsscript,
                                        marketsessionscript = x.fundamentalandtechnicaltabsection_marketsessionscript,
                                        privatenotes = x.fundamentalandtechnicaltabsection_privatenotes,
                                        relatedresorces = x.fundamentalandtechnicaltabsection_relatedresorces,
                                        technicalheading = x.fundamentalandtechnicaltabsection_technicalheading,
                                        marketsentimentstitle = x.fundamentalandtechnicaltabsection_marketsessiontitle,
                                        marketsessiontitle = x.fundamentalandtechnicaltabsection_marketsessiontitle
                                    }
                                }).ToListAsync();

                foreach (ComodityModel data in datas)
                {
                    data.fundamentalandtechnicaltabsection.urlsectionlist = await _Context.Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSections.Where(x => x.comodityid == data.id).Select(x => new Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSectionDto()
                    {
                        id = x.id,
                        comodityid = x.comodityid,
                        url = x.url,
                        urltitle = x.urltitle
                    }).ToListAsync();

                    data.fundamentalandtechnicaltabsection.pdfsectionlist = await _Context.Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSections.Where(x => x.comodityid == data.id).Select(x => new Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSectionDto()
                    {
                        id = x.id,
                        comodityid = x.comodityid,
                        author = x.author,
                        pdfshortcodeid = x.pdfshortcodeid,
                        pdftitle = x.pdftitle,
                        shortdescription = x.shortdescription
                    }).ToListAsync();

                    data.fundamentalandtechnicaltabsection.technicaltabs = await _Context.Comodities_Fundamentalandtechnicaltabsection_TechnicalTabses.Where(x => x.comodityid == data.id).Select(x => new Comodities_Fundamentalandtechnicaltabsection_TechnicalTabsDto()
                    {
                        id = x.id,
                        comodityid = x.comodityid,
                        script = x.script,
                        tabtitle = x.tabtitle
                    }).ToListAsync();

                    foreach (Comodities_Fundamentalandtechnicaltabsection_TechnicalTabsDto tab in data.fundamentalandtechnicaltabsection.technicaltabs)
                    {
                        tab.technicalbreakingnewslist = await _Context.Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses.Where(x => x.comodityid == data.id && x.comoditiesfundamentalandtechnicaltabsectiontechnicaltabsid == tab.id).Select(x => new Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewsDto()
                        {
                            id = x.id,
                            comodityid = x.comodityid,
                            comoditiesfundamentalandtechnicaltabsectiontechnicaltabsid = x.comoditiesfundamentalandtechnicaltabsectiontechnicaltabsid,
                            description = x.description,
                            link = x.link,
                            title = x.title,
                            filecontenttype = x.filecontenttype,
                            filename = x.filename,
                            filepath = x.filepath
                        }).ToListAsync();
                    }

                    data.fundamentalandtechnicaltabsection.fundamentalnewssections = await _Context.Comodities_fundamentalandtechnicaltabsection_fundamentalnewssections.Where(x => x.comodityid == data.id).Select(x => new Comodities_fundamentalandtechnicaltabsection_fundamentalnewssectionDto()
                    {
                        id = x.id,
                        comodityid = x.comodityid,
                        script = x.script,
                        maintitle = x.maintitle
                    }).ToListAsync();

                    foreach (Comodities_fundamentalandtechnicaltabsection_fundamentalnewssectionDto news in data.fundamentalandtechnicaltabsection.fundamentalnewssections)
                    {
                        news.newsmaincontentlist = await _Context.Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents.Where(x => x.comodityid == data.id && x.comodities_fundamentalandtechnicaltabsection_fundamentalnewssectionid == news.id).Select(x => new Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontentDto()
                        {
                            id = x.id,
                            comodityid = x.comodityid,
                            comodities_fundamentalandtechnicaltabsection_fundamentalnewssectionid = x.comodities_fundamentalandtechnicaltabsection_fundamentalnewssectionid,
                            description = x.description,
                            link = x.link,
                            title = x.title,
                            filecontenttype = x.filecontenttype,
                            filename = x.filename,
                            filepath = x.filepath
                        }).ToListAsync();
                    }



                    data.comodities = await _Context.ComoditiyFlexibleBlocks.Where(x => x.comodityid == data.id).Select(x => new Comodities_FlexibleBlockDto()
                    {
                        id = x.id,
                        comodityid = x.comodityid,
                        bottomdescription = x.bottomdescription,
                        bottomdescriptionfilecontenttype = x.bottomdescriptionfilecontenttype,
                        bottomdescriptionfilename = x.bottomdescriptionfilename,
                        bottomdescriptionfilepath = x.bottomdescriptionfilepath,
                        bottomdescriptionfileurl = x.bottomdescriptionfileurl,
                        chartdescription = x.chartdescription,
                        chartdescriptionfilecontenttype = x.chartdescriptionfilecontenttype,
                        chartdescriptionfilename = x.chartdescriptionfilename,
                        chartdescriptionfilepath = x.chartdescriptionfilepath,
                        chartdescriptionfileurl = x.chartdescriptionfileurl,
                        firstcontrydescription = x.firstcontrydescription,
                        firstcontrydescriptionfilecontentype = x.firstcontrydescriptionfilecontentype,
                        firstcontrydescriptionfilename = x.firstcontrydescriptionfilename,
                        firstcontrydescriptionfilepath = x.firstcontrydescriptionfilepath,
                        firstcontrydescriptionfileurl = x.firstcontrydescriptionfileurl,
                        firstcontryheading = x.firstcontryheading,
                        maindescrition = x.maindescrition,
                        maindescritionfilename = x.maindescritionfilename,
                        maindescritionfilepath = x.maindescritionfilepath,
                        maindescritionfileurl = x.maindescritionfileurl,
                        maintitle = x.maintitle,
                        oneyeardescription = x.oneyeardescription,
                        maindescritionfilecontenttype = x.maindescritionfilecontenttype,
                        oneyeardescriptionfilename = x.oneyeardescriptionfilename,
                        oneyeardescriptionfilecontenttype = x.oneyeardescriptionfilecontenttype,
                        oneyeardescriptionfilepath = x.oneyeardescriptionfilepath,
                        oneyeardescriptionfileurl = x.oneyeardescriptionfileurl,
                        secondcountryheading = x.secondcountryheading,
                        secoundcontrydescription = x.secoundcontrydescription,
                        secoundcontrydescriptionfilecontenttype = x.secoundcontrydescriptionfilecontenttype,
                        secoundcontrydescriptionfilename = x.secoundcontrydescriptionfilename,
                        secoundcontrydescriptionfilepath = x.secoundcontrydescriptionfilepath,
                        secoundcontrydescriptionfileurl = x.secoundcontrydescriptionfileurl,
                        singlepagechartimage = x.singlepagechartimage
                    }).ToListAsync();

                    foreach (Comodities_FlexibleBlockDto FlexibleBlockitem in data.comodities)
                    {
                        FlexibleBlockitem.comoditiescountriesdatalist = await _Context.ComoditiesCountriesDatas.Where(x => x.comodityid == data.id && x.comodityflexibleblockid == FlexibleBlockitem.id).Select(x => new ComoditiesCountriesDataDto()
                        {
                            id = x.id,
                            comodityflexibleblockid = x.comodityflexibleblockid,
                            comodityid = x.comodityid,
                            contries = x.contries,
                            dailyaveragemovementinpips = x.dailyaveragemovementinpips,
                            highslows = x.highslows,
                            pairsthatcorrelate = x.pairsthatcorrelate,
                            pairtype = x.pairtype
                        }).ToListAsync();

                        FlexibleBlockitem.comoditiessecondcountrydatacountriesdatalist = await _Context.ComoditiesSecondCountryDataCountriesDatas.Where(x => x.comodityid == data.id && x.comodityflexibleblockid == FlexibleBlockitem.id).Select(x => new ComoditiesSecondCountryDataCountriesDataDto()
                        {
                            id = x.id,
                            comodityid = x.comodityid,
                            centeralbank = x.centeralbank,
                            nickname = x.nickname,
                            ofaveragedailyturnover = x.ofaveragedailyturnover,
                            contries = x.contries
                        }).ToListAsync();

                        FlexibleBlockitem.comoditiesfirstcountrydatacountriesdatalist = await _Context.ComoditiesFirstCountryDataCountriesDatas.Where(x => x.comodityid == data.id && x.comodityflexibleblockid == FlexibleBlockitem.id).Select(x => new ComoditiesFirstCountryDataCountriesDataDto()
                        {
                            id = x.id,
                            comodityid = x.comodityid,
                            centeralbank = x.centeralbank,
                            nickname = x.nickname,
                            ofaveragedailyturnover = x.ofaveragedailyturnover,
                            contries = x.contries
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


                List<Comodities_FlexibleBlock> _ComoditiyFlexibleBlocks = await _Context.ComoditiyFlexibleBlocks.Where(x => x.comodityid == data.id).ToListAsync();
                _Context.ComoditiyFlexibleBlocks.RemoveRange(_ComoditiyFlexibleBlocks);

                List<ComoditiesCountriesData> _ComoditiesCountriesDatas = await _Context.ComoditiesCountriesDatas.Where(x => x.comodityid == data.id).ToListAsync();
                _Context.ComoditiesCountriesDatas.RemoveRange(_ComoditiesCountriesDatas);

                List<ComoditiesFirstCountryDataCountriesData> _ComoditiesFirstCountryDataCountriesDatas = await _Context.ComoditiesFirstCountryDataCountriesDatas.Where(x => x.comodityid == data.id).ToListAsync();
                _Context.ComoditiesFirstCountryDataCountriesDatas.RemoveRange(_ComoditiesFirstCountryDataCountriesDatas);

                List<ComoditiesSecondCountryDataCountriesData> _ComoditiesSecondCountryDataCountriesDatas = await _Context.ComoditiesSecondCountryDataCountriesDatas.Where(x => x.comodityid == data.id).ToListAsync();
                _Context.ComoditiesSecondCountryDataCountriesDatas.RemoveRange(_ComoditiesSecondCountryDataCountriesDatas);

                List<Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection> _Comodities_fundamentalandtechnicaltabsection_fundamentalnewssections = await _Context.Comodities_fundamentalandtechnicaltabsection_fundamentalnewssections.Where(x => x.comodityid == data.id).ToListAsync();
                _Context.Comodities_fundamentalandtechnicaltabsection_fundamentalnewssections.RemoveRange(_Comodities_fundamentalandtechnicaltabsection_fundamentalnewssections);

                List<Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontent> _Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents = await _Context.Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents.Where(x => x.comodityid == data.id).ToListAsync();
                _Context.Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents.RemoveRange(_Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents);

                List<Comodities_Fundamentalandtechnicaltabsection_TechnicalTabs> _Comodities_Fundamentalandtechnicaltabsection_TechnicalTabses = await _Context.Comodities_Fundamentalandtechnicaltabsection_TechnicalTabses.Where(x => x.comodityid == data.id).ToListAsync();
                _Context.Comodities_Fundamentalandtechnicaltabsection_TechnicalTabses.RemoveRange(_Comodities_Fundamentalandtechnicaltabsection_TechnicalTabses);

                List<Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNews> _Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses = await _Context.Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses.Where(x => x.comodityid == data.id).ToListAsync();
                _Context.Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses.RemoveRange(_Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses);

                List<Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSection> _Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSections = await _Context.Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSections.Where(x => x.comodityid == data.id).ToListAsync();
                _Context.Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSections.RemoveRange(_Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSections);

                List<Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSection> _Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSectionsawait = await _Context.Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSections.Where(x => x.comodityid == data.id).ToListAsync();
                _Context.Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSections.RemoveRange(_Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSectionsawait);



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


        public async Task<SystemMessageModel> UpdateComodityItem(ComodityModel model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.id == null)
                    return new SystemMessageModel() { MessageCode = -102, MessageDescription = "Id is Wrong" };

                Comodity data = await _Context.Comodities.FindAsync(model.id);
                if (data == null)
                    return new SystemMessageModel() { MessageCode = -103, MessageDescription = "data not find" };

                if (data.categoryid != model.categoryid && await _Context.Comodities.AnyAsync(x => x.categoryid == model.categoryid))
                    return new SystemMessageModel() { MessageCode = -104, MessageDescription = "this category is exist" };

                data.categoryid = (long)model.categoryid;
                data.isvisible = model.isvisible;
                data.courseleveltypeId = model.courseleveltypeId;
                data.title = model.title;
                data.excerpt = model.excerpt;
                data.authorid = model.authorid;
                data.authorname = model.authorname;
                data.changestatusdate = DateTime.Now;
                data.coursestatusid = model.coursestatusid;
                data.fundamentalandtechnicaltabsection_instrumentname = model.fundamentalandtechnicaltabsection.instrumentname;
                data.fundamentalandtechnicaltabsection_fundamentalheading = model.fundamentalandtechnicaltabsection.fundamentalheading;
                data.fundamentalandtechnicaltabsection_technicalheading = model.fundamentalandtechnicaltabsection.technicalheading;
                data.fundamentalandtechnicaltabsection_marketsentimentstitle = model.fundamentalandtechnicaltabsection.marketsentimentstitle;
                data.fundamentalandtechnicaltabsection_marketsentimentsscript = model.fundamentalandtechnicaltabsection.marketsentimentsscript;
                data.fundamentalandtechnicaltabsection_marketsessiontitle = model.fundamentalandtechnicaltabsection.marketsessiontitle;
                data.fundamentalandtechnicaltabsection_marketsessionscript = model.fundamentalandtechnicaltabsection.marketsessionscript;
                data.fundamentalandtechnicaltabsection_relatedresorces = model.fundamentalandtechnicaltabsection.relatedresorces;
                data.fundamentalandtechnicaltabsection_privatenotes = model.fundamentalandtechnicaltabsection.privatenotes;



                _Context.Comodities.Update(data);

                await _Context.SaveChangesAsync();

                List<Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSection> PDFSectionlist2 = new List<Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSection>();
                List<Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSection> PDFSectionlist = new List<Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSection>();
                List<Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSection> UrlSectionlist2 = new List<Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSection>();
                List<Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSection> UrlSectionlist = new List<Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSection>();
                if (model.fundamentalandtechnicaltabsection.pdfsectionlist != null && model.fundamentalandtechnicaltabsection.pdfsectionlist.Count > 0)
                {
                    PDFSectionlist2 = await _Context.Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSections.Where(x => x.comodityid == data.id).ToListAsync();
                    _Context.Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSections.RemoveRange(PDFSectionlist2);
                    await _Context.SaveChangesAsync();

                    foreach (Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSectionDto pdf in model.fundamentalandtechnicaltabsection.pdfsectionlist)
                    {
                        PDFSectionlist.Add(new Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSection()
                        {
                            id = Guid.NewGuid(),
                            comodityid = data.id,
                            author = pdf.author,
                            pdfshortcodeid = pdf.pdfshortcodeid,
                            pdftitle = pdf.pdftitle,
                            shortdescription = pdf.shortdescription
                        });
                    }
                    if (PDFSectionlist.Count > 0)
                        await _Context.Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSections.AddRangeAsync(PDFSectionlist);
                }
                await _Context.SaveChangesAsync();
                if (model.fundamentalandtechnicaltabsection.urlsectionlist != null && model.fundamentalandtechnicaltabsection.urlsectionlist.Count > 0)
                {
                    List<Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSection> _Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSectionsawait = await _Context.Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSections.Where(x => x.comodityid == data.id).ToListAsync();
                    _Context.Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSections.RemoveRange(_Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSectionsawait);
                    await _Context.SaveChangesAsync();

                    foreach (Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSectionDto pdf in model.fundamentalandtechnicaltabsection.urlsectionlist)
                    {
                        UrlSectionlist.Add(new Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSection()
                        {
                            id = Guid.NewGuid(),
                            comodityid = data.id,
                            url = pdf.url,
                            urltitle = pdf.urltitle
                        });
                    }
                    if (UrlSectionlist.Count > 0)
                        await _Context.Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSections.AddRangeAsync(UrlSectionlist);
                    await _Context.SaveChangesAsync();
                }

                

                if (model.fundamentalandtechnicaltabsection.fundamentalnewssections != null && model.fundamentalandtechnicaltabsection.fundamentalnewssections.Count > 0)
                {
                    List<Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection> _Comodities_fundamentalandtechnicaltabsection_fundamentalnewssections = await _Context.Comodities_fundamentalandtechnicaltabsection_fundamentalnewssections.Where(x => x.comodityid == data.id).ToListAsync();
                    _Context.Comodities_fundamentalandtechnicaltabsection_fundamentalnewssections.RemoveRange(_Comodities_fundamentalandtechnicaltabsection_fundamentalnewssections);

                    List<Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontent> _Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents = await _Context.Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents.Where(x => x.comodityid == data.id).ToListAsync();
                    _Context.Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents.RemoveRange(_Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents);

                    List<Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection> newssectionlist = new List<Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection>();
                    List<Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontent> newsmaincontentlist = new List<Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontent>();

                    foreach (Comodities_fundamentalandtechnicaltabsection_fundamentalnewssectionDto nitem in model.fundamentalandtechnicaltabsection.fundamentalnewssections)
                    {
                        Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection news = new Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection()
                        {
                            id = Guid.NewGuid(),
                            comodityid = data.id,
                            maintitle = nitem.maintitle,
                            script = nitem.script
                        };
                        if (nitem.newsmaincontentlist != null && nitem.newsmaincontentlist.Count > 0)
                        {
                            foreach (Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontentDto item in nitem.newsmaincontentlist)
                                newsmaincontentlist.Add(new Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontent()
                                {
                                    id = Guid.NewGuid(),
                                    comodityid = data.id,
                                    comodities_fundamentalandtechnicaltabsection_fundamentalnewssectionid = news.id,
                                    description = item.description,
                                    title = item.title,
                                    link = item.link,
                                    filecontenttype = item.filecontenttype,
                                    filename = item.filename,
                                    filepath = item.filepath
                                });
                        }
                        newssectionlist.Add(news);

                    }
                    if (newssectionlist.Count > 0)
                        await _Context.Comodities_fundamentalandtechnicaltabsection_fundamentalnewssections.AddRangeAsync(newssectionlist);
                    if (newsmaincontentlist.Count > 0)
                        await _Context.Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents.AddRangeAsync(newsmaincontentlist);
                }
                if (model.fundamentalandtechnicaltabsection.technicaltabs != null && model.fundamentalandtechnicaltabsection.technicaltabs.Count > 0)
                {
                    List<Comodities_Fundamentalandtechnicaltabsection_TechnicalTabs> _Comodities_Fundamentalandtechnicaltabsection_TechnicalTabses = await _Context.Comodities_Fundamentalandtechnicaltabsection_TechnicalTabses.Where(x => x.comodityid == data.id).ToListAsync();
                    _Context.Comodities_Fundamentalandtechnicaltabsection_TechnicalTabses.RemoveRange(_Comodities_Fundamentalandtechnicaltabsection_TechnicalTabses);

                    List<Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNews> _Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses = await _Context.Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses.Where(x => x.comodityid == data.id).ToListAsync();
                    _Context.Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses.RemoveRange(_Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses);

                    List<Comodities_Fundamentalandtechnicaltabsection_TechnicalTabs> technicaltablist = new List<Comodities_Fundamentalandtechnicaltabsection_TechnicalTabs>();
                    List<Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNews> technicalbreakingnewslist = new List<Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNews>();
                    foreach (Comodities_Fundamentalandtechnicaltabsection_TechnicalTabsDto tab in model.fundamentalandtechnicaltabsection.technicaltabs)
                    {
                        Comodities_Fundamentalandtechnicaltabsection_TechnicalTabs newtab = new Comodities_Fundamentalandtechnicaltabsection_TechnicalTabs()
                        {
                            id = Guid.NewGuid(),
                            comodityid = data.id,
                            script = tab.script,
                            tabtitle = tab.tabtitle
                        };

                        if (tab.technicalbreakingnewslist != null && tab.technicalbreakingnewslist.Count > 0)
                        {
                            foreach (Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewsDto breakingnews in tab.technicalbreakingnewslist)
                            {
                                technicalbreakingnewslist.Add(new Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNews()
                                {
                                    id = Guid.NewGuid(),
                                    comodityid = data.id,
                                    comoditiesfundamentalandtechnicaltabsectiontechnicaltabsid = newtab.id,
                                    description = breakingnews.description,
                                    filecontenttype = breakingnews.filecontenttype,
                                    filename = breakingnews.filename,
                                    filepath = breakingnews.filepath,
                                    link = breakingnews.link,
                                    title = breakingnews.title
                                });
                            }
                        }
                        technicaltablist.Add(newtab);
                    }

                    if (technicaltablist.Count > 0)
                        await _Context.Comodities_Fundamentalandtechnicaltabsection_TechnicalTabses.AddRangeAsync(technicaltablist);
                    if (technicalbreakingnewslist.Count > 0)
                        await _Context.Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses.AddRangeAsync(technicalbreakingnewslist);
                }

                await _Context.SaveChangesAsync();

                List<Comodities_FlexibleBlock> flexibleBlocklist = new List<Comodities_FlexibleBlock>();
                List<ComoditiesCountriesData> countriesDatalist = new List<ComoditiesCountriesData>();
                List<ComoditiesFirstCountryDataCountriesData> firstcountriesDatalist = new List<ComoditiesFirstCountryDataCountriesData>();
                List<ComoditiesSecondCountryDataCountriesData> SecondcountriesDatalist = new List<ComoditiesSecondCountryDataCountriesData>();

                if (model.comodities != null)
                {
                    List<Comodities_FlexibleBlock> _ComoditiyFlexibleBlocks = await _Context.ComoditiyFlexibleBlocks.Where(x => x.comodityid == data.id).ToListAsync();
                    _Context.ComoditiyFlexibleBlocks.RemoveRange(_ComoditiyFlexibleBlocks);


                    foreach (Comodities_FlexibleBlockDto flex in model.comodities)
                    {
                        Comodities_FlexibleBlock FlexibleBlock = new Comodities_FlexibleBlock();
                        FlexibleBlock.id = Guid.NewGuid();
                        FlexibleBlock.bottomdescription = flex.bottomdescription;
                        FlexibleBlock.bottomdescriptionfilecontenttype = flex.bottomdescriptionfilecontenttype;
                        FlexibleBlock.bottomdescriptionfilename = flex.bottomdescriptionfilename;
                        FlexibleBlock.bottomdescriptionfilepath = flex.bottomdescriptionfilepath;
                        FlexibleBlock.bottomdescriptionfileurl = flex.bottomdescriptionfileurl;
                        FlexibleBlock.chartdescription = flex.chartdescription;
                        FlexibleBlock.chartdescriptionfilecontenttype = flex.chartdescriptionfilecontenttype;
                        FlexibleBlock.chartdescriptionfilename = flex.chartdescriptionfilename;
                        FlexibleBlock.chartdescriptionfilepath = flex.chartdescriptionfilepath;
                        FlexibleBlock.chartdescriptionfileurl = flex.chartdescriptionfileurl;
                        FlexibleBlock.comodityid = data.id;
                        FlexibleBlock.firstcontryheading = flex.firstcontryheading;
                        FlexibleBlock.firstcontrydescriptionfilename = flex.firstcontrydescriptionfilename;
                        FlexibleBlock.firstcontrydescriptionfilecontentype = flex.firstcontrydescriptionfilecontentype;
                        FlexibleBlock.firstcontrydescription = flex.firstcontrydescription;
                        FlexibleBlock.firstcontrydescriptionfilepath = flex.firstcontrydescriptionfilepath;
                        FlexibleBlock.firstcontrydescriptionfileurl = flex.firstcontrydescriptionfileurl;
                        FlexibleBlock.maindescrition = flex.maindescrition;
                        FlexibleBlock.maindescritionfilecontenttype = flex.maindescritionfilecontenttype;
                        FlexibleBlock.maindescritionfilename = flex.maindescritionfilename;
                        FlexibleBlock.maindescritionfilepath = flex.maindescritionfilepath;
                        FlexibleBlock.maindescritionfileurl = flex.maindescritionfileurl;
                        FlexibleBlock.oneyeardescription = flex.oneyeardescription;
                        FlexibleBlock.oneyeardescriptionfilecontenttype = flex.oneyeardescriptionfilecontenttype;
                        FlexibleBlock.oneyeardescriptionfilename = flex.oneyeardescriptionfilename;
                        FlexibleBlock.oneyeardescriptionfilepath = flex.oneyeardescriptionfilepath;
                        FlexibleBlock.oneyeardescriptionfileurl = flex.oneyeardescriptionfileurl;
                        FlexibleBlock.secoundcontrydescription = flex.secoundcontrydescription;
                        FlexibleBlock.maintitle = flex.maintitle;
                        FlexibleBlock.secondcountryheading = flex.secondcountryheading;
                        FlexibleBlock.secoundcontrydescriptionfilecontenttype = flex.secoundcontrydescriptionfilecontenttype;
                        FlexibleBlock.secoundcontrydescriptionfilename = flex.secoundcontrydescriptionfilename;
                        FlexibleBlock.secoundcontrydescriptionfilepath = flex.secoundcontrydescriptionfilepath;
                        FlexibleBlock.secoundcontrydescriptionfileurl = flex.secoundcontrydescriptionfileurl;
                        FlexibleBlock.singlepagechartimage = flex.singlepagechartimage;


                        if (flex.comoditiescountriesdatalist != null && flex.comoditiescountriesdatalist.Count > 0)
                        {
                            List<ComoditiesCountriesData> _ComoditiesCountriesDatas = await _Context.ComoditiesCountriesDatas.Where(x => x.comodityid == data.id).ToListAsync();
                            _Context.ComoditiesCountriesDatas.RemoveRange(_ComoditiesCountriesDatas);

                            foreach (ComoditiesCountriesDataDto contrydata in flex.comoditiescountriesdatalist)
                            {
                                countriesDatalist.Add(new ComoditiesCountriesData()
                                {
                                    id = Guid.NewGuid(),
                                    comodityid = data.id,
                                    comodityflexibleblockid = FlexibleBlock.id,
                                    contries = contrydata.contries,
                                    highslows = contrydata.highslows,
                                    pairsthatcorrelate = contrydata.pairsthatcorrelate,
                                    pairtype = contrydata.pairtype,
                                    dailyaveragemovementinpips = contrydata.dailyaveragemovementinpips
                                });
                            }
                            await _Context.ComoditiesCountriesDatas.AddRangeAsync(countriesDatalist);
                        }
                        if (flex.comoditiesfirstcountrydatacountriesdatalist != null && flex.comoditiesfirstcountrydatacountriesdatalist.Count > 0)
                        {
                            List<ComoditiesFirstCountryDataCountriesData> _ComoditiesFirstCountryDataCountriesDatas = await _Context.ComoditiesFirstCountryDataCountriesDatas.Where(x => x.comodityid == data.id).ToListAsync();
                            _Context.ComoditiesFirstCountryDataCountriesDatas.RemoveRange(_ComoditiesFirstCountryDataCountriesDatas);

                            foreach (ComoditiesFirstCountryDataCountriesDataDto contrydata in flex.comoditiesfirstcountrydatacountriesdatalist)
                            {
                                firstcountriesDatalist.Add(new ComoditiesFirstCountryDataCountriesData()
                                {
                                    id = Guid.NewGuid(),
                                    comodityid = data.id,
                                    comodityflexibleblockid = FlexibleBlock.id,
                                    centeralbank = contrydata.centeralbank,
                                    contries = contrydata.contries,
                                    nickname = contrydata.nickname,
                                    ofaveragedailyturnover = contrydata.ofaveragedailyturnover
                                });
                            }
                            await _Context.ComoditiesFirstCountryDataCountriesDatas.AddRangeAsync(firstcountriesDatalist);
                        }
                        if (flex.comoditiessecondcountrydatacountriesdatalist != null && flex.comoditiessecondcountrydatacountriesdatalist.Count > 0)
                        {
                            List<ComoditiesSecondCountryDataCountriesData> _ComoditiesSecondCountryDataCountriesDatas = await _Context.ComoditiesSecondCountryDataCountriesDatas.Where(x => x.comodityid == data.id).ToListAsync();
                            _Context.ComoditiesSecondCountryDataCountriesDatas.RemoveRange(_ComoditiesSecondCountryDataCountriesDatas);

                            foreach (ComoditiesSecondCountryDataCountriesDataDto contrydata in flex.comoditiessecondcountrydatacountriesdatalist)
                            {
                                SecondcountriesDatalist.Add(new ComoditiesSecondCountryDataCountriesData()
                                {
                                    id = Guid.NewGuid(),
                                    comodityid = data.id,
                                    comodityflexibleblockid = FlexibleBlock.id,
                                    centeralbank = contrydata.centeralbank,
                                    contries = contrydata.contries,
                                    nickname = contrydata.nickname,
                                    ofaveragedailyturnover = contrydata.ofaveragedailyturnover
                                });
                            }
                            await _Context.ComoditiesSecondCountryDataCountriesDatas.AddRangeAsync(SecondcountriesDatalist);
                            await _Context.SaveChangesAsync();
                        }
                        //public List<>?  { get; set; }
                        flexibleBlocklist.Add(FlexibleBlock);
                    }

                    await _Context.ComoditiyFlexibleBlocks.AddRangeAsync(flexibleBlocklist);
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
/*oneyeardescription	ntext	Checked
oneyeardescriptionfilename	nvarchar(500)	Checked
oneyeardescriptionfilecontenttype	nvarchar(50)	Checked
oneyeardescriptionfilepath	nvarchar(2000)	Checked
chartdescription	ntext	Checked
chartdescriptionfilename	nvarchar(500)	Checked
chartdescriptionfilepath	nvarchar(1800)	Checked
chartdescriptionfilecontenttype	nvarchar(50)	Checked
firstcountryheading	nvarchar(3000)	Checked
firstcountrydescription	ntext	Checked
firstcountrydescriptionfilename	nvarchar(500)	Checked
firstcountrydescriptionfilepath	nvarchar(1800)	Checked
firstcountrydescriptionfilecontenttype	nvarchar(50)	Checked
secondcountryheading	nvarchar(1500)	Checked
secondcountrydescription	ntext	Checked
secondcountrydescriptionfilename	nvarchar(500)	Checked
secondcountrydescriptionfilepath	nvarchar(1800)	Checked
secondcountrydescriptionfilecontenttype	nvarchar(50)	Checked
bottomdescription	ntext	Checked
bottomdescriptionfilename	nvarchar(500)	Checked
bottomdescriptionfilepath	nvarchar(1800)	Checked
bottomdescriptionfilecontenttype	nvarchar(50)	Checked
maindescription	ntext	Checked
maindescriptionfilecontenttype	nvarchar(50)	Checked
maindescriptionfilename	nvarchar(500)	Checked
maindescriptionfilepath	nvarchar(1800)	Checked
singlepagechartimage	ntext	Checked*/