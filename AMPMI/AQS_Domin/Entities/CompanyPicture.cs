namespace Domin.Entities;
public partial class CompanyPicture
{
    public Guid Id { get; set; }

    public long? CompanyId { get; set; }

    public virtual Company? Company { get; set; }
}
