using Afisha.Domain.Entities.Abstractions;

namespace Afisha.Domain.Interfaces.Repositories;
public interface IRepository<T, TKey> : IReadRepository<T, TKey>
    where T : EntityBase<TKey>
    where TKey : struct
{
    T Add(T item);

    T Update(T item);

    void Delete(T item);

    Task DeleteAsync(TKey id, CancellationToken cancellationToken);
}
