using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Entities.Dtos
{
    public class StrategyMainLessonContentDto
    {
        public Guid? id { get; set; }
        public Guid? strategyid { get; set; }
        public int? strategycontenttypeid { get; set; }
        public string? descritption { get; set; }
        public IFormFile? descritptionfile { get; set; }
        public string? descritptionfilename { get; set; }
        public string? descritptionfilepath { get; set; }
        public string? descritptionfileurl { get; set; }
        public string? descritptionfilecontenttype { get; set; }
        public IFormFile? imagefile { get; set; }
        public string? imagefilename { get; set; }
        public string? imagefilepath { get; set; }
        public string? imagefileurl { get; set; }
        public string? imagefilecontenttype { get; set; }
        public IFormFile? galleryvideo { get; set; }
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
        public IFormFile? audiobook { get; set; }
        public string? audiobookfilename { get; set; }
        public string? audiobookfilepath { get; set; }
        public string? audiobookfileurl { get; set; }
        public string? audiobookfilecontenttype { get; set; }
        public IFormFile? galleryimage { get; set; }
        public string? galleryimagefilename { get; set; }
        public string? galleryimagefilepath { get; set; }
        public string? galleryimagefileurl { get; set; }
        public string? galleryimagefilecontenttype { get; set; }
    }


}
