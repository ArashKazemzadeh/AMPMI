using Domin.Entities;

namespace AQS_Application.Interfaces.IServices.BaseServices
{
    public interface ISiteAdminService
    {
        Task<bool> Create(SiteAdmin siteAdmin);
        Task<SiteAdmin> ReadById(int id);
        Task<List<SiteAdmin>> Read();
        Task<SiteAdmin> Update(SiteAdmin siteAdmin);
        Task Delete(int id);
    }
}
