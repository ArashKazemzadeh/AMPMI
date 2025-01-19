using AQS_Application.Interfaces.IServices.BaseServices;
using Domin.Entities;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Models.ProductViewModel;

namespace WebSite.EndPoint.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;  
        public ProductController(IProductService productService,ICompanyService companyService)
        {
            this._productService = productService;
        }
        public async Task<IActionResult> ProductList()
        {
            List<Product> result = await _productService.Read();
            List<ProductVM> resultVM = result.Select(x => new ProductVM
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();

            return View(resultVM);
        }
        public async Task<IActionResult> CategoryProductsList(int categoryId)
        {
            List<Product> result = await _productService.ReadByCategoryId(categoryId, isConfirmed: true);
            List<ProductVM> resultVM = result.Select(x => new ProductVM
            {
                Id = x.Id,
                Name = x.Name,
                PictureFileName = (x.ProductPictures != null && x.ProductPictures.Count > 0) ? x.ProductPictures.FirstOrDefault().Rout : ""
            }).ToList();

            return View(nameof(ProductList), resultVM);
        }
        public async Task<IActionResult> ProductDetail(int productId) //Ok
        {
            Product? product = await _productService.ReadByIdIncludeCategoryAndSubCategoryAndCompany(productId,isConfirmed:true);
            if(product != null)
            {
                ProductDetailVM productDetailVM = new ProductDetailVM() 
                {
                    Name = product.Name,
                    Description = product.Description,
                    PictureFileName = product.ProductPictures.Select(m => m.Rout).ToList(),
                    SubCategoryId = product.SubCategoryId,
                    CompanyId = product.CompanyId ?? -1,
                    CompanyName = product.Company?.Name,
                    CompanyLogoRout = product.Company?.LogoRout,
                    CategoryId = product.SubCategory.CategoryId,
                    CategoryName = product.SubCategory.Category.Name                    
                };

                return View(productDetailVM);
            }
            else
            {
                return View(new ProductDetailVM());
            }
        }
        [HttpPost]
        public async Task<IActionResult> SearchProduct(string name, int categorySelected = -1)
        {
            if (string.IsNullOrEmpty(name))
                return View(nameof(ProductList), new List<ProductVM>());
            var result = new List<Product>();
            name = name.Trim();
            if(categorySelected > 0)
            {
                result = await _productService.SearchProductByNameAndCategory(name, categorySelected,isConfirmed:true);
            }
            else
            {
                result = await _productService.SearchProductByName(name,isConfirmed:true);
            }
            List<ProductVM> products = result.Select(x => new ProductVM() 
            {
                Id = x.Id,
                Name = x.Name,
                PictureFileName = (x.ProductPictures != null && x.ProductPictures.Count>0 ) ? x.ProductPictures.FirstOrDefault().Rout : ""
            }).ToList();

            return View(nameof(ProductList), products);
        }
    }
}
