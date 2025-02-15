using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Entities.Dtos
{
    public class FeedBackDto
    {
        public Guid id { get; set; }
        public long? userid { get; set; }
        public string? username { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public DateTime? registerdatetime { get; set; }
        public string? feedbackfilename { get; set; }
        public IFormFile? feedbackfile { get; set; }
        public string? feedbackfileurl { get; set; }
        public string? feedbackfilepath { get; set; }
        public string? feedbackfilecontenttype { get; set; }
        public string? featuredimagefilename { get; set; }
        public IFormFile? featuredimagefile { get; set; }
        public string? featuredimagefileurl { get; set; }
        public string? featuredimagefilepath { get; set; }
        public string? featuredimagefilecontenttype { get; set; }

    }

    public class FeedBackSearchDto :BaseFilterDto
    {
        public Guid? id { get; set; }
        public long? userid { get; set; }
        public string? title { get; set; }
        public DateTime? fromregisterdatetime { get; set; }
        public DateTime? toregisterdatetime { get; set; }
    }

}
