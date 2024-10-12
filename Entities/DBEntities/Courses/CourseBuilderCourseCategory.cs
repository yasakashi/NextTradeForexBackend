using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{

    [Table("tblCourseBuilder_CourseCategories")]
    public class CourseCategory
    {
        [Key]
        public Guid Id { get; set; }
        public Guid courseId { get; set; }
        public long categoryId { get; set; }

        public Category category { get; set; }

    }
}
