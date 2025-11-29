using System.Linq.Expressions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace ConectOne.EntityFrameworkCore.Sql.Interfaces
{
    /// <summary>
    /// Generic repository interface for performing CRUD operations on entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
    public interface IRepository<TEntity, TKey> where TEntity : IAuditableEntity
    {
        #region Read Operations

        /// <summary>
        /// Asynchronously retrieves a list of entities from the data source.
        /// </summary>
        /// <remarks>Use this method to obtain a collection of entities without blocking the calling
        /// thread. The tracking behavior can be controlled via the <paramref name="trackChanges"/> parameter.</remarks>
        /// <param name="trackChanges">Indicates whether the retrieved entities should be tracked for changes. If <see langword="true"/>, changes
        /// to the entities will be tracked; otherwise, they will not be tracked.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="TEntity"/>
        /// with a list of <typeparamref name="TEntity"/> objects.</returns>
        Task<IBaseResult<List<TEntity>>> ListAsync(bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves a list of entities that satisfy the specified criteria.
        /// </summary>
        /// <param name="spec">The specification defining the criteria that the entities must meet.</param>
        /// <param name="trackChanges">A boolean value indicating whether to track changes to the entities.  true to track changes; otherwise,
        /// false.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities  that match
        /// the specified criteria.</returns>
        Task<IBaseResult<List<TEntity>>> ListAsync(ISpecification<TEntity> spec, bool trackChanges = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves the first entity that matches the specified criteria or a default value if no such
        /// entity is found.
        /// </summary>
        /// <param name="spec">The specification defining the criteria that the entity must satisfy.</param>
        /// <param name="trackChanges">A boolean value indicating whether to track changes to the entity. <see langword="true"/> to track changes;
        /// otherwise, <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the first entity that matches
        /// the criteria, or <see langword="null"/> if no such entity is found.</returns>
        Task<IBaseResult<TEntity?>> FirstOrDefaultAsync(ISpecification<TEntity> spec, bool trackChanges = false, CancellationToken cancellationToken = default);

        Task<IBaseResult<TEntity?>> FindByIdAsync(TKey id, bool trackChanges = false, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Asynchronously counts the number of entities that satisfy the specified criteria.
        /// </summary>
        /// <param name="spec">The specification defining the criteria that entities must meet to be counted. If <see langword="null"/>,
        /// all entities are counted.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the count of entities that match
        /// the criteria.</returns>
        Task<IBaseResult<int>> CountAsync(ISpecification<TEntity>? spec = null, CancellationToken ct = default);
        
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
        IBaseResult<IQueryable<TEntity>> FindAll(bool trackChanges = false, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes);

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
        Task<IBaseResult<List<TEntity>>> FindAllAsync(bool trackChanges, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes);

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
        /// <param name="cancellationToken">The cancellation token used, uses default instance if null</param>
        /// <returns>
        /// A result containing a queryable collection of entities matching the condition.
        /// The result indicates success or failure and contains any relevant data or error messages.
        /// </returns>
        IBaseResult<IQueryable<TEntity>> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes);

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
        Task<IBaseResult<List<TEntity>>> FindByConditionAsync(Expression<Func<TEntity, bool>> expression, bool trackChanges, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Asynchronously determines whether any entities satisfy the specified condition.
        /// </summary>
        /// <param name="predicate">An expression that defines the condition to test each entity against.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a value indicating whether any
        /// entities match the specified condition.</returns>
        Task<IBaseResult<bool>> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously determines whether an entity with the specified identifier exists.
        /// </summary>
        /// <param name="id">The identifier of the entity to check for existence.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a value indicating whether the
        /// entity exists.</returns>
        Task<IBaseResult<bool>> ExistsAsync(TKey id, CancellationToken cancellationToken = default);

        #endregion

        #region Create Operations

        /// <summary>
        /// Creates a new entity in the underlying data store.
        /// </summary>
        /// <param name="entity">The entity to add. Cannot be null. The entity's required properties must be set before calling this method.</param>
        /// <returns>A result object containing information about the outcome of the create operation, including the created
        /// entity and any validation or error details.</returns>
        IBaseResult<TEntity> Create(TEntity entity);

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
        Task<IBaseResult<TEntity>> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously creates a collection of entities in the data store.
        /// </summary>
        /// <param name="entities">The collection of entities to create. Cannot be null or contain null elements.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the create operation.</returns>
        Task<IBaseResult> CreateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        IBaseResult CreateRange(IEnumerable<TEntity> entities);

        #endregion

        #region Update Operations

        /// <summary>
        /// Updates the specified entity in the data store.
        /// </summary>
        /// <param name="entity">The entity instance to update. Cannot be null. The entity must have a valid identifier corresponding to an
        /// existing record.</param>
        /// <returns>A result object indicating the outcome of the update operation, including success status and any relevant
        /// messages.</returns>
        IBaseResult<TEntity> Update(TEntity entity);

        IBaseResult UpdateRange(IEnumerable<TEntity> entities);

        #endregion

        #region Delete Operations

        /// <summary>
        /// Deletes the specified entity from the data store.
        /// </summary>
        /// <param name="entity">The entity to be deleted. Cannot be null.</param>
        /// <returns>A result object indicating the outcome of the delete operation, including success status and any relevant
        /// error information.</returns>
        IBaseResult<TEntity> Delete(TEntity entity);

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
        Task<IBaseResult<TEntity>> DeleteAsync(TKey entityId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously deletes a range of entities from the context.
        /// </summary>
        /// <param name="entities">A list of the entities to be removed</param>
        /// <returns>
        /// A task containing a result indicating the success/failure of the operation.
        /// </returns>
        IBaseResult RemoveRange(IEnumerable<TEntity> entities);

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
        Task<IBaseResult> SaveAsync(CancellationToken cancellationToken = default);

        #endregion
    }
}
