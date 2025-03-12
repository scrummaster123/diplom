using Afisha.Application.DTO.Inputs;
using Afisha.Application.Services.Interfaces.Auth;
using Afisha.Domain.Entities;
using AutoMapper;

namespace Afisha.Application.Services

{
    public class RegistrationService : IRegistrationService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public RegistrationService(IPasswordHasher passwordHasher, IMapper mapper)
        {
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }
        public async Task<> RegisterUserAsync(RegistrationUserModel userDto)
        {            
            var hasedPassword = _passwordHasher.Generate(userDto.Password);

            // Маппинг модели в сущность
            var user = _mapper.Map<User>(registrationModel);

            // Хеширование пароля
            user.PasswordHash = _passwordHasher.HashPassword(registrationModel.Password);

            // Сохранение пользователя в базе данных
            await _userRepository.AddAsync(user);

            // Возвращаем успешный результат
            return new OperationResult(true, "Пользователь успешно зарегистрирован.");
        }
    }
}
