using AQS_Domin.Entities;

namespace AQS_Aplication.Interfaces.IServisces
{
    public interface IBannerService
    {
        Task<bool> Create(Banner banner);
        Task Delete(int id);
        Task<List<Banner>> Read();
        Task<Banner> ReadById(int id);
        Task<Banner> Update(Banner banner);
    }
    

}