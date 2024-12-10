using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Entities.Dtos
{
    public class VideoSubtitleDto
    {
        public Guid? id { get; set; }
        public Guid? videoid { get; set; }
        public string? lang { get; set; }
        public string? subtitlefilename { get; set; }
        public string? subtitlefilepath { get; set; }
        public string? subtitlefileurl { get; set; }
        public string? subtitlefilecontenttype { get; set; }

        public IFormFile? subtitlefile { get; set; }
    }

    public class VideoSubtitleSearchDto:BaseFilterDto
    {
        public Guid? id { get; set; }
        public Guid? videoid { get; set; }
    }
}
