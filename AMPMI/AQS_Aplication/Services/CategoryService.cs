using AQS_Aplication.Interfaces.IInfrastructure.IContext;
using AQS_Aplication.Interfaces.IServisces.BaseServices;
using AQS_Common.Enums;
using Domin.Entities;
using Microsoft.EntityFrameworkCore;

namespace AQS_Aplication.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IDbAmpmiContext _context;

        public CategoryService(IDbAmpmiContext context)
        {
            _context = context;
        }

        /// <summary>
        /// ایجاد دسته‌بندی جدید
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task<int> Create(Category category)
        {
            var row = _context.Categories.Add(category);
            int result = await _context.SaveChangesAsync();
            return result > 0 ? row.Entity.Id : -1;
        }

        /// <summary>
        /// حذف دسته‌بندی
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResultServiceMethods> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                return await _context.SaveChangesAsync() > 0 ?
                    ResultServiceMethods.savechanged : ResultServiceMethods.dontSaved;
            }
            return ResultServiceMethods.recordNotFounded;
        }

        /// <summary>
        /// بازیابی تمامی دسته‌بندی‌ها
        /// </summary>
        /// <returns></returns>
        public async Task<List<Category>> Read()
        {
            var categories = await _context.Categories.Include(c => c.SubCategories).ToListAsync();
            return categories ?? new List<Category>();
        }

        /// <summary>
        /// بازیابی یک دسته‌بندی بر اساس ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Category?> ReadById(int id)
        {
            return await _context.Categories.Include(c => c.SubCategories)
                                            .FirstOrDefaultAsync(c => c.Id == id);
        }

        /// <summary>
        /// به‌روزرسانی دسته‌بندی
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task<ResultServiceMethods> Update(Category category)
        {
            var existingCategory = await _context.Categories.FindAsync(category.Id);
            if (existingCategory == null)
                return ResultServiceMethods.recordNotFounded;
            if (category.Name != null && existingCategory.Name != category.Name)
                existingCategory.Name = category.Name;
            int result = await _context.SaveChangesAsync();
            return result > 0 ? ResultServiceMethods.savechanged : ResultServiceMethods.dontSaved;
        }

        /// <summary>
        /// به‌روزرسانی عکس دسته‌بندی
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pictureFileName"></param>
        /// <returns></returns>
        public async Task<ResultServiceMethods> UpdatePicture(int id, Guid pictureFileName)
        {
            var existingCategory = await _context.Categories.FindAsync(id);
            if (existingCategory == null)
                return ResultServiceMethods.recordNotFounded;

            existingCategory.PictureFileName = pictureFileName;

            int result = await _context.SaveChangesAsync();
            return result > 0 ? ResultServiceMethods.savechanged : ResultServiceMethods.dontSaved;
        }
    }
}
