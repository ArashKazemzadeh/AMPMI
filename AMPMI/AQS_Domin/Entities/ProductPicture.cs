namespace Domin.Entities;

public class ProductPicture
{
    public long Id { get; set; }
    public string Rout { get; set; } = null!;
    public long ProductId { get; set; }
    public virtual Product Product { get; set; }
}   