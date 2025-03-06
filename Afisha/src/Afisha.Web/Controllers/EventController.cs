using Afisha.Application.DTO;
using Afisha.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Afisha.Web.Controllers;

[ApiController]
[Route("[controller]")]

public class EventController(IEventService eventService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] long id)
    {
        var eventItem = await eventService.GetEventyByIdAsync(id, HttpContext.RequestAborted);
        return Ok(eventItem);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEvent createEvent)
    {
        await eventService.CreateEvent(createEvent, HttpContext.RequestAborted);
        return Ok("Событие создано");
    }
}


