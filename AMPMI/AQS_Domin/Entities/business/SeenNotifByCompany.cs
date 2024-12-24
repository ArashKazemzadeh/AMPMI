namespace AQS_Domin.Entities.business;
public partial class SeenNotifByCompany
{
    public long Id { get; set; }

    public long? CompanyId { get; set; }

    public long? NotificationId { get; set; }

    public virtual Company? Company { get; set; }

    public virtual Notification? Notification { get; set; }
}
