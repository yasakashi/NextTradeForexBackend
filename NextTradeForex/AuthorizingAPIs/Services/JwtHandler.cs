using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using AuthorizingAPIs.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using DataLayers;
using Entities.DBEntities;

namespace AuthorizingAPIs.Services
{
    public class JwtHandler : AuthorizingAPIs.Interfaces.IJwtHandler
    {
        private readonly SBbContext _context;
        private readonly IConfiguration _config;
        private readonly Jwt _jwt;
        private SecurityKey _issuerSigningKey;
        private SigningCredentials _signingCredentials;

        public JwtHandler(SBbContext context, IOptionsSnapshot<Jwt> jwt, IConfiguration config)
        {

            _context = context;
            _config = config;
            _jwt = jwt.Value;

            if (_jwt.UseRsa)
                InitializeRsa();
            else
                InitializeHmac();
        }

        #region Initialize RSA
        private void InitializeRsa()
        {
            using (RSA publicRsa = RSA.Create())
            {
                var currentDirectory = Path.Combine(Directory.GetCurrentDirectory() + "\\Keys\\" + _jwt.RsaPublicKeyXML);
                publicRsa.FromXmlString(File.ReadAllText(currentDirectory));

                _issuerSigningKey = new RsaSecurityKey(publicRsa)
                {
                    CryptoProviderFactory = new CryptoProviderFactory()
                    {
                        CacheSignatureProviders = false
                    }
                };
            }

            if (string.IsNullOrWhiteSpace(_jwt.RsaPrivateKeyXML))
                return;

            using (RSA privateRsa = RSA.Create())
            {
                var currentDirectory = Path.Combine(Directory.GetCurrentDirectory() + "\\Keys\\" + _jwt.RsaPrivateKeyXML);
                privateRsa.FromXmlString(File.ReadAllText(currentDirectory));

                _ = new RsaSecurityKey(privateRsa)
                {
                    CryptoProviderFactory = new CryptoProviderFactory()
                    {
                        CacheSignatureProviders = false
                    }
                };

                _signingCredentials = new SigningCredentials(new RsaSecurityKey(privateRsa), SecurityAlgorithms.RsaSha256)
                {
                    CryptoProviderFactory = new CryptoProviderFactory()
                    {
                        CacheSignatureProviders = false
                    }
                };
            }
        }
        #endregion

        #region Initialize HMAC
        private void InitializeHmac()
        {
            _issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
            _signingCredentials = new SigningCredentials(_issuerSigningKey, SecurityAlgorithms.HmacSha256);
        }
        #endregion

        #region Create Token
        /// <summary>
        /// تولید توکن JWT
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<JwtSecurityToken> Create(LoginLog user)
        {
            try
            {
                //var roles = await _context.UserRoles.Where(u => u.UserId == user.Id)
                //.Include(u => u.Role).ToListAsync();
                var roleClaims = new List<Claim>();
                var entityInfos = new List<Claim>();

                //for (int i = 0; i < roles.Count; i++)
                //{
                //    roleClaims.Add(new Claim("roles", roles[i].Role.Name));
                //    //بدست اوردن لیست آدرس روتهایی که کاربر می تواند به انها دسترسی داشته باشد
                //    // var entityMappingRoles = await _context.EntityMappingRoles.Where(e => e.RoleId == roles[i].Role.Id && e.PermissionId == 1)
                //    //                                 .Include(e => e.EntityMapping)
                //    //                                 .ToListAsync();
                //    // for (int j = 0; j < entityMappingRoles.Count; j++)
                //    // {
                //    //     entityInfos.Add(new Claim("routeLists", entityMappingRoles[j].EntityMapping.FullRouteAddress));
                //    // }
                //}

                user.Expiretime = Convert.ToInt64(_config["JWT:ExpireMin"]);

                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(ClaimTypes.Name, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("uid", user.LoginLogId.ToString())
            }
                .Union(roleClaims)
                .Union(entityInfos);

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _config["JWT:Issuer"],
                    audience: _config["JWT:Audience"],
                    claims: claims,
                    expires: user.LoginDate.AddMinutes(Convert.ToDouble(_config["JWT:ExpireMin"])),
                    signingCredentials: _signingCredentials);
                return jwtSecurityToken;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return null;
            }
        }
        public async Task<JwtSecurityToken> Create(LoginLog user, int expireday = 1)
        {
            try
            {
                //var roles = await _context.UserRoles.Where(u => u.UserId == user.Id)
                //.Include(u => u.Role).ToListAsync();
                var roleClaims = new List<Claim>();
                var entityInfos = new List<Claim>();

                //for (int i = 0; i < roles.Count; i++)
                //{
                //    roleClaims.Add(new Claim("roles", roles[i].Role.Name));
                //    //بدست اوردن لیست آدرس روتهایی که کاربر می تواند به انها دسترسی داشته باشد
                //    // var entityMappingRoles = await _context.EntityMappingRoles.Where(e => e.RoleId == roles[i].Role.Id && e.PermissionId == 1)
                //    //                                 .Include(e => e.EntityMapping)
                //    //                                 .ToListAsync();
                //    // for (int j = 0; j < entityMappingRoles.Count; j++)
                //    // {
                //    //     entityInfos.Add(new Claim("routeLists", entityMappingRoles[j].EntityMapping.FullRouteAddress));
                //    // }
                //}

                user.Expiretime = (expireday== null)? Convert.ToInt64(_config["JWT:ExpireMin"]) : (expireday * 1440);

                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(ClaimTypes.Name, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("uid", user.LoginLogId.ToString())
            }
                .Union(roleClaims)
                .Union(entityInfos);

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _config["JWT:Issuer"],
                    audience: _config["JWT:Audience"],
                    claims: claims,
                    expires: user.LoginDate.AddMinutes(user.Expiretime),
                    signingCredentials: _signingCredentials);
                return jwtSecurityToken;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return null;
            }
        }
        #endregion

        /// <summary>
        /// بررسی برای فعال بودن توکن
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> CkeckTokenIsValid(string token)
        {
            bool TokenIsValid = false;
            try
            {
                var Key = Encoding.UTF8.GetBytes(_config["JWT:Secret"]);

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Key),
                    ClockSkew = TimeSpan.Zero
                };

                var handler = new JwtSecurityTokenHandler();
                var decodedValue = handler.ReadJwtToken(token);
                handler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    TokenIsValid = false;
                }
                else
                {
                    TokenIsValid = true;
                }
            }
            catch (Exception ex) { }
            return TokenIsValid;
        }

        public async Task<string> GetTokenParameterValue(string token, string parametersname)
        {
            string parametersvalue = string.Empty;
            try
            {

                var handler = new JwtSecurityTokenHandler();
                var decodedValue = handler.ReadJwtToken(token);

                parametersvalue = decodedValue.Claims.First(c => c.Type == parametersname).Value;
            }
            catch (Exception ex) { }
            return parametersvalue;
        }
    }
}
