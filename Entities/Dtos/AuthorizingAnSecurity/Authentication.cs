using System.Text.Json.Serialization;

namespace Entities.Dtos
{
    /// <summary>
    /// برای احراز هویت کاربر
    /// </summary>
    public class Authentication
    {
        /// <summary>
        /// تایین وضعیت احراز هویت
        /// </summary>
        public bool IsAuthenticated { get; set; }
        /// <summary>
        /// پیام ارسالی
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// رمز ارسالی جهت احراز هویت
        /// </summary>
        public string Token { get; set; }
        //public DateTime TokenExpiration { get; set; }
    }
}
