﻿using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Application.Services;
using AQS_Common.Enums;
using Domin.Entities;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Areas.Admin.Models.Product;
using WebSite.EndPoint.Utility;

namespace WebSite.EndPoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IFileServices _fileServices;

        static List<SubCategory> subCategories;

        public ProductController(IProductService productService, ICategoryService categoryService,IFileServices fileServices)
        {
            this._productService = productService;
            this._categoryService = categoryService;
            this._fileServices = fileServices;
        }
        public static List<SubCategory> GetSubCategoryByCategory(int categoryId)
        {
            if (subCategories != null && subCategories.Count() > 0)
            {
                return subCategories.Where(x => x.CategoryId == categoryId).ToList();
            }
            else
            {
                // Seed Data Just For Test
                return new List<SubCategory>() {
                   new SubCategory{ Id =1 ,Name = "اولیش" },
                   new SubCategory{ Id =2 ,Name = "دومیش" },
                   new SubCategory{ Id =3 ,Name = "سومیش" },
                   new SubCategory{ Id =4 ,Name = "چهارمیش" },
                };
            }
            //return new List<SubCategory>();
        }
        public async Task<IActionResult> ProductList()
        {
            List<Product> data = await _productService.Read();
            int rowNum = 1;
            List<ListProductVM> products = data.Select(x => new ListProductVM()
            {
                RowNum = rowNum++,
                Id = x.Id,
                CompanyId = x.CompanyId,
                Description = x.Description,
                CompanyName = x.Company.Name,
                Name = x.Name,
                IsConfirmed = x.IsConfirmed,
                PictureFileSrc = x.PictureFileName,
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
                return View("EditProduct", productVM);
            }
            if (productVM.Id > 0)
                return await EditProduct(productVM);
            else
                return await NewProduct(productVM);
        }
        public async Task<IActionResult> NewProduct()
        {
            List<Category> categories = await _categoryService.Read();
            subCategories = categories.SelectMany(x => x.SubCategories).ToList();

            return View("EditProduct", new ProductVM() { Categories = categories });
        }
        [HttpPost]
        public async Task<IActionResult> NewProduct(ProductVM productVM)
        {
            Product newProduct = new Product()
            {
                Name = productVM.Name,
                Description = productVM.Description,
                IsConfirmed = false,
                SubCategoryId = productVM.SubCategoryId
            };
            try
            {
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

        public async Task<IActionResult> EditProduct(long id)
        {
            Product product = await _productService.ReadById(id);
            if (product != null)
            {
                List<Category> categories = await _categoryService.Read();
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
                    Categories = categories
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
            long companyId = Convert.ToInt64(User.Identity.Name);
            Product existProdcut = new Product()
            {
                Id = productVM.Id,
                Name = productVM.Name,
                Description = productVM.Description,
                PictureFileName = null, // TODO
                CompanyId = companyId,
                SubCategoryId = productVM.SubCategoryId
            };
            try
            {
                if (productVM.IsPictureChanged)
                {
                    if (_fileServices.DeleteFile(productVM.PictureFileSrc))
                    {
                        string newPicture = await _fileServices.SaveFileAsync(productVM.PictureFileName, "Product");
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
                if (result == ResultOutPutMethodEnum.savechanged)
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

        [HttpGet]
        public IActionResult ChangeCategory(int categoryId)
        {
            return Json(GetSubCategoryByCategory(categoryId));
        }
        public async Task<IActionResult> NotConfirmedProductList()
        {
            long companyId = 1;
            if (User.Identity.IsAuthenticated)
            {
                companyId = Convert.ToInt64(User.Identity.Name);
            }
            List<Product> data = await _productService.ReadNotConfirmed();
            int rowNum = 1;
            List<ListProductVM> products = data.Select(x => new ListProductVM()
            {
                RowNum = rowNum++,
                Id = x.Id,
                CompanyId = x.CompanyId,
                Description = x.Description,
                Name = x.Name,
                IsConfirmed = x.IsConfirmed,
                PictureFileSrc = x.PictureFileName,
                SubCategoryName = x.SubCategory.Name,
                CategoryName = x.SubCategory.Category.Name
            }).ToList();

            return View(products);
        }
        /// <summary>
        /// تایید محصول
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IActionResult ConfirmProduct(int productId)
        {
            return RedirectToAction(nameof(NotConfirmedProductList));
        }
        public async Task<IActionResult> DeleteProduct(long id)
        {
            Product product = await _productService.ReadById(id);
            if (product != null)
            {
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
    }
}
