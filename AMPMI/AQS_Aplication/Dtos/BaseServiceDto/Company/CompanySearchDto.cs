using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQS_Application.Dtos.BaseServiceDto.Company
{
    public class CompanySearchDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string MobileNumber { get; set; } = null!;
        public string ManagerName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; } = null!;
        public string? LogoRout { get; set; }
        public string? TeaserGuid { get; set; }
        public string? Tel { get; set; }
        public string? Website { get; set; }

        /// <summary>
        /// گرید کیفی + ظرفیت تولید + ISO + همکاری با شرکت ها
        /// </summary>
        /// <summary>
        /// ظرفیت
        /// </summary>
        public int Capacity { get; set; }
        /// <summary>
        /// همکاری
        /// </summary>
        public string? Partnership { get; set; }
        /// <summary>
        /// گرید کیفی
        /// </summary>
        public string? QualityGrade { get; set; }
        public string? Iso { get; set; }
    }
}
