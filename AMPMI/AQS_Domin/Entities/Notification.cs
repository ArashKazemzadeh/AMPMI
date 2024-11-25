namespace Domin.Entities;
public partial class Notification
{
    public long Id { get; set; }

    public string Subject { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime? CreateAt { get; set; }

    public virtual ICollection<SeenNotifByCompany> SeenNotifByCompanies { get; set; } = new List<SeenNotifByCompany>();
}
