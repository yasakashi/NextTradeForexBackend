using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{


    [Table("tblCourseBuilder_Questions")]
    public class Question
    {
        [Key]
        public Guid Id { get; set; }
        //public Guid lessionId { get; set; }
        public Guid coursebuilderquizId { get; set; }
        public string questionTitle { get; set; }
        /*
         'true/false', 'single choice', 'multiple choice', 'open-ended', 'fill in the blanks', 'short answer', 'matching', 'image matching', 'image answering', 'ordering'
         */
        public int questionType { get; set; }
        public bool isAnswerRequired { get; set; }
        public bool isRandomized { get; set; }
        public int points { get; set; }
        public bool displayPoints { get; set; }
        public string questionDescription { get; set; }
        public List<QuestionOption> options { get; set; }
    }

 
}
