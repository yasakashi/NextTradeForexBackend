using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblAdminPanel_podcasts")]
    public class ADPodcast
    {
        [Key]
        public Guid id { get; set; }
        public string? title { get; set; }
        public string? shortcode { get; set; }
        public string? excerpt { get; set; }
        public string? description { get; set; }
        public string? audiofilename { get; set; }
        public string? audiofilepath { get; set; }
        public string? audiofilecontenttype { get; set; }
        public string? audiofileurl { get; set; }
        public string? featuredimagename { get; set; }
        public string? featuredimagepath { get; set; }
        public string? featuredimageurl { get; set; }
        public string? featuredimagecontenttype { get; set; }
    }


    [Table("tblAdminPanel_podcastsCategories")]
    public class ADPodcastCategory
    {
        [Key]
        public Guid id { get; set; }
        public Guid podcastid { get; set; }
        public long categoryid { get; set; }

        public virtual ADPodcast podcast { get; set; }
        public virtual Category category { get; set; }
    }
}
