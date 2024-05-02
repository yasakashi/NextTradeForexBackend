using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Systems
{
    /// <summary>
    /// نوع لاگ های سیستم
    /// </summary>
    [Table("tblLogTypes")]
    public class LogType
    {
        /// <summary>
        /// شناسه سیستمی 
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// متن
        /// </summary>
        public string Name { get; set; }
    }
}
