using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblMarketPuls_Forex_SecondCountryDatas")]
    public class SecondCountryData
    {
        [Key]
        public Guid id { get; set; }
        public Guid marketpulsforexid { get; set; }
        public string countries { get; set; }
        public string centralbank { get; set; }
        public string nickname { get; set; }
        public string avragedaily { get; set; }
    }
}
