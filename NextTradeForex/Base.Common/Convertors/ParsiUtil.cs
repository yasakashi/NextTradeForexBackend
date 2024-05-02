using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Base.Common.Convertors
{
    /// <summary>
    /// Summary description for ParsiUtil.
    /// </summary>
    public class ParsiUtil
    {
        /// <summary>
        /// Convert DateTime To Day Of Week Name
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetDayOfWeekName(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Saturday: return "شنبه";
                case DayOfWeek.Sunday: return "يکشنبه";
                case DayOfWeek.Monday: return "دوشنبه";
                case DayOfWeek.Tuesday: return "سه‏ شنبه";
                case DayOfWeek.Wednesday: return "چهارشنبه";
                case DayOfWeek.Thursday: return "پنجشنبه";
                case DayOfWeek.Friday: return "جمعه";
                default: return "";
            }
        }

        /// <summary>
        /// Convert DateTime To Month Name
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetMonthName(DateTime date)
        {
            PersianCalendar jc = new PersianCalendar();
            string pdate = string.Format("{0:0000}/{1:00}/{2:00}", jc.GetYear(date), jc.GetMonth(date), jc.GetDayOfMonth(date));

            string[] dates = pdate.Split('/');
            int month = Convert.ToInt32(dates[1]);

            switch (month)
            {
                case 1: return "فررودين";
                case 2: return "ارديبهشت";
                case 3: return "خرداد";
                case 4: return "تير‏";
                case 5: return "مرداد";
                case 6: return "شهريور";
                case 7: return "مهر";
                case 8: return "آبان";
                case 9: return "آذر";
                case 10: return "دي";
                case 11: return "بهمن";
                case 12: return "اسفند";
                default: return "";
            }

        }

        /// <summary>
        /// Convert All Ye To Arabic
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static public string ConvertAllYeToArabic(string str)
        {
            string res = str.Trim();

            res = res.Replace('\u0649', '\u064A');  // shift + X --> shift + X 
            res = res.Replace('\u06CC', '\u064A'); // shift + V --> shift + X 
            return res;
        }

        /// <summary>
        /// Convert All Ye To Parsi
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static public string ConvertAllYeToFarsi(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            string res = str.Trim();
            //064A
            //قبلا اینجوری بوده 
            res = res.Replace('\u0649', '\u06CC');  // shift + X --> shift + X 
            res = res.Replace('\u064A', '\u06CC'); // shift + V --> shift + X 
            return res;
        }

        /// <summary>
        /// Convert All Space To common one
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static public string ConvertAllSpace(string str)
        {
            string res = str.Trim();

            res = res.Replace('\u202F', '\u0020');
            res = res.Replace('\u200D', '\u0020');
            res = res.Replace('\u200C', '\u0020');

            return res;
        }

        /// <summary>
        /// Convert All Kaf To Parsi
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static public string ConvertAllKaf(string str)
        {
            string res = str.Trim();

            res = res.Replace('˜', '\u06A9');
            res = res.Replace('\u0643', '\u06A9');
            res = res.Replace('\u06AA', '\u06A9');

            return res;
        }

        /// <summary>
        /// convert all ambiguous Parsi characters to a common one
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static public string ConvertAllCharToFarsi(string str)
        {
            if (string.IsNullOrEmpty(str))
                str = "";
            string res = str.Trim();

            res = ConvertAllYeToFarsi(res);
            res = ConvertAllKaf(res);

            return res;
        }

        /// <summary>
        /// convert all ambiguous Arabic characters to a common one
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static public string ConvertAllCharToArabic(string str)
        {
            if (string.IsNullOrEmpty(str))
                str = "";
            string res = str.Trim();

            res = ConvertAllYeToArabic(res);
            res = ConvertAllKaf(res);

            return res;
        }

        /// <summary>
        /// convert all ambiguous Parsi characters to a common one 
        /// </summary>
        /// <param name="ds"></param>
        public static DataSet ConvertAllChar(DataSet ds)
        {
            try
            {
                //	DataSet ds = (DataSet)ds1.Clone();
                if (ds.Tables == null || ds.Tables.Count <= 0)
                    return ds;
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    if (ds.Tables[i].Rows == null || ds.Tables[i].Rows.Count <= 0)
                        continue;
                    for (int j = 0; j < ds.Tables[i].Rows.Count; j++)
                    {
                        //object [] itmArray = ds.Tables[i].Rows[j].ItemArray;
                        if (ds.Tables[i].Rows[j].RowState == DataRowState.Added || ds.Tables[i].Rows[j].RowState == DataRowState.Modified)
                            for (int k = 0; k < ds.Tables[i].Columns.Count; k++)
                            {
                                if (ds.Tables[i].Rows[j][k] is string)
                                    ds.Tables[i].Rows[j][k] = (object)ParsiUtil.ConvertAllCharToFarsi((string)ds.Tables[i].Rows[j][k]);
                            }
                    }
                }
                return ds;
            }
            catch
            {
                return ds;
            }
        }

        public static DataTable ConvertAllChar(DataTable dt)
        {
            try
            {
                //	DataSet ds = (DataSet)ds1.Clone();
                if (dt == null || dt.Rows.Count <= 0)
                    return dt;
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    //object [] itmArray = ds.Tables[i].Rows[j].ItemArray;
                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        if (dt.Rows[j][k] is string)
                            dt.Rows[j][k] = (object)ParsiUtil.ConvertAllCharToFarsi((string)dt.Rows[j][k]);
                    }
                }
                return dt;
            }
            catch
            {
                return dt;
            }
        }

        public static DataTable ConvertAllCharReport(DataTable dt)
        {
            try
            {
                //	DataSet ds = (DataSet)ds1.Clone();
                if (dt == null || dt.Rows.Count <= 0)
                    return dt;
                else
                {
                    dt = ParsiUtil.ConvertAllChar(dt);
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        //object [] itmArray = ds.Tables[i].Rows[j].ItemArray;
                        for (int k = 0; k < dt.Columns.Count; k++)
                        {
                            if (dt.Rows[j][k] is string)
                                dt.Rows[j][k] = (object)ParsiUtil.ConvertAllCharToFarsi((string)dt.Rows[j][k]).Replace("\"", "'");
                        }
                    }
                    return dt;
                }
            }
            catch
            {
                return dt;
            }
        }

        /// <summary>
        /// Convert Persian DateTime String To Standard Calendar
        /// </summary>
        /// <param name="persianCalendarText"></param>
        /// <returns></returns>
        public static string ChangePersianToStandardCalendar(string persianCalendarText)
        {
            if (string.IsNullOrEmpty(persianCalendarText))
            {
                return string.Empty;
            }

            PersianCalendar pc = new PersianCalendar();

            // The "persianCalendarText" parameter must always be in yyyy/mm/dd format. Do NOT change the functionality
            string[] dateParts = persianCalendarText.Split('/');

            int year = Convert.ToInt32(dateParts[0]);
            int month = Convert.ToInt32(dateParts[1]);
            int day = Convert.ToInt32(dateParts[2]);

            string changedDatetime = pc.ToDateTime(year, month, day, 0, 0, 0, 0).ToString("yyyy-MM-dd");

            return changedDatetime;
        }

        /// <summary>
        /// Convert DateTime String To Standard Calendar
        /// </summary>
        /// <param name="persianCalendarText"></param>
        /// <returns></returns>
        public static string ChangeStringToStandardCalendar(string persianCalendarText)
        {
            if (string.IsNullOrEmpty(persianCalendarText))
            {
                return string.Empty;
            }

            PersianCalendar pc = new PersianCalendar();
            DateTime oDate = Convert.ToDateTime(persianCalendarText);
            // The "persianCalendarText" parameter must always be in yyyy/mm/dd format. Do NOT change the functionality
            string[] dateParts = persianCalendarText.Split('/');

            int year = Convert.ToInt32(dateParts[0]);
            int month = Convert.ToInt32(dateParts[1]);
            int day = Convert.ToInt32(dateParts[2]);

            string changedDatetime = pc.ToDateTime(year, month, day, 0, 0, 0, 0).ToString("yyyy-MM-dd");

            return changedDatetime;
        }

        public static string LeftZeroFillToNumber(string currentText, char delimiter, int numberFixLength)
        {
            string[] valueParts = currentText.Split(delimiter);

            for (int partIndex = 0; partIndex < valueParts.Length; partIndex++)
            {
                if (valueParts[partIndex].ToString().Length < numberFixLength)
                {
                    valueParts[partIndex] = valueParts[partIndex].PadLeft(numberFixLength, '0');

                    //string replacedValue = "";

                    //for (int i = 0; i < numberFixLength - valueParts[partIndex].ToString().Length; i++)
                    //{
                    //    replacedValue += "0";
                    //}

                    //valueParts[partIndex] = replacedValue + valueParts[partIndex];
                }
            }

            return string.Join(delimiter.ToString(), valueParts);
        }

        public static string ReplaceValues(string inputString, string pattern)
        {
            try
            {
                Regex regex = new Regex(pattern);
                var matchValues = Regex.Matches(inputString, pattern);
                var itemValues = new Dictionary<string, string>();

                foreach (Match match in matchValues)
                {
                    if (!itemValues.ContainsKey(match.Value))
                    {
                        itemValues.Add(match.Value, LeftZeroFillToNumber(match.Value, '/', 2));
                    }
                }

                string result = Regex.Replace(inputString, pattern, m => itemValues.ContainsKey(m.Value) ? itemValues[m.Value] : m.Value);
                return result;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string FitDateNumber(string currentText)
        {
            if (currentText.Trim().Length == 0) return currentText;
            string strrettext = currentText;
            string output = "";
            bool Hascharacter = false;
            if (strrettext.IndexOf("'") > -1)
            {
                Hascharacter = true;
                strrettext = strrettext.Replace("'", "");
            }
            string[] valueParts = strrettext.Split('/');
            if (valueParts.Length > 2)
            {
                foreach (string str in valueParts)
                {
                    if (str.Length > 2)
                        output += str + "/";
                    else
                        output += str.PadLeft(2, '0') + "/";
                }
                output = output.Substring(0, output.Length - 1);
                if (Hascharacter) output = "'" + output + "'";
            }
            else
            {
                output = currentText;
            }

            return output;
        }

        #region AddComma
        public static string AddComma(string text, int pos)
        {
            string str;
            string str2;
            int index = text.IndexOf('.');
            if ((index >= 0) && ((index + 1) < pos))
            {
                return text;
            }
            if (index >= 1)
            {
                str = text.Substring(0, index);
                str2 = text.Substring(index).Replace(",", "");
            }
            else
            {
                str = text;
                str2 = "";
            }
            int num2 = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == ',')
                {
                    num2++;
                    if (i < pos)
                    {
                        pos--;
                    }
                }
            }
            str = str.Replace(",", "");
            int num4 = 0;
            int num5 = 0;
            if ((str.Length > 0) && (str[0] == '-'))
            {
                num5 = 1;
            }
            for (int j = str.Length - 1; j > num5; j--)
            {
                num4++;
                if (num4 == 3)
                {
                    str = str.Insert(j, ",");
                    if (j < pos)
                    {
                        pos++;
                    }
                    num4 = 0;
                }
            }
            return (str + str2);
        }

        public static string AddComma(string text)
        {
            int pos = 0;
            string str;
            string str2;
            int index = text.IndexOf('.');
            if ((index >= 0) && ((index + 1) < pos))
            {
                return text;
            }
            if (index >= 1)
            {
                str = text.Substring(0, index);
                str2 = text.Substring(index).Replace(",", "");
            }
            else
            {
                str = text;
                str2 = "";
            }
            int num2 = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == ',')
                {
                    num2++;
                    if (i < pos)
                    {
                        pos--;
                    }
                }
            }
            str = str.Replace(",", "");
            int num4 = 0;
            int num5 = 0;
            if ((str.Length > 0) && (str[0] == '-'))
            {
                num5 = 1;
            }
            for (int j = str.Length - 1; j > num5; j--)
            {
                num4++;
                if (num4 == 2)
                {
                    str = str.Insert(j, ",");
                    if (j < pos)
                    {
                        pos++;
                    }
                    num4 = 0;
                }
            }
            return (str + str2);
        }
        #endregion
        /// <summary>
        /// Convert Kaf And Ye And He And Half Space And Space to Parsi
        /// </summary>
        /// <param name="persianCalendarText"></param>
        /// <returns></returns>
        public static string FixPersianChars(string str)
        {
            return str.Replace("ﮎ", "ک")
                .Replace("ﮏ", "ک")
                .Replace("ﮐ", "ک")
                .Replace("ﮑ", "ک")
                .Replace("ك", "ک")
                .Replace("ي", "ی")
                .Replace(" ", " ")
                .Replace("‌", " ")
                .Replace("ھ", "ه");//.Replace("ئ", "ی");
        }
    }
}