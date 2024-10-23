using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{

    [Table("tblCourseBuilder_AdvancedSettings")]
    public class AdvancedSetting
    {
        [Key]
        public Guid Id { get; set; }
        public Guid coursebuilderquizId {  get; set; }
        public bool quizAutoStart { get; set; }
        public Guid quizLayoutId { get; set; }
        public Guid questionsOrderId { get; set; }
        public bool hideQuestionNumber { get; set; }
        public int shortAnswerCharactersLimit { get; set; }
        public int openEndedEssayQuestionsAnswerCharactersLimit { get; set; }

        public virtual CourseBuilderQuiz coursebuilderquiz { get; set; }
    }

}
