using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Text.RegularExpressions;

namespace Base.Common.Convertors
{
    public static class FixedTexts
    {
        public static string FixedEMail(string eMail)
        {
            return eMail.Trim().ToLower();
        }

        public static string FixInjected(this string value)
        {
            string result = Regex.Replace(value, "<script", String.Empty, RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "<link", String.Empty, RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "javascript:", String.Empty, RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "<select", String.Empty, RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "'", "''", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, ";", ",", RegexOptions.IgnoreCase);

            return result;
        }
    }
    public static class JsonExtensions
    {
        public static string ToJson<T>(this T obj, bool includeNull = true)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new JsonConverter[] { new StringEnumConverter() },
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,//newly added
                                                                                     //PreserveReferencesHandling =Newtonsoft.Json.PreserveReferencesHandling.Objects,
                NullValueHandling = includeNull ? NullValueHandling.Include : NullValueHandling.Ignore
            };
            return JsonConvert.SerializeObject(obj, settings);
        }
    }
}
