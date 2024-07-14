using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblSiteMessages")]
    public class SiteMessage
    {
        [Key]
        public Guid Id { get; set; }
        public string messagetitle { get; set; }
        public string messagebody { get; set; }
        public DateTime registerdatetime { get; set; }
        public long reciveruserId { get; set; }
        public bool isvisited { get; set; }
        public virtual User reciveruser { get; set; }
    }
}
