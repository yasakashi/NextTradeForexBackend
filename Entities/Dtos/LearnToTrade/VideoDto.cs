using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Entities.Dtos
{
    public class VideoDto
    {
        public Guid? id { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public bool? downloadable { get; set; }
        public string? excerpt { get; set; }
        public int? lessonCategoryLevelId { get; set; }
        public string? videofilename { get; set; }
        public string? videofilepath { get; set; }
        public string? videofileurl { get; set; }
        public string? videofilecontenttype { get; set; }
        public string? featuredimagename { get; set; }
        public string? featuredimagepath { get; set; }
        public string? featuredimageurl { get; set; }
        public string? featuredimagecontenttype { get; set; }

        public List<long>? categoriesIds { get; set; }
        public List<CategoryBaseDto> categories { get; set; }

        public IFormFile? videofile { get; set; }
        public IFormFile? featuredimage { get; set; }
    }

    public class VideoSearchDto:BaseFilterDto
    {
        public Guid? id { get; set; }
        public string? title { get; set; }
        public int? lessonCategoryLevelId { get; set; }
        public long? categoryId { get; set; }
        public bool? downloadable { get; set; }
    }
}
