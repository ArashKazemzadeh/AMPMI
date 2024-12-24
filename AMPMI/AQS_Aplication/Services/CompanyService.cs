using AQS_Aplication.Dtos.IdentityServiceDto;
using AQS_Aplication.Interfaces.IInfrastructure.IContext;
using AQS_Aplication.Interfaces.IServisces.BaseServices;
using AQS_Common.Enums;
using Domin.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AQS_Aplication.Services
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

        public async Task<ResultOutPutMethodEnum> Update(Company company)
        {
            var existingCompany = await _context.Companies.FindAsync(company.Id);

            if (existingCompany == null)
                return ResultOutPutMethodEnum.recordNotFounded;

            if (company.Name != null && existingCompany.Name != company.Name)
                existingCompany.Name = company.Name;

            if (company.ManagerName != null && existingCompany.ManagerName != company.ManagerName)
                existingCompany.ManagerName = company.ManagerName;

            if (company.MobileNumber != null && existingCompany.MobileNumber != company.MobileNumber)
                existingCompany.MobileNumber = company.MobileNumber;

            if (company.Email != null && existingCompany.Email != company.Email)
                existingCompany.Email = company.Email;

            if (company.Address != null && existingCompany.Address != company.Address)
                existingCompany.Address = company.Address;

            if (company.Brands != null && existingCompany.Brands != company.Brands)
                existingCompany.Brands = company.Brands;

            if (company.Capacity != existingCompany.Capacity)
                existingCompany.Capacity = company.Capacity;

            if (company.Partnership != null && existingCompany.Partnership != company.Partnership)
                existingCompany.Partnership = company.Partnership;

            if (company.QualityGrade != null && existingCompany.QualityGrade != company.QualityGrade)
                existingCompany.QualityGrade = company.QualityGrade;

            if (company.Iso != null && existingCompany.Iso != company.Iso)
                existingCompany.Iso = company.Iso;

            if (company.About != null && existingCompany.About != company.About)
                existingCompany.About = company.About;

            //ToDo : Create Method
            if (company.TeaserGuid != null && existingCompany.TeaserGuid != company.TeaserGuid)
                existingCompany.TeaserGuid = company.TeaserGuid;// 

            int result = await _context.SaveChangesAsync();

            return result > 0 ? ResultOutPutMethodEnum.savechanged : ResultOutPutMethodEnum.dontSaved;
        }

        public async Task<ResultOutPutMethodEnum> UpdateLogoRout(int id, Guid logoRout)
        {
            var existingCompany = await _context.Companies.FindAsync(id);

            if (existingCompany == null)
                return ResultOutPutMethodEnum.recordNotFounded;

            existingCompany.LogoRout = logoRout;
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
    }
}

