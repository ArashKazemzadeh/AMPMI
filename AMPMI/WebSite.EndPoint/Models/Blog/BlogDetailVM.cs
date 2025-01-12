namespace WebSite.EndPoint.Models.Blog
{
    public class BlogDetailVM
    {
        public int Id { get; set; }

        public string Subject { get; set; } = null!;

        public string Description { get; set; } = null!;

        public DateTime? CreateUpdateAt { get; set; }

        public string? VideoFileName { get; set; }
        public string? HeaderPictureFileName { get; set; }
        public List<string>? BlogPictures { get; set; }

    }
}
