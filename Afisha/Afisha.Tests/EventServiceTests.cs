using Afisha.Application.Mappers;
using Afisha.Application.Services.Managers;
using Afisha.Application.Services.Managers.EventRegistration;
using Afisha.Domain.Interfaces;
using Afisha.Domain.Interfaces.Repositories;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Event = Afisha.Domain.Entities.Event;

namespace Afisha.Tests;

public class EventServiceTests
{
    [Fact]
    public void FilterValidation()
    {
        /// закрываем зависимости заглушками
        var mockRepo = new Mock<IRepository<Event, long>>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockAutoMapperConfig = new Mock<AutoMapperConfiguration>();
        var mockEventsRepo = new Mock<IEventRepository>();
        var mockMapper = new Mock<IMapper>();
        /// создаем экземпл€р сервиса
        var mockUserRepo = new Mock<IUserRepository>();
        var mockEventRule = new Mock<IEventRegistrationRule>();
        var mockLogger = new Mock<ILogger<EventService>>();
        var mockPublishEndpoint = new Mock<IPublishEndpoint>();

        var service = new EventService(
            mockRepo.Object,
            mockUnitOfWork.Object,
            mockAutoMapperConfig.Object,
            mockEventsRepo.Object,
            mockUserRepo.Object,
            mockMapper.Object,
            mockEventRule.Object,
            mockLogger.Object,
            mockPublishEndpoint.Object);

        /// вызываем метод с параметрами, которые вызывают ArgumentException. ∆дем ArgumentException
        Assert.ThrowsAsync<ArgumentException>(() => service.GetEventsByFilterAsync(DateOnly.MaxValue, DateOnly.MinValue, CancellationToken.None));
    }
}