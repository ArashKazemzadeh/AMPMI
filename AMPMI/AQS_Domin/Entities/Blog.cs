namespace Domin.Entities;
public partial class Blog
{
    public int Id { get; set; }
    public string Subject { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime? CreateUpdateAt { get; set; }
    public string? VideoFileName { get; set; }
    public string? HeaderPictureFileName { get; set; }
    public virtual ICollection<BlogPicture>? BlogPictures { get; set; } = new List<BlogPicture>();
}
