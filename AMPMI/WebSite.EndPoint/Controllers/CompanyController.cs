using AQS_Aplication.Interfaces.IServisces;
using Domin.Entities;
using Microsoft.AspNetCore.Mvc;
using NuGet.DependencyResolver;
using System.Runtime.InteropServices;
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
                Address = x.Address,
                LogoRout = x.LogoRout,
                MobileNumber = x.MobileNumber,
                Name = x.Name,
                TeaserGuid = x.TeaserGuid
            }).ToList();

            return View(resultVM);
        }
        public async Task<IActionResult> CompanyDetail(int companyId)
        {
            Company? result = await _companyService.ReadById(companyId);
            if (result != null)
            {
                //CompanyDetailVM companyDetailVM = new CompanyDetailVM()
                //{
                //    LogoRout = result.LogoRout,
                //    Address = result.Address,
                //    MobileNumber = result.MobileNumber,
                //    Name = result.Name,
                //    TeaserGuid = result.TeaserGuid,
                //    Products = result.Products.Select(x => new ProductVM()
                //    {
                //        Name = x.Name,
                //        PictureFileName = x.PictureFileName
                //    }).ToList()
                //};

                //return View(companyDetailVM);
            }
            CompanyDetailVM obj = new CompanyDetailVM() 
            {
                Address = "تهرانسر",
                MobileNumber= "42342324",
                Name="پرتو سازان",
                
            };
            return View(obj);
            //return NotFound();
        }
    }
}
