using Afisha.Application.DTO.Elastics;
using Afisha.Application.DTO.Inputs;
using Afisha.Application.DTO.Outputs;
using Afisha.Application.Services.Interfaces;
using Afisha.Application.Specifications.Location;
using Afisha.Domain.Entities;
using Afisha.Domain.Interfaces;
using Afisha.Domain.Interfaces.Repositories;

namespace Afisha.Application.Services.Managers;

public class LocationService(
    IRepository<Location, long> locationRepository,
    IRepository<User, long> userRepository,
    IElasticService elasticService,
    IUnitOfWork unitOfWork) : ILocationService
{
    /// <summary>
    ///     Получение общей информации о локации по идентификатору
    /// </summary>
    public async Task<OutputLocationFull> GetLocationByIdAsync(long id, CancellationToken cancellationToken)
    {
        var location = await locationRepository.GetByIdAsync(
            id,
            new LocationWithOwnerAndEventsSpecification(),
            cancellationToken: cancellationToken);

        if (location == null)
            throw new Exception($"Локация с идентификатором {id} не найдена");

        // Маппинг сущности локации из модели
        var outputLocation = new OutputLocationFull
        {
            Name = location.Name,
            Pricing = location.Pricing,
            IsWarmPlace = location.IsWarmPlace,
            Events = location.Events?.Select(x => x.DateStart.ToString()).ToList() ?? [],
            OwnerId = location.OwnerId
        };

        return outputLocation;
    }

    /// <summary>
    ///     Создание новой локации
    /// </summary>
    public async Task<OutputLocationBase> CreateLocation(CreateLocationModel location,
        CancellationToken cancellationToken)
    {
        // TODO Метод ожидает изменений с приходом отдельных моделей и создания токенов для дальнейшего получения владельца
        var owner = await userRepository.GetByIdOrThrowAsync(location.UserId, cancellationToken: cancellationToken);


        // Преобразование модели локации в сущность для базы данных
        // TODO нужно будет подумать: используем ли библиотеку для маппинга сущностей
        var newLocation = new Location
        {
            Name = location.Name,
            Owner = owner,
            IsWarmPlace = location.IsWarmPlace,
            Pricing = location.Pricing
        };

        var addedLocation = locationRepository.Add(newLocation);

        // Если удалось записать сущность в базу данных, то переменная будет > 0
        var affectedRows = await unitOfWork.CommitAsync(cancellationToken);
        if (affectedRows > 0)
        {
            // TODO нужно будет подумать: используем ли библиотеку для маппинга сущностей
            var mappedLocation = new OutputLocationBase
            {
                Name = addedLocation.Name,
                Pricing = addedLocation.Pricing,
                IsWarmPlace = addedLocation.IsWarmPlace,
                OwnerId = addedLocation.OwnerId
            };

            await elasticService.WriteAsync(new ElasticLocation { Id = addedLocation.Id, Date = addedLocation.Name });

            return mappedLocation;
        }

        // В случае, если верхний if не отработал, выбрасывается исключение с общим описанием для пользователя
        throw new Exception("Не удалось добавить локацию");
    }

    public async Task<IEnumerable<OutputLocationBase>> GetBySearchString(string request)
    {
        var elasticLocations = await elasticService.GetAsync(request);

        var locations = new List<OutputLocationBase>();
        foreach (var elasticLocation in elasticLocations)
        {
            var location = await GetLocationByIdAsync(elasticLocation.Id, new CancellationToken());
            locations.Add(location);
        }

        return locations;
    }
}