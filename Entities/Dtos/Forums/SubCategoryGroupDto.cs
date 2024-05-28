using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos.Forums
{
    public class SubCategoryGroupDto
    {
        public long? Id { get; set; }
        public string name { get; set; }
        public long categotyId { get; set; }
        public long subcategotyId { get; set; }
    }
}
