using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblLearnToTrade_EnableAutoplayAutomaticallys")]
    public class EnableAutoplayAutomatically
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
    }
}
