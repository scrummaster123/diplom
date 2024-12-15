using Afisha.Domain;
using Afisha.Web.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCoreServices();
builder.Services.AddControllers();

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
        opt.MigrationsAssembly("Afisha.Domain");
        opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    });
    
#if DEBUG
    context.EnableSensitiveDataLogging();
    context.EnableDetailedErrors();
#endif
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
