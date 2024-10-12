using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{

    [Table("tblCourseBuilder_Courses")]
    public class CourseBuilderCourse
    {
        [Key]
        public Guid Id { get; set; }
        public string? courseName { get; set; }
        public string? courseDescription { get; set; }
        public string? courseFilename { get; set; }
        public string? courseFilepath { get; set; }
        public string? courseFilecontent { get; set; }
        public string? excerpt { get; set; }
        public long? authorId { get; set; }
        public int? maximumStudents { get; set; }
        public int? courseleveltypeId { get; set; }
        public bool? isPublicCourse { get; set; }
        public bool? allowQA { get; set; }
        public decimal? coursePrice { get; set; }
        public string? whatWillILearn { get; set; }
        public string? targetedAudience { get; set; }
        public int? courseDuration { get; set; }
        public string? materialsIncluded { get; set; }
        public string? requirementsInstructions { get; set; }
        public string? courseIntroVideo { get; set; }
        public string? courseTags { get; set; }
        public string? featuredImagename { get; set; }
        public string? featuredImagepath { get; set; }
        public string? featuredImagecontent { get; set; }
        public DateTime registerdatetime { get; set; }
        public List<CourseBuilderMeeting> meetings { get; set; }
        public List<CourseBuildeVideoPdfUrl> videoPdfUrls { get; set; }
        public List<CourseCategory> courseCategorys { get; set; }
    }





    public class AdvancedSettings
    {
        public string quizAutoStart { get; set; }
        public string quizLayoutId { get; set; }
        public string questionsOrderId { get; set; }
        public string hideQuestionNumber { get; set; }
        public string shortAnswerCharactersLimit { get; set; }
        public string openEndedEssayQuestionsAnswerCharactersLimit { get; set; }
    }

    public class Option
    {
        public string option { get; set; }
        public string isAnswer { get; set; }
    }

    public class Question
    {
        public string questionTitle { get; set; }
        public string questionType { get; set; }
        public string isAnswerRequired { get; set; }
        public string isRandomized { get; set; }
        public string points { get; set; }
        public string displayPoints { get; set; }
        public string questionDescription { get; set; }
        public List<Option> options { get; set; }
    }

    public class CourseBuilderQuiz
    {
        public string courseId { get; set; }
        public string topicId { get; set; }
        public string quizTitle { get; set; }
        public string quizDescription { get; set; }
        public List<Question> questions { get; set; }
        public string timeLimit { get; set; }
        public string displayQuizTime { get; set; }
        public string quizFeedbackModeId { get; set; }
        public string attemptsAllowed { get; set; }
        public string passingGrade { get; set; }
        public string maxQuestionsAllowedToAnswer { get; set; }
        public AdvancedSettings advancedSettings { get; set; }
    }

}
