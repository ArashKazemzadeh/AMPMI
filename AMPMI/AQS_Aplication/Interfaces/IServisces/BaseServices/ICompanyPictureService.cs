using Domin.Entities;

namespace AQS_Application.Interfaces.IServices.BaseServices
{
    public interface ICompanyPictureService
    {
        Task<bool> Create(CompanyPicture companyPicture);
        Task Delete(int id);
        Task<List<CompanyPicture>> Read();
        Task<CompanyPicture> ReadById(int id);
        Task<CompanyPicture> Update(CompanyPicture companyPicture);
    }


}