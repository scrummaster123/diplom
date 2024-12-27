using System.Linq.Expressions;
using Afisha.Domain.Entities.Abstractions;
using Afisha.Domain.Enums;
using Afisha.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

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
    /// Получает <see cref="{T}[]"/> основываясь на predicate, include, orderBy делегатах. По-умолчанию работает, как no-tracking запрос.
    /// </summary>
    /// <param name="predicate">Функция проверки соответствия каждого элемента условию выборки</param>
    /// <param name="include">Функция для включения навигационных свойств</param>
    /// <param name="orderBy">Функция сортировки элементов</param>
    /// <param name="trackingType"><c>NoTracking</c> для отключения кеширования; <c>Tracking</c> для включения кеширования; <c>NoTrackingWithIdentityResolution</c> для отключения кеширования, но с вычислением одинаковых сущностей в результате запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns><see cref="{T}[]"/>, который содержит элементы, соответствующие условию, переданному в <paramref name="predicate"/></returns>
    public Task<T[]> GetAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        TrackingType trackingType = TrackingType.NoTracking,
        CancellationToken cancellationToken = default)
    {
        var query = BuildQueryable(predicate, include, orderBy, trackingType);
        return query.ToArrayAsync(cancellationToken);
    }

    /// <summary>
    /// Получает <see cref="{T}[]"/> основываясь на predicate, include, orderBy делегатах и пагинации. По-умолчанию работает, как no-tracking запрос.
    /// </summary>
    /// <param name="predicate">Функция проверки соответствия каждого элемента условию выборки</param>
    /// <param name="include">Функция для включения навигационных свойств</param>
    /// <param name="orderBy">Функция сортировки элементов</param>
    /// <param name="trackingType"><c>NoTracking</c> для отключения кеширования; <c>Tracking</c> для включения кеширования; <c>NoTrackingWithIdentityResolution</c> для отключения кеширования, но с вычислением одинаковых сущностей в результате запроса</param>
    /// <param name="pageIndex">Индекс страницы</param>
    /// <param name="pageSize">Размер страницы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns><see cref="{T}[]"/>, который содержит элементы, соответствующие условию, переданному в <paramref name="predicate"/></returns>
    /// <exception cref="ArgumentException">Ошибка указания параметров пагинации запроса</exception>
    public Task<T[]> GetPagedAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        TrackingType trackingType = TrackingType.NoTracking,
        int pageIndex = 0,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        if (pageIndex < 1)
            throw new ArgumentException("Индекс страницы должен быть больше, либо равен 0");
        if (pageSize < 1)
            throw new ArgumentException("Размер страницы должен быть больше, либо равен 1");
        int skip = pageIndex * pageSize;

        var query = BuildQueryable(predicate, include, orderBy, trackingType);

        return query
            .Skip(skip)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);
    }

    /// <summary>
    /// Получает <see cref="{T?}"/> основываясь Id сущности и include делегате. По-умолчанию работает, как no-tracking запрос.
    /// </summary>
    /// <param name="id">Id искомой сущности</param>
    /// <param name="include">Функция для включения навигационных свойств</param>
    /// <param name="trackingType"><c>NoTracking</c> для отключения кеширования; <c>Tracking</c> для включения кеширования; <c>NoTrackingWithIdentityResolution</c> для отключения кеширования, но с вычислением одинаковых сущностей в результате запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns><see cref="{T?}"/> с Id, переданным в параметре <paramref name="id"/></returns>
    public Task<T?> GetByIdAsync(
        TKey id,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        TrackingType trackingType = TrackingType.NoTracking,
        CancellationToken cancellationToken = default)
    {
        var query = GetTrackingConfiguredQuery(trackingType);

        if (include != null)
            query = include(query);

        return query.FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
    }

    /// <summary>
    /// Получает <see cref="{T}"/> основываясь Id сущности и include делегате. По-умолчанию работает, как no-tracking запрос.
    /// </summary>
    /// <param name="id">Id искомой сущности</param>
    /// <param name="include">Функция для включения навигационных свойств</param>
    /// <param name="trackingType"><c>NoTracking</c> для отключения кеширования; <c>Tracking</c> для включения кеширования; <c>NoTrackingWithIdentityResolution</c> для отключения кеширования, но с вычислением одинаковых сущностей в результате запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns><see cref="{T}"/> с Id, переданным в параметре <paramref name="id"/></returns>
    /// <exception cref="ArgumentException">Ошибка, если элемент <see cref="{T}"/> не найден</exception>
    public async Task<T> GetByIdOrThrowAsync(
    TKey id,
    Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
    TrackingType trackingType = TrackingType.NoTracking,
    CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, include, trackingType, cancellationToken);
        if (entity is null)
            throw new ArgumentException($"Запись {typeof(T).Name} с id:'{id}' не была найдена.");
        return entity;
    }

    private IQueryable<T> BuildQueryable(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        TrackingType trackingType = TrackingType.NoTracking)
    {
        var query = GetTrackingConfiguredQuery(trackingType);

        if (include is not null)
            query = include(query);

        if (predicate is not null)
            query = query.Where(predicate);

        return orderBy is null ? query : orderBy(query);
    }

    private IQueryable<T> GetTrackingConfiguredQuery(TrackingType trackingType) =>
        trackingType switch
        {
            TrackingType.NoTracking => _dbSet.AsNoTracking(),
            TrackingType.NoTrackingWithIdentityResolution => _dbSet.AsNoTrackingWithIdentityResolution(),
            TrackingType.Tracking => _dbSet,
            _ => throw new ArgumentOutOfRangeException(nameof(trackingType), trackingType, null)
        };
}
