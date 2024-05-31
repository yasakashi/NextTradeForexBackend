using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entities.DBEntities;

[Table("tblCommunityGroups")]

    public class CommunityGroup
    {
    [Key]
    public Guid Id { get; set; }
    [Column("grouptitle")]
    public string title { get; set; }
    public DateTime createdatetime { get; set; }
    public long owneruserid { get; set; }
    public long categoryid { get; set; }
    public byte[] coverimage { get; set; }

    public virtual Category category { get; set; }
    public virtual User owneruser { get; set; }
}

