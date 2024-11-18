using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class Indice_TechnicalTabsDto
    {
        public Guid? id { get; set; }
        public string? tabtitle { get; set; }
        public string? script { get; set; }
        public List<Indice_TechnicalTabs_TechnicalBreakingNewsDto>? technicalbreakingnewslist { get; set; }
    }
}
