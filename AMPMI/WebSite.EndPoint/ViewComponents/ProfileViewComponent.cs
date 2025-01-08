using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Application.Interfaces.IServices.IdentityServices;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Models.ViewComponents;

namespace WebSite.EndPoint.ViewComponents
{
    public class ProfileViewComponent : ViewComponent
    {
        private readonly ILoginService _loginService;
        public ProfileViewComponent(ILoginService loginService,ICompanyService companyService)
        {
            this._loginService = loginService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            long companyId = await _loginService.GetUserIdAsync(UserClaimsPrincipal);
            ProfileVM profileVM = new ProfileVM();
            if (companyId > 0)
            {
//Arash-UserList-20250108
                //var company = await _companyService.ReadByIdAsync(companyId);
//=======
//Developers
                profileVM.IsLogin = true;
                profileVM.UserId = companyId;
                profileVM.UserName = await _loginService.GetManagerNameByClaims(UserClaimsPrincipal);
            }

            return View(profileVM);
        }
    }
}
