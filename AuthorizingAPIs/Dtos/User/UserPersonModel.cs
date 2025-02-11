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
        public List<int>? financialinstrumentIds { get; set; }
        public List<FinancialInstrumentDto>? financialinstruments { get; set; }
        public int? forexexperiencelevelId { get; set; }
        public List<int>? trainingmethodIds { get; set; }
        public List<TrainingMethodDto>? trainingmethods { get; set; }
        public List<int>? targettrainerIds { get; set; }
        public List<TargetTrainerDto>? targettrainers { get; set; }
        public int? partnertypeId { get; set; }
        public int? interestforexId { get; set; }
        public string? interestforexname { get; set; }
        public string? forexexperiencelevelname { get; set; }
        public bool? hobbyoftradingfulltime { get; set; }
        public int? countryId { get; set; }
        public int? stateId { get; set; }
        public int? cityId { get; set; }
        public string? countryname { get; set; }
        public long? usertypeId { get; set; }
        public string? usertypename { get; set; }
        public string? userpicurl { get; set; }
        public string? statename { get; set; }
        public string? cityname { get; set; }
        public string? website { get; set; }
        public string? language { get; set; }
        public bool? sendNotification { get; set; }
        public int? forumRoleId { get; set; }
        public int? pagecount { get; set; }
    }


    public class UserPersonLookupModel
    {
        public long? userid { get; set; }
        /// <summary>
        /// نام کاربری
        /// </summary>
        public string? username { get; set; }
        public string? fname { get; set; }
        public string? lname { get; set; }
    }
}
