using AQS_Aplication.Dtos.IdentityServiceDto;

namespace AQS_Aplication.Interfaces.IServisces.IdentityServices
{
    public interface IRegistrationService
    {
        Task<ResultRegisterIdentityDto> RegisterAsync(RegisterIdentityDTO registerIdentityDTO, string role);
    }

}
