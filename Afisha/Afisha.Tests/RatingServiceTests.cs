using Afisha.Application.Services.Managers;
using Afisha.Domain.Entities;
using Afisha.Domain.Entities.Dto;
using Afisha.Domain.Enums;
using Afisha.Domain.Interfaces;
using Afisha.Domain.Interfaces.Repositories;
using Afisha.Domain.Interfaces.Specifications;
using Moq;

namespace Afisha.Tests;

public class RatingServiceTests
{
    [Fact]
    public void AddRaiting()
    {
        /// мокаем объекты
        var mockRepo = new Mock<IRepository<Rating, long>>();
        var mockUser = new Mock<IRepository<User, long>>();
        var mockEvent = new Mock<IRepository<Event, long>>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        /// задаем поведение для мок-объекта - для любого UserId  GetByIdAsync вернет null
        mockUser.Setup(r => r.GetByIdAsync(
                It.IsAny<long>(),
                It.IsAny<IIncludeSpecification<User>>(),
                It.IsAny<TrackingType>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        var ratingService = new RatingService(mockRepo.Object,
                                              mockUser.Object,
                                              mockEvent.Object,
                                              mockUnitOfWork.Object);

        var ratingDto = new RatingDto();
        Assert.ThrowsAsync<ArgumentNullException>(() => ratingService.AddRating(ratingDto, CancellationToken.None));
    }
}