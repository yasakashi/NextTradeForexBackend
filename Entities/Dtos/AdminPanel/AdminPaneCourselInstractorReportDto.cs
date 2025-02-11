using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class AdminPaneCourselInstractorReportDto
    {
        public int? allInstructorsCount { get; set; }
        public int? approvedInstructorsCount { get; set; }
        public int? pendingInstructorsCount { get; set; }
        public int? blockedInstructorsCount { get; set; }

    }
}
