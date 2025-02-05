using Afisha.Application.Contracts.Specifications;
using Afisha.Domain.Interfaces.Specifications;
using Microsoft.EntityFrameworkCore.Query;

namespace Afisha.Application.Specifications;

/// <summary>
/// Спецификация получени данных основываясь на include делегате. 
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <param name="include">Функция для включения навигационных свойств</param>
public abstract class SpecificationIncludeBase<TEntity>(
    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null
) : IIncludeSpecification<TEntity> where TEntity : class
{
    /// <summary>
    /// Построить запрос на данных полученных из конструктора
    /// </summary>
    /// <param name="query">Исходны запрос</param>
    /// <returns>Результирующий запрос</returns>
    public IQueryable<TEntity> BuildQueryable(IQueryable<TEntity> query)
    {
        if (include != null)
            query = include(query);
        return query;
    }
}
