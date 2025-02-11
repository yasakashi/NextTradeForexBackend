using Base.Common.Enums;
using DataLayers;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Entities.DBEntities;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NextTradeAPIs.Dtos;
using System.Diagnostics;

namespace NextTradeAPIs.Services
{
    public class MarketPulsCryptoServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;
        public MarketPulsCryptoServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
        }


        public async Task<SystemMessageModel> SaveCryptoItem(CryptoDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (await _Context.Cryptos.Where(x => x.categoryid == model.categoryid).AnyAsync())
                {
                    return new SystemMessageModel() { MessageCode = -103, MessageDescription = "For this category save before use update service", MessageData = model };
                }

                Crypto data = new Crypto()
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
                    cryptofundamentalandtechnicaltabsection_instrumentname = model.fundamentalandtechnicaltabsection.instrumentname,
                    cryptofundamentalandtechnicaltabsection_fundamentalheading = model.fundamentalandtechnicaltabsection.fundamentalheading,
                    cryptofundamentalandtechnicaltabsection_technicalheading = model.fundamentalandtechnicaltabsection.technicalheading,
                    cryptofundamentalandtechnicaltabsection_marketsentimentstitle = model.fundamentalandtechnicaltabsection.marketsentimentstitle,
                    cryptofundamentalandtechnicaltabsection_marketsentimentsscript = model.fundamentalandtechnicaltabsection.marketsentimentsscript,
                    cryptofundamentalandtechnicaltabsection_marketsessiontitle = model.fundamentalandtechnicaltabsection.marketsessiontitle,
                    cryptofundamentalandtechnicaltabsection_marketsessionscript = model.fundamentalandtechnicaltabsection.marketsessionscript,
                    cryptofundamentalandtechnicaltabsection_relatedresorces = model.fundamentalandtechnicaltabsection.relatedresorces,
                    cryptofundamentalandtechnicaltabsection_privatenotes = model.fundamentalandtechnicaltabsection.privatenotes,
                    tags = model.tags
                };
                await _Context.Cryptos.AddAsync(data);
                if (model.fundamentalandtechnicaltabsection != null)
                {
                    List<Crypto_FundamentalandNewsSection> NewsSectioList = new List<Crypto_FundamentalandNewsSection>();
                    List<Crypto_NewsMainContent> NewsMainContetList = new List<Crypto_NewsMainContent>();
                    foreach (Crypto_FundamentalandNewsSectionDto news in model.fundamentalandtechnicaltabsection.fundamentalnewssections)
                    {
                        Crypto_FundamentalandNewsSection newssection =  new Crypto_FundamentalandNewsSection() { 
                            id = Guid.NewGuid(),
                            cryptoid = data.id,
                            maintitle = news.maintitle,
                            script = news.script
                        };

                        foreach (Crypto_NewsMainContentDto newsmaincontent in news.newsmaincontentlist)
                        {
                            NewsMainContetList.Add(new Crypto_NewsMainContent() {
                                id = Guid.NewGuid(),
                                cryptoid = data.id,
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

                    await _Context.CryptoFundamentalandNewsSections.AddRangeAsync(NewsSectioList);
                    await _Context.CryptoNewsMainContents.AddRangeAsync(NewsMainContetList);

                    List<Crypto_TechnicalTab> technicaltabList = new List<Crypto_TechnicalTab>();
                    List<Crypto_TechnicalBreakingNews> technicalbreakList = new List<Crypto_TechnicalBreakingNews>();
                    foreach (Crypto_TechnicalTabDto tab in model.fundamentalandtechnicaltabsection.technicaltabs)
                    {
                        Crypto_TechnicalTab newtab = new Crypto_TechnicalTab()
                        {
                            id = Guid.NewGuid(),
                            cryptoid = data.id,
                            tabtitle = tab.tabtitle,
                            script = tab.script
                        };

                        foreach (Crypto_TechnicalBreakingNewsDto newsmaincontent in tab.newsmaincontentlist)
                        {
                            technicalbreakList.Add(new Crypto_TechnicalBreakingNews()
                            {
                                id = Guid.NewGuid(),
                                cryptoid = data.id,
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

                    await _Context.CryptoTechnicalTabs.AddRangeAsync(technicaltabList);
                    await _Context.CryptoTechnicalBreakingNewss.AddRangeAsync(technicalbreakList);

                    List<Crypto_FundamentalandNewsSection_RelatedReSorces_PDFSection> PDFSectionlist = new List<Crypto_FundamentalandNewsSection_RelatedReSorces_PDFSection>();
                if (model.fundamentalandtechnicaltabsection.pdfsectionlist != null && model.fundamentalandtechnicaltabsection.pdfsectionlist.Count > 0)
                {
                    foreach (Crypto_FundamentalandNewsSection_RelatedReSorces_PDFSectionDto pdf in model.fundamentalandtechnicaltabsection.pdfsectionlist)
                    {
                        PDFSectionlist.Add(new Crypto_FundamentalandNewsSection_RelatedReSorces_PDFSection()
                        {
                            id = Guid.NewGuid(),
                            cryptoid = data.id,
                            author = pdf.author,
                            pdfshortcodeid = pdf.pdfshortcodeid,
                            pdftitle = pdf.pdftitle,
                            shortdescription = pdf.shortdescription
                        });
                    }
                    if (PDFSectionlist.Count > 0)
                        await _Context.CryptoFundamentalandNewsSection_RelatedReSorces_PDFSections.AddRangeAsync(PDFSectionlist);
                }
                List<Crypto_FundamentalandNewsSection_RelatedReSorces_URLSection> UrlSectionlist = new List<Crypto_FundamentalandNewsSection_RelatedReSorces_URLSection>();
                if (model.fundamentalandtechnicaltabsection.urlsectionlist != null && model.fundamentalandtechnicaltabsection.urlsectionlist.Count > 0)
                {
                    foreach (Crypto_FundamentalandNewsSection_RelatedReSorces_URLSectionDto pdf in model.fundamentalandtechnicaltabsection.urlsectionlist)
                    {
                        UrlSectionlist.Add(new Crypto_FundamentalandNewsSection_RelatedReSorces_URLSection()
                        {
                            id = Guid.NewGuid(),
                            cryptoid = data.id,
                            url = pdf.url,
                            urltitle = pdf.urltitle
                        });
                    }
                    if (UrlSectionlist.Count > 0)
                        await _Context.CryptoFundamentalandNewsSection_RelatedReSorces_URLSections.AddRangeAsync(UrlSectionlist);
                }

                }


                List<Crypto_FlexibleBlock> flexibleBlocklist = new List<Crypto_FlexibleBlock>();
                List<CryptoCountriesData> countriesDatalist = new List<CryptoCountriesData>();
                List<CryptoCountryData> countryDatalist = new List<CryptoCountryData>();

                foreach (Crypto_FlexibleBlockDto flex in model.crypto_flexi_block)
                {
                    Crypto_FlexibleBlock FlexibleBlock = new Crypto_FlexibleBlock();
                    FlexibleBlock.id = Guid.NewGuid();
                    FlexibleBlock.cryptoid = data.id;
                    FlexibleBlock.maintitle = flex.maintitle;
                    FlexibleBlock.oneyeardescription = flex.oneyeardescription;
                    FlexibleBlock.oneyeardescriptionfilecontenttype = flex.oneyeardescriptionfilecontenttype;
                    FlexibleBlock.oneyeardescriptionfilename = flex.oneyeardescriptionfilename;
                    FlexibleBlock.oneyeardescriptionfilepath = flex.oneyeardescriptionfilepath;
                    FlexibleBlock.oneyeardescriptionfileurl = flex.oneyeardescriptionfileurl;
                    FlexibleBlock.chartdescription = flex.chartdescription;
                    FlexibleBlock.chartdescriptionfilecontenttype = flex.chartdescriptionfilecontenttype;
                    FlexibleBlock.chartdescriptionfilename = flex.chartdescriptionfilename;
                    FlexibleBlock.chartdescriptionfilepath = flex.chartdescriptionfilepath;
                    FlexibleBlock.chartdescriptionfileurl = flex.chartdescriptionfileurl;
                    FlexibleBlock.contryheading = flex.contryheading;
                    FlexibleBlock.contrydescription = flex.contrydescription;
                    FlexibleBlock.contrydescriptionfilename = flex.contrydescriptionfilename;
                    FlexibleBlock.contrydescriptionfilecontentype = flex.contrydescriptionfilecontentype;
                    FlexibleBlock.contrydescriptionfilepath = flex.contrydescriptionfilepath;
                    FlexibleBlock.contrydescriptionfileurl = flex.contrydescriptionfileurl;
                    FlexibleBlock.bottomdescription = flex.bottomdescription;
                    FlexibleBlock.bottomdescriptionfilecontenttype = flex.bottomdescriptionfilecontenttype;
                    FlexibleBlock.bottomdescriptionfilename = flex.bottomdescriptionfilename;
                    FlexibleBlock.bottomdescriptionfilepath = flex.bottomdescriptionfilepath;
                    FlexibleBlock.bottomdescriptionfileurl = flex.bottomdescriptionfileurl;
                    FlexibleBlock.maindescrition = flex.maindescrition;
                    FlexibleBlock.maindescritionfilecontenttype = flex.maindescritionfilecontenttype;
                    FlexibleBlock.maindescritionfilename = flex.maindescritionfilename;
                    FlexibleBlock.maindescritionfilepath = flex.maindescritionfilepath;
                    FlexibleBlock.maindescritionfileurl = flex.maindescritionfileurl;
                    FlexibleBlock.singlepagechartimage = flex.singlepagechartimage;


                    if (flex.countriesdatalist != null && flex.countriesdatalist.Count > 0)
                    {
                        foreach (CryptoCountriesDataDto contrydata in flex.countriesdatalist)
                        {
                            countriesDatalist.Add(new CryptoCountriesData()
                            {
                                id = Guid.NewGuid(),
                                cryptoid = data.id,
                                cryptoflexibleblockid = FlexibleBlock.id,
                                contry = contrydata.contry,
                                centeralbank = contrydata.centeralbank,
                                nickname = contrydata.nickname,
                                ofaveragedailyturnover = contrydata.ofaveragedailyturnover
                            });
                        }
                        await _Context.CryptoCountriesDatas.AddRangeAsync(countriesDatalist);
                    }


                    if (flex.countrydatalist != null && flex.countrydatalist.Count > 0)
                    {
                        foreach (CryptoCountryDataDto contrydata in flex.countrydatalist)
                        {
                            countryDatalist.Add(new CryptoCountryData()
                            {
                                id = Guid.NewGuid(),
                                cryptoid = data.id,
                                cryptoflexibleblockid = FlexibleBlock.id,
                                contries = contrydata.contries,
                                pairsthatcorrelate = contrydata.pairsthatcorrelate,
                                highslows = contrydata.highslows,
                                dailyaveragemovementinpips = contrydata.dailyaveragemovementinpips,
                                pairtype = contrydata.pairtype
                            });
                        }
                        await _Context.CryptoCountryDatas.AddRangeAsync(countryDatalist);
                    }
                    flexibleBlocklist.Add(FlexibleBlock);
                }

                await _Context.CryptoFlexibleBlocks.AddRangeAsync(flexibleBlocklist);

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

        public async Task<SystemMessageModel> GetCryptoItems(CryptoFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                List<CryptoDto> datas;
                IQueryable<Crypto> query = _Context.Cryptos;

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
                                .Take(PageRowCount).Select(x => new CryptoDto()
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
                                    fundamentalandtechnicaltabsection = new CryptoFundamentalAndTechnicalTabSectionDto()
                                    {
                                        instrumentname = x.cryptofundamentalandtechnicaltabsection_instrumentname,
                                        fundamentalheading = x.cryptofundamentalandtechnicaltabsection_fundamentalheading,
                                        marketsentimentsscript = x.cryptofundamentalandtechnicaltabsection_marketsentimentsscript,
                                        marketsessionscript = x.cryptofundamentalandtechnicaltabsection_marketsessionscript,
                                        privatenotes = x.cryptofundamentalandtechnicaltabsection_privatenotes,
                                        relatedresorces = x.cryptofundamentalandtechnicaltabsection_relatedresorces,
                                        technicalheading = x.cryptofundamentalandtechnicaltabsection_technicalheading,
                                        marketsentimentstitle = x.cryptofundamentalandtechnicaltabsection_marketsessiontitle,
                                        marketsessiontitle = x.cryptofundamentalandtechnicaltabsection_marketsessiontitle
                                    }
                                }).ToListAsync();

                foreach (CryptoDto data in datas)
                {
                    data.fundamentalandtechnicaltabsection.urlsectionlist = await _Context.CryptoFundamentalandNewsSection_RelatedReSorces_URLSections.Where(x => x.cryptoid == data.id).Select(x => new Crypto_FundamentalandNewsSection_RelatedReSorces_URLSectionDto()
                    {
                        id = x.id,
                        cryptoid = x.cryptoid,
                        url = x.url,
                        urltitle = x.urltitle
                    }).ToListAsync();

                    data.fundamentalandtechnicaltabsection.pdfsectionlist = await _Context.CryptoFundamentalandNewsSection_RelatedReSorces_PDFSections.Where(x => x.cryptoid == data.id).Select(x => new Crypto_FundamentalandNewsSection_RelatedReSorces_PDFSectionDto()
                    {
                        id = x.id,
                        cryptoid = x.cryptoid,
                        author = x.author,
                        pdfshortcodeid = x.pdfshortcodeid,
                        pdftitle = x.pdftitle,
                        shortdescription = x.shortdescription
                    }).ToListAsync();

                    data.fundamentalandtechnicaltabsection.fundamentalnewssections = await _Context.CryptoFundamentalandNewsSections.Where(x => x.cryptoid == data.id).Select(x => new Crypto_FundamentalandNewsSectionDto() {id = x.id, cryptoid = x.cryptoid, maintitle = x.maintitle, script = x.script }).ToListAsync();

                    foreach (Crypto_FundamentalandNewsSectionDto news in data.fundamentalandtechnicaltabsection.fundamentalnewssections)
                    {
                        news.newsmaincontentlist = await _Context.CryptoNewsMainContents.Where(x => x.cryptoid == data.id && x.fundamentalandnewssectionid == news.id).Select(x => new Crypto_NewsMainContentDto() { id = x.id, cryptoid = x.cryptoid, fundamentalandnewssectionid = x.fundamentalandnewssectionid ,
                            description = x.description,
                            descriptionfilecontenttype = x.descriptionfilecontenttype,
                            descriptionfilename = x.descriptionfilename,
                            descriptionfilepath = x.descriptionfilepath,
                            descriptionfileurl = x.descriptionfileurl,
                            link = x.link,
                            title = x.title
                        }).ToListAsync();
                    }

                    data.fundamentalandtechnicaltabsection.technicaltabs = await _Context.CryptoTechnicalTabs.Where(x => x.cryptoid == data.id).Select(x => new Crypto_TechnicalTabDto() { id = x.id, cryptoid = x.cryptoid, tabtitle = x.tabtitle, script = x.script }).ToListAsync();

                    foreach (Crypto_TechnicalTabDto tab in data.fundamentalandtechnicaltabsection.technicaltabs)
                    {
                        tab.newsmaincontentlist = await _Context.CryptoNewsMainContents.Where(x => x.cryptoid == data.id && x.fundamentalandnewssectionid == tab.id).Select(x => new Crypto_TechnicalBreakingNewsDto()
                        {
                            id = x.id,
                            cryptoid = x.cryptoid,
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

                    data.crypto_flexi_block = await _Context.CryptoFlexibleBlocks.Where(x => x.cryptoid == data.id).Select(x => new Crypto_FlexibleBlockDto()
                    {
                        id = x.id,
                        cryptoid = x.cryptoid,
                        maintitle = x.maintitle,
                        oneyeardescription = x.oneyeardescription,
                        oneyeardescriptionfilecontenttype = x.oneyeardescriptionfilecontenttype,
                        oneyeardescriptionfilename = x.oneyeardescriptionfilename,
                        oneyeardescriptionfilepath = x.oneyeardescriptionfilepath,
                        oneyeardescriptionfileurl = x.oneyeardescriptionfileurl,
                        chartdescription = x.chartdescription,
                        chartdescriptionfilecontenttype = x.chartdescriptionfilecontenttype,
                        chartdescriptionfilename = x.chartdescriptionfilename,
                        chartdescriptionfilepath = x.chartdescriptionfilepath,
                        chartdescriptionfileurl = x.chartdescriptionfileurl,
                        contryheading = x.contryheading,
                        contrydescription = x.contrydescription,
                        contrydescriptionfilename = x.contrydescriptionfilename,
                        contrydescriptionfilecontentype = x.contrydescriptionfilecontentype,
                        contrydescriptionfilepath = x.contrydescriptionfilepath,
                        contrydescriptionfileurl = x.contrydescriptionfileurl,
                        bottomdescription = x.bottomdescription,
                        bottomdescriptionfilecontenttype = x.bottomdescriptionfilecontenttype,
                        bottomdescriptionfilename = x.bottomdescriptionfilename,
                        bottomdescriptionfilepath = x.bottomdescriptionfilepath,
                        bottomdescriptionfileurl = x.bottomdescriptionfileurl,
                        maindescrition = x.maindescrition,
                        maindescritionfilecontenttype = x.maindescritionfilecontenttype,
                        maindescritionfilename = x.maindescritionfilename,
                        maindescritionfilepath = x.maindescritionfilepath,
                        maindescritionfileurl = x.maindescritionfileurl,
                        singlepagechartimage = x.singlepagechartimage
                    }).ToListAsync();

                    foreach (Crypto_FlexibleBlockDto FlexibleBlockitem in data.crypto_flexi_block)
                    {
                        FlexibleBlockitem.countrydatalist = await _Context.CryptoCountryDatas.Where(x => x.cryptoid == data.id && x.cryptoflexibleblockid == FlexibleBlockitem.id).Select(x => new CryptoCountryDataDto()
                        {
                            id = x.id,
                            cryptoflexibleblockid = x.cryptoflexibleblockid,
                            cryptoid = x.cryptoid,
                            contries = x.contries,
                            dailyaveragemovementinpips = x.dailyaveragemovementinpips,
                            highslows = x.highslows,
                            pairsthatcorrelate = x.pairsthatcorrelate,
                            pairtype = x.pairtype
                        }).ToListAsync();

                        FlexibleBlockitem.countriesdatalist = await _Context.CryptoCountriesDatas.Where(x => x.cryptoid == data.id && x.cryptoflexibleblockid == FlexibleBlockitem.id).Select(x => new CryptoCountriesDataDto()
                        {
                            id = x.id,
                            cryptoflexibleblockid = x.cryptoflexibleblockid,
                            cryptoid = x.cryptoid,
                            contry = x.contry,
                            centeralbank = x.centeralbank,
                            nickname = x.nickname,
                            ofaveragedailyturnover = x.ofaveragedailyturnover
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

        public async Task<SystemMessageModel> DeleteCryptoItem(CryptoFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                Crypto data;
                IQueryable<Crypto> query = _Context.Cryptos;

                if (model.id != null)
                    query = query.Where(x => x.id == model.id);

                if (model.categoryid != null)
                    query = query.Where(x => x.categoryid == model.categoryid);

                data = await query.FirstOrDefaultAsync();

                _Context.Cryptos.Remove(data);


                List<Crypto_FlexibleBlock> _CryptoFlexibleBlocks = await _Context.CryptoFlexibleBlocks.Where(x => x.cryptoid == data.id).ToListAsync();
                _Context.CryptoFlexibleBlocks.RemoveRange(_CryptoFlexibleBlocks);

                List<CryptoCountriesData> _CryptoCountriesDatas = await _Context.CryptoCountriesDatas.Where(x => x.cryptoid == data.id).ToListAsync();
                _Context.CryptoCountriesDatas.RemoveRange(_CryptoCountriesDatas);

                List<CryptoCountryData> _CryptoFirstCountryDataCountriesDatas = await _Context.CryptoCountryDatas.Where(x => x.cryptoid == data.id).ToListAsync();
                _Context.CryptoCountryDatas.RemoveRange(_CryptoFirstCountryDataCountriesDatas);

                List<Crypto_FundamentalandNewsSection> _Crypto_fundamentalandtechnicaltabsection_fundamentalnewssections = await _Context.CryptoFundamentalandNewsSections.Where(x => x.cryptoid == data.id).ToListAsync();
                _Context.CryptoFundamentalandNewsSections.RemoveRange(_Crypto_fundamentalandtechnicaltabsection_fundamentalnewssections);

                List<Crypto_NewsMainContent> _Crypto_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents = await _Context.CryptoNewsMainContents.Where(x => x.cryptoid == data.id).ToListAsync();
                _Context.CryptoNewsMainContents.RemoveRange(_Crypto_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents);

                List<Crypto_TechnicalTab> _Crypto_Fundamentalandtechnicaltabsection_TechnicalTabses = await _Context.CryptoTechnicalTabs.Where(x => x.cryptoid == data.id).ToListAsync();
                _Context.CryptoTechnicalTabs.RemoveRange(_Crypto_Fundamentalandtechnicaltabsection_TechnicalTabses);

                List<Crypto_TechnicalBreakingNews> _Crypto_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses = await _Context.CryptoTechnicalBreakingNewss.Where(x => x.cryptoid == data.id).ToListAsync();
                _Context.CryptoTechnicalBreakingNewss.RemoveRange(_Crypto_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses);

                List<Crypto_FundamentalandNewsSection_RelatedReSorces_PDFSection> _Crypto_FundamentalandTechnicalTabSection_RelatedResorces_PDFSections = await _Context.CryptoFundamentalandNewsSection_RelatedReSorces_PDFSections.Where(x => x.cryptoid == data.id).ToListAsync();
                _Context.CryptoFundamentalandNewsSection_RelatedReSorces_PDFSections.RemoveRange(_Crypto_FundamentalandTechnicalTabSection_RelatedResorces_PDFSections);

                List<Crypto_FundamentalandNewsSection_RelatedReSorces_URLSection> _Crypto_FundamentalandTechnicalTabSection_RelatedResorces_URLSectionsawait = await _Context.CryptoFundamentalandNewsSection_RelatedReSorces_URLSections.Where(x => x.cryptoid == data.id).ToListAsync();
                _Context.CryptoFundamentalandNewsSection_RelatedReSorces_URLSections.RemoveRange(_Crypto_FundamentalandTechnicalTabSection_RelatedResorces_URLSectionsawait);



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


        public async Task<SystemMessageModel> UpdateCryptoItem(CryptoDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.id == null)
                    return new SystemMessageModel() { MessageCode = -102, MessageDescription = "Id is Wrong" };

                Crypto data = await _Context.Cryptos.FindAsync(model.id);
                if (data == null)
                    return new SystemMessageModel() { MessageCode = -103, MessageDescription = "data not find" };

                if (data.categoryid != model.categoryid && await _Context.Cryptos.AnyAsync(x => x.categoryid == model.categoryid))
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
                data.cryptofundamentalandtechnicaltabsection_instrumentname = model.fundamentalandtechnicaltabsection.instrumentname;
                data.cryptofundamentalandtechnicaltabsection_fundamentalheading = model.fundamentalandtechnicaltabsection.fundamentalheading;
                data.cryptofundamentalandtechnicaltabsection_technicalheading = model.fundamentalandtechnicaltabsection.technicalheading;
                data.cryptofundamentalandtechnicaltabsection_marketsentimentstitle = model.fundamentalandtechnicaltabsection.marketsentimentstitle;
                data.cryptofundamentalandtechnicaltabsection_marketsentimentsscript = model.fundamentalandtechnicaltabsection.marketsentimentsscript;
                data.cryptofundamentalandtechnicaltabsection_marketsessiontitle = model.fundamentalandtechnicaltabsection.marketsessiontitle;
                data.cryptofundamentalandtechnicaltabsection_marketsessionscript = model.fundamentalandtechnicaltabsection.marketsessionscript;
                data.cryptofundamentalandtechnicaltabsection_relatedresorces = model.fundamentalandtechnicaltabsection.relatedresorces;
                data.cryptofundamentalandtechnicaltabsection_privatenotes = model.fundamentalandtechnicaltabsection.privatenotes;
                data.tags = model.tags;

                _Context.Cryptos.Update(data);




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

        public async Task<SystemMessageModel> SaveFile(byte[] filecontent, SiteMediaFileDto model, long userid, string FileName, string sitePath, string hosturl)
        {
            string filegroupname = "sitemediafile";
            try
            {
                if (filecontent != null)
                {
                    string _filePath = sitePath + "\\" + filegroupname + "\\" + model.id.ToString().Replace("-", "") + "\\";
                    if (!Directory.Exists(_filePath))
                        Directory.CreateDirectory(_filePath);

                    _filePath += FileName;
                    string fileurl = hosturl + "/" + filegroupname + "/" + model.id.ToString().Replace("-", "") + "/" + FileName;

                    if (!File.Exists(_filePath))
                    {
                        File.WriteAllBytes(_filePath, filecontent);
                    }
                    FileActionDto dto = new FileActionDto()
                    {
                        filepath = _filePath,
                        fileurl = fileurl
                    };
                    return new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = dto };
                }
                else
                {
                    return new SystemMessageModel() { MessageCode = -501, MessageDescription = "File Error", MessageData = null };
                }
            }
            catch (Exception ex) { return new SystemMessageModel() { MessageCode = -501, MessageDescription = "File saving Error", MessageData = ex.Message }; }
        }

        public async Task<SystemMessageModel> DeleteFile(string filename)
        {
            try
            {
                if (File.Exists(filename))
                    File.Delete(filename);

                return new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = null };
            }
            catch (Exception ex) { return new SystemMessageModel() { MessageCode = -501, MessageDescription = "File saving Error", MessageData = ex.Message }; }
        }
    }
}
