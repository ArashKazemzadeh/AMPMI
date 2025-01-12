namespace Domin.Entities;
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? PictureFileName { get; set; }
    public virtual ICollection<SubCategory>? SubCategories { get; set; } = new List<SubCategory>();
}
