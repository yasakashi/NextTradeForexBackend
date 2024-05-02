using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Entities.DBEntities
{
    /// <summary>
    /// نوع کاربر
    /// </summary>
    [Table("tblUserTypes")]
    public class UserType
    {
        /// <summary>
        /// شناسه سیستمی کاربر
        /// </summary>
        [Key]
        [Column("Id")]
        public long Id { get; set; }

        [Display(Name = "نام")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Name { get; set; }
    }
}
