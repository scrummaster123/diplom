using Afisha.Infrastructure.Data;
using Afisha.Infrastructure;
using Afisha.Web.Infrastructure.Configuration;
using Afisha.Web.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; 
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Добавление основных сервисов
builder.Services.AddCoreServices();
builder.Services.RegisterMapperProfiles();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

// Add authentication with JWT Bearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtOptions:Issuer"],
        ValidAudience = builder.Configuration["JwtOptions:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is missing")))
    };
});

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
app.UseStaticFiles(); // Serve static files (e.g., map.png)
// Add authentication middleware before authorization
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AfishaDbContext>();
db.Database.Migrate();

app.Run();