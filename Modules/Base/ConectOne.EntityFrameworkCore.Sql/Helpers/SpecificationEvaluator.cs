using ConectOne.EntityFrameworkCore.Sql.Interfaces;

namespace ConectOne.EntityFrameworkCore.Sql.Helpers
{
    /// <summary>
    /// Static utility class that evaluates a specification and applies its query criteria, ordering, paging, and includes to an <see cref="IQueryable{T}"/>.
    /// </summary>
    internal static class SpecificationEvaluator
    {
        /// <summary>
        /// Applies a specification to the given query, including filters, ordering, paging, and eager loading instructions.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity being queried.</typeparam>
        /// <param name="inputQuery">The base <see cref="IQueryable{T}"/> to apply the specification to.</param>
        /// <param name="spec">The specification defining the query behavior.</param>
        /// <returns>An <see cref="IQueryable{T}"/> with the specification applied.</returns>
        public static IQueryable<TEntity> GetQuery<TEntity>(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery;

            // Apply filter criteria (WHERE clause)
            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            // Apply ordering if defined (ORDER BY clause)
            if (spec.OrderBy is not null)
                query = spec.OrderBy(query);

            // Apply pagination if Skip is defined (OFFSET)
            if (spec.Skip.HasValue)
                query = query.Skip(spec.Skip.Value);

            // Apply pagination limit if Take is defined (LIMIT)
            if (spec.Take.HasValue)
                query = query.Take(spec.Take.Value);

            // Apply eager loading for related entities using Include and ThenInclude
            query = spec.IncludeBuilders.Aggregate(query, (current, includeBuilder) => includeBuilder(current));

            return query;
        }
    }

}
