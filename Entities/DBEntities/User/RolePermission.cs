using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities;

    [Table("tblRolePermissions")]
    public class RolePermission
    {
        /// <summary>
        /// شناسه سیستمی 
        /// </summary>
        [Key]
        public long Id { get; set; }
        public long RoleId { get; set; }
        public long PermissionId { get; set; }
        public bool IsActived { get; set; }
    }

