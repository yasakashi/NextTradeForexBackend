using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class SterategyIndicesCourseDto
    {
        public Guid? sterategid { get; set; }
        public string? indiceschartimage { get; set; }
        public List<SterategyInputInformationDto>? inputinformationlist { get; set; }
        public List<SterategyPlotInformationDto>? plotinformationlist { get; set; }
        public string? plotinformaiondescription { get; set; }
        public string? plotinformaiondescriptionfilename { get; set; }
        public string? plotinformaiondescriptionfilepath { get; set; }
        public string? plotinformaiondescriptionfileurl { get; set; }
        public string? plotinformaiondescriptionfilecontenttype { get; set; }
        public string? marketsynopis { get; set; }
        public string? marketsynopisfilename { get; set; }
        public string? marketsynopisfilepath { get; set; }
        public string? marketsynopisfileurl { get; set; }
        public string? marketsynopisfilecontenttype { get; set; }
        public List<SterategyRelatedFunctionDto>? relatedfunctionlist { get; set; }
        public List<SterategyRelatedTopicDto>? relatedtopiclist { get; set; }
        public string? remarks { get; set; }
        public string? remarksfilename { get; set; }
        public string? remarksfilepath { get; set; }
        public string? remarksfileurl { get; set; }
        public string? remarksfilecontenttype { get; set; }
    }
    public class SterategyRelatedFunctionDto
    { 
    }
    public class SterategyRelatedTopicDto
    { 
    }

}
