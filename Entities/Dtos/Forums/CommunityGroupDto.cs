using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class CommunityGroupDto
{
    public Guid? Id { get; set; }
    public string title { get; set; }
    public DateTime? createdatetime { get; set; }
    public long? owneruserid { get; set; }
    public long categoryid { get; set; }
    public string categoryname { get; set; }
    public byte[] coverimage { get; set; }
}
