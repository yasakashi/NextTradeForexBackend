using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Systems
{
    /// <summary>
    /// سیستم های که در خارج از مجموعه هستند
    /// </summary>
    [Table("tblOutSystems")]
    public class OutSystem
    {
        /// <summary>
        /// شناسه سیستمی 
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// نام
        /// </summary>
        public string Name { get; set; }
    }
}
