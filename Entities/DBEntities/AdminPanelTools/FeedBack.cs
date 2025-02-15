using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblAdminPanel_UserFeedBacks")]

    public class FeedBack
    {
        [Key]
        public Guid id { get; set; }
        public long userid { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public DateTime? registerdatetime { get; set; }
        public string? feedbackfilename { get; set; }
        public string? feedbackfileurl { get; set; }
        public string? feedbackfilepath { get; set; }
        public string? feedbackfilecontenttype { get; set; }
        public string? featuredimagefilename { get; set; }
        public string? featuredimagefileurl { get; set; }
        public string? featuredimagefilepath { get; set; }
        public string? featuredimagefilecontenttype { get; set; }

        public virtual User user { get; set; }
    }
}
