using Afisha.Application.Services.Interfaces;
using Afisha.Application.Specifications;
using Afisha.Domain.Entities;
using Afisha.Domain.Entities.Dto;
using Afisha.Domain.Interfaces.Repositories;

namespace Afisha.Application.Services.Managers;

public class RatingService(
    IRepository<Rating, long> ratingRepository
    ) : IRatingService
{
    public async Task<Rating> AddRating(RatingDto ratingDto, CancellationToken cancellationToken)
    {
        var rating = ratingRepository.Add(new Rating()
        {
            Value = ratingDto.Value,
            Description = ratingDto.Description,
            Event = null,
            User = null
        });

        return rating;
    }

    public Task<List<RatingDto>> GetRatingsByEvent(long eventId, CancellationToken cancellationToken)
    {
        var result = ratingRepository.GetAsync(new SpecificationBase<Rating>(rating => rating.Event.Id == eventId), cancellationToken: cancellationToken);

        throw new NotImplementedException();
    }
}