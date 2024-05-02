using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Base.Common.GobalFunction
{
    public class ValidationFunction
    {
        /// <summary>
        /// Check Is PhoneNumber Is Correct
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsPhoneNumber(string number)
        {
            string mobilepatern = @"^((\+989)|(989)|(00989)|(09|9))([1|2|3][0-9]\d{7}$)";
            bool isvalid = Regex.IsMatch(number, mobilepatern);
            return isvalid;
        }
    }
}
