using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DBEntities;
using Microsoft.AspNetCore.Http;

namespace Entities.Dtos
{
    public class LearnToTradeTopicDto
    {
        public Guid? id { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public IFormFile? topicfile { get; set; }
        public string? topicfilename { get; set; }
        public string? topicfilepath { get; set; }
        public string? topicfilecontenttype { get; set; }
        public string? topicfileurl { get; set; }
        public int? typeId { get; set; }
        public int? statusId { get; set; }
        public Guid? forumId { get; set; }
        public string? topicTags { get; set; }
        public string? typename { get; set; }
        public string? statusname { get; set; }
    }
    public class LearnToTradeTopicSearchDto:BaseFilterDto
    {
        public Guid? id { get; set; }
        public string? title { get; set; }
        public int? typeId { get; set; }
        public int? statusId { get; set; }
        public Guid? forumId { get; set; }
    }
}
