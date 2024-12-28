using AQS_Application.Dtos.IdentityServiceDto;

namespace AQS_Application.Interfaces.IServices.IdentityServices
{
    public interface IRegistrationService
    {
        Task<ResultRegisterIdentityDto> RegisterAsync(RegisterIdentityDTO registerIdentityDTO, string role);
    }

}
