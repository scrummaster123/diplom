using Afisha.Application.DTO.Inputs;
using Afisha.Application.Enum;
using Afisha.Application.Services.Interfaces;
using Afisha.Application.Services.Managers;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RabbitMQModels;
using System.Security.Claims;

namespace Afisha.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class EventController(IEventService eventService, IPublishEndpoint pub) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] long id)
    {
        await pub.Publish(new EmailMessage
        {
            Content = DateTime.Now.ToLongDateString(),
            Email = "user@mail.ri"
        });
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

    [HttpPost]
    [Route("create-map")]
    [Authorize]
    public async Task<IActionResult> CreateOnMapEvent([FromBody] CreateEvent createEvent)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out long userId))
        {
            return Unauthorized("User ID not found in token");
        }
        createEvent.SponsorId = userId;
        await eventService.CreateEvent(createEvent, HttpContext.RequestAborted);

        return Ok("Событие создано");
    }

    [HttpGet]
    [Route("filtered-events")]
    public async Task<IActionResult> GetEventsByFilter([FromQuery] DateOnly dateStart, [FromQuery] DateOnly dateEnd,
        [FromQuery] long? locationId, [FromQuery] long? sponsorId, [FromQuery] OrderByEnum orderBy = OrderByEnum.Default)
    {
        var events = await eventService.GetEventsByFilterAsync(dateStart, dateEnd, HttpContext.RequestAborted,
            locationId, sponsorId, orderBy);

        if (events.Count == 0)
            return NotFound("События не найдены");

        return Ok(events);
    }

    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetEvents([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (page < 1 || pageSize < 1 || pageSize > 20)
        {
            return BadRequest("Invalid page or pageSize. Page must be >= 1, pageSize must be between 1 and 20.");
        }

        var events = await eventService.GetEventsPagedAsync(page, pageSize, HttpContext.RequestAborted);
        var totalCount = await eventService.GetTotalEventsCountAsync(HttpContext.RequestAborted);

        return Ok(new
        {
            events = events.ToArray(),
            totalCount,
            totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        });
    }

    [HttpPost]
    [Route("join")]
    [Authorize]
    public async Task<IActionResult> JoinEvent([FromQuery] long eventId)
    {
        
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out long userId))
        {
            return Unauthorized("User ID not found in token");
        }

        try
        {
            await eventService.JoinEventAsync(eventId, userId, HttpContext.RequestAborted);
            return Ok("Successfully joined the event");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("leave")]
    [Authorize]
    public async Task<IActionResult> LeaveEvent([FromQuery] long eventId)
    {

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out long userId))
        {
            return Unauthorized("User ID not found in token");
        }

        try
        {
            await eventService.LeaveEventAsync(eventId, userId, HttpContext.RequestAborted);
            return Ok("Successfully leave the event");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}


