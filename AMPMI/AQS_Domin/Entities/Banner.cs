using System.ComponentModel.DataAnnotations;

namespace AQS_Domin.Entities
{
    public class Banner
    {
        public BannerIdEnum Id { get; set; }
        public string Rout { get; set; }
        public BannerTypeEnum Type { get; set; }
    }
    public enum BannerIdEnum
    {
        rout1 = 1,
        rout2 = 2,
        rout3 = 3
    }
    public enum BannerTypeEnum
    {
        Image = 1,
        Video = 2,
        Gif = 3
    }
}
