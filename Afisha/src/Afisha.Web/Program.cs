using Afisha.Infrastructure.Data;
using Afisha.Infrastructure;
using Afisha.Web.Infrastructure.Configuration;
using Afisha.Web.Middleware;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

// Добавление основных сервисов
builder.Services.AddCoreServices();
builder.Services.RegisterMapperProfiles();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

// Регистрация RabbitMQ
builder.RegisterRabbitMq();

// Добавление PostgreSQL
builder.AddPostgres();

// Конфигурируем Serilog
builder.ConfigureSerilog();
builder.Host.UseSerilog();

// Добавление версионирования API
builder.AddApiVersioning();
// Добавление Swagger
builder.AddSwagger();

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Afisha API v1");
    options.RoutePrefix = string.Empty;
});

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AfishaDbContext>();
db.Database.Migrate();

app.Run();