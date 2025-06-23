using Afisha.Application.Mappers;
using Afisha.Application.Services.Managers;
using Afisha.Domain.Entities;
using Afisha.Domain.Interfaces;
using Afisha.Domain.Interfaces.Repositories;
using AutoMapper;
using Moq;

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
        var service = new EventService(mockRepo.Object,
                                        mockUnitOfWork.Object,
                                        mockAutoMapperConfig.Object,
                                        mockEventsRepo.Object,
                                        mockMapper.Object);
        /// вызываем метод с параметрами, которые вызывают ArgumentException. ∆дем ArgumentException
        Assert.ThrowsAsync<ArgumentException>(() => service.GetEventsByFilterAsync(DateOnly.MaxValue, DateOnly.MinValue, CancellationToken.None));
    }
}