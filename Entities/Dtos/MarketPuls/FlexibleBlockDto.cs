using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class FlexibleBlockDto
    {
        public Guid? id { get; set; }
        public Guid? marketpulsforexid { get; set; }
        public string? MainTitle { get; set; }
        public string? countries { get; set; }
        public string? pairsthatcorrelate { get; set; }
        public string? highslows { get; set; }
        public string? pairtype { get; set; }
        public string? dailyavrage { get; set; }
    }
}
