using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace Base.Common.Convertors
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime value)
        {
            string converteddate = String.Empty;
            PersianCalendar pc=new PersianCalendar();
            try
            {
                converteddate = pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" +
                       pc.GetDayOfMonth(value).ToString("00");
            }
            catch { }
            return converteddate;
        }
        public static DateTime ToDateTime(this string value)
        {
            return Convert.ToDateTime(value, new CultureInfo("fa-IR"));
        }
        public static long ConvertToTimestamp(this DateTime value)
        {
            long epoch = (value.Ticks - 621355968000000000) / 10000;
            return epoch;
        }

        public static DateTime ConvertTimestampToDateTime(this long timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1).AddMilliseconds(timestamp);
            return origin;
        }

        public static DateTime StartOfDay(this DateTime theDate)
        {
            return new DateTime(theDate.Year, theDate.Month, theDate.Day,0,0,1);
        }

        public static DateTime EndOfDay(this DateTime theDate)
        {
            return theDate.Date.AddDays(1).AddTicks(-1);
        }
    }
}
