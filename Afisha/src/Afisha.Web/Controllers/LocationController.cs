using Afisha.Application.Services.Interfaces;
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
        
        return await locationService.GetLocationByIdAsync(id, HttpContext.RequestAborted);
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