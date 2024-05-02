// using ERP.Common.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace Base.Common.Encryption
{
    public class RequestAuthorization
    {
        public static bool DecodeAuthorization(HttpContext context, string Username, string password)
        {
            StringValues authHeader = String.Empty;
            var outdata = context.Request.Headers.TryGetValue("Authorization", out authHeader);
            if (outdata && !StringValues.IsNullOrEmpty(authHeader) && authHeader.ToString().StartsWith("basic", StringComparison.OrdinalIgnoreCase))
            {
                string token = authHeader.ToString().Replace("Basic", "").Replace("basic", "").Trim();
                var credentialstring = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                string[] credentials = credentialstring.Split(':');
                string user = credentials[0];
                string pass = credentials[1];
                if (user == Username && pass == password)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                //var claims = new[] { new Claim("name", credentials[0]), new Claim(ClaimTypes.Role, "Admin") };
                //            var identity = new ClaimsIdentity(claims, "Basic");
                //            context.User = new ClaimsPrincipal(identity);
            }
            else
            {
                return false;
            }
        }


        public static List<string> DecodeAuthorization(HttpContext context)
        {
            List<string> returnlist = new List<string>();

            StringValues authHeader = String.Empty;
            var outdata = context.Request.Headers.TryGetValue("Authorization", out authHeader);
            if (outdata && !StringValues.IsNullOrEmpty(authHeader) && authHeader.ToString().StartsWith("basic", StringComparison.OrdinalIgnoreCase))
            {
                string token = authHeader.ToString().Replace("Basic", "").Replace("basic", "").Trim();
                var credentialstring = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                string[] credentials = credentialstring.Split(':');
                string user = credentials[0];
                string pass = credentials[1];

                returnlist.Add(user);
                returnlist.Add(pass);
            }
            return returnlist;
        }

        public static string GetKeyValueFromHeader(HttpContext context, string HeaderKey)
        {
            List<string> returnlist = new List<string>();
            string returnvalue = string.Empty;
            StringValues authHeader = String.Empty;
            var outdata = context.Request.Headers.TryGetValue(HeaderKey, out authHeader);
            if (outdata && !StringValues.IsNullOrEmpty(authHeader))
            {
                returnvalue = authHeader.ToString().Replace(HeaderKey, "").Replace(HeaderKey, "").Trim();
            }
            return returnvalue;
        }

        public static string GetToken(HttpContext context)
        {
            var credentials = "";
            try
            {
                var header = AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]);
                credentials = header.Parameter;
            }
            catch
            {
                var header = context.Request.Headers["token"].ToString();
                credentials = header;
            }
            return credentials;
        }

        public static long GetUserIdFromToken(HttpContext context)
        {
            var mySecret = JwtConfig.Secret;
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                string token = GetToken(context);
                if (string.IsNullOrEmpty(token))
                {
                    return -1;
                }

                var jsonToken = tokenHandler.ReadToken(token);
                var tokenS = jsonToken as JwtSecurityToken;
                var useridcliam = tokenS.Claims.FirstOrDefault(claim => claim.Type == "uid");
                if (useridcliam != null)
                    return Convert.ToInt32(useridcliam.Value);
                else
                    return -1;
            }
            catch (Exception ex)
            {
                string str = ex.ToString();
                return -1;
            }
        }

        public static string GetUsernameFromToken(HttpContext context)
        {
            var mySecret = JwtConfig.Secret;
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                string token = GetToken(context);
                if (string.IsNullOrEmpty(token))
                {
                    return "";
                }

                var jsonToken = tokenHandler.ReadToken(token);
                var tokenS = jsonToken as JwtSecurityToken;
                var useridcliam = tokenS.Claims.FirstOrDefault(claim => claim.Type == "sub");
                if (useridcliam != null)
                    return useridcliam.Value;
                else
                    return "";
            }
            catch (Exception ex)
            {
                string str = ex.ToString();
                return "";
            }
        }
    }
}
