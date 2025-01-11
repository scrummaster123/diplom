namespace Afisha.Domain.Interfaces.Specifications;

public interface IIncludeSpecification<TEntity> where TEntity : class
{
    IQueryable<TEntity> BuildQueryable(IQueryable<TEntity> query);
}
