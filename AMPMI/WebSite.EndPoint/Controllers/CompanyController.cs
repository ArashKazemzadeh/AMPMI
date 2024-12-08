using AQS_Aplication.Interfaces.IServisces;
using Domin.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using WebSite.EndPoint.Models.CompanyViewModel;

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
            List<CompanyDetailVM> resultVM = result.Select(x => new CompanyDetailVM 
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
                CompanyDetailVM companyDetailVM = new CompanyDetailVM()
                {
                    LogoRout = result.LogoRout,
                    Address = result.Address,
                    MobileNumber = result.MobileNumber,
                    Name = result.Name,
                    TeaserGuid = result.TeaserGuid
                };
                
                return View(companyDetailVM);
            }

            return NotFound();
        }
    }
}
