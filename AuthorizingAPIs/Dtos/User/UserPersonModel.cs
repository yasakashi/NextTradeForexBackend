using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class UserPersonModel
    {
        public long? userid { get; set; }
        /// <summary>
        /// نام کاربری
        /// </summary>
        public string? username { get; set; }
        public string? fname { get; set; }
        public string? lname { get; set; }
        public string? nationalcode { get; set; }
        public int? sex { get; set; }
        public long? marriedstatusid { get; set; }
        public long? PersonTypeId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Mobile { get; set; }
        public string? taxcode { get; set; }
        public string? legalNationalCode { get; set; }
        public string? Companyname { get; set; }
        public string? companyregisterdate { get; set; }
        public string? postalcode { get; set; }
        public string? address { get; set; }
        public string? email { get; set; }
    }
}
