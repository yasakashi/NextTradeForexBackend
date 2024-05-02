using System.Globalization;

namespace AuthorizingAPIs
{
    public class GlobalFunctions
    {
        static string baseServiceProviderID = "0111";
        public static string GenerateRequestID()
        { 
            return baseServiceProviderID + 
                   DateTime.Now.Year.ToString() + 
                   DateTime.Now.Month.ToString().PadLeft(2, '0') +
                   DateTime.Now.Day.ToString().PadLeft(2, '0') +
                   DateTime.Now.Hour.ToString().PadLeft(2, '0') +
                   DateTime.Now.Minute.ToString().PadLeft(2, '0') +
                   DateTime.Now.Second.ToString().PadLeft(2, '0') +
                   DateTime.Now.Millisecond.ToString().PadLeft(6, '0');
        }

        public static string _GenerateRequestID()
        {
            return  DateTime.Now.ToString("yyyyMMddHHmmssffffff", CultureInfo.GetCultureInfo("en-US"));
        }
    }
}
