using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos;

public class BaseInformationSearchFilterDto
{
    public long? categoryid { get; set; }
    public long? subcategoryid { get; set; }
    public long? subcategorygroupid { get; set; }
    public long? id { get; set; }
}
