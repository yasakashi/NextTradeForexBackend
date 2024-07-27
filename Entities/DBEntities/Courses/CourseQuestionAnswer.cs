using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Entities.DBEntities;

[Table("tblCourseQuestionsAnswers")]
public class CourseQuestionAnswer
{
    [Key]
    public Guid Id { get; set; }
    public Guid courseId { get; set; }
    public string question { get; set; }
    public string answer { get; set; }
    public DateTime questiondatetime { get; set; }
    public DateTime answerdatetime { get; set; }
    public long questionuserId { get; set; }
    public long answeruserId { get; set; }

    public virtual User questionuser { get; set; }
    public virtual User answeruser { get; set; }
    public virtual Course course { get; set; }
}
