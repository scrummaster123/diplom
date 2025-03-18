using Afisha.Application.DTO.Inputs;

namespace Afisha.Application.Services.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string email, string password, CancellationToken cancellationToken);
        Task<bool> UserRegistrationAsync(RegistrationUserModel userModel, CancellationToken cancellationToken);
    }
}