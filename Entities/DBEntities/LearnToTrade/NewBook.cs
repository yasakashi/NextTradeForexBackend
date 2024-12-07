using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblNewBooks")]
    public class NewBook
    {
        [Key]
        public Guid id { get; set; }
        public string? title { get; set; }
        public string? author { get; set; }
        public string? description { get; set; }
        public string? bgColor { get; set; }
        public string? flipDuration { get; set; }
        public string? containerHeight { get; set; }
        public string? autoplayDuration { get; set; }
        public bool? forcePageFit { get; set; }
        public bool? autoEnableOutline { get; set; }
        public bool? autoEnableThumbnail { get; set; }
        public bool? overwritePdfOutline { get; set; }

        public int? bookSourceTypeId { get; set; }
        public int? displayMode { get; set; }
        public int? hardPageId { get; set; }
        public int? pdfPageRenderSize { get; set; }
        public int? autoEnableSound { get; set; }
        public int? enableDownload { get; set; }
        public int? pageMode { get; set; }
        public int? singlePageMode { get; set; }
        public int? controlsPosition { get; set; }
        public int? direction { get; set; }
        public int? enableAutoplay { get; set; }
        public int? enableAutoplayAutomatically { get; set; }
        public int? pageSize { get; set; }
        public int? lessonCategoryLevelId { get; set; }


        public string? featuredimagename { get; set; }
        public string? featuredimagepath { get; set; }
        public string? featuredimageurl { get; set; }
        public string? featuredimagecontenttype { get; set; }
        public string? bgimagename { get; set; }
        public string? bgimagepath { get; set; }
        public string? bgimageurl { get; set; }
        public string? bgimagecontenttype { get; set; }

        public string? pdffilename { get; set; }
        public string? pdffilepath { get; set; }
        public string? pdffileurl { get; set; }
        public string? pdffilecontenttype { get; set; }
        public string? pdfthumbnailimagename { get; set; }
        public string? pdfthumbnailimagepath { get; set; }
        public string? pdfthumbnailimageurl { get; set; }
        public string? pdfthumbnailimagecontenttype { get; set; }

    }

    [Table("tblNewBookCategories")]
    public class NewBookCategory
    {
        [Key]
        public Guid id { get; set; }
        public Guid newbookid { get; set; }
        public long categoryid { get; set; }
        public virtual NewBook newbook { get; set; }
        public virtual Category category { get; set; }
    }

    [Table("tblNewBookPageImages")]
    public class NewBookPageImage
    {
        [Key]
        public Guid id { get; set; }
        public Guid newbookid { get; set; }
        public string? pageimagename { get; set; }
        public string? pageimagepath { get; set; }
        public string? pageimageurl { get; set; }
        public string? pageimagecontenttype { get; set; }

        public virtual NewBook newbook { get; set; }
    }
}
