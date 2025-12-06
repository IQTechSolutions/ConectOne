using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces;

/// <summary>
/// Service contract for managing <see cref="CustomVariableTag"/> entities.
/// </summary>
public interface ICustomVariableTagService
{
    /// <summary>
    /// Asynchronously retrieves all custom variable tags.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a result object with a collection of
    /// custom variable tag data transfer objects.</returns>
    Task<IBaseResult<IEnumerable<CustomVariableTagDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a custom variable tag by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the custom variable tag to retrieve. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="IBaseResult{CustomVariableTagDto}"/> with the requested custom variable tag if found; otherwise, the
    /// result indicates failure.</returns>
    Task<IBaseResult<CustomVariableTagDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously adds a new custom variable tag to the data store.
    /// </summary>
    /// <param name="dto">The custom variable tag to add. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see
    /// cref="IBaseResult{CustomVariableTagDto}"/> indicating the outcome of the add operation and the added custom
    /// variable tag data.</returns>
    Task<IBaseResult<CustomVariableTagDto>> AddAsync(CustomVariableTagDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates an existing custom variable tag with the specified data.
    /// </summary>
    /// <param name="dto">The data transfer object containing the updated values for the custom variable tag. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see
    /// cref="IBaseResult{CustomVariableTagDto}"/> indicating the outcome of the update and the updated custom variable
    /// tag data.</returns>
    Task<IBaseResult<CustomVariableTagDto>> EditAsync(CustomVariableTagDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously deletes the entity identified by the specified ID.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the operation.</returns>
    Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default);
}
