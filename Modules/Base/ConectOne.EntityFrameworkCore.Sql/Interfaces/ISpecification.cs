using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace ConectOne.EntityFrameworkCore.Sql.Interfaces
{
    /// <summary>
    /// Defines a specification pattern for querying entities of type TEntity, including criteria, related data to
    /// include, ordering, and paging options.
    /// </summary>
    /// <remarks>Implementations of this interface allow encapsulation of query logic, such as filtering,
    /// eager loading of related data, sorting, and pagination, in a reusable and composable manner. Specifications can
    /// be used to build complex queries in a type-safe way, promoting separation of concerns and testability in data
    /// access code.</remarks>
    /// <typeparam name="TEntity">The type of entity to which the specification applies.</typeparam>
    public interface ISpecification<TEntity>
    {
        /// <summary>
        /// Gets the filter expression used to determine which entities are included in the query.
        /// </summary>
        /// <remarks>The criteria is typically used to specify conditions for selecting entities from a
        /// data source. If the value is null, no filtering is applied and all entities are included.</remarks>
        Expression<Func<TEntity, bool>>? Criteria { get; }

        /// <summary>
        /// Gets the collection of functions used to specify related entities to include in query results.
        /// </summary>
        /// <remarks>Each function in the collection defines an include operation for related data when
        /// querying entities. This enables eager loading of navigation properties using patterns compatible with Entity
        /// Framework's Include and ThenInclude methods.</remarks>
        List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> IncludeBuilders { get; }

        /// <summary>
        /// Gets the function used to apply an ordering to the queryable collection of entities.
        /// </summary>
        /// <remarks>Use this property to specify a custom ordering for queries involving the entity type.
        /// If the value is null, no explicit ordering is applied.</remarks>
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? OrderBy { get; }

        /// <summary>
        /// Gets the number of items to skip before processing the remaining items in a collection or query.
        /// </summary>
        int? Skip { get; }

        /// <summary>
        /// Gets the maximum number of items to return in a query result, or null if no limit is set.
        /// </summary>
        int? Take { get; }
    }
}
