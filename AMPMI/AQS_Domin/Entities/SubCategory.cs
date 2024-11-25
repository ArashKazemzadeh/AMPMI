namespace Domin.Entities;
public partial class SubCategory
{
    public int Id { get; set; }

    public int? CategoryId { get; set; }

    public string? Name { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
