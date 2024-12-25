namespace WebSite.EndPoint.Areas.Company.Models
{
    public class NotifVM
    {
        public long Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; }
        public bool IsSeen { get; set; }

        internal static List<NotifVM> Seed()
        {
            return new List<NotifVM>
            {
                new NotifVM
                {   Id = 1,
                    Subject = "گروه مهم",
                    Description = "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است.\r\n\r\n",
                    CreateAt = DateTime.Now.AddDays(-10).ToPersianDate(),
                    IsSeen = true
                },
                new NotifVM
                {   Id = 1,
                    Subject = "گروه مهم",
                    Description = "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است.\r\n\r\n",
                    CreateAt = DateTime.Now.AddDays(-12).ToPersianDate(),
                    IsSeen = false
                },
                new NotifVM
                {   Id = 1,
                    Subject = "گروه مهم",
                    Description = "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است.\r\n\r\n",
                    CreateAt = DateTime.Now.AddDays(-5).ToPersianDate(),
                    IsSeen = true
                },
            };
        }
    }
}
