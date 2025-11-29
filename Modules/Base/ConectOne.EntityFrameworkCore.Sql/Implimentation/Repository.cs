// ReSharper disable MustUseReturnValue

using System.Linq.Expressions;
using ConectOne.Domain.Entities;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Helpers;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConectOne.EntityFrameworkCore.Sql.Implimentation
{
    /// <summary>
    /// Generic repository implementation for performing CRUD operations on entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
    public class Repository<TEntity, TKey>(GenericDbContextFactory context) : IRepository<TEntity, TKey>, IAsyncDisposable where TEntity : EntityBase<TKey> where TKey : IEquatable<TKey>
    {
        private readonly DbContext repositoryContext = context.CreateDbContext();

        #region Helper Methods

        /// <summary>
        /// Handles exceptions and returns a failed result with the base exception message.
        /// This method centralizes exception handling for consistency.
        /// </summary>
        /// <typeparam name="T">The type of the result data.</typeparam>
        /// <param name="ex">The exception to handle.</param>
        /// <returns>A failed result containing the base exception message.</returns>
        private static IBaseResult<T> HandleException<T>(Exception ex)
        {
            // Get the base exception to find the root cause of the error
            var baseException = ex.GetBaseException();

            // Return a failed result containing the exception message
            return Result<T>.Fail(baseException.Message);
        }

        /// <summary>
        /// Creates a failed result containing the message from the base exception of the specified exception.
        /// </summary>
        /// <param name="ex">The exception from which to extract the base exception message. Cannot be null.</param>
        /// <returns>A failed result containing the message of the base exception.</returns>
        private static IBaseResult HandleException(Exception ex)
        {
            var baseException = ex.GetBaseException();
            return Result.Fail(baseException.Message);
        }

        /// <summary>
        /// Applies include expressions to the query for eager loading related entities.
        /// Uses expression trees to specify the related entities to include.
        /// </summary>
        /// <param name="query">The query to which includes will be applied.</param>
        /// <param name="includes">The include expressions specifying related entities.</param>
        /// <returns>The query with includes applied for eager loading.</returns>
        private IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includes)
        {
            if (includes is null || includes.Length == 0)
            {
                return query;
            }

            // Aggregate the includes, applying each one to the query
            return includes.Aggregate(query, (current, include) => current.Include(include));
        }

        /// <summary>
        /// Creates a base query for the entity type.
        /// </summary>
        /// <param name="track">Flag to indicate if query should be tracked</param>
        /// <returns>The query with includes applied for eager loading.</returns>
        private IQueryable<TEntity> BaseQuery(bool track) => track ? repositoryContext.Set<TEntity>() : repositoryContext.Set<TEntity>().AsNoTracking();

        #endregion

        #region Read Operations

        /// <summary>
        /// Asynchronously retrieves all entities of type TEntity from the data source.
        /// </summary>
        /// <param name="trackChanges">true to enable change tracking for the retrieved entities; otherwise, false. When false, entities are not
        /// tracked by the context, which can improve performance for read-only operations.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IBaseResult with a list of
        /// all TEntity instances found in the data source. The list will be empty if no entities are found.</returns>
        public async Task<IBaseResult<List<TEntity>>> ListAsync(bool trackChanges = false, CancellationToken ct = default)
        {
            try
            {
                IQueryable<TEntity> baseQuery = trackChanges ? repositoryContext.Set<TEntity>() : repositoryContext.Set<TEntity>().AsNoTracking();

                var data = await baseQuery.ToListAsync(ct);
                return await Result<List<TEntity>>.SuccessAsync(data);
            }
            catch (Exception ex)
            {
                return HandleException<List<TEntity>>(ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves a list of entities that satisfy the specified criteria.
        /// </summary>
        /// <remarks>If change tracking is disabled by setting trackChanges to false, the returned
        /// entities are not tracked by the context and will not reflect subsequent changes. This method uses the
        /// provided specification to filter, sort, or include related data as defined by the specification.</remarks>
        /// <param name="spec">The specification that defines the criteria used to filter and shape the returned entities. Cannot be null.</param>
        /// <param name="trackChanges">true to enable change tracking for the retrieved entities; otherwise, false to retrieve entities without
        /// tracking.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IBaseResult with a list of
        /// entities matching the specification. If no entities match, the list will be empty.</returns>
        public async Task<IBaseResult<List<TEntity>>> ListAsync(ISpecification<TEntity> spec, bool trackChanges = false, CancellationToken ct = default)
        {
            try
            {
                IQueryable<TEntity> baseQuery = trackChanges ? repositoryContext.Set<TEntity>() : repositoryContext.Set<TEntity>().AsNoTracking();

                var query = SpecificationEvaluator.GetQuery(baseQuery, spec);
                var data = await query.ToListAsync(ct);
                return await Result<List<TEntity>>.SuccessAsync(data);
            }
            catch (Exception ex)
            {
                return HandleException<List<TEntity>>(ex);
            }
        }

        /// <summary>
        /// Asynchronously returns the first entity that matches the specified criteria, or a default value if no such
        /// entity is found.
        /// </summary>
        /// <param name="spec">The specification that defines the criteria used to filter entities.</param>
        /// <param name="trackChanges">true to enable change tracking for the returned entity; otherwise, false to retrieve the entity without
        /// tracking.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IBaseResult<TEntity?> with
        /// the first entity that matches the specification, or null if no entity is found.</returns>
        public async Task<IBaseResult<TEntity?>> FirstOrDefaultAsync(ISpecification<TEntity> spec, bool trackChanges = false, CancellationToken ct = default)
        {
            try
            {
                IQueryable<TEntity> baseQuery = trackChanges ? repositoryContext.Set<TEntity>() : repositoryContext.Set<TEntity>().AsNoTracking();

                var query = SpecificationEvaluator.GetQuery(baseQuery, spec);
                var data = await query.FirstOrDefaultAsync(ct);
                return await Result<TEntity>.SuccessAsync(data);
            }
            catch (Exception ex)
            {
                return HandleException<TEntity>(ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves an entity by its identifier, with optional change tracking and related entity
        /// inclusion.
        /// </summary>
        /// <remarks>If no entity with the specified identifier exists, the result will contain null.
        /// Including related entities can impact query performance.</remarks>
        /// <param name="id">The unique identifier of the entity to retrieve.</param>
        /// <param name="trackChanges">true to enable change tracking for the retrieved entity; otherwise, false. Change tracking is typically
        /// required for update scenarios.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <param name="includes">An array of expressions specifying related entities to include in the query result. Use to eagerly load
        /// navigation properties.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IBaseResult<TEntity?> with
        /// the entity if found; otherwise, null.</returns>
        public async Task<IBaseResult<TEntity?>> FindByIdAsync(TKey id, bool trackChanges = false, CancellationToken ct = default, params Expression<Func<TEntity, object>>[] includes)
        {
            try
            {
                var query = BaseQuery(trackChanges).Where(entity => entity.Id.Equals(id));
                query = ApplyIncludes(query, includes);
                var entity = await query.FirstOrDefaultAsync(ct);
                return await Result<TEntity>.SuccessAsync(entity);
            }
            catch (Exception ex)
            {
                return HandleException<TEntity>(ex);
            }
        }

        /// <summary>
        /// Asynchronously counts the number of entities that satisfy the specified criteria.
        /// </summary>
        /// <remarks>This method does not track the returned entities in the context. If the operation is
        /// canceled or an error occurs, the result will indicate the failure.</remarks>
        /// <param name="spec">An optional specification that defines the criteria to filter entities. If null, all entities are counted.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with the number of entities that match the specification.</returns>
        public async Task<IBaseResult<int>> CountAsync(ISpecification<TEntity>? spec = null, CancellationToken ct = default)
        {
            try
            {
                var baseQuery = repositoryContext.Set<TEntity>().AsNoTracking();
                if (spec is not null)
                {
                    baseQuery = SpecificationEvaluator.GetQuery(baseQuery, spec);
                }
                var count = await baseQuery.CountAsync(ct);
                return await Result<int>.SuccessAsync(count);
            }
            catch (Exception ex)
            {
                return HandleException<int>(ex);
            }
        }

        #endregion

        /// <summary>
        /// Retrieves all entities from the database.
        /// Allows optional tracking of changes and eager loading of related entities.
        /// </summary>
        /// <param name="trackChanges">
        /// If <c>true</c>, the entities will be tracked for changes; otherwise, no tracking is applied.
        /// Tracking allows updates to be detected when <see cref="SaveAsync"/> is called.
        /// </param>
        /// <param name="includes">
        /// Optional expressions specifying related entities to include for eager loading.
        /// Example: <c>entity => entity.RelatedEntities</c>.
        /// </param>
        /// <param name="cancellationToken">The cancellation token used, uses default instance if null</param>
        /// <returns>
        /// A result containing a queryable collection of all entities.
        /// The result indicates success or failure and contains any relevant data or error messages.
        /// </returns>
        public IBaseResult<IQueryable<TEntity>> FindAll(bool trackChanges, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
        {
            try
            {
                IQueryable<TEntity> query = repositoryContext.Set<TEntity>();
                if (!trackChanges)
                    query = query.AsNoTracking();

                query = ApplyIncludes(query, includes);

                return Result<IQueryable<TEntity>>.Success(query);
            }
            catch (Exception ex)
            {
                return HandleException<IQueryable<TEntity>>(ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves all entities from the database.
        /// Allows optional tracking of changes and eager loading of related entities.
        /// </summary>
        /// <param name="trackChanges">
        /// If <c>true</c>, the entities will be tracked for changes; otherwise, no tracking is applied.
        /// Tracking allows updates to be detected when <see cref="SaveAsync"/> is called.
        /// </param>
        /// <param name="includes">
        /// Optional expressions specifying related entities to include for eager loading.
        /// Example: <c>entity => entity.RelatedEntities</c>.
        /// </param>
        /// <param name="cancellationToken">The cancellation token used, uses default instance if null</param>
        /// <returns>
        /// A task containing a result with a list of all entities.
        /// The result indicates success or failure and contains any relevant data or error messages.
        /// </returns>
        public async Task<IBaseResult<List<TEntity>>> FindAllAsync(bool trackChanges, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
        {
            try
            {
                var query = trackChanges ? repositoryContext.Set<TEntity>() : repositoryContext.Set<TEntity>().AsNoTracking();
                query = ApplyIncludes(query, includes);
                var entities = await query.ToListAsync(cancellationToken);
                return await Result<List<TEntity>>.SuccessAsync(entities);
            }
            catch (Exception ex)
            {
                return HandleException<List<TEntity>>(ex);
            }
        }

        /// <summary>
        /// Retrieves entities that match a specified condition.
        /// Allows optional tracking of changes and eager loading of related entities.
        /// </summary>
        /// <param name="expression">
        /// A lambda expression representing the condition to filter entities.
        /// Example: <c>entity => entity.Property == value</c>.
        /// </param>
        /// <param name="trackChanges">
        /// If <c>true</c>, the entities will be tracked for changes; otherwise, no tracking is applied.
        /// </param>
        /// <param name="includes">
        /// Optional expressions specifying related entities to include for eager loading.
        /// </param>
        /// <returns>
        /// A result containing a queryable collection of entities matching the condition.
        /// The result indicates success or failure and contains any relevant data or error messages.
        /// </returns>
        public IBaseResult<IQueryable<TEntity>> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
        {
            try
            {
                IQueryable<TEntity> query = repositoryContext.Set<TEntity>().Where(expression);
                if (!trackChanges)
                    query = query.AsNoTracking();

                query = ApplyIncludes(query, includes);

                return Result<IQueryable<TEntity>>.Success(query);
            }
            catch (Exception ex)
            {
                return HandleException<IQueryable<TEntity>>(ex);
            }
        }

        /// <summary>
        /// Asynchronously finds entities that match a specified condition.
        /// Allows optional tracking of changes and eager loading of related entities.
        /// </summary>
        /// <param name="expression">
        /// A lambda expression representing the condition to filter entities.
        /// Example: <c>entity => entity.Property == value</c>.
        /// </param>
        /// <param name="trackChanges">
        /// If <c>true</c>, the entities will be tracked for changes; otherwise, no tracking is applied.
        /// </param>
        /// <param name="includes">
        /// Optional expressions specifying related entities to include for eager loading.
        /// </param>
        /// <param name="cancellationToken">The cancellation token used, uses default instance if null</param>
        /// <returns>
        /// A task containing a result with a list of entities matching the condition.
        /// The result indicates success or failure and contains any relevant data or error messages.
        /// </returns>
        public async Task<IBaseResult<List<TEntity>>> FindByConditionAsync(Expression<Func<TEntity, bool>> expression, bool trackChanges, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
        {
            try
            {
                var query = BaseQuery(trackChanges).Where(expression);
                query = ApplyIncludes(query, includes);
                var entities = await query.ToListAsync(cancellationToken);
                return await Result<List<TEntity>>.SuccessAsync(entities);
            }
            catch (Exception ex)
            {
                return HandleException<List<TEntity>>(ex);
            }
        }

        /// <summary>
        /// Asynchronously determines whether any entities of type TEntity satisfy the specified condition.
        /// </summary>
        /// <param name="predicate">An expression that defines the condition to test each entity against.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IBaseResult<bool> indicating
        /// whether any entities match the specified condition. The value is <see langword="true"/> if at least one
        /// entity matches; otherwise, <see langword="false"/>.</returns>
        public async Task<IBaseResult<bool>> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            try
            {
                var exists = await repositoryContext.Set<TEntity>().AsNoTracking().AnyAsync(predicate, ct);
                return await Result<bool>.SuccessAsync(exists);
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex);
            }
        }

        /// <summary>
        /// Asynchronously determines whether an entity with the specified identifier exists.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to check for existence.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{Boolean}"/> indicating <see langword="true"/> if the entity exists; otherwise, <see
        /// langword="false"/>.</returns>
        public Task<IBaseResult<bool>> ExistsAsync(TKey id, CancellationToken ct = default)
        {
            return ExistsAsync(entity => entity.Id.Equals(id), ct);
        }

        #region Create Operations

        /// <summary>
        /// Creates a new entity in the context.
        /// </summary>
        /// <param name="entity">
        /// The entity to add to the context.
        /// Must not be <c>null</c>.
        /// </param>
        /// <param name="cancellationToken">The cancellation token used, uses default instance if null</param>
        /// <returns>
        /// A result containing the added entity.
        /// The result indicates success or failure and contains any relevant data or error messages.
        /// </returns>
        public IBaseResult<TEntity> Create(TEntity entity)
        {
            try
            {
                repositoryContext.Add(entity);
                return Result<TEntity>.Success(entity);
            }
            catch (Exception ex)
            {
                return HandleException<TEntity>(ex);
            }
        }

        /// <summary>
        /// Asynchronously creates a new entity in the context.
        /// The changes are not saved to the database until <see cref="SaveAsync"/> is called.
        /// </summary>
        /// <param name="entity">
        /// The entity to add to the context.
        /// Must not be <c>null</c>.
        /// </param>
        /// <param name="cancellationToken">The cancellation token used, uses default instance if null</param>
        /// <returns>
        /// A task containing a result with the added entity.
        /// The result indicates success or failure and contains any relevant data or error messages.
        /// </returns>
        public async Task<IBaseResult<TEntity>> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            try
            {
                await repositoryContext.Set<TEntity>().AddAsync(entity, cancellationToken);
                return await Result<TEntity>.SuccessAsync(entity);
            }
            catch (Exception ex)
            {
                return HandleException<TEntity>(ex);
            }
        }

        /// <summary>
        /// Asynchronously creates a range of entities in the context.
        /// </summary>
        /// <param name="entities">The entities to be added</param>
        /// <returns>
        /// A task containing a result indicating the success/failure of the operation.
        /// </returns>
        public async Task<IBaseResult> CreateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            try
            {
                await repositoryContext.AddRangeAsync(entities, cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Adds a collection of entities to the underlying data store in a single operation.
        /// </summary>
        /// <param name="entities">The collection of entities to add. Cannot be null. Each entity in the collection will be added to the data
        /// store.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the success or failure of the operation.</returns>
        public IBaseResult CreateRange(IEnumerable<TEntity> entities)
        {
            try
            {
                repositoryContext.AddRange(entities);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        #endregion

        #region Update Operations

        /// <summary>
        /// Updates an existing entity in the context.
        /// </summary>
        /// <param name="entity">
        /// The entity with updated values.
        /// The entity must be tracked by the context.
        /// </param>
        /// <returns>
        /// <param name="cancellationToken">The cancellation token used, uses default instance if null</param>
        /// A result containing the updated entity.
        /// The result indicates success or failure and contains any relevant data or error messages.
        /// </returns>
        public IBaseResult<TEntity> Update(TEntity entity)
        {
            try
            {
                repositoryContext.Update(entity);
                return Result<TEntity>.Success(entity);
            }
            catch (Exception ex)
            {
                return HandleException<TEntity>(ex);
            }
        }

        /// <summary>
        /// Updates a collection of entities in the data store.
        /// </summary>
        /// <param name="entities">The collection of entities to update. Cannot be null. Each entity in the collection must be valid for
        /// update.</param>
        /// <returns>An <see cref="IBaseResult"/> indicating the outcome of the update operation. The result contains success or
        /// failure information.</returns>
        public IBaseResult UpdateRange(IEnumerable<TEntity> entities)
        {
            try
            {
                repositoryContext.UpdateRange(entities);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        #endregion

        #region Delete Operations

        /// <summary>
        /// Deletes an entity from the context.
        /// </summary>
        /// <param name="entity">
        /// The entity to remove.
        /// The entity must be tracked by the context.
        /// </param>
        /// <param name="cancellationToken">The cancellation token used, uses default instance if null</param>
        /// <returns>
        /// A result containing the deleted entity.
        /// The result indicates success or failure and contains any relevant data or error messages.
        /// </returns>
        public IBaseResult<TEntity> Delete(TEntity entity)
        {
            try
            {
                repositoryContext.Remove(entity);
                return Result<TEntity>.Success(entity);
            }
            catch (Exception ex)
            {
                return HandleException<TEntity>(ex);
            }
        }
        
        /// <summary>
        /// Asynchronously deletes an entity identified by its key from the context.
        /// The changes are not saved to the database until <see cref="SaveAsync"/> is called.
        /// </summary>
        /// <param name="entityId">
        /// The key of the entity to remove.
        /// Must match the type <typeparamref name="TKey"/>.
        /// </param>
        /// <param name="cancellationToken">The cancellation token used, uses default instance if null</param>
        /// <returns>
        /// A task containing a result indicating the success of the operation.
        /// The result includes the deleted entity if successful.
        /// If the entity is not found, the result indicates failure.
        /// </returns>
        public async Task<IBaseResult<TEntity>> DeleteAsync(TKey entityId, CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = await repositoryContext.Set<TEntity>().FirstOrDefaultAsync(c => c.Id.Equals(entityId), cancellationToken);
                if (entity == null) return await Result<TEntity>.FailAsync($"Entity with ID {entityId} not found.");

                repositoryContext.Remove(entity);
                return await Result<TEntity>.SuccessAsync(entity);
            }
            catch (Exception ex)
            {
                return HandleException<TEntity>(ex);
            }
        }

        /// <summary>
        /// Asynchronously deletes a range of entities from the context.
        /// </summary>
        /// <param name="entities">A list of the entities to be removed</param>
        /// <returns>
        /// A task containing a result indicating the success/failure of the operation.
        /// </returns>
        public IBaseResult RemoveRange(IEnumerable<TEntity> entities)
        {
            try
            {
                repositoryContext.RemoveRange(entities);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        #endregion

        #region Save Operations

        /// <summary>
        /// Asynchronously saves all pending changes in the context to the database.
        /// This method should be called after creating, updating, or deleting entities.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token used, uses default instance if null</param>
        /// <returns>
        /// A task containing a result indicating the success of the save operation.
        /// The result indicates success or failure and contains any relevant data or error messages.
        /// </returns>
        public async Task<IBaseResult> SaveAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await repositoryContext.SaveChangesAsync(cancellationToken);
                return await Result.SuccessAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return await Result.FailAsync("The entity was updated by another user or process.");
            }
            catch (Exception ex)
            {
                return HandleException<TEntity>(ex);
            }
        }

        #endregion

        /// <summary>
        /// Asynchronously releases the unmanaged resources used by the object and suppresses finalization.
        /// </summary>
        /// <remarks>Call this method to release resources when the object is no longer needed. After
        /// calling this method, the object should not be used.</remarks>
        /// <returns>A task that represents the asynchronous dispose operation.</returns>
        public async ValueTask DisposeAsync()
        {
            await repositoryContext.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}
