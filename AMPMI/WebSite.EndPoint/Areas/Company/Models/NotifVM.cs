namespace WebSite.EndPoint.Areas.Company.Models
{
    public class NotifVM
    {
        public long Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; }
        public bool IsSeen { get; set; }

    }
}
