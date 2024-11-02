using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class Comodities_fundamentalandtechnicaltabsection_fundamentalnewssectionDto
    {
        public Guid? id { get; set; }
        public Guid? comodityid { get; set; }
        public string? fundamentalandtechnicaltabsection_fundamentalnewssection_maintitle { get; set; }
        public string? fundamentalandtechnicaltabsection_fundamentalnewssection_script { get; set; }
        public List<Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontentDto> Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontentlist { get; set; }
    }
}
