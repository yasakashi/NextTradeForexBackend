using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class CourseCategoryDto
    {
        public Guid? Id { get; set; }
        public Guid? courseId { get; set; }
        public long categoryId { get; set; }
        public string? categoryname { get; set; }
    }
}
