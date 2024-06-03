using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities;

    [Table("tblGroupTypes")]
    public class GroupType
    {
        [Key]
        public long Id { get; set; }
        public string name { get; set; }
    }

