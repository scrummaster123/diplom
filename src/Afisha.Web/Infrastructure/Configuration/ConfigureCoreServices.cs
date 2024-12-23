using Afisha.Application.Contracts.Repositories;
using Afisha.Application.Services;
using Afisha.Domain;
using Afisha.Domain.Contracts;
using Afisha.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Afisha.Web.Infrastructure.Configuration
{
    public static class ConfigureCoreServices
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddSingleton<IUserSomeActionService, UserSomeActionService>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<ILocationRepository, LocationRepository>();
            return services;

        }
        
        public static WebApplicationBuilder AddPostgres(this WebApplicationBuilder builder)
        {
            var pgsqlHost = builder.Configuration.GetValue<string>("Postgres:Host");
            var pgsqlPort = builder.Configuration.GetValue<string>("Postgres:Port");
            var pgsqlUser = builder.Configuration.GetValue<string>("Postgres:User");
            var pgsqlPassword = builder.Configuration.GetValue<string>("Postgres:Password");
            var pgsqlDb = builder.Configuration.GetValue<string>("Postgres:Database");

            // Формирование строки с данными для подключения к БД
            var connectionString =
                $"Host={pgsqlHost};Port={pgsqlPort};Database={pgsqlDb};Username={pgsqlUser};Password={pgsqlPassword};";

            // Конфигурация подключения к БД ( DB context )
            builder.Services.AddDbContext<AfishaDbContext>(context =>
            {
                context.UseNpgsql(connectionString, opt =>
                {
                    opt.MigrationsAssembly("Afisha.Web");
                    opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
#if DEBUG
                context.EnableSensitiveDataLogging();
                context.EnableDetailedErrors();
#endif
            });

            return builder;
        }
    }
}
