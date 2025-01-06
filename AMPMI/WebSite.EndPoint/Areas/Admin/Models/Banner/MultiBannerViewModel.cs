using AQS_Domin.Entities;

namespace WebSite.EndPoint.Areas.Admin.Models.Banner
{
    public class MultiBannerViewModel
    {
        public IFormFile Banner1 { get; set; }
        public IFormFile Banner2 { get; set; }
        public IFormFile Banner3 { get; set; }
        public string rout1 { get; set; }
        public string rout2 { get; set; }
        public string rout3 { get; set; }
    }
}
