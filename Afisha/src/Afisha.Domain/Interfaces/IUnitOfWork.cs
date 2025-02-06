namespace Afisha.Domain.Interfaces;
public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken);
}
