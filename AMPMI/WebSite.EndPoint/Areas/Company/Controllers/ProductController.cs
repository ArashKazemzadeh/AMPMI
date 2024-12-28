using AQS_Aplication.Interfaces.IServisces.BaseServices;
using Domin.Entities;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Areas.Company.Models.Product;

namespace WebSite.EndPoint.Areas.Company.Controllers
{
    [Area("Company")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ISubCategoryService _subCategoryService;
        public ProductController(IProductService productService, ISubCategoryService subCategoryService)
        {
            this._productService = productService;
            this._subCategoryService = subCategoryService;
        }
        public async Task<IActionResult> ProductList()
        {
            long companyId = 9;
            List<Product> data = await _productService.ReadByCompanyId(companyId);
            int rowNum = 1;
            List<ListProductVM> products = data.Select(x => new ListProductVM()
            {
                RowNum=rowNum++,
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
        public async Task<IActionResult> EditProduct(long id)
        {
            Product product = await _productService.ReadById(id);
            if (product != null)
            {
                List<SubCategory> subCategories = await _subCategoryService.ReadAll();
                return View(new ProductVM() 
                {
                    Id = product.Id,
                    CompanyId = product.CompanyId,
                    Description = product.Description,
                    Name = product.Name,
                    IsConfirmed = product.IsConfirmed,
                    PictureFileName = product.PictureFileName,
                    SubCategoryId = product.SubCategoryId,
                    CategoryId = product.SubCategory.CategoryId,
                    SubCategories = subCategories
                });
            }
            return NotFound();
        }
        public async Task<IActionResult> DeleteProduct(long id)
        {
            Product product = await _productService.ReadById(id);
            if (product != null)
            {
                await _productService.Delete(id);
                return RedirectToAction(nameof(ProductList));
            }
            return NotFound();
        }
        //[HttpPost]
        //public IActionResult NewProduct()
        //{
        //    return RedirectToAction(nameof(ProductList));
        //}
    }
}
