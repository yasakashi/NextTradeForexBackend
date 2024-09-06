using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities;

[Table("tblCourseMeetFiles")]
public class CourseMeetFile
{
    [Key]
    public Guid Id { get; set; }
    public Guid courseMeetId { get; set; }
    public byte[]? attachment { get; set; }
    public string? fileextention { get; set; }
    public string? contenttype { get; set; }
    public string? filename { get; set; }

    public virtual CourseMeet courseMeet { get; set; }

}
