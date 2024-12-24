namespace AQS_Domin.Entities.business;
public partial class CompanyPicture
{
    /// <summary>
    /// شناسه و نام فایل 
    /// </summary>
    public Guid Id { get; set; }

    public long? CompanyId { get; set; }

    public virtual Company? Company { get; set; }
}
