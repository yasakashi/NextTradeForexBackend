using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class UserPartnerSearchDto : BaseFilterDto
{
    public string? username { get; set; }
    public string? refferalcode { get; set; }
    public string? fname { get; set; }
    public string? lname { get; set; }
    public string? name { get; set; }
    public int? countryId { get; set; }
    public int? stateId { get; set; }
    public int? cityId { get; set; }
    public List<int>? trainingmethodIds { get; set; }
    public List<int>? financialinstrumentIds { get; set; }
    public List<int>? targettrainerIds { get; set; }
    public List<int>? partnertypeIds { get; set; }
    
}
