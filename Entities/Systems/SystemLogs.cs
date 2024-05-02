using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Systems
{
    /// <summary>
    /// لاگهای سیستم
    /// </summary>
    [Table("tblSystemLogs")]
    public class SystemLog
    {
        /// <summary>
        /// شناسه سیستمی 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// متن
        /// </summary>
        public string logDescription { get; set; }
        /// <summary>
        /// نوع لاگ
        /// </summary>
        public long LogTypeId { get; set; }
        /// <summary>
        /// تاریخ و زمان لاگ
        /// </summary>
        public DateTime LogDatetime { get; set; }
        /// <summary>
        /// محل لاگ
        /// </summary>
        public string LogLocation { get; set; }
        /// <summary>
        /// آدرس درخواست کننده
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// شناسه پروسس
        /// </summary>
        public string ProcessId { get; set; }
        /// <summary>
        /// توکن
        /// </summary>
        public string Token { get; set; }
    }
}
