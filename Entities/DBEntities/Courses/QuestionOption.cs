using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblCourseBuilder_QuestionOptions")]
    public class QuestionOption
    {
        [Key]
        public Guid Id { get; set; }
        public Guid lesssonId { get; set; }
        public Guid questionId { get; set; }
        [Column("questionoption")]
        public string? option { get; set; }
        public bool? isAnswer { get; set; }
    }
}
