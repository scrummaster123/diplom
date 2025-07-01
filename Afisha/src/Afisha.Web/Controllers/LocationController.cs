using Afisha.Application.DTO.Inputs;
using Afisha.Application.DTO.Outputs;
using Afisha.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Afisha.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationController(ILocationService locationService, IServiceProvider services) : ControllerBase
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

    [HttpGet]
    [Route("search")]
    public async Task<IEnumerable<OutputLocationBase>> Search([FromQuery] string search) =>
        await locationService.GetBySearchString(search);

    [HttpGet]
    [Route("all")]
    public async Task<ActionResult<OutputLocationBase>> GetLocations()
    {
        var result = await locationService.GetLocations(HttpContext.RequestAborted);
        return Ok(result);
    }
}