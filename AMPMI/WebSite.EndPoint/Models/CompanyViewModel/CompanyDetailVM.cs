namespace WebSite.EndPoint.Models.CompanyViewModel
{
    public class CompanyDetailVM
    {
        public string Name { get; set; } = null!;
        public string MobileNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public Guid? LogoRout { get; set; }
        public string? TeaserGuid { get; set; }

        // TODO : Where is main photo for company

    }
}
