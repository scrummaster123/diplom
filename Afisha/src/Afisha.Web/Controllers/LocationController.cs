using Afisha.Application.DTO.Inputs;
using Afisha.Application.DTO.Outputs;
using Afisha.Application.Services.Interfaces;
using Afisha.Application.Services.Managers;
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
    [Route("list")]
    public async Task<IActionResult> GetLocations([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (page < 1 || pageSize < 1 || pageSize > 20)
        {
            return BadRequest("Invalid page or pageSize. Page must be >= 1, pageSize must be between 1 and 20.");
        }

        var locations = await locationService.GetLocationsPagedAsync(page, pageSize, HttpContext.RequestAborted);
        var totalCount = await locationService.GetTotalLocationsCountAsync(HttpContext.RequestAborted);

        return Ok(new
        {
            Locations = locations.ToArray(),
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        });
    }

    [HttpGet]
    [Route("map")]
    public IActionResult GetMap()
    {
        return Ok(new
        {
            MapUrl = "images/map.jpg",
            MapWidth = 1024, // Pixel width of map.png
            MapHeight = 800 // Pixel height of map.png
        });
    }

    [HttpGet]
    [Route("map-locations")]
    public async Task<IActionResult> GetLocations()
    {
        var locations = await locationService.GetAllAsync(HttpContext.RequestAborted);
        return Ok(locations);
    }

}