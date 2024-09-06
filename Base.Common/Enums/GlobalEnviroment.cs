namespace Base.Common.Enums
{
    public enum LogTypeIds
    { 
        GetToken=1,
        TokenError = -1,
        SystemError=-99
    }
    public class LogTypes
    {
        public static long ApiRequest { get { return 1; } }
        public static long GetToken { get { return 2; } }
        public static long TokenError { get { return -2; } }
        public static long SystemError { get { return -99; } }
    }

    /// <summary>
    /// انواع کاربر
    /// </summary>
    public enum UserTypes
    {
        /// <summary>
        /// کاربر مدیر
        /// </summary>
        SuperAdmin = 1,
        /// <summary>
        /// کاربر مدیریت هر اپ موبایل
        /// </summary>
        Admin =2,
        /// <summary>
        /// کاربر استاد
        /// </summary>
        Instructor = 3,
        /// <summary>
        /// کاربر عادی
        /// </summary>
        Student = 4
    }
    public class GlobalEnviroment
    {
    }
}
