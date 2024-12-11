using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblLearnToTrade_Videos")]
    public class Video
    {
        [Key]
        public Guid id { get; set; }
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
    }


    [Table("tblLearnToTrade_VideoCategories")]
    public class VideoCategory
    {
        [Key]
        public Guid id { get; set; }
        public Guid videoid { get; set; }
        public long categoryid { get; set; }

        public virtual Video video { get; set; }
        public virtual Category category { get; set; }
    }


    [Table("tblLearnToTrade_VideoSubtitles")]
    public class VideoSubtitle
    {
        [Key]
        public Guid id { get; set; }
        public Guid videoid { get; set; }
        public string? lang { get; set; }
        public string? subtitlefilename { get; set; }
        public string? subtitlefilepath { get; set; }
        public string? subtitlefileurl { get; set; }
        public string? subtitlefilecontenttype { get; set; }

        public virtual Video video { get; set; }

    }
}
