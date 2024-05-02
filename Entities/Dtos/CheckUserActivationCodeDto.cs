using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class CheckUserActivationCodeDto
    {
        public string mobile { get; set; }
        public string activationcode { get; set; }
    }
}
