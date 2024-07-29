using Base.Common.Enums;
using DataLayers;
using DocumentFormat.OpenXml.InkML;
using Entities.DBEntities;
using Entities.Dtos;
using Entities.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NextTradeAPIs.Dtos;
using System.Diagnostics;

namespace NextTradeAPIs.Services
{
    public class CommunityGroupServices
    {
        SBbContext _Context { get; set; }
        LogSBbContext _LogContext { get; set; }
        SystemLogServices _systemLogServices;
        private readonly IConfiguration _config;
        public CommunityGroupServices(SBbContext context, LogSBbContext logcontext, IConfiguration config, SystemLogServices systemLogServices)
        {
            _Context = context;
            _LogContext = logcontext;
            _config = config;
            _systemLogServices = systemLogServices;
        }

        public async Task<SystemMessageModel> CreateCommunityGroup(CommunityGroupDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                CommunityGroup data = new CommunityGroup()
                {
                    Id = Guid.NewGuid(),
                    grouptypeId = (model.grouptypeId == 0) ? model.grouptypeId : (int)GroupTypes.PublicGroup,
                    owneruserid = userlogin.userid,
                    createdatetime = DateTime.Now,
                    description = model.description,
                    title = model.title
                };

                _Context.CommunityGroups.Add(data);
                await _Context.SaveChangesAsync();

                model.Id = data.Id;

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

        public async Task<SystemMessageModel> EditCommunityGroup(CommunityGroupDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                CommunityGroup data = await _Context.CommunityGroups.FindAsync(model.Id);
                if (data != null)
                {
                    data.groupimage = model.coverimage;

                    _Context.CommunityGroups.Update(data);
                    await _Context.SaveChangesAsync();
                }

                CommunityGroup data = new CommunityGroup()
                {
                    Id = Guid.NewGuid(),
                    grouptypeId = (model.grouptypeId == 0) ? model.grouptypeId : (int)GroupTypes.PublicGroup,
                    owneruserid = userlogin.userid,
                    createdatetime = DateTime.Now,
                    description = model.description,
                    title = model.title
                };

                _Context.CommunityGroups.Add(data);
                await _Context.SaveChangesAsync();

                model.Id = data.Id;

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


        public async Task<SystemMessageModel> GetCommunityGroup(GroupSearchFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<CommunityGroup> query = _Context.CommunityGroups;
                if (model.owneruserid != null)
                    query = query.Where(x => x.owneruserid == model.owneruserid);

                if (model.grouptypeId != null)
                    query = query.Where(x => x.grouptypeId == model.grouptypeId);

                if (model.id != null)
                    query = query.Where(x => x.Id == model.id);

                if (!string.IsNullOrEmpty(model.title))
                    query = query.Where(x => x.title.Contains(model.title));

                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 10 : (int)model.rowcount;


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
                                case "title":
                                    query = query.OrderBy(x => x.title);
                                    break;
                                case "grouptypeId":
                                    query = query.OrderBy(x => x.grouptypeId);
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
                                case "title":
                                    query = query.OrderByDescending(x => x.title);
                                    break;
                                case "grouptypeId":
                                    query = query.OrderByDescending(x => x.grouptypeId);
                                    break;
                            };
                        }
                    }
                }

                List<CommunityGroupDto> datas = await query
                                .Skip(pageIndex - 1)
                                .Take(PageRowCount)
                                .Include(x => x.grouptype)
                                .Include(x => x.owneruser)
                                .Select(x => new CommunityGroupDto()
                                {
                                    Id = x.Id,
                                    owneruserid = x.owneruserid,
                                    grouptypeId = x.grouptypeId,
                                    createdatetime = x.createdatetime,
                                    description = x.description,
                                    title = x.title,
                                    ownerusername = x.owneruser.Username,
                                    grouptypename = x.grouptype.name,
                                }).ToListAsync();
                if (model.showdetail)
                {
                    foreach (var data in datas)
                    {
                        data.membercount = await _Context.CommunityGroupMembers.Where(x => x.communitygroupId == data.Id).CountAsync();
                        data.signalchannelcount = await _Context.SignalChannels.Where(x => x.communitygroupId == data.Id).CountAsync();
                        data.messagecount = await _Context.ForumMessages.Where(x => x.communitygroupid == data.Id).CountAsync();
                    }
                }
                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = datas };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> GetTopCommunityGroup(GroupSearchFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<CommunityGroup> query = _Context.CommunityGroups;
                if (model.owneruserid != null)
                    query = query.Where(x => x.owneruserid == model.owneruserid);

                if (model.grouptypeId != null)
                    query = query.Where(x => x.grouptypeId == model.grouptypeId);

                if (model.id != null)
                    query = query.Where(x => x.Id == model.id);


                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 10 : (int)model.rowcount;

                List<Guid> topCommunityGroupIds = await _Context.CommunityGroupMembers
                                                          .GroupBy(m => m.communitygroupId)
                                                          .OrderByDescending(g => g.Count())
                                                          .Take(PageRowCount)
                                                          .Select(g => g.Key).ToListAsync();

                if (topCommunityGroupIds != null && topCommunityGroupIds.Count > 0)
                {
                    query = query.Where(x => topCommunityGroupIds.Contains(x.Id));
                }

                List<CommunityGroupDto> datas = await query.Skip(pageIndex - 1).Take(PageRowCount)
                                .Include(x => x.grouptype)
                                .Include(x => x.owneruser)
                                .Select(x => new CommunityGroupDto()
                                {
                                    Id = x.Id,
                                    owneruserid = x.owneruserid,
                                    grouptypeId = x.grouptypeId,
                                    createdatetime = x.createdatetime,
                                    description = x.description,
                                    title = x.title,
                                    ownerusername = x.owneruser.Username,
                                    grouptypename = x.grouptype.name,
                                }).ToListAsync();
                if (model.showdetail)
                {
                    foreach (var data in datas)
                    {
                        data.membercount = await _Context.CommunityGroupMembers.Where(x => x.communitygroupId == data.Id).CountAsync();
                        data.signalchannelcount = await _Context.SignalChannels.Where(x => x.communitygroupId == data.Id).CountAsync();
                        data.messagecount = await _Context.ForumMessages.Where(x => x.communitygroupid == data.Id).CountAsync();
                    }
                }
                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = datas };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> DeleteCommunityGroup(GroupSearchFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                CommunityGroup data = await _Context.CommunityGroups.FindAsync(model.id);

                if (data != null)
                {
                    if (userlogin.UserTypeId == (long)UserTypes.Admin || userlogin.UserTypeId == (long)UserTypes.SuperAdmin || data.owneruserid == userlogin.userid)
                    {
                        List<CommunityGroupMember> communityGroupMember = await _Context.CommunityGroupMembers.Where(x => x.communitygroupId == data.Id).ToListAsync();

                        if (communityGroupMember != null && communityGroupMember.Count > 0)
                        {
                            _Context.CommunityGroupMembers.RemoveRange(communityGroupMember);
                        }
                        _Context.CommunityGroups.Remove(data);

                        try
                        {
                            string _LogPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\communitygroups\\" + data.Id.ToString().Replace("-", "") + "\\";
                            if (!Directory.Exists(_LogPath))
                            {
                                Directory.Delete(_LogPath, true);
                            }
                        }
                        catch { }
                        await _Context.SaveChangesAsync();
                        message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = data };
                    }
                    else
                    {
                        message = new SystemMessageModel()
                        {
                            MessageCode = -401,
                            MessageDescription = "you do not have permission",
                            MessageData = data
                        };
                    }
                }
                else
                {
                    message = new SystemMessageModel()
                    {
                        MessageCode = -503,
                        MessageDescription = "data is wrong",
                        MessageData = data
                    };
                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }


        public async Task<SystemMessageModel> UpdateCommunityGroupImage(CommunityGroupDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                CommunityGroup data = await _Context.CommunityGroups.FindAsync(model.Id);
                if (data != null)
                {
                    data.groupimage = model.coverimage;

                    _Context.CommunityGroups.Update(data);
                    await _Context.SaveChangesAsync();
                }


                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request success", MessageData = model };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> UpdateCommunityCoverImage(CommunityGroupDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                CommunityGroup data = await _Context.CommunityGroups.FindAsync(model.Id);
                if (data != null)
                {
                    data.coverimage = model.coverimage;
                }

                _Context.CommunityGroups.Update(data);
                await _Context.SaveChangesAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request success", MessageData = model };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }


        public async Task<SystemMessageModel> GetCommunityGroupImageURL(Guid Id)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                string _LogPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\communitygroups\\";
                if (!Directory.Exists(_LogPath))
                {
                    Directory.CreateDirectory(_LogPath);
                }

                CommunityGroup data = await _Context.CommunityGroups.FindAsync(Id);

                _LogPath += data.Id.ToString().Replace("-","") + "\\";
                if (!Directory.Exists(_LogPath))
                {
                    Directory.CreateDirectory(_LogPath);
                }

                string filePath = AppDomain.CurrentDomain.BaseDirectory + "/communitygroups/" + data.Id.ToString().Replace("-", "") + "/" + "groupimage.png";
                Uri uri = new Uri("/communitygroups/" + data.Id.ToString().Replace("-", "") + "/" + "groupimage.png", UriKind.Relative);

                if (data != null && data.groupimage != null)
                {
                    _LogPath += "groupimage.png";
                    if (!File.Exists(_LogPath))
                    {
                        File.WriteAllBytes(_LogPath, data.groupimage);
                        
                    }
                    return new SystemMessageModel() { MessageCode=200, MessageDescription = "Request Compeleted Successfully", MessageData = uri.ToString() } ;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, "", "", methodpath, LogTypes.SystemError);
            }
            return null;
        }

        public async Task<SystemMessageModel> GetCommunityCoverImageURL(Guid Id)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                string _LogPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\communitygroups\\";
                if (!Directory.Exists(_LogPath))
                {
                    Directory.CreateDirectory(_LogPath);
                }

                CommunityGroup data = await _Context.CommunityGroups.FindAsync(Id);

                _LogPath += data.Id.ToString().Replace("-", "") + "\\";
                if (!Directory.Exists(_LogPath))
                {
                    Directory.CreateDirectory(_LogPath);
                }

                string filePath = AppDomain.CurrentDomain.BaseDirectory + "/communitygroups/" + data.Id.ToString().Replace("-", "") + "/" + "coverimage.png";
                Uri uri = new Uri("/communitygroups/" + data.Id.ToString().Replace("-", "") + "/" + "coverimage.png", UriKind.Relative);

                if (data != null && data.coverimage != null)
                {
                    _LogPath += "coverimage.png";
                    if (!File.Exists(_LogPath))
                    {
                        File.WriteAllBytes(_LogPath, data.coverimage);

                    }
                    return new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = uri.ToString() };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, "", "", methodpath, LogTypes.SystemError);
            }
            return null;
        }

        public async Task<byte[]> GetCommunityGroupImage(Guid Id)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                CommunityGroup data = await _Context.CommunityGroups.FindAsync(Id);

                if (data != null && data.groupimage != null)
                {
                    return data.groupimage;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, "", "", methodpath, LogTypes.SystemError);
            }
            return null;
        }

        public async Task<byte[]> GetCommunityCoverImage(Guid Id)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                CommunityGroup data = await _Context.CommunityGroups.FindAsync(Id);

                if (data != null && data.coverimage != null)
                {
                    return data.coverimage;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, "", "", methodpath, LogTypes.SystemError);
            }
            return null;
        }

        public async Task<SystemMessageModel> AcceptCommunityGroupMember(CommunityGroupMemberDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 130000;

            try
            {
                CommunityGroupMember data = await _Context.CommunityGroupMembers.Where(x => x.communitygroupId == model.communitygroupId && x.userId == model.userId).SingleOrDefaultAsync();
                if (data != null)
                {
                    if ((userlogin.UserTypeId == (long)UserTypes.Admin) || (userlogin.UserTypeId == (long)UserTypes.SuperAdmin))
                    {
                        data.isaccepted = true;
                    }
                    else
                    {
                        CommunityGroup ccproup = await _Context.CommunityGroups.Where(x => x.Id == data.communitygroupId && x.owneruserid == userlogin.userid).SingleOrDefaultAsync();
                        if (ccproup != null)
                        {
                            data.isaccepted = true;
                        }
                    }
                    _Context.CommunityGroupMembers.Update(data);
                    await _Context.SaveChangesAsync();
                    message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request success", MessageData = model };
                }
                else
                {
                    message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "this user not requested", MessageData = model };
                }

            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> GetCommunityGroupMember(CommunityGroupMemberDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                IQueryable<CommunityGroupMember> query = _Context.CommunityGroupMembers;

                if (model.communitygroupId != null)
                    query = query.Where(x => x.communitygroupId == model.communitygroupId);
                if (model.isaccepted != null)
                    query = query.Where(x => x.isaccepted == model.isaccepted);

                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 10 : (int)model.rowcount;


                List<CommunityGroupMemberDto> datas = await query.Skip(pageIndex - 1).Take(PageRowCount)
                                                    .Include(x => x.communitygroup)
                                                    .Include(x => x.user)
                                           .Select(x => new CommunityGroupMemberDto()
                                           {
                                               accepteddatetime = x.accepteddatetime,
                                               communitygroupId = x.communitygroupId,
                                               Id = x.Id,
                                               isaccepted = x.isaccepted,
                                               requestdatetime = x.requestdatetime,
                                               userId = x.userId,
                                               username = x.user.Username,
                                               communitygrouptitle = x.communitygroup.title
                                           }).ToListAsync();

                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = datas };
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> AddCommunityGroupMember(CommunityGroupMemberDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                long requesteduserid = (long)model.userId;
                if (requesteduserid == null)
                    requesteduserid = userlogin.userid;
                CommunityGroup group = await _Context.CommunityGroups.FindAsync(model.communitygroupId);
                CommunityGroupMember data = await _Context.CommunityGroupMembers.Where(x => x.communitygroupId == model.communitygroupId && x.userId == requesteduserid).SingleOrDefaultAsync();

                if (data == null)
                {
                    data = new CommunityGroupMember()
                    {
                        Id = Guid.NewGuid(),
                        communitygroupId = model.communitygroupId,
                        userId = requesteduserid,
                        requestdatetime = DateTime.Now,
                        isaccepted = false
                    };
                    if (group.grouptypeId == (int)GroupTypes.PublicGroup)
                    {
                        data.isaccepted = true;
                        data.accepteddatetime = DateTime.Now;
                    }

                    _Context.CommunityGroupMembers.Add(data);
                    await _Context.SaveChangesAsync();

                    model.Id = data.Id;
                    message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = model };
                }
                else
                {
                    model.Id = data.Id;
                    if (!data.isaccepted)
                    {
                        message = new SystemMessageModel() { MessageCode = -160, MessageDescription = "You have been requested before", MessageData = model };
                    }
                    else
                    {
                        message = new SystemMessageModel() { MessageCode = -160, MessageDescription = "You are member of group", MessageData = model };
                    }
                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> DeleteCommunityGroupMember(CommunityGroupMemberDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 130000;
            CommunityGroupMember data = null;
            try
            {
                if (model.userId != null)
                {
                    data = await _Context.CommunityGroupMembers.Where(x => x.communitygroupId == model.communitygroupId && x.userId == model.userId).SingleOrDefaultAsync();
                    if (data != null)
                    {
                        if ((userlogin.UserTypeId == (long)UserTypes.Admin) || (userlogin.UserTypeId == (long)UserTypes.SuperAdmin))
                        {
                            _Context.CommunityGroupMembers.Remove(data);
                        }
                        else
                        {
                            CommunityGroup ccproup = await _Context.CommunityGroups.Where(x => x.Id == data.communitygroupId && x.owneruserid == userlogin.userid).SingleOrDefaultAsync();
                            if (ccproup != null)
                            {
                                _Context.CommunityGroupMembers.Remove(data);
                            }
                        }
                    }
                    await _Context.SaveChangesAsync();
                    message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request success", MessageData = model };
                }
                else
                {
                    data = await _Context.CommunityGroupMembers.Where(x => x.communitygroupId == model.communitygroupId && x.userId == userlogin.userid).SingleOrDefaultAsync();
                    if (data != null)
                    {
                        _Context.CommunityGroupMembers.Remove(data);
                        await _Context.SaveChangesAsync();

                        message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request success", MessageData = model };
                    }
                    else
                    {
                        message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "this user is not memeber of group", MessageData = model };
                    }
                }
            }
            catch (Exception ex)
            {
                message = new SystemMessageModel() { MessageCode = ((ServiceUrlConfig.SystemCode + SerrvieCode + 501) * -1), MessageDescription = "Error In doing Request", MessageData = ex.Message };
                string error = $"'ErrorLocation':'{methodpath}','ProccessID':'{processId}','ErrorMessage':'{JsonConvert.SerializeObject(message)}','ErrorDescription':'{JsonConvert.SerializeObject(ex)}'";
                await _systemLogServices.InsertLogs(error, processId, clientip, methodpath, LogTypes.SystemError);
            }
            return message;
        }

        public async Task<SystemMessageModel> GetUserCommunityGroup(GroupSearchFilterDto model, UserModel? userlogin, string processId, string clientip, string hosturl)
        {
            SystemMessageModel message;
            StackTrace stackTrace = new StackTrace();
            string methodpath = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + " => " + stackTrace.GetFrame(0).GetMethod().Name;
            long SerrvieCode = 129000;

            try
            {
                List<CommunityGroupMember> CommunityGroupList = await _Context.CommunityGroupMembers.Where(x => x.userId == userlogin.userid).ToListAsync();

                List<Guid> CommunityGroupListIDs = CommunityGroupList.Select(x => x.communitygroupId).ToList();
                List<Guid> AcceptedCommunityGroupListIDs = CommunityGroupList.Where(x => x.isaccepted == true).Select(x => x.communitygroupId).ToList();

                IQueryable<CommunityGroup> query = _Context.CommunityGroups;

                if (model.grouptypeId != null)
                    query = query.Where(x => x.grouptypeId == model.grouptypeId);

                if (model.id != null)
                    query = query.Where(x => x.Id == model.id);

                if (CommunityGroupListIDs != null && CommunityGroupListIDs.Count() > 0)
                    query = query.Where(x => CommunityGroupListIDs.Contains(x.Id));

                int pageIndex = (model.pageindex == null || model.pageindex == 0) ? 1 : (int)model.pageindex;
                int PageRowCount = (model.rowcount == null || model.rowcount == 0) ? 10 : (int)model.rowcount;


                List<CommunityGroupDto> datas = await query.Skip(pageIndex - 1).Take(PageRowCount)
                                .Include(x => x.grouptype)
                                .Include(x => x.owneruser)
                                .Select(x => new CommunityGroupDto()
                                {
                                    Id = x.Id,
                                    owneruserid = x.owneruserid,
                                    grouptypeId = x.grouptypeId,
                                    createdatetime = x.createdatetime,
                                    description = x.description,
                                    title = x.title,
                                    ownerusername = x.owneruser.Username,
                                    grouptypename = x.grouptype.name
                                }).ToListAsync();

                foreach (CommunityGroupDto data in datas)
                    data.isaccepted = AcceptedCommunityGroupListIDs.Contains((Guid)data.Id);

                if (model.showdetail)
                {
                    foreach (var data in datas)
                    {
                        data.membercount = await _Context.CommunityGroupMembers.Where(x => x.communitygroupId == data.Id).CountAsync();
                        data.signalchannelcount = await _Context.SignalChannels.Where(x => x.communitygroupId == data.Id).CountAsync();
                        data.messagecount = await _Context.ForumMessages.Where(x => x.communitygroupid == data.Id).CountAsync();
                    }
                }
                message = new SystemMessageModel() { MessageCode = 200, MessageDescription = "Request Compeleted Successfully", MessageData = datas };
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
