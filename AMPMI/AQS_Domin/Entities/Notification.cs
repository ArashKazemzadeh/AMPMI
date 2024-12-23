namespace Domin.Entities;
public partial class Notification
{
    public long Id { get; set; }

    public required string Subject { get; set; }

    public required string Description { get; set; }

    public required DateTime CreateAt { get; set; } = DateTime.Now;

    public virtual ICollection<SeenNotifByCompany> SeenNotifByCompanies { get; set; } = new List<SeenNotifByCompany>();
}
