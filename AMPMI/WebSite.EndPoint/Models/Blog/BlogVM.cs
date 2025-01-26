namespace WebSite.EndPoint.Models.Blog
{
    public class BlogVM
    {
        public int Id { get; set; }

        public string Subject { get; set; } = null!;

        public string Description { get; set; } = null!;

        public DateTime? CreateUpdateAt { get; set; }

        public string? HeaderPictureFileName { get; set; }
    }
}
