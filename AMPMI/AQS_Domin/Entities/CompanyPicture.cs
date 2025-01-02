namespace Domin.Entities;
public partial class CompanyPicture
{
    /// <summary>
    /// شناسه  
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// مسیر فایل
    /// </summary>
    public required string PictureFileName { get; set; }

    public long? CompanyId { get; set; }

    public virtual Company? Company { get; set; }
}
