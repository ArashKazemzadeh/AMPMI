using Domin.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQS_Application.Dtos.BaseServiceDto.Company
{
    public class CompanyEditProfileDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ManagerName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string? Brands { get; set; }
        public int Capacity { get; set; }
        public string? Partnership { get; set; }
        public string? QualityGrade { get; set; }
        public string? Iso { get; set; }
        public string? About { get; set; }
        public string? LogoRout { get; set; }
        public bool IsCompany { get; set; }
        public bool SendRequest { get; set; }
        public string? TeaserGuid { get; set; }
        public string? BannerRout { get; set; }
        public string? Tel { get; set; }
        public virtual ICollection<CompanyPicture> CompanyPictures { get; set; } = new List<CompanyPicture>();
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        public virtual ICollection<SeenNotifByCompany> SeenNotifByCompanies { get; set; } = new List<SeenNotifByCompany>();
    }

}