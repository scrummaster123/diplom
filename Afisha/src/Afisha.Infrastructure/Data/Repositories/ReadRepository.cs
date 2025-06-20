using Afisha.Application.Contracts.Specifications;
using Afisha.Domain.Entities.Abstractions;
using Afisha.Domain.Enums;
using Afisha.Domain.Interfaces.Repositories;
using Afisha.Domain.Interfaces.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Afisha.Infrastructure.Data.Repositories;
public class ReadRepository<T, TKey> : IReadRepository<T, TKey>
    where T : EntityBase<TKey>
    where TKey : struct
{
    private protected AfishaDbContext _context;
    private protected DbSet<T> _dbSet;

    public ReadRepository(AfishaDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    /// <summary>
    /// Получает <see cref="{T}[]"/> основываясь на спецификации. По-умолчанию работает, как tracking запрос.
    /// </summary>
    /// <param name="specification">Спецификация</param>
    /// <param name="trackingType"><c>NoTracking</c> для отключения кеширования; <c>Tracking</c> для включения кеширования; <c>NoTrackingWithIdentityResolution</c> для отключения кеширования, но с вычислением одинаковых сущностей в результате запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns><see cref="{T}[]"/>, который содержит элементы, соответствующие условию, переданному в <paramref name="specification"/></returns>
    public async Task<T[]> GetAsync(ISpecification<T> specification, TrackingType trackingType = TrackingType.Tracking, CancellationToken cancellationToken = default)
    {
        var query = specification.BuildQueryable(GetTrackingConfiguredQuery(trackingType));
        return await query.ToArrayAsync(cancellationToken);
    }

    /// <summary>
    /// Получает <see cref="{T}[]"/> основываясь на спецификации и пагинации. По-умолчанию работает, как tracking запрос.
    /// </summary>
    /// <param name="specification">Спецификация</param>
    /// <param name="trackingType"><c>NoTracking</c> для отключения кеширования; <c>Tracking</c> для включения кеширования; <c>NoTrackingWithIdentityResolution</c> для отключения кеширования, но с вычислением одинаковых сущностей в результате запроса</param>
    /// <param name="pageIndex">Индекс страницы</param>
    /// <param name="pageSize">Размер страницы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns><see cref="{T}[]"/>, который содержит элементы, соответствующие условию, переданному в <paramref name="specification"/></returns>
    /// <exception cref="ArgumentException">Ошибка указания параметров пагинации запроса</exception>
    public async Task<T[]> GetPagedAsync(ISpecification<T> specification, TrackingType trackingType = TrackingType.Tracking, int pageIndex = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        if (pageIndex < 1)
            throw new ArgumentException("Индекс страницы должен быть больше, либо равен 1");
        if (pageSize < 1)
            throw new ArgumentException("Размер страницы должен быть больше, либо равен 1");
        int skip = (pageIndex - 1) * pageSize;

        var query = specification.BuildQueryable(GetTrackingConfiguredQuery(trackingType));

        return await query
            .Skip(skip)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);
    }

    /// <summary>
    /// Получает <see cref="{T?}"/> основываясь Id сущности и include делегате. По-умолчанию работает, как tracking запрос.
    /// </summary>
    /// <param name="id">Id искомой сущности</param>
    /// <param name="specification">Спецификация</param>
    /// <param name="trackingType"><c>NoTracking</c> для отключения кеширования; <c>Tracking</c> для включения кеширования; <c>NoTrackingWithIdentityResolution</c> для отключения кеширования, но с вычислением одинаковых сущностей в результате запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns><see cref="{T?}"/> с Id, переданным в параметре <paramref name="id"/></returns>
    public async Task<T?> GetByIdAsync(TKey id, IIncludeSpecification<T>? specification = null, TrackingType trackingType = TrackingType.Tracking, CancellationToken cancellationToken = default)
    {
        var query = GetTrackingConfiguredQuery(trackingType);
        if (specification != null)
            query = specification.BuildQueryable(query);
        return await query.FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
    }

    /// <summary>
    /// Получает <see cref="{T}"/> основываясь Id сущности и спецификации. По-умолчанию работает, как tracking запрос.
    /// </summary>
    /// <param name="id">Id искомой сущности</param>
    /// <param name="specification">Спецификация</param>
    /// <param name="trackingType"><c>NoTracking</c> для отключения кеширования; <c>Tracking</c> для включения кеширования; <c>NoTrackingWithIdentityResolution</c> для отключения кеширования, но с вычислением одинаковых сущностей в результате запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns><see cref="{T}"/> с Id, переданным в параметре <paramref name="id"/></returns>
    /// <exception cref="ArgumentException">Ошибка, если элемент <see cref="{T}"/> не найден</exception>
    public async Task<T> GetByIdOrThrowAsync(TKey id, IIncludeSpecification<T>? specification = null, TrackingType trackingType = TrackingType.Tracking, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, specification, trackingType, cancellationToken);
        if (entity is null)
            throw new ArgumentException($"Запись {typeof(T).Name} с id:'{id}' не была найдена.");
        return entity;
    }

    private IQueryable<T> GetTrackingConfiguredQuery(TrackingType trackingType) =>
    trackingType switch
    {
        TrackingType.NoTracking => _dbSet.AsNoTracking(),
        TrackingType.NoTrackingWithIdentityResolution => _dbSet.AsNoTrackingWithIdentityResolution(),
        TrackingType.Tracking => _dbSet,
        _ => throw new ArgumentOutOfRangeException(nameof(trackingType), trackingType, null)
    };

    public Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return _dbSet.CountAsync(cancellationToken);
    }
}
