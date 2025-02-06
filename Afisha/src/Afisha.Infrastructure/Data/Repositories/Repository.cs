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

    /// <summary>
    /// Добавление элемента <see cref="{T}"/> в репозиторий
    /// </summary>
    /// <param name="item">Добавляемая сущность</param>
    /// <returns>Добавленная сущность <see cref="{T}"/></returns>
    public T Add(T item)
    {
        var addResult = _context.Add(item);
        return addResult.Entity;
    }

    /// <summary>
    /// Обновление элемента <see cref="{T}"/>
    /// </summary>
    /// <param name="item">Сущность с обновленными данными</param>
    /// <returns>Обновленная сущность <see cref="{T}"/></returns>
    public T Update(T item)
    {
        var updateResult = _dbSet.Update(item);
        return updateResult.Entity;
    }

    /// <summary>
    /// Удаление элемента <see cref="{T}"/>
    /// </summary>
    /// <param name="item">Сущность для удаления</param>
    public void Delete(T item)
    {
        if (_context.Entry(item).State == EntityState.Detached)
        {
            _dbSet.Attach(item);
        }
        _context.Remove(item);
    }

    /// <summary>
    /// Удаление сущность <see cref="{T}"/> с указанным <paramref name="id"/>
    /// </summary>
    /// <param name="id">Id удаляемой сущности</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Task выполняемой операции</returns>
    public async Task DeleteAsync(TKey id, CancellationToken cancellationToken)
    {
        var item = await _dbSet.FirstOrDefaultAsync(u => u.Id.Equals(id), cancellationToken);
        if (item is not null)
            Delete(item);
    }
}