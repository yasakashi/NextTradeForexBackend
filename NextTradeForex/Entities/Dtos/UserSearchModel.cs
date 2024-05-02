using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    /// <summary>
    /// پارامترهای جستحوی کاربران
    /// </summary>
    public class UserSearchModel
    {
        public string? username { get; set; }
        public string? parentid { get; set; }
        public string? fname { get; set; }
        public string? lname { get; set; }
        public string? mobile { get; set; }
        public string? nationalcode { get; set; }
    }
}
