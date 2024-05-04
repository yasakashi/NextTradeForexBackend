
using System.ServiceModel;
using Entities.DBEntities;
using Entities.Dtos;

namespace NextTradeAPIs.Interfaces
{
    //[ServiceContract]
    public interface IAuthorizationService
    {
        //[OperationContract]
        Task<SystemMessageModel> GetToken(LoginLog user);

        Task<SystemMessageModel> CheckToken(string token);

        Task<SystemMessageModel> CheckUserAccess(string token, int ServiceCode);
    }
}
