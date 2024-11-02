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

        public string? fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontent_title { get; set; }
        public string? fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontent_description { get; set; }
        public string? fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontent_link { get; set; }
        public string? fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontent_filename { get; set; }
        public string? fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontent_filepath { get; set; }
        public string? fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontent_filecontenttype { get; set; }
    }
}
