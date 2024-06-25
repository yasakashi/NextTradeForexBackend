using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class CourseDto : BaseFilterDto
{
    public Guid? Id { get; set; }
    public int courseleveltypeId { get; set; }
    public string? courseleveltypename { get; set; }
    public string? coursename { get; set; }
    public int? courseduringtime { get; set; }
    public decimal? courseprice { get; set; }
    public DateTime? startdate { get; set; }
    public DateTime? enddate { get; set; }
    public int? lessencount { get; set; }
    public bool? allowdownload { get; set; }
    public byte[]? coursecoverimage { get; set; }
    public int coursetypeId { get; set; }
    public string? coursetypename { get; set; }
    public DateTime? registerdatetime { get; set; }
    public bool isprelesson { get; set; }
    public long? owneruserId { get; set; }
    public string? ownerusername { get; set; }
    public string? coursedescription { get; set; }
    public bool? isadminaccepted { get; set; }
}
