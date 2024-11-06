using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class CourseBuilderTopicDto : BaseFilterDto
    {
        public Guid? Id { get; set; }
        public Guid? courseId { get; set; }
        public string? topicName { get; set; }
        public string? topicSummary { get; set; }
        public int? topicorder { get; set; }
    }
}
