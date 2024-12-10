using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Cryptos")]
    public class Crypto
    {
        [Key]
        public Guid id { get; set; }
        public long? categoryid { get; set; }
        public string? title { get; set; }
        public string? tags { get; set; }
        public string? excerpt { get; set; }
        public string? authorname { get; set; }
        public long? authorid { get; set; }
        public bool? isvisible { get; set; }
        public int? courseleveltypeId { get; set; }
        public int? coursestatusid { get; set; }
        public long? creatoruserid { get; set; }
        public DateTime? createdatetime { get; set; }
        public DateTime? changestatusdate { get; set; }
        public string? cryptofundamentalandtechnicaltabsection_instrumentname { get; set; }
        public string? cryptofundamentalandtechnicaltabsection_fundamentalheading { get; set; }
        public string? cryptofundamentalandtechnicaltabsection_technicalheading { get; set; }
        public string? cryptofundamentalandtechnicaltabsection_marketsessiontitle { get; set; }
        public string? cryptofundamentalandtechnicaltabsection_marketsessionscript { get; set; }
        public string? cryptofundamentalandtechnicaltabsection_marketsentimentstitle { get; set; }
        public string? cryptofundamentalandtechnicaltabsection_marketsentimentsscript { get; set; }
        public string? cryptofundamentalandtechnicaltabsection_relatedresorces { get; set; }
        public string? cryptofundamentalandtechnicaltabsection_privatenotes { get; set; }

        public virtual Category category{ get; set; }
    }
}
