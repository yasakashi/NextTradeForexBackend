using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Common.Convertors
{
    public static class ValidatePhoneNumber
    {
        /// <summary>
        /// Check PhoneNumber is Valid Or Not
        /// </summary>
        /// <param name="countryCode"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsValidMobileNumber(int countryCode, string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return false;

            var phoneUtil = PhoneNumberUtil.GetInstance();
            var regionCode = phoneUtil.GetRegionCodeForCountryCode(countryCode);
            
            try
            {
                PhoneNumber phoneNumber = phoneUtil.Parse(phone, regionCode);
                var phoneNumberType = phoneUtil.GetNumberType(phoneNumber);

                return phoneNumberType == PhoneNumberType.MOBILE;
            }
            catch (Exception)
            {
            }

            return false;
        }
    }
}
