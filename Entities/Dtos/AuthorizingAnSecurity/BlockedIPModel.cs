using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class BlockedIPModel
    {
        public long? id { get; set; }
        /// <summary>
        /// IP
        /// که باید بسته شود
        /// </summary>
        public string bip { get; set; }
    }
}
