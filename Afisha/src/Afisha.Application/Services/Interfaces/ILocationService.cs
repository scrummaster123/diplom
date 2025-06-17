using Afisha.Application.DTO.Inputs;
using Afisha.Application.DTO.Outputs;
using Afisha.Domain.Entities;

namespace Afisha.Application.Services.Interfaces;
public interface ILocationService
{
    /// <summary>
    ///     Получение общей информации о локации по идентификатору
    /// </summary>
    Task<OutputLocationFull> GetLocationByIdAsync(long id, CancellationToken cancellationToken);

    /// <summary>
    ///     Создание новой локации
    /// </summary>
    Task<OutputLocationBase> CreateLocation(CreateLocationModel location, CancellationToken cancellationToken);

    /// <summary>
    /// Поиск локаций в Elastic по примерному описанию
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    Task<IEnumerable<OutputLocationBase>> GetBySearchString(string search);
}
