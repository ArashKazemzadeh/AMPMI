using AQS_Application.Interfaces.IServices.BaseServices;
using Domin.Entities;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Models.CompanyViewModel;
using WebSite.EndPoint.Models.ProductViewModel;

namespace WebSite.EndPoint.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;
        public CompanyController(ICompanyService companyService )
        {
            _companyService = companyService;
        }
        public async Task<IActionResult> CompanyList()
        {
            List<Company> result = await _companyService.Read();
            List<CompanyVM> resultVM = result.Select(x => new CompanyVM 
            {
                Id = x.Id,
                Address = x.Address,
                LogoRout = x.LogoRout,
                MobileNumber = x.MobileNumber,
                Name = x.Name,
                TeaserGuid = x.TeaserGuid,
                Email = x.Email,
                ManagerName = x.ManagerName,
                Tel = x.Tel,
            }).ToList();

            return View(resultVM);
        }
        public async Task<IActionResult> CompanyDetail(long companyId)
        {
            var result = await _companyService.ReadByIdIncludePicturesAndProducts(companyId);
            if (result != null)
            {
                CompanyDetailVM companyDetailVM = new CompanyDetailVM()
                {
                    LogoRout = result.LogoRout,
                    Address = result.Address,
                    MobileNumber = result.MobileNumber,
                    Name = result.Name,
                    About = result.About,
                    TeaserGuid = result.TeaserGuid,
                    ManagerName= result.ManagerName,
                    CompanyPictures = result.CompanyPictures.Select(x=>x.PictureFileName).ToList(),
                    Email = result.Email,
                    Tel = result.Tel,
                    Products = result.Products.Select(x => new ProductVM()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        PictureFileName = (x.ProductPictures != null && x.ProductPictures.Count > 0) 
                        ? x.ProductPictures.FirstOrDefault().Rout 
                        : ""
                        //PictureFileName = x.PictureFileName
                    }).ToList()
                };

                return View(companyDetailVM);
            }
            else
            {
                return NotFound();
            }
            
        }
    }
}
