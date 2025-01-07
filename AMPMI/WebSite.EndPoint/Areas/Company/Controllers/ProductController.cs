using AQS_Application.Dtos.BaseServiceDto.CategoryDtos;
using AQS_Application.Dtos.BaseServiceDto.SubCategoryDto;
using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Application.Interfaces.IServices.IdentityServices;
using AQS_Common.Enums;
using Domin.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Areas.Company.Models.Product;
using WebSite.EndPoint.Utility;

namespace WebSite.EndPoint.Areas.Company.Controllers
{
    [Area("Company")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ISubCategoryService _subCategoryService;
        private readonly ICategoryService _categoryService;
        private readonly IFileServices _fileServices;
        private readonly ILoginService _loginService;

        static List<SubCategoryReadDto> subCategories;
        static List<CategoryIncludeSubCategoriesDto> _Category;

        const string PictureFolder = "Product";
        public ProductController(IProductService productService, ISubCategoryService subCategoryService,
            ICategoryService categoryService, IFileServices fileServices,ILoginService loginService)
        {
            this._productService = productService;
            this._subCategoryService = subCategoryService;
            this._categoryService = categoryService;
            this._fileServices = fileServices;
            this._loginService = loginService;
        }
        public static List<SubCategoryReadDto> GetSubCategoryByCategory(int categoryId)
        {
            if (subCategories != null && subCategories.Count() > 0)
            {
                return subCategories.Where(x => x.CategoryId == categoryId).ToList();
            }
            else
            {
                return new List<SubCategoryReadDto>();
            }
        }
        public static List<CategoryIncludeSubCategoriesDto> GetCategory()
        {
            if (_Category != null && _Category.Count() > 0)
                return _Category;
            else
                return new List<CategoryIncludeSubCategoriesDto>();
        }
        public async Task<IActionResult> ProductList()
        {
            long companyId = await _loginService.GetUserIdAsync(User);
            List<Product> data = await _productService.ReadByCompanyId(companyId);
            int rowNum = 1;
            List<ListProductVM> products = data.Select(x => new ListProductVM()
            {
                RowNum = rowNum++,
                Id = x.Id,
                CompanyId = x.CompanyId,
                Description = x.Description,
                Name = x.Name,
                IsConfirmed = x.IsConfirmed,
                PictureFileName = x.PictureFileName,
                SubCategoryName = x.SubCategory.Name,
                CategoryName = x.SubCategory.Category.Name
            }).ToList();

            return View(products);
        }
        [HttpPost]
        public async Task<IActionResult> Save(ProductVM productVM)
        {
            if (!ModelState.IsValid)
            {
                productVM.Categories = GetCategory();
                return View("EditProduct", productVM);
            }
            if (productVM.Id > 0)
                return await EditProduct(productVM);
            else
                return await NewProduct(productVM);
        }
        public async Task<IActionResult> NewProduct()
        {
            _Category = await _categoryService.ReadAlIncludeSub();
            subCategories = _Category.SelectMany(x => x.SubCategories).ToList();
            return View("EditProduct", new ProductVM() { Categories = _Category });
        }
        [HttpPost]
        public async Task<IActionResult> NewProduct(ProductVM productVM)
        {
            productVM.Categories = GetCategory();
            long companyId = await _loginService.GetUserIdAsync(User);
            Product newProduct = new Product()
            {
                Name = productVM.Name,
                Description = productVM.Description,
                CompanyId = companyId,
                IsConfirmed = false,
                SubCategoryId = productVM.SubCategoryId
            };
            if (productVM.PictureFileName != null)
            {
                string picureFilePath = await _fileServices.SaveFileAsync(productVM.PictureFileName, "Product");
                if (string.IsNullOrEmpty(picureFilePath))
                {
                    ViewData["error"] = "خطایی در هنگام ثبت تصویر رخ داد";
                    return View("EditProduct", productVM);
                }
                else
                {
                    newProduct.PictureFileName = picureFilePath;
                }
            }
            try
            {
                long id = await _productService.Create(newProduct);
                if (id > 0)
                    return RedirectToAction(nameof(ProductList));
                else
                {
                    ViewData["error"] = "خطایی در هنگام ثبت کالا رخ داد";
                    return View("EditProduct", productVM);
                }
            }
            catch (Exception)
            {
                ViewData["error"] = "خطایی در هنگام ثبت کالا رخ داد";
                return View("EditProduct", productVM);
            }
        }
        [HttpGet]
        public IActionResult ChangeCategory(int categoryId)
        {
            return Json(GetSubCategoryByCategory(categoryId));
        }
        public async Task<IActionResult> EditProduct(long id)
        {
            Product product = await _productService.ReadById(id);
            if (product != null)
            {
                _Category = await _categoryService.ReadAlIncludeSub();
                subCategories = _Category.SelectMany(x => x.SubCategories).ToList();

                return View(new ProductVM()
                {
                    Id = product.Id,
                    CompanyId = product.CompanyId,
                    Description = product.Description,
                    Name = product.Name,
                    IsConfirmed = product.IsConfirmed,
                    PictureFileSrc = product.PictureFileName,
                    SubCategoryId = product.SubCategoryId,
                    CategoryId = product.SubCategory.CategoryId,
                    Categories = _Category
                });
            }
            else
            {
                TempData["error"] = "محصول مورد نظر یافت نشد";
                return RedirectToAction(nameof(ProductList));
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductVM productVM)
        {
            long companyId = await _loginService.GetUserIdAsync(User);
            productVM.Categories = GetCategory();

            Product existProdcut = new Product()
            {
                Id = productVM.Id,
                Name = productVM.Name,
                Description = productVM.Description,
                CompanyId = companyId,
                SubCategoryId = productVM.SubCategoryId
            };
            try
            {
                if (productVM.IsPictureChanged)
                {
                    if (await _fileServices.DeleteFile(productVM.PictureFileSrc))
                    {
                        string newPicture = await _fileServices.SaveFileAsync(productVM.PictureFileName, PictureFolder);
                        if (string.IsNullOrEmpty(newPicture))
                        {
                            ViewData["error"] = "خطایی در هنگام ثبت تصویر رخ داد";
                            return View("EditProduct", productVM);
                        }
                        else
                        {
                            existProdcut.PictureFileName = newPicture;
                        }
                    }
                }
                var result = await _productService.Update(existProdcut);
                if (result == ResultOutPutMethodEnum.savechanged || result == ResultOutPutMethodEnum.dontSaved)
                    return RedirectToAction(nameof(ProductList));
                else
                {
                    ViewData["error"] = "خطایی در هنگام ثبت کالا رخ داد";
                    return View(productVM);
                }
            }
            catch (Exception)
            {
                ViewData["error"] = "خطایی در هنگام ثبت کالا رخ داد";
                return View(productVM);
            }
        }
        public async Task<IActionResult> DeleteProduct(long id)
        {
            Product product = await _productService.ReadById(id);
            if (product != null)
            {
                if (!string.IsNullOrEmpty(product.PictureFileName) && await _fileServices.DeleteFile(product.PictureFileName))
                {
                    var result = await _productService.Delete(id);
                    if (result != ResultOutPutMethodEnum.savechanged)
                        TempData["error"] = "خطایی در هنگام حذف کالا رخ داد";
                }
                else
                    TempData["error"] = "خطایی در هنگام حذف کالا رخ داد";

            }
            else
            {
                TempData["error"] = "محصول مورد نظر یافت نشد";
            }
            return RedirectToAction(nameof(ProductList));
        }
    }
}
