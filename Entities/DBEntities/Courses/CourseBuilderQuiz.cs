using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{

    [Table("tblCourseBuilder_Quizs")]
    public class CourseBuilderQuiz
    {
        [Key]
        public Guid Id { get; set; }
        public Guid courseId { get; set; }
        public Guid topicId { get; set; }
        public string? quizTitle { get; set; }
        public string? quizDescription { get; set; }
        public int? timeLimit { get; set; }
        public bool? displayQuizTime { get; set; }
        public long? quizFeedbackModeId { get; set; }
        public int? attemptsAllowed { get; set; }
        [Range(1, 100)]
        public decimal passingGrade { get; set; }
        public int maxQuestionsAllowedToAnswer { get; set; }
        public List<Question> questions { get; set; }
    }
}
