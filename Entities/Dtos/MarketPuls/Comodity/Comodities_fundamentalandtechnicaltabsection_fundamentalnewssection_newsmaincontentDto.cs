using Entities.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontentDto
    {
        public Guid? id { get; set; }
        public Guid? comodityid { get; set; }
        public Guid? comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_id { get; set; }

        public string? title { get; set; }
        public string? description { get; set; }
        public string? link { get; set; }
        public string? filename { get; set; }
        public string? filepath { get; set; }
        public string? filecontenttype { get; set; }
    }
}
