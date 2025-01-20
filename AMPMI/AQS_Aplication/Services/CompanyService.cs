using AQS_Application.Dtos.BaseServiceDto.Company;
using AQS_Application.Dtos.IdentityServiceDto;
using AQS_Application.Interfaces.IInfrastructure.IContext;
using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Common.Enums;
using Domin.Entities;
using Microsoft.EntityFrameworkCore;

namespace AQS_Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IDbAmpmiContext _context;
        public CompanyService(IDbAmpmiContext context)
        {
            _context = context;
        }

        public async Task<long> Create(RegisterIdentityDTO company, long id)
        {
            var row = _context.Companies
                .Add(new Company
                {
                    Id = id,
                    MobileNumber = company.Mobile,
                    Name = company.CompanyName,
                    ManagerName = company.ManagerName,
                    Email = company.Email,
                    Password = company.Password,
                    Address = company.Address
                });

            int result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return row.Entity.Id;
            }
            return -1;
        }

        public async Task<ResultOutPutMethodEnum> Delete(long id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company != null)
            {
                _context.Companies.Remove(company);
                return await _context.SaveChangesAsync() > 0 ?
                    ResultOutPutMethodEnum.savechanged : ResultOutPutMethodEnum.dontSaved;
            }
            return ResultOutPutMethodEnum.recordNotFounded;
        }

        public async Task<List<Company>> Read()
        {
            var result = await _context.Companies.ToListAsync();
            return result ?? new List<Company>();
        }
        public async Task<List<Company>> ReadConfirmedComapanies()
        {
            var result = await _context.Companies.Where(x=>x.IsCompany).ToListAsync();
            return result ?? new List<Company>();
        }

        public async Task<CompanyEditProfileDto?> ReadByIdAsync(long id)
        {
            return await _context.Companies
                .Where(c => c.Id == id)
                .Select(c => new CompanyEditProfileDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ManagerName = c.ManagerName,
                    MobileNumber = c.MobileNumber,
                    Email = c.Email,
                    Address = c.Address,
                    Brands = c.Brands,
                    Capacity = c.Capacity,
                    Partnership = c.Partnership,
                    QualityGrade = c.QualityGrade,
                    Iso = c.Iso,
                    About = c.About,
                    TeaserGuid = c.TeaserGuid,
                    LogoRout = c.LogoRout ?? string.Empty,
                    BannerRout = c.BannerRout ?? string.Empty,
                    SendRequest = c.SendRequst,
                    IsCompany = c.IsCompany,
                    Tel = c.Tel,
                    Website = c.Website
                })
                .FirstOrDefaultAsync();
        }
        public async Task<CompanyEditProfileDto?> ReadByIdIncludePicturesAndProducts(long id)
        {
            return await _context.Companies
                .Where(c => c.Id == id)
                .Include(x=>x.CompanyPictures)
                .Include(y=>y.Products)
                .ThenInclude(z=>z.ProductPictures)
                .Select(c => new CompanyEditProfileDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ManagerName = c.ManagerName,
                    MobileNumber = c.MobileNumber,
                    Email = c.Email,
                    Address = c.Address,
                    Brands = c.Brands,
                    Capacity = c.Capacity,
                    Partnership = c.Partnership,
                    QualityGrade = c.QualityGrade,
                    Iso = c.Iso,
                    About = c.About,
                    LogoRout = c.LogoRout ?? string.Empty,
                    BannerRout = c.BannerRout ?? string.Empty,
                    SendRequest = c.SendRequst,
                    TeaserGuid = c.TeaserGuid,
                    CompanyPictures = c.CompanyPictures,
                    Products = c.Products,
                    Tel = c.Tel,
                    Website = c.Website
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistByMobileNumber(string mobile)
        {
            return await _context.Companies.AnyAsync(c => c.MobileNumber == mobile);
        }

        public async Task<ResultOutPutMethodEnum> UpdateTeaser(CompanyEditProfileDto company)
        {
            var existingCompany = await _context.Companies.FindAsync(company.Id);

            if (existingCompany == null)
                return ResultOutPutMethodEnum.recordNotFounded;

            if (company.TeaserGuid != null && existingCompany.TeaserGuid != company.TeaserGuid)
                existingCompany.TeaserGuid = company.TeaserGuid;// 

            _context.Companies.Update(existingCompany);

            int result = await _context.SaveChangesAsync();

            return result > 0 ? ResultOutPutMethodEnum.savechanged : ResultOutPutMethodEnum.dontSaved;
        }

        public async Task<ResultOutPutMethodEnum> UpdateLogoRout(int id, string logoRout)
        {
            var existingCompany = await _context.Companies.FindAsync(id);

            if (existingCompany == null)
                return ResultOutPutMethodEnum.recordNotFounded;

            existingCompany.LogoRout = logoRout;
            _context.Companies.Update(existingCompany);
            int result = await _context.SaveChangesAsync();

            return result > 0 ?
                ResultOutPutMethodEnum.savechanged : ResultOutPutMethodEnum.dontSaved;
        }

        public async Task<ResultOutPutMethodEnum> IsCompany(long id, bool isCompany)
        {
            var existingCompany = await _context.Companies.FindAsync(id);

            if (existingCompany == null)
                return ResultOutPutMethodEnum.recordNotFounded;

            existingCompany.IsCompany = isCompany;

            return await _context.SaveChangesAsync() > 0 ?
                ResultOutPutMethodEnum.savechanged : ResultOutPutMethodEnum.dontSaved;
        }
        public async Task<ResultOutPutMethodEnum> SendRequest(long id, bool sendRequest)
        {
            var existingCompany = await _context.Companies.FindAsync(id);

            if (existingCompany == null)
                return ResultOutPutMethodEnum.recordNotFounded;

            if (existingCompany.SendRequst)
                return ResultOutPutMethodEnum.duplicateRecord;

            existingCompany.SendRequst = sendRequest;

            return await _context.SaveChangesAsync() > 0 ?
                ResultOutPutMethodEnum.savechanged : ResultOutPutMethodEnum.dontSaved;
        }
        public async Task<bool> IsExistById(long id)
        {
            return await _context.Companies.AnyAsync(c => c.Id == id);
        }
        public async Task<Company?> ReadByMobileNumber(string mobile)
        {
            return await _context.Companies.FirstOrDefaultAsync(c => c.MobileNumber == mobile);
        }

        public async Task<ResultOutPutMethodEnum> UpdateEditProfile(CompanyEditProfileDto company)
        {
            var existingCompany = await _context.Companies.FindAsync(company.Id);

            if (existingCompany == null)
                return ResultOutPutMethodEnum.recordNotFounded;

            if (existingCompany.Name != company.Name)
                existingCompany.Name = company.Name;

            if (existingCompany.ManagerName != company.ManagerName)
                existingCompany.ManagerName = company.ManagerName;

            if (existingCompany.MobileNumber != company.MobileNumber)
                existingCompany.MobileNumber = company.MobileNumber;

            if (existingCompany.Email != company.Email)
                existingCompany.Email = company.Email;

            if (existingCompany.Address != company.Address)
                existingCompany.Address = company.Address;

            if (existingCompany.Brands != company.Brands)
                existingCompany.Brands = company.Brands;

            if (company.Capacity != existingCompany.Capacity)
                existingCompany.Capacity = company.Capacity;

            if (existingCompany.Partnership != company.Partnership)
                existingCompany.Partnership = company.Partnership;

            if (existingCompany.QualityGrade != company.QualityGrade)
                existingCompany.QualityGrade = company.QualityGrade;

            if (existingCompany.Iso != company.Iso)
                existingCompany.Iso = company.Iso;

            if (existingCompany.About != company.About)
                existingCompany.About = company.About;

            if (existingCompany.LogoRout != company.LogoRout)
                existingCompany.LogoRout = company.LogoRout;
            
            if(existingCompany.BannerRout != company.BannerRout)
                existingCompany.BannerRout = company.BannerRout;

            if(existingCompany.Tel != company.Tel)
                existingCompany.Tel = company.Tel;

            if(existingCompany.Website != company.Website)
                existingCompany.Website = company.Website;

            _context.Companies.Update(existingCompany);

            int result = await _context.SaveChangesAsync();

            return result > 0 ? ResultOutPutMethodEnum.savechanged : ResultOutPutMethodEnum.dontSaved;
        }


    }
}

