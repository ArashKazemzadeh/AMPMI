namespace Domin.Entities;
public partial class BlogPicture
{
    public int Id { get; set; }
    /// <summary>
    /// نام فایل
    /// </summary>
    public string Route { get; set; }

    public int? BlogId { get; set; }

    public virtual Blog? Blog { get; set; }
}
