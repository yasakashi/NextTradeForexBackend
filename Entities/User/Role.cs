using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
// using Base.DataLayer.Entities.Permissions;

namespace Entities.DBEntities
{
    public class Role
    {
        public Role()
        {

        }
        /// <summary>
        /// شناسه سیستمی 
        /// </summary>
        [Key]
        [Column("Id")]
        public long RoleId { get; set; }
        /// <summary>
        /// عنوان نقش 
        /// </summary>
        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string RoleTitle { get; set; }
        /// <summary>
        /// وضعیت حذف منطقی
        /// </summary>
        public bool IsDelete { get; set; }


        #region Relations

        // public virtual List<UserRole> UserRoles { get; set; }
        // public List<RolePermission> RolePermissions { get; set; }


        #endregion
    }
}
