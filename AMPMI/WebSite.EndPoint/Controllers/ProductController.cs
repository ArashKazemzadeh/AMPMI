using AQS_Aplication.Interfaces.IServisces.BaseServices;
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
        public IActionResult Index()
        {
            return View();
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
                //ProductDetailVM productDetailVM = new ProductDetailVM()
                //{
                //    Name = "محصول تست",
                //    Description = "چرتو پرت",
                //    PictureFileName = null,
                //    SubCategoryId = 2,
                //    CompanyId = 1,
                //    CompanyName = "تست است",
                //    CompanyLogoRout = null
                //};
                //return View(productDetailVM);
                return NotFound();
            }
        }
    }
}
