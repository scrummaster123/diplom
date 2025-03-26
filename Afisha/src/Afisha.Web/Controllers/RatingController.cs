using Afisha.Application.Services.Interfaces;
using Afisha.Domain.Entities.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Afisha.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class RatingController(IRatingService ratingService) : Controller
{
    [HttpPost]
    [Route("AddRating")]
    public async Task<IActionResult> AddRating([FromBody] RatingDto ratingDto)
    {
        if (ratingDto is null)
            throw new BadHttpRequestException(nameof(ratingDto));

        if (!(1 <= ratingDto.Value && ratingDto.Value <= 5))
            throw new BadHttpRequestException("Значение рейтинга вне диапазона [0..5]");

        await ratingService.AddRating(ratingDto, HttpContext.RequestAborted);

        return Ok("Ваш отзыв успешно опубликован");
    }

    [HttpGet]
    [Route("GetRatingsByEvent")]
    public async Task<IActionResult> GetRatingsByEvent(long eventId)
    {
        var ratings = await ratingService.GetRatingsByEvent(eventId, HttpContext.RequestAborted);

        return Ok(ratings);
    }
}
