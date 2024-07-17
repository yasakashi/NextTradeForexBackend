using Entities.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class SiteMessageDto:BaseFilterDto
    {
        public Guid? Id { get; set; }
        public string? messagetitle { get; set; }
        public string? messagebody { get; set; }
        public DateTime? registerdatetime { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public long? reciveruserId { get; set; }
        public long? creatoruserId { get; set; }
        public string? creatorusername { get; set; }
        public bool? isvisited { get; set; }
        public string? reciverusername { get; set; }

    }
}
