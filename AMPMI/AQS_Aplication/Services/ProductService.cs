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
            var row = await _context.Products.AddAsync(product);

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
            var result = await _context.Products.Include(x => x.Company).Include(x => x.SubCategory).
                ThenInclude(x => x.Category).AsNoTracking().ToListAsync();
            return result == null ? new List<Product>() : result;
        }

        public async Task<Product?> ReadById(long id)
        {
            return await _context.Products
                .Include(x => x.Company)
                .Include(m=>m.ProductPictures)
                .FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<List<Product>> SearchProductByName(string name,bool isConfirmed)
        {
            name = $"%{name}%";
            return await _context.Products
                .Where(p => EF.Functions.Like(p.Name, name) && p.IsConfirmed == isConfirmed)
                .Include(x=>x.ProductPictures)
                .ToListAsync();
        }
        public async Task<List<Product>> SearchProductByNameAndCategory(string name, int categoryId,bool isConfirmed)
        {
            name = $"%{name}%";
            return await _context.Products
                .Include(x => x.SubCategory)
                .Include(o=>o.ProductPictures)
                .Where(m => m.SubCategory.CategoryId == categoryId && m.IsConfirmed == isConfirmed)
                .Where(p => EF.Functions.Like(p.Name, name))
                .ToListAsync();
        }
        public async Task<List<Product>> SearchByProductNameAndCompanyId(string name, long companyId, bool isConfirmed)
        {
            name = $"%{name}%";
            return await _context.Products
                .Where(p => EF.Functions.Like(p.Name, name) && p.IsConfirmed == isConfirmed && p.CompanyId == companyId)
                .Include(x => x.ProductPictures)
                .ToListAsync();
        }
        public async Task<Product?> ReadByIdIncludeCategoryAndSubCategoryAndCompany(long id, bool isConfirmed)
        {
            return await _context.Products
                .Include(x => x.Company)
                .Include(x => x.SubCategory)
                .ThenInclude(m => m.Category)
                .Include(l => l.ProductPictures)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsConfirmed==isConfirmed);
        }
        public async Task<List<Product>> ReadByCategoryId(int categoryId,bool isConfirmed)
        {
            return await _context.Products
                .Include(x => x.SubCategory)
                .Include(m=>m.ProductPictures)
                .Where(l => l.SubCategory.CategoryId == categoryId && l.IsConfirmed == isConfirmed)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<List<Product>> ReadByCompanyId(long id)
        {
            return await _context.Products
                .Where(x => x.CompanyId == id)
                .Include(x => x.SubCategory)
                .ThenInclude(x => x.Category)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<List<Product>> ReadNotConfirmed()
        {
            return await _context.Products
                .Where(x => x.IsConfirmed == false)
                .Include(x=>x.Company)
                .Include(x => x.SubCategory)
                .ThenInclude(x => x.Category)
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

            int result = await _context.SaveChangesAsync();

            return result > 0 ? ResultOutPutMethodEnum.savechanged : ResultOutPutMethodEnum.dontSaved;
        }
        public async Task<ResultOutPutMethodEnum> UpdatePictureRout(long ProductId, string rout)
        {
            ProductPicture productPicture = new()
            {
                ProductId = ProductId,
                Rout = rout
            };
            await _context.ProductPictures.AddAsync(productPicture);

            int result = await _context.SaveChangesAsync();
            return result > 0 ?
               ResultOutPutMethodEnum.savechanged :
               ResultOutPutMethodEnum.dontSaved;
        }
        public async Task<List<ProductPicture>> ReadPictureRouts(long productId)
        {
            return await _context.ProductPictures
                .Where(x => x.ProductId == productId)
                .ToListAsync();
        }
        public async Task<ResultOutPutMethodEnum> DeleteProductPicture(long pictureId)
        {
            var picture = await _context.ProductPictures.FindAsync(pictureId);
            if (picture != null)
            {
                _context.ProductPictures.Remove(picture);
                return await _context.SaveChangesAsync() > 0 ?
                    ResultOutPutMethodEnum.savechanged : ResultOutPutMethodEnum.dontSaved;
            }
            return ResultOutPutMethodEnum.recordNotFounded;
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

