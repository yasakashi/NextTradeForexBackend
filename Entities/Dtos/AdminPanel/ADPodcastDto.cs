using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Entities.Dtos
{
    public class ADPodcastDto
    {
        public Guid? id { get; set; }
        public string? title { get; set; }
        public string? excerpt { get; set; }
        public string? shortcode { get; set; }
        public string? description { get; set; }
        public string? audiofilename { get; set; }
        public string? audiofilepath { get; set; }
        public string? audiofilecontenttype { get; set; }
        public string? audiofileurl { get; set; }
        public string? featuredimagename { get; set; }
        public string? featuredimagepath { get; set; }
        public string? featuredimageurl { get; set; }
        public string? featuredimagecontenttype { get; set; }
        public IFormFile? audioFile { get; set; }
        public IFormFile? featuredImage { get; set; }

        public List<long>? categoryids { get; set; }
        public List<CategoryBaseDto>? categories { get; set; }
    }

    public class ADPodcastSearchDto : BaseFilterDto
    {
        public Guid? id { get; set; }
        public string? title { get; set; }
        public long? categoryid { get; set; }
        public string? shortcode { get; set; }
    }
}
