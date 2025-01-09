using AQS_Application.Interfaces.IInfrastructure.IContext;
using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Common.Enums;
using Domin.Entities;
using Microsoft.EntityFrameworkCore;

namespace AQS_Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IDbAmpmiContext _context;
        public ProductService(IDbAmpmiContext context)
        {
            _context = context;
        }

        public async Task<long> Create(Product product)
        {
            var row = _context.Products.Add(product);

            int result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return row.Entity.Id; 
            }
            return -1;
        }

        public async Task<ResultOutPutMethodEnum> Delete(long id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                return await _context.SaveChangesAsync() > 0 ?
                    ResultOutPutMethodEnum.savechanged : ResultOutPutMethodEnum.dontSaved;
            }
            return ResultOutPutMethodEnum.recordNotFounded;
        }

        public async Task<List<Product>> Read()
        {
            var result = await _context.Products.Include(x=>x.Company).Include(x=>x.SubCategory).
                ThenInclude(x=>x.Category).AsNoTracking().ToListAsync();
            return result == null ? new List<Product>() : result;
        }

        public async Task<Product?> ReadById(long id)
        {
            return await _context.Products.Include(x=>x.Company).FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<Product?> ReadByIdIncludeCategoryAndSubCategoryAndCompany(long id)
        {
            return await _context.Products
                .Include(x=>x.Company)
                .Include(x => x.SubCategory)
                .ThenInclude(m => m.Category)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<List<Product>> ReadByCategoryId(int categoryId)
        {
            return await _context.Products
                .Include(x => x.SubCategory)
                .Where(l => l.SubCategory.CategoryId == categoryId)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<List<Product>> ReadByCompanyId(long id)
        {
            return await _context.Products.Where(x => x.CompanyId == id).Include(x => x.SubCategory)
                .ThenInclude(x => x.Category).AsNoTracking().ToListAsync();
        }
        public async Task<List<Product>> ReadNotConfirmed()
        {
            return await _context.Products
                .Where(x => x.IsConfirmed == false)
                .Include(x=>x.SubCategory)
                .ThenInclude(x=>x.Category)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<ResultOutPutMethodEnum> Update(Product product)
        {
            var existingProduct = await _context.Products.FindAsync(product.Id);

            if (existingProduct == null)
                return ResultOutPutMethodEnum.recordNotFounded;

            if (product.Name != null && existingProduct.Name != product.Name)
                existingProduct.Name = product.Name;

            if (product.Description != null && existingProduct.Description != product.Description)
                existingProduct.Description = product.Description;

            if (product.PictureFileName != null && existingProduct.PictureFileName != product.PictureFileName)
                existingProduct.PictureFileName = product.PictureFileName;

            int result = await _context.SaveChangesAsync();

            return result > 0 ? ResultOutPutMethodEnum.savechanged : ResultOutPutMethodEnum.dontSaved;
        }
        public async Task<ResultOutPutMethodEnum> UpdatePictureFileName(int id, string pictureFileName)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
                return ResultOutPutMethodEnum.recordNotFounded;

            existingProduct.PictureFileName = pictureFileName;

            return await _context.SaveChangesAsync() > 1 ?
               ResultOutPutMethodEnum.savechanged : ResultOutPutMethodEnum.dontSaved;
        }
        public async Task<ResultOutPutMethodEnum> IsConfirmed(long id, bool isConfirmed)
        {
            var existingProduct = await _context.Products.FindAsync(id);

            if (existingProduct == null)
                return ResultOutPutMethodEnum.recordNotFounded;

            existingProduct.IsConfirmed = isConfirmed;

            return await _context.SaveChangesAsync() > 1 ?
                ResultOutPutMethodEnum.savechanged : ResultOutPutMethodEnum.dontSaved;
        }
    }

}

