using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class BaseFilterDto
    {
        public int? rowcount { get; set; }
        public int? pageindex { get; set; }
        public List<SortItem> sortitem { get; set; }
    }

    public class  SortItem
    {
        public string fieldname { get; set; }
        public bool? ascending { get; set; }
    }
}
