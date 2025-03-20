using Afisha.Application;
using Afisha.Application.Mappers;
﻿using Afisha.Application.Services;
using Afisha.Application.Services.Interfaces;
using Afisha.Application.Services.Interfaces.Auth;
using Afisha.Application.Services.Managers;
using Afisha.Domain.Interfaces;
using Afisha.Domain.Interfaces.Repositories;
using Afisha.Infrastructure;
using Afisha.Infrastructure.Data;
using Afisha.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Afisha.Web.Infrastructure.Configuration;

public static class ConfigureCoreServices
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IReadRepository<,>), typeof(ReadRepository<,>));
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEventService, EventService>();
        services.AddSingleton<AutoMapperConfiguration>();
        services.AddScoped<IRatingService, RatingService>();
        services.AddScoped<IUserSomeActionService, UserSomeActionService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtOptions, JwtOptions>();
        services.AddScoped<IJwtProvider, JwtProvider>();
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
        builder.Services.AddDbContext<IUnitOfWork, AfishaDbContext>(context =>
        {
            context.UseNpgsql(connectionString, opt =>
            {
                opt.MigrationsAssembly("Afisha.Infrastructure");
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