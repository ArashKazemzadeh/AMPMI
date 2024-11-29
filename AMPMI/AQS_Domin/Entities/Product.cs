namespace Domin.Entities;
public partial class Product
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public Guid? PictureFileName { get; set; }

    public bool IsConfirmed { get; set; }

    public long? CompanyId { get; set; }

    public int SubCategoryId { get; set; }

    public virtual Company? Company { get; set; }

    public virtual SubCategory SubCategory { get; set; }
}
