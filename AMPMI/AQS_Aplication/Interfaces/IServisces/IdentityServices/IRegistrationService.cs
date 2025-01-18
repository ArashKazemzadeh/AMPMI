using AQS_Application.Dtos.IdentityServiceDto;

namespace AQS_Application.Interfaces.IServices.IdentityServices
{
    public interface IRegistrationService
    {
        /// <summary>
        /// بعد از ثبت نام به صورت خودکار لاکین هم میشود
        /// </summary>
        /// <param name="registerIdentityDTO"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<ResultRegisterIdentityDto> RegisterAsync(RegisterIdentityDTO registerIdentityDTO, string role);
        Task<ResultRegisterIdentityDto> ChangePasswordAsync(long userId, string currentPassport, string newPassword);
        Task<ResultRegisterIdentityDto> RegisterAsync(RegisterIdentityDTO registerIdentityDTO, string role, bool isLogin);
        Task<bool> DeleteUserAsync(long userId);
    }

}
