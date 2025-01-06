using AQS_Application.Dtos.BaseServiceDto.Company;
using AQS_Application.Dtos.IdentityServiceDto;
using AQS_Application.Interfaces.IInfrastructure.IContext;
using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Common.Enums;
using Domin.Entities;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AQS_Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IDbAmpmiContext _context;
        public CompanyService(IDbAmpmiContext context)
        {
            _context = context;
        }

        public async Task<long> Create(RegisterIdentityDTO company , long id)
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

        public async Task<Company?> ReadById(long id)
        {
            return await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<bool> IsExistByMobileNumber(string mobile)
        {
            return await _context.Companies.AnyAsync(c => c.MobileNumber == mobile);
        }

        public async Task<ResultOutPutMethodEnum> UpdateTeaser(Company company)
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

            if ( existingCompany.ManagerName != company.ManagerName)
                existingCompany.ManagerName = company.ManagerName;

            if ( existingCompany.MobileNumber != company.MobileNumber)
                existingCompany.MobileNumber = company.MobileNumber;

            if ( existingCompany.Email != company.Email)
                existingCompany.Email = company.Email;

            if (existingCompany.Address != company.Address)
                existingCompany.Address = company.Address;

            if ( existingCompany.Brands != company.Brands)
                existingCompany.Brands = company.Brands;

            if (company.Capacity != existingCompany.Capacity)
                existingCompany.Capacity = company.Capacity;

            if (existingCompany.Partnership != company.Partnership)
                existingCompany.Partnership = company.Partnership;

            if ( existingCompany.QualityGrade != company.QualityGrade)
                existingCompany.QualityGrade = company.QualityGrade;

            if (existingCompany.Iso != company.Iso)
                existingCompany.Iso = company.Iso;

            if ( existingCompany.About != company.About)
                existingCompany.About = company.About;

            if (existingCompany.LogoRout != company.LogoRout)
                existingCompany.LogoRout = company.LogoRout;

            _context.Companies.Update(existingCompany);

            int result = await _context.SaveChangesAsync();

            return result > 0 ? ResultOutPutMethodEnum.savechanged : ResultOutPutMethodEnum.dontSaved;
        }
    }
}

