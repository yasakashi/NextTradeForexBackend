using Entities.DBEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities;

[Table("tblCommunityGroupGalleries")]
public class CommunityGroupGallery
{
    [Key]
    public Guid Id { get; set; }
    public long userId { get; set; }
    public Guid communitygroupId { get; set; }
    public string galleryname { get; set; }
    public string description { get; set; }
    public int? gallerytypeId { get; set; }
    public int? galleryaccesslevelId { get; set; }

    #region [ relations ]
    public virtual User user { get; set; }
    public virtual CommunityGroup communitygroup { get; set; }
    public virtual CommunityGroupGalleryType gallerytype { get; set; }
    public virtual CommunityGroupGalleryAccessLevel galleryaccesslevel { get; set; }
    #endregion [ relations ]
}
