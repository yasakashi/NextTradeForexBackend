using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class AdvancedSettingDto
    {
        public Guid? Id { get; set; }
        public bool? quizAutoStart { get; set; }
        public Guid? quizLayoutId { get; set; }
        public Guid? questionsOrderId { get; set; }
        public bool?  hideQuestionNumber { get; set; }
        public int? shortAnswerCharactersLimit { get; set; }
        public int? openEndedEssayQuestionsAnswerCharactersLimit { get; set; }
    }
}
