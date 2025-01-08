using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQS_Application.Dtos.BaseServiceDto.Company
{
    public class CompanyReadDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Logo { get; set; }
    }
}
