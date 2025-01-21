using AQS_Application.Dtos.BaseServiceDto.Company;
using AQS_Application.Interfaces.IServices.BaseServices;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebSite.EndPoint.Models;
using WebSite.EndPoint.Models.HomeViewModel;

namespace WebSite.EndPoint.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly ICompanyService _companyService;
        private readonly IBlogService _blogService;
        private readonly IBannerService _bannerService;

        public HomeController
            (
                ILogger<HomeController> logger,
                ICategoryService categoryService,
                ICompanyService companyService,
                IBlogService blogService,
                IBannerService bannerService
            )
        {
            _logger = logger;
            _categoryService = categoryService;
            _companyService = companyService;
            _blogService = blogService;
            _bannerService = bannerService;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM();

            var categories = await _categoryService.ReadAll();
            homeVM.Categories = categories;

            var companies = await _companyService.ReadConfirmedComapanies();
            homeVM.Companies = companies.Select(x => new CompanyReadDto()
            {
                Id = x.Id,
                Logo = x.LogoRout,
                Name = x.Name,
            }).ToList();

            var blogs = await _blogService.ReadTop3();
            foreach (var item in blogs)
            {
                int end = item.Description.IndexOf("</");
                int start = 0;
                for (int i = end; i > 0; i--)
                {
                    if (item.Description[i] == '>')
                    {
                        start = i;
                        break;
                    }
                }
                if (start > 10 && end > 20)
                {
                    item.Description = item.Description.Substring(start + 1, end - start - 1);
                    // شاید در آینده استفاده شود
                    //if (item.Description.Length > 200)
                    //{
                    //    item.Description = item.Description.Substring(0,200);   
                    //}
                }
                else
                    item.Description = string.Empty;
            }
            homeVM.Blogs = blogs;

            var banners = await _bannerService.ReadAll();
            homeVM.Banners = banners;

            return await Task.FromResult<IActionResult>(View(homeVM));
        }

        public Task<IActionResult> Privacy()
        {
            return Task.FromResult<IActionResult>(View());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public Task<IActionResult> Error()
        {
            return Task.FromResult<IActionResult>(View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }));
        }
    }
}
