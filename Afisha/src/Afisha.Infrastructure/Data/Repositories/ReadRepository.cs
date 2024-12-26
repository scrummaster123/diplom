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

    public Task<T[]> Get(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        TrackingType trackingType = TrackingType.NoTracking,
        CancellationToken cancellationToken = default)
    {
        var query = GetQuery(trackingType);

        if (include is not null)
            query = include(query);

        if (predicate is not null)
            query = query.Where(predicate);

        if (orderBy is not null)
            query = orderBy(query);

        return query.ToArrayAsync(cancellationToken);
    }

    public Task<T?> GetById(
        TKey id,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        TrackingType trackingType = TrackingType.NoTracking,
        CancellationToken cancellationToken = default)
    {
        var query = GetQuery(trackingType);

        if (include != null)
            query = include(query);

        return query.FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
    }

    public async Task<T> GetByIdOrThrow(
    TKey id,
    Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
    TrackingType trackingType = TrackingType.NoTracking,
    CancellationToken cancellationToken = default)
    {
        var entity = await GetById(id, include, trackingType, cancellationToken);
        if (entity is null)
            throw new ArgumentException($"Запись {typeof(T).Name} с id:'{id}' не была найдена.");
        return entity;
    }

    private IQueryable<T> GetQuery(TrackingType trackingType) =>
        trackingType switch
        {
            TrackingType.NoTracking => _dbSet.AsNoTracking(),
            TrackingType.NoTrackingWithIdentityResolution => _dbSet.AsNoTrackingWithIdentityResolution(),
            TrackingType.Tracking => _dbSet,
            _ => throw new ArgumentOutOfRangeException(nameof(trackingType), trackingType, null)
        };
}
