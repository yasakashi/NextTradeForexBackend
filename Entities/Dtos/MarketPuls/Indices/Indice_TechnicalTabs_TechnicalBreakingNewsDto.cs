using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class Indice_TechnicalTabs_TechnicalBreakingNewsDto
    {
        public Guid? id { get; set; }
        public Guid? marketpulsindiceid { get; set; }
        public Guid? technicaltabsid { get; set; }

        public string? title { get; set; }

        public string? description { get; set; }
        public string? link { get; set; }
        public string? newsmaincontentfilename { get; set; }
        public string? newsmaincontentfilepath { get; set; }
        public string? newsmaincontentfilecontenttype { get; set; }
    }
}
