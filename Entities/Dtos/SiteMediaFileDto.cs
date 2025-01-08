using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DBEntities;
using Microsoft.AspNetCore.Http;

namespace Entities.Dtos
{
    public class SiteMediaFileDto
    {
        public Guid? id { get; set; }
        public string? filename { get; set; }
        public string? filepath { get; set; }
        public string? fileurl { get; set; }
        public int? filetypeid { get; set; }
        public string? filecontenttype { get; set; }
        public string? filedescription { get; set; }
        public int? systempartid { get; set; }
        public string? auther { get; set; }
        public DateTime? updatedatetime { get; set; }

        public string? filetypename { get; set; }
        public IFormFile mediafile { get; set; }
    }
    public class SiteMediaFileSearchDto : BaseFilterDto
    {
        public Guid? id { get; set; }
        public int? systempartid { get; set; }
        public int? filetypeid { get; set; }
    }
}
