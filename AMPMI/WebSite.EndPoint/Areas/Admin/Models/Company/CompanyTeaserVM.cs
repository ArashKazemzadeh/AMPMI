namespace WebSite.EndPoint.Areas.Admin.Models.Company
{
    public class CompanyTeaserVM
    {
        public long CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string? TeaserRoute { get; set; }
        public IFormFile Teaser { get; set; }
    }
}
