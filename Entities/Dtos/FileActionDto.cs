using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class FileActionDto
    {
        public string? filename { get; set; }
        public string? filepath { get; set; }
        public string? filecontent { get; set; }
        public string? fileurl { get; set; }
    }
}
