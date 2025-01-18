namespace WebSite.EndPoint.Models.CompanyViewModel
{
    public class CompanyVM
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string MobileNumber { get; set; } = null!;
        public string ManagerName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; } = null!;
        public string? LogoRout { get; set; }
        public string? TeaserGuid { get; set; }
        public string? Tel { get; set; }

        // TODO : Where is main photo for company

    }
}
