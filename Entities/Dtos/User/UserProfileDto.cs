using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DBEntities;

namespace Entities.Dtos
{
	public class UserProfileDto
	{
		public long userid { get; set; }
		public string? language { get; set; }
		public string? username { get; set; }
		public string? fname { get; set; }
		public string? lname { get; set; }
		public string? nickname { get; set; }
		public string? displayPublicName { get; set; }
		public string? email { get; set; }
		public string? website { get; set; }
		public string? biographicalInfo { get; set; }
		public string? sessions { get; set; }

		public string? jobTitle { get; set; }
		public string? profileBio { get; set; }
		//        public string profilePic: file,
		//public string profilePhoto: file,
		public string? timezone { get; set; }
        public int? forexexperiencelevelId { get; set; }
        //public string interestInForexId: id(Forex, commodities, indices, stocks, Binary Trading, crypto)

        public string hobbyOfTrading { set; get; }
        public int? countryId { get; set; }
		public string countryCode { get; set; }
		public string? mobile { get; set; }
		public int? stateId { get; set; }
		public int? cityId { get; set; }
		public long? refferedBy { get; set; }
		public DateTime? activeTill { get; set; }
	}
}
	/*
	customerBillingAddress{
		fname: ,
		lname: ,
		company: ,
		address1: ,
		address2: ,
		city{get;set;}
		postCode: ,
		countryId, id,
		state{get;set;}
		phone: ,
		email: ,
	}: ,
	
		customerShippingAddress{
		fname: ,
		lname: ,
		company: ,
		address1: ,
		address2: ,
		city{get;set;}
		postCode: ,
		countryId, id,
		state{get;set;}
		phone: ,
		email: ,
	}: ,
	
}

    }
}
	*/