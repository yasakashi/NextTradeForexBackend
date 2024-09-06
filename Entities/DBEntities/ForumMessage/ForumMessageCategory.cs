using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities;

[Table("tblForumMessageCategories")]
public class ForumMessageCategory
{
    [Key]
    public Guid Id  { get; set; }
    public Guid forummessageId { get; set; }
    public long categoryid { get; set; }

    #region [ Releations ]
    public virtual ForumMessage forummessage { get; set; }
    public virtual Category category { get; set; }
    #endregion
}
