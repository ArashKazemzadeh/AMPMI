using AQS_Application.Interfaces.IInfrastructure.IContext;
using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Common.Enums;
using Domin.Entities;
using Microsoft.EntityFrameworkCore;

namespace AQS_Application.Services
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly IDbAmpmiContext _context;

        public SubCategoryService(IDbAmpmiContext context)
        {
            _context = context;
        }

        public async Task<int> Create(SubCategory subCategory)
        {
            var row = _context.SubCategories.Add(subCategory);

            int result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return row.Entity.Id;
            }

            return -1;
        }

        public async Task<ResultOutPutMethodEnum> Delete(int id)
        {
            var subCategory = await _context.SubCategories.FindAsync(id);

            if (subCategory != null)
            {
                _context.SubCategories.Remove(subCategory);
                return await _context.SaveChangesAsync() > 0
                    ? ResultOutPutMethodEnum.savechanged
                    : ResultOutPutMethodEnum.dontSaved;
            }

            return ResultOutPutMethodEnum.recordNotFounded;
        }

        public async Task<List<SubCategory>> ReadAll()
        {
            var result = await _context.SubCategories
                .Include(sc => sc.Category)
                .ToListAsync();

            return result ?? new List<SubCategory>();
        }

        public async Task<SubCategory?> ReadById(int id)
        {
            return await _context.SubCategories
                .Include(sc => sc.Category)
                .Include(sc => sc.Products)
                .FirstOrDefaultAsync(sc => sc.Id == id);
        }

        public async Task<ResultOutPutMethodEnum> Update(SubCategory subCategory)
        {
            var existingSubCategory = await _context.SubCategories.FindAsync(subCategory.Id);

            if (existingSubCategory == null)
            {
                return ResultOutPutMethodEnum.recordNotFounded;
            }
            existingSubCategory.Name = subCategory.Name;
            existingSubCategory.CategoryId = subCategory.CategoryId;

            int result = await _context.SaveChangesAsync();

            return result > 0
                ? ResultOutPutMethodEnum.savechanged
                : ResultOutPutMethodEnum.dontSaved;
        }
        public async Task<bool> IsSubCategoryHaveProduct(int id)
        {
            var subcategory = await _context.SubCategories
                .Where(x => x.Id == id)
                .Include(x => x.Products)
                .FirstOrDefaultAsync();

            if(subcategory != null && subcategory.Products != null && subcategory.Products.Count > 0)
                return true;
            else
                return false;

        }
    }
}
