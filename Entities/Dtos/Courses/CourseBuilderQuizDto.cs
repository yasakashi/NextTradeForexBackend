using Entities.DBEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class CourseBuilderQuizDto : BaseFilterDto
    {
        public Guid? Id { get; set; }
        public Guid? courseId { get; set; }
        public Guid? topicId { get; set; }
        public string? quizTitle { get; set; }
        public string? quizDescription { get; set; }
        public int? timeLimit { get; set; }
        public bool? displayQuizTime { get; set; }
        public long? quizFeedbackModeId { get; set; }
        public int? attemptsAllowed { get; set; }
        public decimal? passingGrade { get; set; }
        public int? maxQuestionsAllowedToAnswer { get; set; }
        public AdvancedSettingDto? advancedSettings { get; set; }
        public List<QuestionDto> questions { get; set; }
    }
}
