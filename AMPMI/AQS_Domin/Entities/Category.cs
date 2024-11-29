namespace Domin.Entities;
public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; }

    public Guid? PictureFileName { get; set; }

    public virtual ICollection<SubCategory>? SubCategories { get; set; } = new List<SubCategory>();
}
