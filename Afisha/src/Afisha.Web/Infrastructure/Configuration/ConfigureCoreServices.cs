using System.Reflection;
using System.Text.Json.Serialization;
using Afisha.Application;
using Afisha.Application.Mappers;
using Afisha.Application.Mappers.UserMappper;
using Afisha.Application.Services;
using Afisha.Application.Services.Interfaces;
using Afisha.Application.Services.Interfaces.Auth;
using Afisha.Application.Services.Managers;
using Afisha.Application.Services.Managers.EventRegistration;
using Afisha.Domain.Interfaces;
using Afisha.Domain.Interfaces.Repositories;
using Afisha.Infrastructure;
using Afisha.Infrastructure.Data;
using Afisha.Infrastructure.Data.Repositories;
using Asp.Versioning;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.OpenApi.Models;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using static System.Int32;

namespace Afisha.Web.Infrastructure.Configuration;

public static class ConfigureCoreServices
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IReadRepository<,>), typeof(ReadRepository<,>));
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEventService, EventService>();
        services.AddSingleton<AutoMapperConfiguration>();
        services.AddScoped<IRatingService, RatingService>();
        services.AddScoped<IUserSomeActionService, UserSomeActionService>();
        services.AddScoped<IEventRepository, EventRepository>();

        services.AddScoped<IEventRegistrationRule, EventRegistrationBaseRule>();
        
        services.AddAutoMapper(typeof(UserMapper));

        
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            options.JsonSerializerOptions.MaxDepth = 0;
        }); 
        
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtOptions, JwtOptions>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        return services;
    }

    public static IServiceCollection RegisterMapperProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(UserMapper));
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
    
    public static WebApplicationBuilder RegisterRabbitMq(this WebApplicationBuilder builder)
    {
        
        var rabbitHost = builder.Configuration.GetValue<string>("RabbitMQ:Host");
        var result = TryParse(builder.Configuration.GetValue<string>("RabbitMQ:Port"), out var rabbitPort);
        var rabbitUser = builder.Configuration.GetValue<string>("RabbitMQ:User");
        var rabbitPassword = builder.Configuration.GetValue<string>("RabbitMQ:Password");
        var rabbitVirtualHost = builder.Configuration.GetValue<string>("RabbitMQ:VirtualHost");

        
        builder.Services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitHost,port: (ushort)(result ? rabbitPort : 5672), rabbitVirtualHost,"", h =>
                {
                    h.Username(rabbitUser);
                    h.Password(rabbitPassword);
                });
                cfg.ConfigureEndpoints(context);
            });
        });
        return builder;
    }
    
    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Afisha API", Version = "v1" });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //options.IncludeXmlComments(xmlPath);
        });
        return builder;
    }
    
    public static WebApplicationBuilder AddApiVersioning(this WebApplicationBuilder builder)
    {
        
        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });
        return builder;
    }

    /// <summary>
    /// Конфигурирование Serilog
    /// </summary>
    /// <param name="builder"></param>
    public static void ConfigureSerilog(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Error()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
            {
                IndexFormat = "logs-{0:yyyy.MM.dd}",
                AutoRegisterTemplate = true,
                NumberOfShards = 2,
                NumberOfReplicas = 1
            })
            .CreateLogger();
    }
}