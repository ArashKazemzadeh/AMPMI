using Domin.Entities;
using WebSite.EndPoint.Models.ProductViewModel;

namespace WebSite.EndPoint.Models.CompanyViewModel
{
    public class CompanyDetailVM
    {
        public string Name { get; set; } = null!;
        public string MobileNumber { get; set; } = null!;
        public string ManagerName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; } = null!;
        public string? Website { get; set; }

        public string? LogoRout { get; set; }
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
        public string Information 
        {
            get
            {
                string result = string.Empty;
                if (!string.IsNullOrEmpty(QualityGrade))
                {
                    result += QualityGrade;
                }
                if(Capacity > 0)
                {
                    result += "_"+Capacity;
                }
                if (!string.IsNullOrEmpty(Iso))
                {
                    result += "_"+Iso;
                }
                if (!string.IsNullOrEmpty(Partnership))
                {
                    result += "_"+Partnership;
                }
                if (result.StartsWith('_'))
                    result.Remove(0,1);

                return result;
            }
        }

        public string? Tel { get; set; }
        public string? TeaserGuid { get; set; }
        public string? About { get; set; }
        public List<ProductVM>? Products { get; set; }
        public List<string>? CompanyPictures { get; set; }

    }
}
