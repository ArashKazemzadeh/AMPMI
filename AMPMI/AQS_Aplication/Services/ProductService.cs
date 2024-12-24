using AQS_Aplication.Interfaces.IInfrastructure.IContext;
using AQS_Aplication.Interfaces.IServisces.BaseServices;
using AQS_Common.Enums;
using AQS_Domin.Entities.business;
using Microsoft.EntityFrameworkCore;

namespace AQS_Aplication.Services
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
            var result = await _context.Products.ToListAsync();
            return result == null ? new List<Product>() : result;
        }

        public async Task<Product?> ReadById(long id)
        {
            return await _context.Products.Include(x=>x.Company).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
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
        public async Task<ResultOutPutMethodEnum> UpdatePictureFileName(int id, Guid pictureFileName)
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

