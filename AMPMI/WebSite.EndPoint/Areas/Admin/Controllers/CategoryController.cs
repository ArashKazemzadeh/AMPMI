using AQS_Aplication.Interfaces.IServisces.BaseServices;
using AQS_Aplication.Services;
using AQS_Common.Enums;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Areas.Admin.Models.Category;
using WebSite.EndPoint.Areas.Admin.Models.Notification;

namespace WebSite.EndPoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> CategoryList()
        {
            var message = TempData["Message"]?.ToString();

            if (!string.IsNullOrEmpty(message))
                ViewData["Message"] = message;

            var categorys = await _categoryService.ReadAll();
            var model = CategoryReadVM.ConvertToModel(categorys);

            if (model.Count < 1)
                model = CategoryReadVM.Seed();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string name, string img)
        {
            //Todo: سرویس جدا برای ذخیره عکس اختصاصی برای همین

            var resultMessage = await _categoryService.Update(id, name);

            if (resultMessage == ResultOutPutMethodEnum.savechanged)
                await _categoryService.UpdatePicture(id, img);

            TempData["Message"] = resultMessage == ResultOutPutMethodEnum.savechanged ? "دسته بندی اصلی حذف شد" :
                                  resultMessage == ResultOutPutMethodEnum.recordNotFounded ? "دسته بندی اصلی یافت نشد" :
                                  "دسته بندی اصلی حذف نشد";

            return RedirectToAction(nameof(CategoryList));
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name, string img)
        {
            //Todo: سرویس جدا برای ذخیره عکس اختصاصی برای همین
            long id = await _categoryService.Create(name, name);

            TempData["Message"] = id > 0 ? "اعلان با موفقیت ایجاد شد" : "اعلان جدید ایجاد نشد";

            return RedirectToAction(nameof(CategoryList));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var resultMessage = await _categoryService.Delete(id);

            TempData["Message"] = resultMessage == ResultOutPutMethodEnum.savechanged ? "دسته بندی اصلی حذف شد" :
                                  resultMessage == ResultOutPutMethodEnum.recordNotFounded ? "دسته بندی اصلی یافت نشد" :
                                  "دسته بندی اصلی حذف نشد";

            return RedirectToAction(nameof(CategoryList));
        }

        [HttpPost]
        public async Task<IActionResult> Save(int id, string name, string img)
        {
            if (id > 0)
                return RedirectToAction(nameof(Edit), new { id, name, img });
            else
                return RedirectToAction(nameof(Create), new { name, img });
        }
    }
}