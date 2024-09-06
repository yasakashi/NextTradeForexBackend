using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class CourseDto : BaseFilterDto
{
    public Guid? Id { get; set; }
    public int? courseleveltypeId { get; set; }
    public string? courseleveltypename { get; set; }
    public string? coursename { get; set; }
    public int? courseduringtime { get; set; }
    public decimal? courseprice { get; set; }
    public DateTime? startdate { get; set; }
    public DateTime? enddate { get; set; }
    public int? lessencount { get; set; }
    public bool? allowdownload { get; set; }
    public byte[]? coursecoverimage { get; set; }
    public int? coursetypeId { get; set; }
    public string? coursetypename { get; set; }
    public DateTime? registerdatetime { get; set; }
    public bool? isprelesson { get; set; }
    public long? owneruserId { get; set; }
    public string? ownerusername { get; set; }
    public string? coursedescription { get; set; }
    public bool? isadminaccepted { get; set; }
    public bool? issitecourse { get; set; }
    public string? username { get; set; }
    public string? coursetgas { get; set; }
    public long? grouptypeId { get; set; }
    public string? grouptypename { get; set; }
    public string? courseintrofileurl { get; set; }
    public string? courseintrofilefileextention { get; set; }
    public string? courseintrofilecontenttype { get; set; }
    public int? pagecount { get; set; }
    public string? whatlearn { get; set; }
    public string? targetedaudience { get; set; }
    public string? materialsincluded { get; set; }
    public string? requirementsinstructions { get; set; }
    public bool? allowQA { get; set; }
    public byte? courseStatusId { get; set; } = 0;

}
