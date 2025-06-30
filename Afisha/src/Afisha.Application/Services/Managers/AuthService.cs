using Afisha.Application.DTO.Inputs;
using Afisha.Application.Services.Interfaces;
using Afisha.Application.Services.Interfaces.Auth;
using Afisha.Domain.Entities;
using Afisha.Domain.Interfaces;
using Afisha.Domain.Interfaces.Repositories;
using Afisha.Infrastructure;
using AutoMapper;

namespace Afisha.Application.Services.Managers
{
    public class AuthService(IUserService userService, IUserRepository userRepository, IJwtProvider jwtProvider, IPasswordHasher passwordHasher, IMapper mapper, IUnitOfWork unitOfWork, HackService hackService) : IAuthService
    {
        public async Task<bool> UserRegistrationAsync(RegistrationUserModel userModel, CancellationToken cancellationToken)
        {
            userModel.Email = userModel.Email.ToLowerInvariant();

            var hashedPassword = passwordHasher.Generate(userModel.Password);

            var userMap = mapper.Map<User>(userModel);

            userMap.PasswordHash = hashedPassword;

            User? user = await userService.GetUserByEmailAsync(userMap.Email.ToLowerInvariant(), cancellationToken);

            if (user != null)
            {
                throw new Exception($"Пользователь с email {userMap.Email.ToLowerInvariant()} уже существует");
            }

            user = await userService.GetUserByLoginAsync(userMap.Login.ToLowerInvariant(), cancellationToken);

            if (user != null)
            {
                throw new Exception($"Пользователь с логином {userMap.Login.ToLowerInvariant()} уже существует");
            }

            var result = await userRepository.AddRegisterUserAsync(userMap, cancellationToken);

            if (result) 
            { 
                await unitOfWork.CommitAsync(cancellationToken);
            }

            return result;
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
            hackService.UserId = user.Id;
            return token;
        }
    }
}
