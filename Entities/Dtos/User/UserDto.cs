using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class UserDto
{
    public long? Id { get; set; }
    public string? username { get; set; }
    public string? oldpassword { get; set; }
    public string? newpassword { get; set; }
}
