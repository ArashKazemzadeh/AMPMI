using AQS_Aplication.Interfaces.Context;
using AQS_Aplication.Interfaces.IServisces;
using AQS_Common.Enums;
using Domin.Entities;
using Microsoft.EntityFrameworkCore;

namespace AQS_Aplication.Services
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

        public async Task<ResultServiceMethods> Delete(int id)
        {
            var subCategory = await _context.SubCategories.FindAsync(id);

            if (subCategory != null)
            {
                _context.SubCategories.Remove(subCategory);
                return await _context.SaveChangesAsync() > 0
                    ? ResultServiceMethods.savechanged
                    : ResultServiceMethods.dontSaved;
            }

            return ResultServiceMethods.recordNotFounded;
        }

        public async Task<List<SubCategory>> ReadAll()
        {
            var result = await _context.SubCategories
                .Include(sc => sc.Category) 
                .Include(sc => sc.Products) 
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

        public async Task<ResultServiceMethods> Update(SubCategory subCategory)
        {
            var existingSubCategory = await _context.SubCategories.FindAsync(subCategory.Id);

            if (existingSubCategory == null)
            {
                return ResultServiceMethods.recordNotFounded;
            }

            if (subCategory.Name != null && existingSubCategory.Name != subCategory.Name)
            {
                existingSubCategory.Name = subCategory.Name;
            }

            int result = await _context.SaveChangesAsync();

            return result > 0 
                ? ResultServiceMethods.savechanged 
                : ResultServiceMethods.dontSaved;
        }
    }
}
