using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblPeopleCompleteInfo")]
    public class PersonCompleteInfo
    {
        [Key]
        public long personId { get; set; }
        public string? biographicalInfo { get; set; }
        public string? jobtitle { get; set; }
        public string? profilebio { get; set; }
        public string? timezone { get; set; }
        public string? hobbyOfTrading { get; set; }
        public DateTime? activeTill { get; set; }
        public long? interestinforexid { get; set; }

    }

    [Table("tblPeopleBillingAddress")]
    public class PersonBillingAddress
    {
        [Key]
        public long personId { get; set; }
        public string? fname { get; set; }
        public string? lname { get; set; }
        public string? company { get; set; }
        public string? address1 { get; set; }
        public string? address2 { get; set; }
        public string? city { get; set; }
        public string? postCode { get; set; }
        public int? countryId { get; set; }
        public string? state { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
    }

    [Table("tblPeopleShippingAddress")]
    public class PersonShippingAddress
    {
        [Key]
        public long personId { get; set; }
        public string? fname { get; set; }
        public string? lname { get; set; }
        public string? company { get; set; }
        public string? address1 { get; set; }
        public string? address2 { get; set; }
        public string? city { get; set; }
        public string? postCode { get; set; }
        public int? countryId { get; set; }
        public string? state { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
    }
}
