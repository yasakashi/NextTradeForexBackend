namespace Entities.Dtos
{
    /// <summary>
    /// لیست پیام های سیستم
    /// </summary>
    public class SystemMessageModel
    {
        /// <summary>
        /// کد پیام
        /// </summary>
        public long MessageCode { get; set; }
        /// <summary>
        /// متن پیام
        /// </summary>
        public string MessageDescription { get; set; }
        /// <summary>
        /// فایل های ضمیمه پیام
        /// </summary>
        public object? MessageData { get; set; }
    }
}
