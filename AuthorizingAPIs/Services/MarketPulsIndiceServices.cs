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

    public class MarketPulsIndiceServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;
        public MarketPulsIndiceServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
        }

        public async Task<SystemMessageModel> SaveIndiceItem(IndiceDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (await _Context.Indices.Where(x => x.categoryid == model.categoryid).AnyAsync())
                {
                    return new SystemMessageModel() { MessageCode = -103, MessageDescription = "For this category save before use update service", MessageData = model };
                }
                //await DeleteIndiceItem(new IndiceFilterDto() {id = model.id, categoryid = model.categoryid }, userlogin, processId, clientip, hosturl);

                Indice data = new Indice()
                {
                    id = Guid.NewGuid(),
                    categoryid = model.categoryid,
                    creatoruserid = userlogin.userid,
                    price = 0,
                    isvisible = model.isvisible ?? true,
                    courseleveltypeId = model.courseleveltypeId ?? 0,
                    coursetitle = model.coursetitle,
                    instrumentname = model.instrumentname,
                    fundamentalheading = model.fundamentalheading,
                    technicalheading = model.technicalheading,
                    marketsessiontitle = model.marketsessiontitle,
                    marketsessionscript = model.marketsessionscript,
                    marketsentimentstitle = model.marketsentimentstitle,
                    marketsentimentsscript = model.marketsentimentsscript,
                    chart = model.chart,
                    indicesinformations_alltimehigh = model.indicesinformations_alltimehigh,
                    indicesinformations_alltimelow = model.indicesinformations_alltimelow,
                    indicesinformations_centralbank = model.indicesinformations_centralbank,
                    indicesinformations_countriesrepresented = model.indicesinformations_countriesrepresented,
                    indicesinformations_nickname = model.indicesinformations_nickname,
                    indicesinformations_pricetoearningratio = model.indicesinformations_pricetoearningratio,
                    indicesinformations_relatedconstituents = model.indicesinformations_relatedconstituents,
                    indicesinformations_warketcapitalization = model.indicesinformations_warketcapitalization,
                    indicesinformations_weightageoflargestconstituent = model.indicesinformations_weightageoflargestconstituent,
                    indicesinformations_weightageoftop5constituents = model.indicesinformations_weightageoftop5constituents,
                    indicesinformations_weightingmethodology = model.indicesinformations_weightingmethodology,
                    indicesinformations_yeartodatereturn = model.indicesinformations_yeartodatereturn,
                    maindescription_filecontenttype = model.maindescription_filecontenttype,
                    newstickerimportant = model.newstickerimportant,
                    parentindex = model.parentindex,
                    newstickernew = model.newstickernew,
                    maindescription = model.maindescription,
                    newstickerupdate = model.newstickerupdate,
                    relatedresorces = model.relatedresorces,
                    privatenotes = model.privatenotes,
                    excerpt = model.excerpt,
                    author = model.author,
                    createdatetime = DateTime.Now
                };
                await _Context.Indices.AddAsync(data);


                if (model.indiceurlsectionlist != null && model.indiceurlsectionlist.Count() > 0)
                {
                    List<Indice_URLSection> URLSectionlist = new List<Indice_URLSection>();
                    foreach (Indice_URLSectionDto x in model.indiceurlsectionlist)
                    {
                        URLSectionlist.Add(new Indice_URLSection()
                        {
                            url = x.url,
                            urltitle = x.urltitle,
                            id = Guid.NewGuid(),
                            marketpulsindiceid = data.id
                        });
                    }
                    await _Context.IndiceURLSections.AddRangeAsync(URLSectionlist);
                }

                if (model.indicetechnicaltabslist != null && model.indicetechnicaltabslist.Count() > 0)
                {
                    List<Indice_TechnicalTabs> TechnicalTabslist = new List<Indice_TechnicalTabs>();
                    foreach (Indice_TechnicalTabsDto x in model.indicetechnicaltabslist)
                    {
                        Indice_TechnicalTabs _technicalTabs = new Indice_TechnicalTabs()
                        {
                            tabtitle = x.tabtitle,
                            script = x.script,
                            id = Guid.NewGuid(),
                            marketpulsindiceid = data.id
                        };

                        if (x.technicalbreakingnewslist != null && x.technicalbreakingnewslist.Count() > 0)
                        {
                            List<Indice_TechnicalTabs_TechnicalBreakingNews> TechnicalBreakingNewslist = new List<Indice_TechnicalTabs_TechnicalBreakingNews>();
                            foreach (Indice_TechnicalTabs_TechnicalBreakingNewsDto xn in x.technicalbreakingnewslist)
                            {
                                TechnicalBreakingNewslist.Add(new Indice_TechnicalTabs_TechnicalBreakingNews()
                                {
                                    description = xn.description,
                                    link = xn.link,
                                    id = Guid.NewGuid(),
                                    title = xn.title,
                                    technicaltabsid = _technicalTabs.id,
                                    marketpulsindiceid = data.id,
                                    newsmaincontentfilecontenttype = xn.newsmaincontentfilecontenttype,
                                    newsmaincontentfilename = xn.newsmaincontentfilename,
                                    newsmaincontentfilepath = xn.newsmaincontentfilepath
                                });
                            }
                            await _Context.IndiceTechnicalTabsTechnicalBreakingNewss.AddRangeAsync(TechnicalBreakingNewslist);
                        }
                        TechnicalTabslist.Add(_technicalTabs);

                    }
                    await _Context.IndiceTechnicalTabss.AddRangeAsync(TechnicalTabslist);
                }


                if (model.indicerelatedinstumentlist != null && model.indicerelatedinstumentlist.Count() > 0)
                {
                    List<Indice_RelatedInstument> SecondCountryDatalist = new List<Indice_RelatedInstument>();
                    foreach (Indice_RelatedInstumentDto x in model.indicerelatedinstumentlist)
                    {
                        SecondCountryDatalist.Add(new Indice_RelatedInstument()
                        {
                            id = Guid.NewGuid(),
                            marketpulsindiceid = data.id,
                            label = x.label,
                            link = x.link
                        });
                    }
                    await _Context.IndiceRelatedInstuments.AddRangeAsync(SecondCountryDatalist);
                }

                if (model.indicepdfsectionlist != null && model.indicepdfsectionlist.Count() > 0)
                {
                    List<Indices_PDFSection> PDFSectionlist = new List<Indices_PDFSection>();
                    foreach (Indices_PDFSectionDto x in model.indicepdfsectionlist)
                    {
                        PDFSectionlist.Add(new Indices_PDFSection()
                        {
                            id = Guid.NewGuid(),
                            marketpulsindiceid = data.id,
                            author = x.author,
                            pdfshortcodeid = x.pdfshortcodeid,
                            pdftitle = x.pdftitle,
                            shortdescription = x.shortdescription
                        });
                    }
                    await _Context.IndicePDFSections.AddRangeAsync(PDFSectionlist);
                }

                if (model.indicefundamentalnewssectionlist != null && model.indicefundamentalnewssectionlist.Count() > 0)
                {
                    List<Indice_FundamentalNewsSection> FundamentalNewsSectionlist = new List<Indice_FundamentalNewsSection>();
                    foreach (Indice_FundamentalNewsSectionDto x in model.indicefundamentalnewssectionlist)
                    {
                        Indice_FundamentalNewsSection _fundamentalNewsSection = new Indice_FundamentalNewsSection()
                        {
                            maintitle = x.maintitle,
                            script = x.script,
                            id = Guid.NewGuid(),
                            marketpulsindiceid = data.id
                        };


                        if (x.newsmaincontentlist != null && x.newsmaincontentlist.Count() > 0)
                        {
                            List<Indice_NewsMainContent> NewsMainContentlist = new List<Indice_NewsMainContent>();
                            foreach (Indice_NewsMainContentDto xn in x.newsmaincontentlist)
                            {
                                NewsMainContentlist.Add(new Indice_NewsMainContent()
                                {
                                    link = xn.link,
                                    title = xn.title,
                                    description = xn.description,
                                    id = Guid.NewGuid(),
                                    marketpulsindiceid = data.id,
                                    fundamentalnewssectionid = _fundamentalNewsSection.id,
                                    newsmaincontentfilecontenttype = xn.newsmaincontentfilecontenttype,
                                    newsmaincontentfilename = xn.newsmaincontentfilename,
                                    newsmaincontentfilepath = xn.newsmaincontentfilepath
                                });
                            }
                            await _Context.IndiceNewsMainContents.AddRangeAsync(NewsMainContentlist);
                        }
                        FundamentalNewsSectionlist.Add(_fundamentalNewsSection);
                    }
                    await _Context.IndiceFundamentalNewsSections.AddRangeAsync(FundamentalNewsSectionlist);
                }

                if (model.indicealternateindicelist != null && model.indicealternateindicelist.Count() > 0)
                {
                    List<Indice_AlternateIndice> FlexibleBlocklist = new List<Indice_AlternateIndice>();
                    foreach (Indice_AlternateIndiceDto x in model.indicealternateindicelist)
                    {
                        FlexibleBlocklist.Add(new Indice_AlternateIndice()
                        {
                            id = Guid.NewGuid(),
                            marketpulsindiceid = data.id,
                            link = x.link,
                            name = x.name
                        });
                    }
                    await _Context.IndiceAlternateIndices.AddRangeAsync(FlexibleBlocklist);
                }

                if (model.indicechildindicelist != null && model.indicechildindicelist.Count() > 0)
                {
                    List<Indice_ChildIndice> ChildIndicelist = new List<Indice_ChildIndice>();
                    foreach (Indice_ChildIndiceDto x in model.indicechildindicelist)
                    {
                        ChildIndicelist.Add(new Indice_ChildIndice()
                        {
                            id = Guid.NewGuid(),
                            marketpulsindiceid = data.id,
                            link = x.link,
                            name = x.name
                        });
                    }
                    await _Context.IndiceChildIndices.AddRangeAsync(ChildIndicelist);
                }

                if (model.indicesectorrepresentedlist != null && model.indicesectorrepresentedlist.Count() > 0)
                {
                    List<Indice_SectorRepresented> FlexibleBlocklist = new List<Indice_SectorRepresented>();
                    foreach (Indice_SectorRepresentedDto x in model.indicesectorrepresentedlist)
                    {
                        FlexibleBlocklist.Add(new Indice_SectorRepresented()
                        {
                            id = Guid.NewGuid(),
                            marketpulsindiceid = data.id,
                            link = x.link,
                            label = x.label
                        });
                    }
                    await _Context.IndiceSectorRepresenteds.AddRangeAsync(FlexibleBlocklist);
                }


                if (model.indicelistedexchangelist != null && model.indicelistedexchangelist.Count() > 0)
                {
                    List<Indice_ListedExchange> FlexibleBlocklist = new List<Indice_ListedExchange>();
                    foreach (Indice_ListedExchangeDto x in model.indicelistedexchangelist)
                    {
                        FlexibleBlocklist.Add(new Indice_ListedExchange()
                        {
                            id = Guid.NewGuid(),
                            marketpulsindiceid = data.id,
                            link = x.link,
                            label = x.label
                        });
                    }
                    await _Context.IndiceListedExchanges.AddRangeAsync(FlexibleBlocklist);
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
        public async Task<SystemMessageModel> GetIndiceItem(IndiceFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                List<IndiceDto> datas;
                IQueryable<Indice> query = _Context.Indices;

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
                                .Take(PageRowCount).Select(x => new IndiceDto()
                                {
                                    id = x.id,
                                    categoryid = x.categoryid,
                                    createdatetime = x.createdatetime,
                                    creatoruserid = x.creatoruserid,
                                    price = x.price,
                                    isvisible = x.isvisible,
                                    courseleveltypeId = x.courseleveltypeId,
                                    coursetitle = x.coursetitle,
                                    maindescription = x.maindescription,
                                    instrumentname = x.instrumentname,
                                    fundamentalheading = x.fundamentalheading,
                                    technicalheading = x.technicalheading,
                                    marketsessiontitle = x.marketsessiontitle,
                                    marketsessionscript = x.marketsessionscript,
                                    marketsentimentstitle = x.marketsentimentstitle,
                                    marketsentimentsscript = x.marketsentimentsscript,
                                    privatenotes = x.privatenotes,
                                    excerpt = x.excerpt,
                                    author = x.author,
                                    chart = x.chart,
                                    indicesinformations_alltimehigh = x.indicesinformations_alltimehigh,
                                    relatedresorces = x.relatedresorces,
                                    parentindex = x.parentindex,
                                    indicesinformations_alltimelow = x.indicesinformations_alltimelow,
                                    indicesinformations_centralbank = x.indicesinformations_centralbank,
                                    indicesinformations_countriesrepresented = x.indicesinformations_countriesrepresented,
                                    indicesinformations_nickname = x.indicesinformations_nickname,
                                    indicesinformations_pricetoearningratio = x.indicesinformations_pricetoearningratio,
                                    indicesinformations_relatedconstituents = x.indicesinformations_relatedconstituents,
                                    indicesinformations_warketcapitalization = x.indicesinformations_warketcapitalization,
                                    indicesinformations_weightageoflargestconstituent = x.indicesinformations_weightageoflargestconstituent,
                                    indicesinformations_weightageoftop5constituents = x.indicesinformations_weightageoftop5constituents,
                                    indicesinformations_weightingmethodology = x.indicesinformations_weightingmethodology,
                                    indicesinformations_yeartodatereturn = x.indicesinformations_yeartodatereturn,
                                    newstickerimportant = x.newstickerimportant,
                                    newstickerupdate = x.newstickerupdate,
                                    newstickernew = x.newstickernew,
                                    maindescription_filepath = x.maindescription_filepath,
                                    maindescription_filename = x.maindescription_filename,
                                    maindescription_filecontenttype = x.maindescription_filecontenttype,


                                }).ToListAsync();

                foreach (IndiceDto data in datas)
                {
                    data.indicerelatedinstumentlist = await _Context.IndiceRelatedInstuments.Where(x => x.marketpulsindiceid == data.id).Select(x => new Indice_RelatedInstumentDto() { link = x.link, label = x.label, id = x.id, marketpulsindiceid = x.marketpulsindiceid }).ToListAsync();

                    data.indicetechnicaltabslist = await _Context.IndiceTechnicalTabss.Where(x => x.marketpulsindiceid == data.id).Select(x => new Indice_TechnicalTabsDto() { tabtitle = x.tabtitle, script = x.script, id = x.id, marketpulsindiceid = x.marketpulsindiceid }).ToListAsync();

                    foreach (Indice_TechnicalTabsDto tabitem in data.indicetechnicaltabslist)
                    {
                        tabitem.technicalbreakingnewslist = await _Context.IndiceTechnicalTabsTechnicalBreakingNewss.Where(x => x.marketpulsindiceid == data.id && x.technicaltabsid == tabitem.id).Select(x => new Indice_TechnicalTabs_TechnicalBreakingNewsDto() { title = x.title, link = x.link, id = x.id, marketpulsindiceid = x.marketpulsindiceid, technicaltabsid = x.technicaltabsid, description = x.description, newsmaincontentfilecontenttype = x.newsmaincontentfilecontenttype, newsmaincontentfilename = x.newsmaincontentfilename, newsmaincontentfilepath = x.newsmaincontentfilepath }).ToListAsync();
                    }

                    data.indiceurlsectionlist = await _Context.IndiceURLSections.Where(x => x.marketpulsindiceid == data.id).Select(x => new Indice_URLSectionDto() { url = x.url, urltitle = x.urltitle, marketpulsindiceid = x.marketpulsindiceid, id = x.id }).ToListAsync();

                    data.indicepdfsectionlist = await _Context.IndicePDFSections.Where(x => x.marketpulsindiceid == data.id).Select(x => new Indices_PDFSectionDto() { author = x.author, pdfshortcodeid = x.pdfshortcodeid, pdftitle = x.pdftitle, shortdescription = x.shortdescription, id = x.id, marketpulsindiceid = x.marketpulsindiceid }).ToListAsync();

                    data.indicefundamentalnewssectionlist = await _Context.IndiceFundamentalNewsSections.Where(x => x.marketpulsindiceid == data.id).Select(x => new Indice_FundamentalNewsSectionDto() { id = x.id, marketpulsindiceid = x.marketpulsindiceid, maintitle = x.maintitle, script = x.script }).ToListAsync();

                    foreach (Indice_FundamentalNewsSectionDto nitem in data.indicefundamentalnewssectionlist)
                    {
                        nitem.newsmaincontentlist = await _Context.IndiceNewsMainContents.Where(x => x.marketpulsindiceid == data.id && x.fundamentalnewssectionid == nitem.id).Select(x => new Indice_NewsMainContentDto() { id = x.id, marketpulsindiceid = x.marketpulsindiceid, description = x.description, link = x.link, fundamentalnewssectionid = x.fundamentalnewssectionid, title = x.title, newsmaincontentfilecontenttype = x.newsmaincontentfilecontenttype, newsmaincontentfilename = x.newsmaincontentfilename, newsmaincontentfilepath = x.newsmaincontentfilepath }).ToListAsync();
                    }

                    data.indicesectorrepresentedlist = await _Context.IndiceSectorRepresenteds.Where(x => x.marketpulsindiceid == data.id).Select(x => new Indice_SectorRepresentedDto() { id = x.id, marketpulsindiceid = x.marketpulsindiceid, link = x.link, label = x.label }).ToListAsync();

                    data.indicechildindicelist = await _Context.IndiceChildIndices.Where(x => x.marketpulsindiceid == data.id).Select(x => new Indice_ChildIndiceDto() { id = x.id, marketpulsindiceid = x.marketpulsindiceid, link = x.link, name = x.name }).ToListAsync();


                    data.indicealternateindicelist = await _Context.IndiceAlternateIndices.Where(x => x.marketpulsindiceid == data.id).Select(x => new Indice_AlternateIndiceDto() { id = x.id, marketpulsindiceid = x.marketpulsindiceid, link = x.link, name = x.name }).ToListAsync();

                    data.indicelistedexchangelist = await _Context.IndiceListedExchanges.Where(x => x.marketpulsindiceid == data.id).Select(x => new Indice_ListedExchangeDto() { id = x.id, marketpulsindiceid = x.marketpulsindiceid, link = x.link, label = x.label }).ToListAsync();
                }



                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = datas, Meta = new { pagecount = pagecount, rowcount = PageRowCount, pageindex = pageIndex } };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> UpdateIndiceItem(IndiceDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                if (model.id == null)
                    return new SystemMessageModel() { MessageCode = -102, MessageDescription = "Id is Wrong" };

                Indice data = await _Context.Indices.FindAsync(model.id);

                if (data == null)
                    return new SystemMessageModel() { MessageCode = -103, MessageDescription = "data not find" };

                if (await _Context.Indices.Where(x => x.categoryid == model.categoryid).AnyAsync())
                {
                    return new SystemMessageModel() { MessageCode = -103, MessageDescription = "For this category save before use update service", MessageData = model };
                }
                //await DeleteIndiceItem(new IndiceFilterDto() {id = model.id, categoryid = model.categoryid }, userlogin, processId, clientip, hosturl);


                data.price = model.price ?? 0;
                data.isvisible = model.isvisible ?? true;
                data.courseleveltypeId = model.courseleveltypeId ?? 0;
                data.coursetitle = model.coursetitle;
                data.instrumentname = model.instrumentname;
                data.fundamentalheading = model.fundamentalheading;
                data.technicalheading = model.technicalheading;
                data.marketsessiontitle = model.marketsessiontitle;
                data.marketsessionscript = model.marketsessionscript;
                data.marketsentimentstitle = model.marketsentimentstitle;
                data.marketsentimentsscript = model.marketsentimentsscript;
                data.chart = model.chart;
                data.indicesinformations_alltimehigh = model.indicesinformations_alltimehigh;
                data.indicesinformations_alltimelow = model.indicesinformations_alltimelow;
                data.indicesinformations_centralbank = model.indicesinformations_centralbank;
                data.indicesinformations_countriesrepresented = model.indicesinformations_countriesrepresented;
                data.indicesinformations_nickname = model.indicesinformations_nickname;
                data.indicesinformations_pricetoearningratio = model.indicesinformations_pricetoearningratio;
                data.indicesinformations_relatedconstituents = model.indicesinformations_relatedconstituents;
                data.indicesinformations_warketcapitalization = model.indicesinformations_warketcapitalization;
                data.indicesinformations_weightageoflargestconstituent = model.indicesinformations_weightageoflargestconstituent;
                data.indicesinformations_weightageoftop5constituents = model.indicesinformations_weightageoftop5constituents;
                data.indicesinformations_weightingmethodology = model.indicesinformations_weightingmethodology;
                data.indicesinformations_yeartodatereturn = model.indicesinformations_yeartodatereturn;
                data.maindescription_filecontenttype = model.maindescription_filecontenttype;
                data.newstickerimportant = model.newstickerimportant;
                data.parentindex = model.parentindex;
                data.newstickernew = model.newstickernew;
                data.maindescription = model.maindescription;
                data.newstickerupdate = model.newstickerupdate;
                data.relatedresorces = model.relatedresorces;
                data.privatenotes = model.privatenotes;
                data.excerpt = model.excerpt;
                data.author = model.author;


                _Context.Indices.Update(data);


                //if (model.URLSectionlist != null && model.URLSectionlist.Count() > 0)
                //{
                //    List<URLSection> URLSectionlist = new List<URLSection>();
                //    foreach (URLSectionDto x in model.URLSectionlist)
                //    {
                //        URLSectionlist.Add(new URLSection()
                //        {
                //            url = x.url,
                //            urltitle = x.urltitle,
                //            id = Guid.NewGuid(),
                //            marketpulsIndiceid = data.id
                //        });
                //    }
                //    await _Context.URLSections.AddRangeAsync(URLSectionlist);
                //}
                //if (model.TechnicalTabslist != null && model.TechnicalTabslist.Count() > 0)
                //{
                //    List<TechnicalTabs> TechnicalTabslist = new List<TechnicalTabs>();
                //    foreach (TechnicalTabsDto x in model.TechnicalTabslist)
                //    {
                //        TechnicalTabs _technicalTabs = new TechnicalTabs()
                //        {
                //            maintitle = x.maintitle,
                //            script = x.script,
                //            id = Guid.NewGuid(),
                //            marketpulsIndiceid = data.id
                //        };

                //        if (x.TechnicalBreakingNewslist != null && x.TechnicalBreakingNewslist.Count() > 0)
                //        {
                //            List<TechnicalBreakingNews> TechnicalBreakingNewslist = new List<TechnicalBreakingNews>();
                //            foreach (TechnicalBreakingNewsDto xn in x.TechnicalBreakingNewslist)
                //            {
                //                TechnicalBreakingNewslist.Add(new TechnicalBreakingNews()
                //                {
                //                    maintitle = xn.maintitle,
                //                    script = xn.script,
                //                    id = Guid.NewGuid(),
                //                    technicaltabid = _technicalTabs.id,
                //                    marketpulsIndiceid = data.id
                //                });
                //            }
                //            await _Context.TechnicalBreakingNewss.AddRangeAsync(TechnicalBreakingNewslist);
                //        }
                //        TechnicalTabslist.Add(_technicalTabs);

                //    }
                //    await _Context.TechnicalTabss.AddRangeAsync(TechnicalTabslist);
                //}


                //if (model.SecondCountryDatalist != null && model.SecondCountryDatalist.Count() > 0)
                //{
                //    List<SecondCountryData> SecondCountryDatalist = new List<SecondCountryData>();
                //    foreach (SecondCountryDataDto x in model.SecondCountryDatalist)
                //    {
                //        SecondCountryDatalist.Add(new SecondCountryData()
                //        {
                //            id = Guid.NewGuid(),
                //            marketpulsIndiceid = data.id,
                //            avragedaily = x.avragedaily,
                //            centralbank = x.centralbank,
                //            countries = x.countries,
                //            nickname = x.nickname
                //        });
                //    }
                //    await _Context.SecondCountryDatas.AddRangeAsync(SecondCountryDatalist);
                //}

                //if (model.PDFSectionlist != null && model.PDFSectionlist.Count() > 0)
                //{
                //    List<PDFSection> PDFSectionlist = new List<PDFSection>();
                //    foreach (PDFSectionDto x in model.PDFSectionlist)
                //    {
                //        PDFSectionlist.Add(new PDFSection()
                //        {
                //            id = Guid.NewGuid(),
                //            marketpulsIndiceid = data.id,
                //            author = x.author,
                //            pdfshortcodeid = x.pdfshortcodeid,
                //            pdftitle = x.pdftitle,
                //            shortdescription = x.shortdescription
                //        });
                //    }
                //    await _Context.PDFSections.AddRangeAsync(PDFSectionlist);
                //}


                //if (model.FundamentalNewsSectionlist != null && model.FundamentalNewsSectionlist.Count() > 0)
                //{
                //    List<FundamentalNewsSection> FundamentalNewsSectionlist = new List<FundamentalNewsSection>();
                //    foreach (FundamentalNewsSectionDto x in model.FundamentalNewsSectionlist)
                //    {
                //        FundamentalNewsSection _fundamentalNewsSection = new FundamentalNewsSection()
                //        {
                //            maintitle = x.maintitle,
                //            script = x.script,
                //            id = Guid.NewGuid(),
                //            marketpulsIndiceid = data.id
                //        };


                //        if (x.NewsMainContentlist != null && x.NewsMainContentlist.Count() > 0)
                //        {
                //            List<NewsMainContent> NewsMainContentlist = new List<NewsMainContent>();
                //            foreach (NewsMainContentDto xn in x.NewsMainContentlist)
                //            {
                //                NewsMainContentlist.Add(new NewsMainContent()
                //                {
                //                    maintitle = xn.maintitle,
                //                    script = xn.script,
                //                    id = Guid.NewGuid(),
                //                    marketpulsIndiceid = data.id,
                //                    fundamentalnewssectionid = _fundamentalNewsSection.id
                //                });
                //            }
                //            await _Context.NewsMainContents.AddRangeAsync(NewsMainContentlist);
                //        }
                //        FundamentalNewsSectionlist.Add(_fundamentalNewsSection);
                //    }
                //    await _Context.FundamentalNewsSections.AddRangeAsync(FundamentalNewsSectionlist);
                //}

                //if (model.FlexibleBlocklist != null && model.FlexibleBlocklist.Count() > 0)
                //{
                //    List<FlexibleBlock> FlexibleBlocklist = new List<FlexibleBlock>();
                //    foreach (FlexibleBlockDto x in model.FlexibleBlocklist)
                //    {
                //        FlexibleBlocklist.Add(new FlexibleBlock()
                //        {
                //            id = Guid.NewGuid(),
                //            marketpulsIndiceid = data.id,
                //            countries = x.countries,
                //            dailyavrage = x.dailyavrage,
                //            highslows = x.highslows,
                //            MainTitle = x.MainTitle,
                //            pairsthatcorrelate = x.pairsthatcorrelate,
                //            pairtype = x.pairtype
                //        });
                //    }
                //    await _Context.FlexibleBlocks.AddRangeAsync(FlexibleBlocklist);
                //}

                //if (model.FirstCountryDatalist != null && model.FirstCountryDatalist.Count() > 0)
                //{
                //    List<FirstCountryData> FirstCountryDatalist = new List<FirstCountryData>();
                //    foreach (FirstCountryDataDto x in model.FirstCountryDatalist)
                //    {
                //        FirstCountryDatalist.Add(new FirstCountryData()
                //        {
                //            id = Guid.NewGuid(),
                //            marketpulsIndiceid = data.id,
                //            countries = x.countries,
                //            avragedaily = x.avragedaily,
                //            centralbank = x.centralbank,
                //            nickname = x.nickname
                //        });
                //    }
                //    await _Context.FirstCountryDatas.AddRangeAsync(FirstCountryDatalist);
                //}

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


        public async Task<SystemMessageModel> DeleteIndiceItem(IndiceFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                Indice data;
                IQueryable<Indice> query = _Context.Indices;

                if (model.id != null)
                    query = query.Where(x => x.id == model.id);

                if (model.categoryid != null)
                    query = query.Where(x => x.categoryid == model.categoryid);

                data = await query.FirstOrDefaultAsync();

                _Context.Indices.Remove(data);


                List<Indice_AlternateIndice> AlternateIndicelist = await _Context.IndiceAlternateIndices.Where(x => x.marketpulsindiceid == data.id).ToListAsync();
                _Context.IndiceAlternateIndices.RemoveRange(AlternateIndicelist);

                List<Indice_ChildIndice> ChildIndicelist = await _Context.IndiceChildIndices.Where(x => x.marketpulsindiceid == data.id).ToListAsync();
                _Context.IndiceChildIndices.RemoveRange(ChildIndicelist);


                List<Indice_SectorRepresented> SectorRepresentedlist = await _Context.IndiceSectorRepresenteds.Where(x => x.marketpulsindiceid == data.id).ToListAsync();
                _Context.IndiceSectorRepresenteds.RemoveRange(SectorRepresentedlist);


                List<Indice_URLSection> URLSectionlist = await _Context.IndiceURLSections.Where(x => x.marketpulsindiceid == data.id).ToListAsync();
                _Context.IndiceURLSections.RemoveRange(URLSectionlist);


                List<Indices_PDFSection> PDFSectionlist = await _Context.IndicePDFSections.Where(x => x.marketpulsindiceid == data.id).ToListAsync();
                _Context.IndicePDFSections.RemoveRange(PDFSectionlist);

                List<Indice_FundamentalNewsSection> NewsMainContentlist = await _Context.IndiceFundamentalNewsSections.Where(x => x.marketpulsindiceid == data.id).ToListAsync();
                _Context.IndiceFundamentalNewsSections.RemoveRange(NewsMainContentlist);


                List<Indice_NewsMainContent> FundamentalNewsSectionlist = await _Context.IndiceNewsMainContents.Where(x => x.marketpulsindiceid == data.id).ToListAsync();
                _Context.IndiceNewsMainContents.RemoveRange(FundamentalNewsSectionlist);


                List<Indice_TechnicalTabs_TechnicalBreakingNews> FlexibleBlocklist = await _Context.IndiceTechnicalTabsTechnicalBreakingNewss.Where(x => x.marketpulsindiceid == data.id).ToListAsync();
                _Context.IndiceTechnicalTabsTechnicalBreakingNewss.RemoveRange(FlexibleBlocklist);

                List<Indice_TechnicalTabs> FirstCountryDatalist = await _Context.IndiceTechnicalTabss.Where(x => x.marketpulsindiceid == data.id).ToListAsync();
                _Context.IndiceTechnicalTabss.RemoveRange(FirstCountryDatalist);

                List<Indice_ListedExchange> ListedExchangelist = await _Context.IndiceListedExchanges.Where(x => x.marketpulsindiceid == data.id).ToListAsync();
                _Context.IndiceListedExchanges.RemoveRange(ListedExchangelist);

                List<Indice_RelatedInstument> RelatedInstumentlist = await _Context.IndiceRelatedInstuments.Where(x => x.marketpulsindiceid == data.id).ToListAsync();
                _Context.IndiceRelatedInstuments.RemoveRange(RelatedInstumentlist);


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
