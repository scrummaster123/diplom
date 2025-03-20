using Afisha.Application.DTO.Inputs;
using Afisha.Application.Services.Interfaces;
using Afisha.Application.Services.Interfaces.Auth;
using Afisha.Domain.Entities;
using Afisha.Domain.Interfaces.Repositories;
using Afisha.Infrastructure;
using AutoMapper;

namespace Afisha.Application.Services.Managers
{
    public class AuthService(IUserService userService, IUserRepository userRepository, IJwtProvider jwtProvider, IPasswordHasher passwordHasher, IMapper mapper) : IAuthService
    {
        public async Task<bool> UserRegistrationAsync(RegistrationUserModel userModel, CancellationToken cancellationToken)
        {
            userModel.Email = userModel.Email.ToLowerInvariant();

            var hashedPassword = passwordHasher.Generate(userModel.Password);

            var userMap = mapper.Map<User>(userModel);

            userMap.PasswordHash = hashedPassword;

            if (userService.GetUserByEmailAsync(userMap.Email.ToLowerInvariant(), cancellationToken) != null)
            {
                throw new Exception($"Пользователь с email {userMap.Email.ToLowerInvariant()} уже существует");
            }

            if (userService.GetUserByLoginAsync(userMap.Login.ToLowerInvariant(), cancellationToken) != null)
            {
                throw new Exception($"Пользователь с логином {userMap.Login.ToLowerInvariant()} уже существует");
            }

            return await userRepository.AddRegisterUserAsync(userMap, cancellationToken);
        }

        public async Task<string> LoginAsync(string email, string password, CancellationToken cancellationToken)
        {
            var user = await userService.GetUserByEmailAsync(email.ToLowerInvariant(), cancellationToken);

            if (user is null)
            {
                throw new Exception($"Пользователь с email {email.ToLowerInvariant()} не существует");
            }

            var resultPasswordVerify = passwordHasher.Verify(password, user.PasswordHash); 

            if (resultPasswordVerify == false)
            {
                throw new Exception("Неверный пароль");
            }

            var token = jwtProvider.GenerateToken(user);

            return token;
        }
    }
}
