using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Application.Interfaces.IServices.IdentityServices;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Models.ViewComponents;

namespace WebSite.EndPoint.ViewComponents
{
    public class ProfileViewComponent : ViewComponent
    {
        private readonly ILoginService _loginService;
        private readonly ICompanyService _companyService;
        public ProfileViewComponent(ILoginService loginService,ICompanyService companyService)
        {
            this._loginService = loginService;
            this._companyService = companyService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            long companyId = await _loginService.GetUserIdAsync(UserClaimsPrincipal);
            ProfileVM profileVM = new ProfileVM();
            if (companyId > 0)
            {
                var company = await _companyService.ReadById(companyId);
                profileVM.IsLogin = true;
                profileVM.UserId = companyId;
                profileVM.UserName = company?.Name;
            }

            return View(profileVM);
        }
    }
}
