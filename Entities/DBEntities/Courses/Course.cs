using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.DBEntities;

namespace Entities.DBEntities;
[Table("tblCourses")]
public class Course
{
    [Key]
    public Guid Id { get; set; }
    public int courseleveltypeId { get; set; }
    public string coursename { get; set; }
    public int courseduringtime { get; set; }
    public decimal courseprice { get; set; }
    public DateTime startdate { get; set; }
    public DateTime enddate { get; set; }
    public int lessencount { get; set; }
    public bool allowdownload { get; set; }
    public byte[]? coursecoverimage { get; set; }
    public int coursetypeId { get; set; }
    public DateTime registerdatetime { get; set; }
    public bool isprelesson { get; set; }
    public long owneruserId { get; set; }
    public string coursedescription { get; set; }
    public bool? isadminaccepted { get; set; }
    public bool? issitecourse { get; set; }
    public string? coursetgas { get; set; }
    public long? grouptypeId { get; set; }
    public string? courseintrofileurl { get; set; }
    public string? courseintrofilefileextention { get; set; }
    public string? courseintrofilecontenttype { get; set; }
    public virtual CourseLevelType courseleveltype { get; set; }
    public virtual CourseType coursetype { get; set; }
    public virtual User owneruser { get; set; }
    public virtual GroupType grouptype { get; set; }
    public virtual List<CourseLesson> CourseLessons { get; set; }

}
