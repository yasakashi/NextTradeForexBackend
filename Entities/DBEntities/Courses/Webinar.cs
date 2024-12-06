using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblWebinars")]
    public class Webinar
    {
        [Key]
        public Guid id { get; set; }
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
    }

    [Table("tblWebinarsCategories")]
    public class WebinarCategory
    {
        [Key]
        public Guid id { get; set; }
        public Guid webinarid { get; set; }
        public long categoryid { get; set; }

        public virtual Webinar webinar { get; set; }
        public virtual Category category { get; set; }
    }
}
