using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblSiteFiles")]
    public class SiteMediaFile
    {
        [Key]
        public Guid id { get; set; }
        public string? filename { get; set; }
        public string? filepath { get; set; }
        public string? fileurl { get; set; }
        public int? filetypeid { get; set; }
        public string? filecontenttype { get; set; }
        public string? filedescription { get; set; }
        public int? systempartid { get; set; }
        public string? auther { get; set; }
        public DateTime? updatedatetime { get; set; }

        public virtual FileType filetype { get; set; }
        public virtual SystemPart systempart { get; set; }
    }
}

