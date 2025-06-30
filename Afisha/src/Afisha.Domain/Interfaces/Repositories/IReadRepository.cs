using Afisha.Application.Contracts.Specifications;
using Afisha.Domain.Entities.Abstractions;
using Afisha.Domain.Enums;
using Afisha.Domain.Interfaces.Specifications;

namespace Afisha.Domain.Interfaces.Repositories;
public interface IReadRepository<T, TKey>
    where T : EntityBase<TKey>
    where TKey : struct
{
    Task<T?> GetByIdAsync(
    TKey id,
    IIncludeSpecification<T>? specification = null,
    TrackingType trackingType = TrackingType.Tracking,
    CancellationToken cancellationToken = default);

    Task<T> GetByIdOrThrowAsync(
        TKey id,
        IIncludeSpecification<T>? specification = null,
        TrackingType trackingType = TrackingType.Tracking,
        CancellationToken cancellationToken = default);

    Task<T[]> GetAsync(
        ISpecification<T> specification,
        TrackingType trackingType = TrackingType.Tracking,
        CancellationToken cancellationToken = default);

    Task<T[]> GetPagedAsync(
        ISpecification<T> specification,
        TrackingType trackingType = TrackingType.Tracking,
        int pageIndex = 0,
        int pageSize = 20,
        CancellationToken cancellationToken = default);

    Task<int> GetTotalCountAsync(
        CancellationToken cancellationToken = default);
}