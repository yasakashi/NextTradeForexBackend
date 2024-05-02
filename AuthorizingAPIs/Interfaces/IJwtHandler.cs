

using Entities.DBEntities;
using System.IdentityModel.Tokens.Jwt;

namespace AuthorizingAPIs.Interfaces
{
    public interface IJwtHandler
    {
        Task<JwtSecurityToken> Create(LoginLog user);
        Task<JwtSecurityToken> Create(LoginLog user, int expireday);
        

        Task<bool> CkeckTokenIsValid(string token);

        Task<string> GetTokenParameterValue(string token, string parametersname);
    }
}
