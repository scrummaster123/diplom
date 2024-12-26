using System;
using Afisha.Domain.Entities.Abstractions;
using Afisha.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Afisha.Infrastructure.Data.Repositories;
public class Repository<T, TKey> :
    ReadRepository<T, TKey>,
    IRepository<T, TKey>
    where T : EntityBase<TKey>
    where TKey : struct
{
    public Repository(AfishaDbContext context) : base(context) { }

    public T Add(T item)
    {
        var addResult = _context.Add(item);
        return addResult.Entity;
    }

    public T Update(T item)
    {
        var updateResult = _dbSet.Update(item);
        return updateResult.Entity;
    }

    public void Delete(T item)
    {
        if (_context.Entry(item).State == EntityState.Detached)
        {
            _dbSet.Attach(item);
        }
        _context.Remove(item);
    }

    public async Task Delete(TKey id, CancellationToken cancellationToken)
    {
        var item = await _dbSet.FirstOrDefaultAsync(u => u.Id.Equals(id));
        if (item is not null)
            Delete(item);
    }

    
}
