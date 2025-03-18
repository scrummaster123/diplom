using Afisha.Application.DTO.Inputs;

namespace Afisha.Application.Services.Interfaces.Auth
{
    public interface IRegistrationService
    {
        Task UserRegistrationAsync(RegistrationUserModel userModel);
    }
}