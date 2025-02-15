using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class PeronCompleteInfoDto
    {
        public long personId { get; set; }
        public string? biographicalInfo { get; set; }
        public string? jobtitle { get; set; }
        public string? profilebio { get; set; }
        public string? timezone { get; set; }
        public string? hobbyOfTrading { get; set; }
        public DateTime? activeTill { get; set; }
        
    }
    public class CustomerBillingAddressDto
    {
        public long? personId { get; set; }
        public string? fname{ get; set; }
		public string? lname{ get; set; }
		public string? company{ get; set; }
		public string? address1{ get; set; }
		public string? address2{ get; set; }
		public string? city { get; set; }
        public string? postCode{ get; set; }
		public int? countryId { get; set; }
        public string? state { get; set; }
        public string? phone{ get; set; }
		public string? email{ get; set; }
	}
    public class PersonShippingAddressDto
    {
        public long? personId { get; set; }
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
