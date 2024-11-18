using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class Indice_AlternateIndiceDto
    {
        public Guid? id { get; set; }
        public Guid? marketpulsindiceid { get; set; }
        public string? name { get; set; }
        public string? link { get; set; }

    }
}
