using Afisha.Application.Services;
using Afisha.Domain.Contracts;

namespace Afisha.Web.Infrastructure.Configuration
{
    public static class ConfigureCoreServices
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddSingleton<IUserSomeActionService, UserSomeActionService>();
            return services;

        }
    }
}
