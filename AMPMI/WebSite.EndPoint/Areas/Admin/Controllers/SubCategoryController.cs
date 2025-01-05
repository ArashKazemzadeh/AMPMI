using AQS_Aplication.Dtos.BaseServiceDto.CategoryDto;
using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Common.Enums;
using Domin.Entities;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Areas.Admin.Models.SubCategory;
using WebSite.EndPoint.Utility;

namespace WebSite.EndPoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoryController : Controller
    {
        private readonly ISubCategoryService _subCategoryService;
        private readonly ICategoryService _categoryService;
        private readonly IFileServices _fileServices;

        const string PictureFolder = "SubCategory";

        public SubCategoryController(ISubCategoryService productService, ICategoryService categoryService, IFileServices fileServices)
        {
            _subCategoryService = productService;
            _categoryService = categoryService;
            _fileServices = fileServices;
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
            List<CategoryReadDto> categories = await _categoryService.ReadAll();

            return View("EditSubCategory", new SubCategoryVM() { Categories = categories });
        }
        [HttpPost]
        public async Task<IActionResult> NewSubCategory(SubCategoryVM subCategoryVM)
        {
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
                List<CategoryReadDto> categories = await _categoryService.ReadAll();
                return View(new SubCategoryVM()
                {
                    Id = subCategory.Id,
                    Name = subCategory.Name,
                    CategoryId = subCategory.CategoryId,
                    Categories = categories
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
                    return View(subCategoryVM);
                }
            }
            catch (Exception)
            {
                ViewData["error"] = "خطایی در هنگام ویرایش گروه فرعی رخ داد";
                return View(subCategoryVM);
            }
        }

        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            var resultMessage = await _subCategoryService.Delete(id);

            TempData["error"] = resultMessage == ResultOutPutMethodEnum.savechanged ? "گروه فرعی حذف شد" :
                          resultMessage == ResultOutPutMethodEnum.recordNotFounded ? "گروه فرعی یافت نشد" :
                          "گروه فرعی حذف نشد";

            return RedirectToAction(nameof(SubCategoryList));
        }
    }
}
