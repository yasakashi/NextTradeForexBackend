using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class UserTrainingmethodDto
{
    public long? Id { get; set; }
    public long? userId { get; set; }
    public int? trainingmethodId { get; set; }
    public string? trainingmethodname { get; set; }
}
