using AQS_Aplication.Dtos.BaseServiceDto.CategoryDto;
using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Common.Enums;
using Domin.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Areas.Admin.Models.SubCategory;
using WebSite.EndPoint.Utility;

namespace WebSite.EndPoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SubCategoryController : Controller
    {
        private readonly ISubCategoryService _subCategoryService;
        private readonly ICategoryService _categoryService;
        private readonly IFileServices _fileServices;

        const string PictureFolder = "SubCategory";
        static List<CategoryReadDto> _Category = new List<CategoryReadDto>();

        public SubCategoryController(ISubCategoryService productService, ICategoryService categoryService, IFileServices fileServices)
        {
            _subCategoryService = productService;
            _categoryService = categoryService;
            _fileServices = fileServices;
        }
        public static List<CategoryReadDto> GetCategory()
        {
            if (_Category != null && _Category.Count() > 0)
                return _Category;
            else
                return new List<CategoryReadDto>();
        }

        public async Task<IActionResult> SubCategoryList()
        {
            var data = await _subCategoryService.ReadAll();
            int rowNum = 1;
            var subCategories = data.Select(subCategory => new ListSubCategoryVM()
            {
                RowNum = rowNum++,
                Id = subCategory.Id,
                Name = subCategory.Name,
                CategoryName = subCategory.Category.Name
            }).ToList();

            return View(subCategories);
        }

        [HttpPost]
        public async Task<IActionResult> Save(SubCategoryVM subCategoryVM)
        {
            if (subCategoryVM.Id > 0)
                return await EditSubCategory(subCategoryVM);
            else
                return await NewSubCategory(subCategoryVM);
        }
        public async Task<IActionResult> NewSubCategory()
        {
            _Category = await _categoryService.ReadAll();

            return View("EditSubCategory", new SubCategoryVM() { Categories = GetCategory()});
        }
        [HttpPost]
        public async Task<IActionResult> NewSubCategory(SubCategoryVM subCategoryVM)
        {
            subCategoryVM.Categories = GetCategory();
            if (subCategoryVM.CategoryId < 1)
            {
                ViewData["error"] = "گروه اصلی نمی تواند خالی باشد"; // TODO : must be in ViewModel
                return View("EditSubCategory", subCategoryVM);
            }
            SubCategory newSubCategory = new SubCategory()
            {
                Name = subCategoryVM.Name,
                CategoryId = subCategoryVM.CategoryId,
            };
            try
            {
                long id = await _subCategoryService.Create(newSubCategory);
                if (id > 0)
                {
                    TempData["error"] = "گروه فرعی با موفقیت ثبت شد";
                    return RedirectToAction(nameof(SubCategoryList));

                }
                else
                {
                    ViewData["error"] = "خطایی در هنگام ثبت گروه فرعی رخ داد";
                    return View("EditSubCategory", subCategoryVM);
                }
            }
            catch (Exception)
            {
                ViewData["error"] = "خطایی در هنگام ثبت گروه فرعی رخ داد";
                return View("EditSubCategory", subCategoryVM);
            }
        }

        public async Task<IActionResult> EditSubCategory(int id)
        {
            SubCategory subCategory = await _subCategoryService.ReadById(id);
            if (subCategory != null)
            {
                _Category = await _categoryService.ReadAll();
                return View(new SubCategoryVM()
                {
                    Id = subCategory.Id,
                    Name = subCategory.Name,
                    CategoryId = subCategory.CategoryId,
                    Categories = GetCategory()
                });
            }
            else
            {
                TempData["error"] = "گروه فرعی مورد نظر یافت نشد";
                return RedirectToAction(nameof(SubCategoryList));
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditSubCategory(SubCategoryVM subCategoryVM)
        {
            subCategoryVM.Categories = GetCategory();
            SubCategory existSubCategory = new SubCategory()
            {
                Id = subCategoryVM.Id,
                Name = subCategoryVM.Name,
                CategoryId = subCategoryVM.CategoryId
            };
            try
            {
                var resultMessage = await _subCategoryService.Update(existSubCategory);
                TempData["error"] = resultMessage == ResultOutPutMethodEnum.savechanged ? "گروه فرعی ویرایش شد" :
                        resultMessage == ResultOutPutMethodEnum.recordNotFounded ? "گروه فرعی یافت نشد" :
                        "گروه فرعی ویرایش نشد";

                if (resultMessage == ResultOutPutMethodEnum.savechanged)
                    return RedirectToAction(nameof(SubCategoryList));
                else
                {
                    return View("EditSubCategory", subCategoryVM);
                }
            }
            catch (Exception)
            {
                ViewData["error"] = "خطایی در هنگام ویرایش گروه فرعی رخ داد";
                return View("EditSubCategory", subCategoryVM);
            }
        }

        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            if(await _subCategoryService.IsSubCategoryHaveProduct(id))
            {
                TempData["error"] = "گروه فرعی شامل محصول است . امکان حذف وجود ندارد";
                return RedirectToAction(nameof(SubCategoryList));
            }
            var resultMessage = await _subCategoryService.Delete(id);

            TempData["error"] = resultMessage == ResultOutPutMethodEnum.savechanged ? "گروه فرعی حذف شد" :
                          resultMessage == ResultOutPutMethodEnum.recordNotFounded ? "گروه فرعی یافت نشد" :
                          "گروه فرعی حذف نشد";

            return RedirectToAction(nameof(SubCategoryList));
        }
    }
}
