using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Entities.Dtos
{
    public class WebinarDto
    {
        public Guid? id { get; set; }
        public DateTime? dateAndTime { get; set; }
        public string? title { get; set; }
        public string? excerpt { get; set; }
        public string? description { get; set; }
        public string? videofilename { get; set; }
        public string? videofilepath { get; set; }
        public string? videofilecontenttype { get; set; }
        public string? videofileurl { get; set; }
        public string? featuredimagename { get; set; }
        public string? featuredimagepath { get; set; }
        public string? featuredimageurl { get; set; }
        public string? featuredimagecontenttype { get; set; }
        public string? meetingLink { get; set; }
        public IFormFile? videoFile { get; set; }
        public IFormFile? featuredImage { get; set; }
        public List<long>? categoryids { get; set; }
        public List<CategoryBaseDto> categories { get; set; }
    }
    public class WebinarSearchDto:BaseFilterDto
    {
        public Guid? id { get; set; }
        public DateTime? fromdateAndTime { get; set; }
        public DateTime? todateAndTime { get; set; }
        public string? title { get; set; }
        public long? categoryid { get; set; }

    }
}
