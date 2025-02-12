using Afisha.Application.Services.Interfaces;
using Afisha.Application.Specifications;
using Afisha.Application.Specifications.Rating;
using Afisha.Domain.Entities;
using Afisha.Domain.Entities.Dto;
using Afisha.Domain.Interfaces;
using Afisha.Domain.Interfaces.Repositories;

namespace Afisha.Application.Services.Managers;

public class RatingService(
    IRepository<Rating, long> ratingRepository,
    IRepository<User, long> userRepository,
    IRepository<Event, long> eventRepository,
    IUnitOfWork unitOfWork
    ) : IRatingService
{
    public async Task<Rating> AddRating(RatingDto ratingDto, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(ratingDto.UserId);
        var myEvent = await eventRepository.GetByIdAsync(ratingDto.EventId);

        if (user is null)
            throw new ArgumentNullException(nameof(user));

        if (myEvent is null)
            throw new ArgumentNullException(nameof(myEvent));

        var rating = ratingRepository.Add(new Rating()
        {
            Value = ratingDto.Value,
            Description = ratingDto.Description,
            Event = myEvent,
            User = user
        });

        await unitOfWork.CommitAsync(cancellationToken);

        return rating;
    }

    public async Task<List<RatingDto>> GetRatingsByEvent(long eventId, CancellationToken cancellationToken)
    {
        var ratings = await ratingRepository.GetAsync(new RatingByEventIdWithUserAndEventSpecification(eventId), cancellationToken: cancellationToken);

        var result = ratings.Select(r => new RatingDto() { Description = r.Description, Value = r.Value, EventId = r.Event.Id, UserId = r.User.Id })
                            .ToList();

        return result;
    }
}