using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblLearnToTrade_Topics")]
    public class LearnToTradeTopic
    {
        [Key]
        public Guid id { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public string? topicfilename { get; set; }
        public string? topicfilepath { get; set; }
        public string? topicfilecontenttype { get; set; }
        public string? topicfileurl { get; set; }
        public int? typeId { get; set; }
        public int? statusId { get; set; }
        public int? forumId { get; set; }
        public string? topicTags { get; set; }

        public virtual TopicType type { get; set; }
        public virtual TopicStatus status { get; set; }
    }
}
