using Afisha.Application;
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
    public async Task<OutputLocationFull> Get([FromQuery] long id)
    {
        
        return await locationService.GetLocationByIdAsync(id, HttpContext.RequestAborted);
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<OutputLocationBase> CreateLocation([FromBody] CreateLocationModel newLocation)
    {
        //TODO Нужно будет добавить в инфрасруктуру какой-нибудь сервис-экстрактор токена

        var result = await locationService.CreateLocation(newLocation, HttpContext.RequestAborted);
        return result;
    }
}