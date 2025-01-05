using AQS_Application.Interfaces.IInfrastructure.IContext;
using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Common.Enums;
using Domin.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AQS_Application.Services
{
    public class CompanyPictureService : ICompanyPictureService
    {
        private readonly IDbAmpmiContext _context;
        public CompanyPictureService(IDbAmpmiContext context)
        {
            this._context = context;
        }
        public async Task<long> Create(long companyId,string pictureFileName)
        {
            CompanyPicture companyPicture = new CompanyPicture()
            {
                CompanyId = companyId,
                PictureFileName = pictureFileName
            };

            var row = _context.CompanyPictures.Add(companyPicture);
            int result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return row.Entity.Id;
            }
            return -1;
        }
        public async Task<ResultOutPutMethodEnum> Delete(long id)
        {
            var row = await _context.CompanyPictures.FindAsync(id);
            if (row != null)
            {
                _context.CompanyPictures.Remove(row);
                return await _context.SaveChangesAsync()>0 ? 
                    ResultOutPutMethodEnum.savechanged:
                    ResultOutPutMethodEnum.dontSaved;
            }
            else
                return ResultOutPutMethodEnum.recordNotFounded;
        }
        public async Task<List<CompanyPicture>> ReadAll()
        {
            var result = await _context.CompanyPictures.ToListAsync();
            return result == null ? new List<CompanyPicture>() : result;
        }
        public async Task<List<CompanyPicture>> ReadAllByCompany(long companyId)
        {
            var result = await _context.CompanyPictures
                .Where(x => x.CompanyId == companyId).ToListAsync();
            return result == null ? new List<CompanyPicture>() : result;
        }
        public async Task<CompanyPicture?> ReadById(long id)
        {
            return await _context.CompanyPictures.FirstOrDefaultAsync(x=>x.Id==id);
        }
        public async Task<ResultOutPutMethodEnum> Update(long id,long companyId,string pictureFileName)
        {
            var row = await _context.CompanyPictures.FirstOrDefaultAsync(x => x.Id == id);
            if (row == null)
                return ResultOutPutMethodEnum.recordNotFounded;
            row.CompanyId = companyId;
            row.PictureFileName = pictureFileName;
            
            int result = await  _context.SaveChangesAsync();
            return result > 0 ? ResultOutPutMethodEnum.savechanged 
                : ResultOutPutMethodEnum.dontSaved;
        }
    }
}
