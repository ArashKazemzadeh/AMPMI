using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Common.Enums;
using Domin.Entities;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Areas.Admin.Models.Category;
using WebSite.EndPoint.Utility;

namespace WebSite.EndPoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IFileServices _fileServices;

        const string PictureFolder = "Categories";
        public CategoryController(ICategoryService categoryService, IFileServices fileServices)
        {
            _categoryService = categoryService;
            _fileServices = fileServices;
        }

        [HttpGet]
        public async Task<IActionResult> CategoryList()
        {
            var message = TempData["Message"]?.ToString();

            if (!string.IsNullOrEmpty(message))
                ViewData["Message"] = message;

            var categories = await _categoryService.ReadAll();
            var model = CategoryReadVM.ConvertToModel(categories);

            //if (model.Count < 1)
            //    model = CategoryReadVM.Seed();

            return View(model);
        }
        public async Task<IActionResult> Create()
        {
            return View("Edit", new CategoryEditVM());
        }
        [HttpPost]
        public async Task<IActionResult> Create(IFormFile picture, string name)
        {
            try
            {
                var rout = await _fileServices.SaveFileAsync(picture, PictureFolder);
                if (string.IsNullOrEmpty(rout))
                {
                    ViewData["Message"] = "خطایی در هنگام ثبت تصویر رخ داد";
                }
                else
                {
                    long id = await _categoryService.Create(name, rout);
                    TempData["Message"] = id > 0 ? "دسته بندی اصلی با موفقیت ایجاد شد" : "دسته بندی اصلی جدید ایجاد نشد";

                }
                return RedirectToAction(nameof(CategoryList));

            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return RedirectToAction(nameof(CategoryList));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Category category = await _categoryService.ReadById(id); // TODO : Use DTO
            if (category != null)
            {
                return View(new CategoryEditVM()
                {
                    Id = category.Id,
                    Name = category.Name,
                    PreviousPictureRout = category.PictureFileName
                });
            }
            else
            {
                TempData["Message"] = "دسته بندی مورد نظر مورد نظر یافت نشد";
                return RedirectToAction(nameof(CategoryList));
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditVM categoryEditVM)
        {
            try
            {
                string newRout = "";
                if (categoryEditVM.IsPictureChanged)
                {

                    var isDelete = await _fileServices.DeleteFile(categoryEditVM.PreviousPictureRout);
                    newRout = await _fileServices.SaveFileAsync(categoryEditVM.Picture, PictureFolder);

                    if (string.IsNullOrEmpty(newRout))
                    {
                        ViewData["Message"] = "خطایی در هنگام ثبت تصویر رخ داد";
                        return RedirectToAction(nameof(CategoryList));
                    }
                }
                var resultMessage = await _categoryService.Update(categoryEditVM.Id, categoryEditVM.Name);

                if (resultMessage == ResultOutPutMethodEnum.savechanged)
                    await _categoryService.UpdatePicture(categoryEditVM.Id, newRout);

                TempData["Message"] = resultMessage == ResultOutPutMethodEnum.savechanged ? "دسته بندی اصلی ویرایش شد" :
                                      resultMessage == ResultOutPutMethodEnum.recordNotFounded ? "دسته بندی اصلی یافت نشد" :
                                      "دسته بندی اصلی ویرایش نشد";

                return RedirectToAction(nameof(CategoryList));
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return RedirectToAction(nameof(CategoryList));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if(await _categoryService.IsCategoryHaveChildren(id))
            {
                TempData["Message"]= "گروه اصلی شامل گروه فرعی است . امکان حذف کردن وجود ندارد";
                return RedirectToAction(nameof(CategoryList));
            }

            var getResultDto = await _categoryService.GetPictureRout(id);
            if (!string.IsNullOrEmpty(getResultDto.Message))
            {
                TempData["Message"] = getResultDto.Message;
                //return RedirectToAction(nameof(CategoryList));
            }
            if(!string.IsNullOrEmpty(getResultDto.Path))
                await _fileServices.DeleteFile(getResultDto.Path);

            var resultMessage = await _categoryService.Delete(id);

            TempData["Message"] = resultMessage == ResultOutPutMethodEnum.savechanged ? "دسته بندی اصلی حذف شد" :
                                  resultMessage == ResultOutPutMethodEnum.recordNotFounded ? "دسته بندی اصلی یافت نشد" :
                                  "دسته بندی اصلی حذف نشد";
            return RedirectToAction(nameof(CategoryList));
        }

        [HttpPost]
        public async Task<IActionResult> Save(CategoryEditVM categoryEditVM)
        {

            if (categoryEditVM.Id > 0)
                return await Edit(categoryEditVM);
            else
                return await Create(categoryEditVM.Picture, categoryEditVM.Name);
        }
    }
}
