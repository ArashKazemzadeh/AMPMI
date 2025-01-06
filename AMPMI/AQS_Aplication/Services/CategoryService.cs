using AQS_Aplication.Dtos.BaseServiceDto.CategoryDto;
using AQS_Application.Dtos.BaseServiceDto.CategoryDtos;
using AQS_Application.Dtos.BaseServiceDto.SubCategoryDto;
using AQS_Application.Interfaces.IInfrastructure.IContext;
using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Common.Enums;
using Domin.Entities;
using Microsoft.EntityFrameworkCore;

namespace AQS_Application.Services
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

        public async Task<List<CategoryIncludeSubCategoriesDto>> ReadAlIncludeSub()
        {
            var categories = await _context.Categories.Include(c => c.SubCategories).ToListAsync();
            return categories.Select(c => new CategoryIncludeSubCategoriesDto
            {
                Id = c.Id,
                Name = c.Name,
                PictureFileName = c.PictureFileName,
                SubCategories = c.SubCategories != null ? c.SubCategories.Select(sc => new SubCategoryReadDto
                {
                    CategoryId = sc.CategoryId,
                    Id = sc.Id,
                    Name = sc.Name
                }).ToList() : new List<SubCategoryReadDto>()
            }).ToList();
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



        public async Task<GetPictureeResultDto> GetPictureRout(int id)
        {
            if (id <= 0)
            {
                return new GetPictureeResultDto
                {
                    Path = "",
                    Message = "شناسه نامعتبر است."
                };
            }

            var picturePath = await _context.Categories
                .Where(c => c.Id == id)
                .Select(c => c.PictureFileName)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(picturePath))
            {
                return new GetPictureeResultDto
                {
                    Path = "",
                    Message = $"هیچ تصویری برای شناسه {id} یافت نشد."
                };
            }

            return new GetPictureeResultDto
            {
                Path = picturePath,
                Message = "تصویر با موفقیت یافت شد."
            };
        }

        public async Task<bool> IsCategoryHaveChildren(int id)
        {
            var category = await _context.Categories
                .Where(x => x.Id == id)
                .Include(x=>x.SubCategories)
                .FirstOrDefaultAsync();

            if (category != null && category.SubCategories != null && category.SubCategories.Count > 0)
                return true;
            else
                return false;
        }
    }
}
