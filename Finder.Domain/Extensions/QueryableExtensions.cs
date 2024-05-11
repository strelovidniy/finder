using System.Linq.Expressions;
using System.Reflection;
using EntityFrameworkCore.RepositoryInfrastructure;
using Finder.Data.Enums;
using Finder.Data.Enums.RichEnums;
using Finder.Domain.Exceptions;
using Finder.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Finder.Domain.Extensions;

internal static class QueryableExtensions
{
    public static IOrderedQueryable<TSource> OrderBy<TSource>(
        this IQueryable<TSource> query,
        string propertyName
    ) => InvokeOrderingQueryableMethod(
        query,
        propertyName,
        nameof(OrderBy)
    );

    public static IOrderedQueryable<TSource> OrderByDescending<TSource>(
        this IQueryable<TSource> query,
        string propertyName
    ) => InvokeOrderingQueryableMethod(
        query,
        propertyName,
        nameof(OrderByDescending)
    );

    private static IOrderedQueryable<TSource> InvokeOrderingQueryableMethod<TSource>(
        IQueryable<TSource> query,
        string propertyName,
        string methodName
    )
    {
        var entityType = typeof(TSource);

        PropertyInfo? propertyInfo = null;

        //Create x=>x.PropName
        var arg = Expression.Parameter(entityType, "x");

        Expression property = arg;

        if (!propertyName.Contains("."))
        {
            propertyInfo = entityType.GetProperty(propertyName);

            property = Expression.Property(arg, propertyName);
        }
        else
        {
            var propertyNames = propertyName.Split(".");

            propertyInfo = propertyNames.Aggregate(propertyInfo,
                (current, name) =>
                    current is null ? entityType.GetProperty(name) : current.PropertyType.GetProperty(name));

            property = propertyName.Split('.').Aggregate(property, Expression.Property);
        }

        var selector = Expression.Lambda(property, arg);

        //Get System.Linq.Queryable.OrderBy() method.
        var enumerableType = typeof(Queryable);

        var method = enumerableType.GetMethods()
            .Where(m => m.Name == methodName && m.IsGenericMethodDefinition)
            .Where(m =>
            {
                var parameters = m.GetParameters().ToList();

                //Put more restriction here to ensure selecting the right overload                
                return parameters.Count == 2; //overload that has 2 parameters
            }).Single();

        //The linq OrderBy<TSource, TKey> has two generic types, which provided here
        var genericMethod = method
            .MakeGenericMethod(entityType, propertyInfo!.PropertyType);

        /*Call query.OrderBy(selector), with query and selector: x=> x.PropName
          Note that we pass the selector as Expression to the method and we don't compile it.
          By doing so EF can extract "order by" columns and generate SQL for it.*/
        var newQuery = (IOrderedQueryable<TSource>) genericMethod
            .Invoke(genericMethod, new object[] { query, selector })!;

        return newQuery;
    }

    public static IQueryable<TEntity> ExpandAndSort<TEntity>(
        this IQueryable<TEntity> queryable,
        QueryParametersModel queryParametersModel,
        ILogger? logger = null
    ) where TEntity : class, IEntity => queryable
        .Expand(queryParametersModel, logger)
        .Sort(queryParametersModel, logger);

    public static IQueryable<TEntity> Sort<TEntity>(
        this IQueryable<TEntity> queryable,
        QueryParametersModel queryParametersModel,
        ILogger? logger = null
    ) where TEntity : class, IEntity
    {
        if (queryParametersModel.SortBy is null)
        {
            return queryable;
        }

        var sortBy = string.Join('.', queryParametersModel.SortBy
            .Split('.')
            .Select(p => p.Trim())
            .Select(p => char.ToUpper(p[0]) + p[1..]));

        try
        {
            queryable = queryParametersModel.SortAscending
                ? queryable.OrderBy(sortBy)
                : queryable.OrderByDescending(sortBy);
        }
        catch (Exception e)
        {
            logger?.LogError(e, ErrorMessage.SortingByPropertyError);

            throw new ApiException(StatusCode.InvalidSortByProperty);
        }

        return queryable;
    }

    public static IQueryable<TEntity> Expand<TEntity>(
        this IQueryable<TEntity> queryable,
        QueryParametersModel queryParametersModel,
        ILogger? logger = null
    ) where TEntity : class, IEntity
    {
        if (queryParametersModel.ExpandProperty is null)
        {
            return queryable;
        }

        try
        {
            queryable = queryParametersModel
                .ExpandProperty
                .Split(',')
                .Select(property => string.Join('.', property
                    .Split('.')
                    .Select(p => p.Trim())
                    .Select(p => char.ToUpper(p[0]) + p[1..])))
                .Aggregate(queryable, (current, next) => current.Include(next));
        }
        catch (Exception e)
        {
            logger?.LogError(e, ErrorMessage.ExpandingPropertyError);

            throw new ApiException(StatusCode.InvalidExpandProperty);
        }

        return queryable;
    }
}