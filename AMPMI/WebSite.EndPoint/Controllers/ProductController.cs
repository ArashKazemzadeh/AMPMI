using AQS_Aplication.Interfaces.IServisces;
using Domin.Entities;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Models.ProductViewModel;

namespace WebSite.EndPoint.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;  
        public ProductController(IProductService productService)
        {
            this._productService = productService;
        }
        public async Task<IActionResult> ProductsList()
        {
            List<Product> result = await _productService.Read();
            List<ProductVM> resultVM = result.Select(x => new ProductVM
            {
                Id = x.Id,
                Name = x.Name,
                PictureFileName = x.PictureFileName
            }).ToList();

            return View(resultVM);
        }
        public async Task<IActionResult> CategoryProductsList(int categoryId)
        {
            List<Product> result = await _productService.ReadByCategoryId(categoryId);
            List<ProductVM> resultVM = result.Select(x => new ProductVM
            {
                Id = x.Id,
                Name = x.Name,
                PictureFileName = x.PictureFileName
            }).ToList();

            return View(nameof(ProductsList),resultVM);
        }
        public async Task<IActionResult> ProductDetail(int productId) //Ok
        {
            Product? product = await _productService.ReadById(productId);
            if(product != null)
            {
                ProductDetailVM productDetailVM = new ProductDetailVM() 
                {
                    Name = product.Name,
                    Description = product.Description,
                    PictureFileName = product.PictureFileName,
                    SubCategoryId = product.SubCategoryId,
                    CompanyId = product.CompanyId ?? -1,
                    CompanyName = product.Company?.Name,
                    CompanyLogoRout = product.Company?.LogoRout
                };

                return View(productDetailVM);
            }
            else
            {
                ////just for test
                ProductDetailVM productDetailVM = new ProductDetailVM()
                {
                    Name = "محصول تست",
                    Description = "چرتو پرت",
                    PictureFileName = null,
                    SubCategoryId = 2,
                    CompanyId = 1,
                    CompanyName = "تست است",
                    CompanyLogoRout = null
                };
                return View(productDetailVM);
                //return NotFound();
            }
        }
    }
}
