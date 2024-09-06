using Entities.DBEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities;

[Table("tblCommunityGroupGalleryFiles")]
public class CommunityGroupGalleryFile
{
    [Key]
    public Guid Id { get; set; }
    public long userId { get; set; }
    public Guid galleryId { get; set; }
    public string fileurl { get; set; }
    public string fileextention { get; set; }
    public string filename { get; set; }
    public string contenttype { get; set; }
    public DateTime? createdatetime { get; set; }
    public string title { get; set; }
    public string description { get; set; }

    #region [ relations ]
    public virtual User user { get; set; }
    public virtual CommunityGroupGallery gallery { get; set; }
    #endregion [ relations ]
}
