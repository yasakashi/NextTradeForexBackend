using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class QuestionOptionDto : BaseFilterDto
    {
        public Guid? Id { get; set; }
        public Guid? lesssonId { get; set; }
        public Guid? questionId { get; set; }
        public string? option { get; set; }
        public bool? isAnswer { get; set; }
    }
}
