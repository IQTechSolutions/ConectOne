using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces;

/// <summary>
/// Provides operations for managing itinerary item templates, including retrieval, creation,  modification, and
/// deletion of templates.
/// </summary>
/// <remarks>This service is designed to handle CRUD operations for itinerary item templates. Each method  returns
/// a result encapsulated in an <see cref="IBaseResult"/> to provide additional  information about the operation's
/// success or failure. Cancellation tokens can be used to  cancel long-running operations.</remarks>
public interface IItineraryItemTemplateService
{
    /// <summary>
    /// Retrieves all itinerary entry item templates asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be canceled if the token is triggered.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> with
    /// a list of <see cref="ItineraryEntryItemTemplateDto"/> objects representing the retrieved templates.</returns>
    Task<IBaseResult<List<ItineraryEntryItemTemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an itinerary entry item template by its unique identifier.
    /// </summary>
    /// <remarks>Use this method to retrieve detailed information about a specific itinerary entry item
    /// template. Ensure the <paramref name="id"/> provided is valid and corresponds to an existing template.</remarks>
    /// <param name="id">The unique identifier of the itinerary entry item template to retrieve. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// wrapping the <see cref="ItineraryEntryItemTemplateDto"/> if found, or an appropriate result indicating failure.</returns>
    Task<IBaseResult<ItineraryEntryItemTemplateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new itinerary entry item template asynchronously.
    /// </summary>
    /// <remarks>Use this method to add a new itinerary entry item template to the system. Ensure that the
    /// provided  <paramref name="dto"/> contains valid data before calling this method. The operation may be canceled 
    /// by passing a cancellation token.</remarks>
    /// <param name="dto">The data transfer object representing the itinerary entry item template to be added.  This parameter cannot be
    /// <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Passing a cancellation token allows the operation to be canceled.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
    /// with the added <see cref="ItineraryEntryItemTemplateDto"/> if the operation is successful.</returns>
    Task<IBaseResult<ItineraryEntryItemTemplateDto>> AddAsync(ItineraryEntryItemTemplateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing itinerary entry item template with the provided data.
    /// </summary>
    /// <remarks>Use this method to modify an existing itinerary entry item template. Ensure that the provided
    /// <paramref name="dto"/> contains valid data for the update. The operation may fail if the template does not exist
    /// or if the provided data violates business rules.</remarks>
    /// <param name="dto">The data transfer object containing the updated details of the itinerary entry item template.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests, allowing the operation to be canceled.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> with
    /// the updated <see cref="ItineraryEntryItemTemplateDto"/> if the operation is successful.</returns>
    Task<IBaseResult<ItineraryEntryItemTemplateDto>> EditAsync(ItineraryEntryItemTemplateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the entity with the specified identifier asynchronously.
    /// </summary>
    /// <remarks>Use this method to delete an entity by its unique identifier. Ensure that the <paramref
    /// name="id"/> is valid  and corresponds to an existing entity. The operation may fail if the entity is in a state
    /// that prevents deletion.</remarks>
    /// <param name="id">The unique identifier of the entity to delete. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result contains an <see cref="IBaseResult"/> 
    /// indicating the outcome of the operation.</returns>
    Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default);
}
