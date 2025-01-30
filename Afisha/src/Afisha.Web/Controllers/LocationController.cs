using System.Transactions;
using Afisha.Domain.Contracts;
using Afisha.Domain.Entities;
using Afisha.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Afisha.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationController(ILocationService locationService,  IServiceProvider services) : ControllerBase
{
    private IServiceProvider Services { get; } = services;
    [HttpGet]
    public async Task<Location> Get([FromQuery] long id)
    {
        
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {

            List<long> ids = new List<long>();
            // ids.Add(await GetTest());
            await Delete();
            // Подтверждение транзакции
            // scope.Complete();
        }
        
        
        return await locationService.GetLocationByIdAsync(id, HttpContext.RequestAborted);
    }
    
    private async Task Delete()
    {
        
        // Получение контекста базы данных из сервисов коллекций
        await using var scope = Services.CreateAsyncScope();
        await using var applicationContext = scope.ServiceProvider.GetRequiredService<AfishaDbContext>();
        
        var test = await applicationContext.Locations.FirstOrDefaultAsync();
        applicationContext.Locations.Remove(test);
        await applicationContext.SaveChangesAsync();
    }

    private async Task<long> GetTest()
    {
        await using var scope = Services.CreateAsyncScope();
        await using var applicationContext = scope.ServiceProvider.GetRequiredService<AfishaDbContext>();

        var test = applicationContext.Locations.Add(new Location
        {
            IsWarmPlace = true,
            Name = "Test",
            Owner = new User()
        });

        await applicationContext.SaveChangesAsync();

        return test.Entity.Id;
    }

    [HttpPost]
    [Route("create")]
    public async Task<Location> CreateLocation([FromBody] Location newLocation)
    {
        //TODO Нужно будет добавить в инфрасруктуру какой-нибудь сервис-экстрактор токена
        var ownerId = newLocation.Owner.Id;

        var result = await locationService.CreateLocation(newLocation, ownerId, HttpContext.RequestAborted);
        return result;
    }
}