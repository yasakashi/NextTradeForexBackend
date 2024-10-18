using Entities.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class QuestionDto
    {
        public Guid? Id { get; set; }
        public Guid? QuizId { get; set; }
        public string? questionTitle { get; set; }
        public int? questionType { get; set; }
        public bool? isAnswerRequired { get; set; }
        public bool? isRandomized { get; set; }
        public int? points { get; set; }
        public bool? displayPoints { get; set; }
        public string? questionDescription { get; set; }
        public List<QuestionOptionDto> options { get; set; }
    }
}
