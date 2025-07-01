using Afisha.Application.DTO.Inputs;
using Afisha.Application.DTO.Outputs;
using Afisha.Application.Enum;
using Afisha.Application.Services.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RabbitMQModels;

namespace Afisha.Web.Controllers;

[ApiController]
[Route("[controller]")]
// [Authorize]
public class EventController(IEventService eventService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<OutputEvent>> Get(long id)
    {
        var eventItem = await eventService.GetEventByIdAsync(id, HttpContext.RequestAborted);
        return Ok(eventItem);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEvent createEvent)
    {
        await eventService.CreateEvent(createEvent, HttpContext.RequestAborted);

        return Ok("Событие создано");
    }

    [HttpGet]
    [Route("filtered-events")]
    public async Task<ActionResult<List<OutputEvent>>> GetEventsByFilter([FromQuery] DateOnly? dateStart, [FromQuery] DateOnly? dateEnd,
        [FromQuery] long? locationId, [FromQuery] long? sponsorId, [FromQuery] OrderByEnum orderBy = OrderByEnum.Default)
    {
        var events = await eventService.GetEventsByFilterAsync(dateStart, dateEnd, HttpContext.RequestAborted,
            locationId, sponsorId, orderBy);

        if (events.Count == 0)
            return NotFound("События не найдены");

        return Ok(events);
    }

    [HttpPost]
    [Route("request-approval")]
    public async Task<ActionResult<string>> RequestApproval([FromQuery] long eventId, long userId)
    {
        var registerMessage = await eventService.RegisterToEventAsync(eventId, userId, HttpContext.RequestAborted);
        return Ok(registerMessage.Reason);
    }
 
}


