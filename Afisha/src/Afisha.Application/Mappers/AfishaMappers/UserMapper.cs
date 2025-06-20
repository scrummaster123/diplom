using Afisha.Application.DTO.Inputs;
using Afisha.Application.DTO.Outputs;
using Afisha.Domain.Entities;
using AutoMapper;

namespace Afisha.Application.Mappers.AfishaMappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            // Маппинг RegistrationUserModel в User
            CreateMap<RegistrationUserModel, User>()

                .ForMember(
                dest => dest.PasswordHash, // Пароль будет хешироваться отдельно
                opt => opt.Ignore()
                )

                .ForMember(
                dest => dest.Locations, // Игнорю сущности, потому что при регистрации их еще нет
                opt => opt.Ignore()
                )

                .ForMember(
                dest => dest.Events, // Игнорю сущности, потому что при регистрации их еще нет
                opt => opt.Ignore()
                );


            // Маппинг User в OutputMiniUserModel
            CreateMap<User, OutputMiniUserModel>()

                .ForMember(dest => dest.FullName,
                opt =>
                    opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Id,
                    opt =>
                        opt.MapFrom(src => src.Id))
                ; // Здесь поле мини-модели FullName составляю из полей Имя/Фамилия сущности User


            // Маппинг User в OutputFullUserModel
            CreateMap<User, OutputFullUserModel>()

                .ForMember(
                dest => dest.Age,
                opt => opt.MapFrom(src => src.Birthday.HasValue ? GetAge(src.Birthday.Value) : (int?)null) // Мапплю дату в возраст или если после пустое ничего не возвращаю 
                )

                .ForMember(
                dest => dest.Locations, // ------ (?) Здесь необходимо будет замаппить в другие модели -------
                opt => opt.MapFrom(src => src.Locations)
                )

                .ForMember(
                dest => dest.Events, // ------ (?) Здесь необходимо замаппить в другие модели -------
                opt => opt.MapFrom(src => src.Events)
                );
        }

        /// <summary>
        ///     Метод для преобразования возраста в выходной модели пользователя (можно будет вынести в отдельный класс инструментария, возможно)
        /// </summary>
        private int GetAge(DateTime birthday)
        {
            var today = DateTime.Today;
            var age = today.Year - birthday.Year;

            if (today < birthday.AddYears(age))
            {
                age--;
            }
            return age;
        }
    }
}
