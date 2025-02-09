using Afisha.Domain.Entities;
using Afisha.Domain.Entities.Dto;

namespace Afisha.Application.Services.Interfaces;

public interface IRatingService
{
    /// <summary>
    /// Добавление оценки проведенному мероприятию
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns></returns>
    Task<Rating> AddRating(RatingDto ratingDto, CancellationToken cancellationToken);
    Task<List<RatingDto>> GetRatingsByEvent(long eventId, CancellationToken cancellationToken);
}
