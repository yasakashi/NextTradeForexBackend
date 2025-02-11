using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Entities.Dtos
{
    public class UserProfilePictureDto
    {
        public long userid { get; set; }
        public IFormFile? profilepic { get; set; }
        public string profilepiccontenttype { get; set; }
        public string profilepicpath { get; set; }
        public string profilepicname { get; set; }
        public string profilepicurl { get; set; }

    }
}
