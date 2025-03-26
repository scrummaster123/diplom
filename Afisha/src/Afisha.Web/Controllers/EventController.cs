using Afisha.Application.DTO.Inputs;
using Afisha.Application.Services.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using RabbitMQModels;

namespace Afisha.Web.Controllers;

[ApiController]
[Route("[controller]")]

public class EventController(IEventService eventService, IPublishEndpoint pub, ILogger<EventController> logger) : ControllerBase
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
        logger.LogInformation($"Create Event: {createEvent.LocationId} {createEvent.SponsorId} {createEvent.DateStart}");

        await eventService.CreateEvent(createEvent, HttpContext.RequestAborted);
        
        return Ok("Событие создано");
    }
}


