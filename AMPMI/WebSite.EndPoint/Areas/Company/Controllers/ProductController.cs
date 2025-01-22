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
            ICategoryService categoryService, IFileServices fileServices, ILoginService loginService)
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
                //PictureFileName = x.PictureFileName,
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
            if(productVM.PictureFileName == null || productVM.PictureFileName.Count < 1)
            {
                ViewData["error"] = "وجود حداقل یک تصویر برای محصول اجباری است";
                return View("EditProduct", productVM);
            }
            Product newProduct = new Product()
            {
                Name = productVM.Name,
                Description = productVM.Description,
                CompanyId = companyId,
                IsConfirmed = false,
                SubCategoryId = productVM.SubCategoryId
            };
            try
            {
                long id = await _productService.Create(newProduct);
                if (id > 0)
                {
                    if (productVM.PictureFileName != null)
                    {
                        foreach (var item in productVM.PictureFileName)
                        {
                            string newPicture = await _fileServices.SaveFileAsync(item, PictureFolder);
                            if (string.IsNullOrEmpty(newPicture))
                            {
                                ViewData["error"] = "خطایی در هنگام ثبت تصویر رخ داد";
                                return View("EditProduct", productVM);
                            }
                            else
                            {
                                var result = await _productService.UpdatePictureRout(id, newPicture);
                                if (result != ResultOutPutMethodEnum.savechanged)
                                {
                                    ViewData["error"] = "خطایی در هنگام ثبت تصویر رخ داد";
                                    return View("EditProduct", productVM);
                                }
                            }
                        }
                    }

                    return RedirectToAction(nameof(ProductList));
                }
                else
                {
                    ViewData["error"] = "خطایی در هنگام ثبت کالا رخ داد";
                    return View("EditProduct", productVM);
                }
            }
            catch (Exception e)
            {
                ViewData["error"] = e.Message;
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
                    Pictures = product.ProductPictures,
                    //PictureFileSrc = product.PictureFileName,
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

            if((productVM.Pictures == null || productVM.Pictures.Count < 1 ) && 
               (productVM.PictureFileName == null || productVM.PictureFileName.Count < 1))
            {
                ViewData["error"] = "وجود حداقل یک تصویر برای محصول اجباری است";
                return View("EditProduct", productVM);
            }

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
                if (productVM.PictureFileName != null)
                {
                    foreach (var item in productVM.PictureFileName)
                    {
                        string newPicture = await _fileServices.SaveFileAsync(item, PictureFolder);
                        if (string.IsNullOrEmpty(newPicture))
                        {
                            ViewData["error"] = "خطایی در هنگام ثبت تصویر رخ داد";
                            return View("EditProduct", productVM);
                        }
                        else
                        {
                            var resultPic = await _productService.UpdatePictureRout(existProdcut.Id, newPicture);
                            if (resultPic != ResultOutPutMethodEnum.savechanged)
                            {
                                ViewData["error"] = "خطایی در هنگام ثبت تصویر رخ داد";
                                return View("EditProduct", productVM);
                            }
                        }
                    }
                }
                var result = await _productService.Update(existProdcut);
                if (result == ResultOutPutMethodEnum.savechanged || result == ResultOutPutMethodEnum.dontSaved)
                    return RedirectToAction(nameof(ProductList));
                else
                {
                    ViewData["error"] = "خطایی در هنگام ثبت کالا رخ داد";
                    return View("EditProduct", productVM);
                }
            }
            catch (Exception e)
            {
                ViewData["error"] = e.Message;
                return View("EditProduct", productVM);
            }
        }
        public async Task<IActionResult> DeleteProduct(long id)
        {
            Product product = await _productService.ReadById(id);
            if (product != null)
            {
                var productPictures = new List<ProductPicture>();
                productPictures.AddRange(product.ProductPictures);
                foreach (var item in productPictures)
                {
                    if (await _fileServices.DeleteFile(item.Rout))
                    {
                        await _productService.DeleteProductPicture(item.Id);
                    }
                }
                var result = await _productService.Delete(id);
                if (result != ResultOutPutMethodEnum.savechanged)
                    TempData["error"] = "خطایی در هنگام حذف کالا رخ داد";
            }
            else
            {
                TempData["error"] = "محصول مورد نظر یافت نشد";
            }
            return RedirectToAction(nameof(ProductList));
        }
        public async Task<IActionResult> DeletePicture(long pictureId, long productId)
        {
            var result = await _productService.DeleteProductPicture(pictureId);
            if (result != ResultOutPutMethodEnum.savechanged)
                TempData["error"] = "خطا در هنگام حذف تصویر محصول";

            return RedirectToAction(nameof(EditProduct), new { id = productId });
        }
    }
}
