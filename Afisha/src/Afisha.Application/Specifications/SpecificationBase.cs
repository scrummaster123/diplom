using Afisha.Application.Contracts.Specifications;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Afisha.Application.Specifications;

/// <summary>
/// Спецификация получени данных основываясь на predicate, include, orderBy делегатах. 
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <param name="predicate">Функция проверки соответствия каждого элемента условию выборки</param>
/// <param name="include">Функция для включения навигационных свойств</param>
/// <param name="orderBy">Функция сортировки элементов</param>
public class SpecificationBase<TEntity>(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null
    ) : ISpecification<TEntity> where TEntity : class
{
    /// <summary>
    /// Построить запрос на данных полученных из конструктора
    /// </summary>
    /// <param name="query">Исходны запрос</param>
    /// <returns>Результирующий запрос</returns>
    public IQueryable<TEntity> BuildQueryable(IQueryable<TEntity> query)
    {
        if (include is not null)
            query = include(query);

        if (predicate is not null)
            query = query.Where(predicate);

        return orderBy is null ? query : orderBy(query);
    }
}
