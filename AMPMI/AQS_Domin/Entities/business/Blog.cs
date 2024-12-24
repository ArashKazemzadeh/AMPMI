namespace AQS_Domin.Entities.business;
public partial class Blog
{
    public int Id { get; set; }

    public string Subject { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime? CreateUpdateAt { get; set; }

    public Guid? VideoFileName { get; set; }
    public Guid? HeaderPictureFileName { get; set; }

    public virtual ICollection<BlogPicture>? BlogPictures { get; set; } = new List<BlogPicture>();
}
