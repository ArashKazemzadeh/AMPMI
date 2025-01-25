namespace WebSite.EndPoint.Areas.Admin.Models.Company
{
    public class CompanyPictureVM
    {
        public long Id { get; set; }
        /// <summary>
        /// مسیر فایل
        /// </summary>
        public string PictureFileName { get; set; } = null!;

        public long? CompanyId { get; set; }
    }
}
