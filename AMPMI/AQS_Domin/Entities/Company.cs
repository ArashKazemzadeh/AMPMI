namespace Domin.Entities;
public partial class Company
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string ManagerName { get; set; } = null!;

    public string MobileNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Brands { get; set; }

    public int Capacity { get; set; }

    public string? Partnership { get; set; }

    public string? QualityGrade { get; set; }

    public string? Iso { get; set; }

    public string? About { get; set; }

    public Guid? LogoRout { get; set; }

    public bool IsCompany { get; set; }

    public string? TeaserGuid { get; set; }

    public virtual ICollection<CompanyPicture> CompanyPictures { get; set; } = new List<CompanyPicture>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<SeenNotifByCompany> SeenNotifByCompanies { get; set; } = new List<SeenNotifByCompany>();
}
