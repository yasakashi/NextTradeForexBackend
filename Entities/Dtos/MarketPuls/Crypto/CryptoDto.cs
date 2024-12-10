using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class CryptoDto
    {
        public Guid? id { get; set; }
        public List<Crypto_FlexibleBlockDto> crypto_flexi_block { get; set; }
        public CryptoFundamentalAndTechnicalTabSectionDto fundamentalandtechnicaltabsection { get; set; }
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
    }
    public class CryptoFilterDto : BaseFilterDto
    {
        public Guid? id { get; set; }

        public long? categoryid { get; set; }
        public long? creatoruserid { get; set; }
        public string? title { get; set; }
        public string? authorname { get; set; }
        public long? authorid { get; set; }
        public bool? isvisible { get; set; }
        public int? courseleveltypeId { get; set; }
        public int? coursestatusid { get; set; }
    }
}
