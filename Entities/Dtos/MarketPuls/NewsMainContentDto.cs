using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class NewsMainContentDto
    {
        public Guid? id { get; set; }
        public Guid? marketpulsforexid { get; set; }

        public string? maintitle { get; set; }

        public string? script { get; set; }

    }
}
