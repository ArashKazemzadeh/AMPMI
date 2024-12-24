﻿using AQS_Aplication.Dtos.BaseServiceDto.CategoryDto;
using AQS_Aplication.Interfaces.IInfrastructure.IContext;
using AQS_Aplication.Interfaces.IServisces.BaseServices;
using AQS_Common.Enums;
using AQS_Domin.Entities.business;
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

        public async Task<int> Create(string name, string img)
        {
            var category = new Category() { Name = name, PictureFileName = img };
            var row = _context.Categories.Add(category);
            int result = await _context.SaveChangesAsync();
            return result > 0 ? row.Entity.Id : -1;
        }

      
        public async Task<ResultOutPutMethodEnum> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                return await _context.SaveChangesAsync() > 0 ?
                    ResultOutPutMethodEnum.savechanged : ResultOutPutMethodEnum.dontSaved;
            }
            return ResultOutPutMethodEnum.recordNotFounded;
        }


        public async Task<List<CategoryReadDto>> ReadAll()
        {
            var categories = await _context.Categories.Select(c => new CategoryReadDto
            {
                Id = c.Id,
                Name = c.Name,
                PictureFileName = c.PictureFileName
            }).ToListAsync();
            return categories ?? new List<CategoryReadDto>();
        }

        public async Task<List<Category>> ReadAllWithSub()
        {
            var categories = await _context.Categories.Include(c => c.SubCategories).ToListAsync();
            return categories ?? new List<Category>();
        }
        public async Task<Category?> ReadById(int id)
        {
            return await _context.Categories.Include(c => c.SubCategories)
                                            .FirstOrDefaultAsync(c => c.Id == id);
        }

     
        public async Task<ResultOutPutMethodEnum> Update(int id, string name)
        {
            var existingCategory = await _context.Categories.FindAsync(id);

            if (existingCategory == null)
                return ResultOutPutMethodEnum.recordNotFounded;

                existingCategory.Name = name;

            int result = await _context.SaveChangesAsync();
            return result > 0 ? ResultOutPutMethodEnum.savechanged : ResultOutPutMethodEnum.dontSaved;
        }

        public async Task<ResultOutPutMethodEnum> UpdatePicture(int id, string pictureFileName)
        {
            var existingCategory = await _context.Categories.FindAsync(id);
            if (existingCategory == null)
                return ResultOutPutMethodEnum.recordNotFounded;

            existingCategory.PictureFileName = pictureFileName;

            int result = await _context.SaveChangesAsync();
            return result > 0 ? ResultOutPutMethodEnum.savechanged : ResultOutPutMethodEnum.dontSaved;
        }
    }
}
