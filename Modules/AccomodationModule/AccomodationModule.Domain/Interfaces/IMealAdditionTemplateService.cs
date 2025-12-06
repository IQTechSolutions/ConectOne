using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;

namespace AccomodationModule.Domain.Interfaces;

/// <summary>
/// Service contract for managing <see cref="MealAdditionTemplate"/> entities.
/// </summary>
public interface IMealAdditionTemplateService
{
    /// <summary>
    ///     Retrieves all meal‑addition templates including their owning <see cref="Restaurant"/> entity.
    /// </summary>
    /// <remarks>
    ///     The method uses a <see cref="LambdaSpec{TEntity}"/> to express an empty filter ("<c>true</c>") and explicitly
    ///     adds an include for the <c>Restaurant</c> navigation property to ensure it is eagerly loaded in a single
    ///     round‑trip to the database.
    /// </remarks>
    /// <param name="cancellationToken">
    ///     Optional token that can be used by the caller to cancel the request.
    /// </param>
    /// <returns>
    ///     An <see cref="IBaseResult"/> wrapping a <see cref="List{T}"/> of <see cref="MealAdditionTemplateDto"/>
    ///     objects.
    /// </returns>
    Task<IBaseResult<List<MealAdditionTemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves a single meal‑addition template by its unique identifier, including the related restaurant.
    /// </summary>
    /// <param name="id">Unique identifier of the template.</param>
    /// <param name="cancellationToken">Cancellation token propagated from the calling context.</param>
    /// <returns>Either a populated DTO wrapped in a success result or a failure result with meaningful messages.</returns>
    Task<IBaseResult<MealAdditionTemplateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Persists a new meal‑addition template to the underlying store.
    /// </summary>
    /// <param name="dto">DTO carrying the state required to create the entity.</param>
    /// <param name="cancellationToken">Token enabling the caller to cancel the operation.</param>
    /// <returns>A success result containing the newly created DTO or a failure result with errors.</returns>
    Task<IBaseResult<MealAdditionTemplateDto>> AddAsync(MealAdditionTemplateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates an existing meal‑addition template. Only fails if the entity cannot be found or persistence fails.
    /// </summary>
    /// <param name="dto">DTO containing the new state.</param>
    /// <param name="cancellationToken">Operation cancellation token.</param>
    /// <returns>A result with the updated DTO or error messages.</returns>
    Task<IBaseResult<MealAdditionTemplateDto>> EditAsync(MealAdditionTemplateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deletes a meal‑addition template with the supplied identifier.
    /// </summary>
    /// <param name="id">Identifier of the template to remove.</param>
    /// <param name="cancellationToken">Token that allows the caller to cancel the request.</param>
    /// <returns>A success or failure result depending on the outcome.</returns>
    Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default);
}
