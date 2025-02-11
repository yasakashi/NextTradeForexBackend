using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    public class TotalUserReport
    {
        public int? allUsersCount { get; set; }
        public int? administratorCount { get; set; }
        public int? studentCount { get; set; }
        public int? instructorCount { get; set; }
        public int? directorCount { get; set; }
        public int? keymasterCount { get; set; }
        public int? participantCount { get; set; }
        public int? pendingEmailActivationCount { get; set; }
    }
}
