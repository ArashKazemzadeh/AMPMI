using Domin.Entities;

namespace AQS_Aplication.Interfaces.IServisces
{
    public interface ISeenNotifByCompanyService
    {
        Task<bool> Create(SeenNotifByCompany seenNotifByCompany);
        Task<SeenNotifByCompany> ReadById(int id);
        Task<List<SeenNotifByCompany>> Read();
        Task<SeenNotifByCompany> Update(SeenNotifByCompany seenNotifByCompany);
        Task Delete(int id);
    }
}
