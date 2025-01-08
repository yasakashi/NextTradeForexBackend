using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Steratgys_StrategyMainLessonContents")]
    public class StrategyMainLessonContent
    {
        [Key]
        public Guid? id { get; set; }
        public Guid? strategyid { get; set; }
        public int? strategycontenttypeid { get; set; }
        public string? descritption { get; set; }
        public string? descritptionfilename { get; set; }
        public string? descritptionfilepath { get; set; }
        public string? descritptionfileurl { get; set; }
        public string? descritptionfilecontenttype { get; set; }
        public string? image { get; set; }
        public string? imagefilename { get; set; }
        public string? imagefilepath { get; set; }
        public string? imagefileurl { get; set; }
        public string? imagefilecontenttype { get; set; }
        public string? galleryvideo { get; set; }
        public string? galleryvideofilename { get; set; }
        public string? galleryvideofilepath { get; set; }
        public string? galleryvideofileurl { get; set; }
        public string? galleryvideofilecontenttype { get; set; }
        public string? youtubevideo { get; set; }
        public string? videofromanyothersource { get; set; }
        public string? pdftitle { get; set; }
        public string? pdfshortcodeid { get; set; }
        public string? pdfauther { get; set; }
        public string? pdfshortdescription { get; set; }
        public string? tabletitle { get; set; }
        public string? tableshortcodeid { get; set; }
        public string? widgetscript { get; set; }
        public string? audiobook { get; set; }
        public string? audiobookfilename { get; set; }
        public string? audiobookfilepath { get; set; }
        public string? audiobookfileurl { get; set; }
        public string? audiobookfilecontenttype { get; set; }
        public string? galleryimage { get; set; }
        public string? galleryimagefilename { get; set; }
        public string? galleryimagefilepath { get; set; }
        public string? galleryimagefileurl { get; set; }
        public string? galleryimagefilecontenttype { get; set; }

        public virtual Strategy strategy { get; set; }

        public virtual StrategyContentType strategycontenttype { get; set; }

    }
}
