using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    [Table("tblOutSystemPermissions")]
    public class SystemPermission
    {
        /// <summary>
        /// شناسه سیستمی 
        /// </summary>
        [Key]
        public long Id { get; set; }
        public long OutSystemId { get; set; }
        public long SystemPermissionCode { get; set; }
        public string SystemPermissionName { get; set; }
        /// <summary>
        /// نام
        /// </summary>
        public string SystemPermissionTitle { get; set; }
    }
}
