using Afisha.Application.DTO.Inputs;
using Afisha.Application.Services.Interfaces;
using Afisha.Application.Services.Interfaces.Auth;
using Afisha.Domain.Entities;
using Afisha.Infrastructure;
using AutoMapper;

namespace Afisha.Application.Services.Managers
{
    public class RegistrationService(IUserService userService, IPasswordHasher passwordHasher, IMapper mapper) : IRegistrationService
    {
        public async Task UserRegistrationAsync(RegistrationUserModel userModel, CancellationToken cancellationToken)
        {
            var hashedPassword = passwordHasher.Generate(userModel.Password);

            var userMap = mapper.Map<User>(userModel);

            userMap.PasswordHash = hashedPassword;
        }
    }
}
