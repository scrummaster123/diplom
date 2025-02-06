namespace Afisha.Application.Contracts.Specifications;

public interface ISpecification<TEntity> where TEntity : class
{
    IQueryable<TEntity> BuildQueryable(IQueryable<TEntity> query);
}
