using Afisha.Infrastructure.Data;
using System.Reflection;
using System.Text.Json.Serialization;
using Afisha.Infrastructure;
using Afisha.Web.Infrastructure.Configuration;
using Afisha.Web.Middleware;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Можно для соответствия RFC 7807
builder.Services.AddProblemDetails(); 

// Добавление основных сервисов
builder.Services.AddCoreServices();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

//  Регистрация в сервисах RabbitMQ
builder.RegisterRabbitMq();

// Добавление Postgresql
builder.AddPostgres();

// конфигурируем Serilog
builder.ConfigureSerilog();
// включаем использование serilog
builder.Host.UseSerilog();

// Добавление версионирования API
builder.AddApiVersioning();
// Добавление сваггера
builder.AddSwagger();


var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Afisha API v1");
    options.RoutePrefix = string.Empty; // Set the Swagger UI at the root URL
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
// влключаем логгирование запросов
app.UseSerilogRequestLogging();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AfishaDbContext>();
db.Database.Migrate();

app.Run();

