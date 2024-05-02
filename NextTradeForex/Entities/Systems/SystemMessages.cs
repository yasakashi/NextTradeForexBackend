using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Systems
{
    /// <summary>
    /// لیست پیام های سیستم
    /// </summary>
    [Table("tblSystemMessages")]
    public class SystemMessage
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
