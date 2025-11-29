using System.Linq.Expressions;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using Microsoft.EntityFrameworkCore.Query;

namespace ConectOne.EntityFrameworkCore.Sql.Implimentation
{
    /// <content>
    /// Extra members that support <see cref="ThenInclude"/>‑style builder functions
    /// for deeply nested eager loading with full compile‑time safety.
    /// </content>
    public abstract class Specification<TEntity> : ISpecification<TEntity>
    {
        /// <summary>
        /// Gets the filter expression used to determine which entities are included in the query.
        /// </summary>
        /// <remarks>The criteria expression defines the conditions that entities must satisfy to be
        /// selected. If the value is null, no filtering is applied and all entities are included.</remarks>
        public Expression<Func<TEntity, bool>>? Criteria { get; protected init; }

        /// <summary>
        /// Gets the collection of functions used to specify related entities to include in query results.
        /// </summary>
        /// <remarks>Each function in the collection defines an include expression for eager loading
        /// related data when querying entities. The functions are typically used with Entity Framework Core's Include
        /// and ThenInclude methods to shape the returned data graph.</remarks>
        public List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> IncludeBuilders { get; } = [];

        /// <summary>
        /// Gets or sets a function that defines the ordering of the query results for the entity type.
        /// </summary>
        /// <remarks>The function should take an <see cref="IQueryable{TEntity}"/> and return an <see
        /// cref="IOrderedQueryable{TEntity}"/> representing the desired sort order. This property can be used to
        /// specify custom ordering logic when querying entities. If not set, the default ordering is applied.</remarks>
        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? OrderBy { get; protected init; }

        /// <summary>
        /// Gets the number of items to skip before starting to return results, or null to indicate no items should be
        /// skipped.
        /// </summary>
        public int? Skip { get; protected init; }

        /// <summary>
        /// Gets or sets the maximum number of items to return in a query result.
        /// </summary>
        public int? Take { get; protected init; }

        /// <summary>
        /// Adds an include expression to specify related entities to include in the query results.
        /// </summary>
        /// <remarks>Use this method to include navigation properties when querying entities, enabling
        /// eager loading of related data. Multiple calls to this method can be used to include multiple related
        /// entities or navigation paths.</remarks>
        /// <param name="includeBuilder">A function that defines the related entities to include by configuring the queryable for the entity type.
        /// Cannot be null.</param>
        public void AddInclude(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeBuilder) => IncludeBuilders.Add(includeBuilder);
    }

    /// <summary>
    /// Ad‑hoc specification that accepts a single criteria lambda.
    /// Use when you need a quick filter without creating a dedicated spec class.
    /// </summary>
    public class LambdaSpec<TEntity> : Specification<TEntity>
    {
        /// <summary>
        /// Initializes a new instance of the LambdaSpec class with the specified filter criteria.
        /// </summary>
        /// <param name="criteria">An expression that defines the filter condition to be applied to entities of type TEntity. Cannot be null.</param>
        /// <exception cref="ArgumentNullException">Thrown if criteria is null.</exception>
        public LambdaSpec(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
        }
    }
}
