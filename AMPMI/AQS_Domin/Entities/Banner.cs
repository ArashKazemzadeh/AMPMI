using System.ComponentModel.DataAnnotations;

namespace AQS_Domin.Entities
{
    public class Banner
    {
        public BannerIdEnum Id { get; set; }
        public string Rout { get; set; }
    }
    public enum BannerIdEnum
    {
        rout1 = 1,
        rout2 = 2,
        rout3 = 3
    }
}
