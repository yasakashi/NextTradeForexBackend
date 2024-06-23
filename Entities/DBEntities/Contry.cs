using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBEntities
{
    [Table("tblcountries")]
    public class Country
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }
    }

    [Table("tblStates")]
    public class State
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }
        public int countryid { get; set; }

        public virtual Country country { get; set; }
    }

    [Table("tblCities")]
    public class City
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }
        public int countryid { get; set; }
        public int stateid { get; set; }

        public virtual Country country { get; set; }
        public virtual State state { get; set; }
    }
}
