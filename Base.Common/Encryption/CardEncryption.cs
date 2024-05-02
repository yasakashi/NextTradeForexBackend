namespace Base.Common.Encryption
{
    public class CardEncryption
    {
        /// <summary>
        /// Hash 8 Numbers From The Card Number
        /// </summary>
        /// <param name="pan"></param>
        /// <returns></returns>
        public static string HashCardNumber(string pan)
        {
            if (pan.Trim().Length > 0)
                return pan.Substring(0, 4) + "-****-****-" + pan.Substring(12, 4);
            else
                return pan.Trim();
        }
    }
}
