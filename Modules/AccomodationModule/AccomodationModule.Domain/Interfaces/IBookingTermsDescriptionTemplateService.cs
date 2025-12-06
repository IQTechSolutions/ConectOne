using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces;

/// <summary>
/// Provides methods for managing booking terms description templates, including retrieving, adding, editing, and
/// deleting templates.
/// </summary>
/// <remarks>This service is designed to handle operations related to booking terms description templates, which
/// may include retrieving all templates, fetching a specific template by its identifier, adding new templates, updating
/// existing ones, and deleting templates. Each operation returns a result encapsulated in an <see
/// cref="IBaseResult"/> to provide additional metadata about the operation's success or failure.</remarks>
public interface IBookingTermsDescriptionTemplateService
{
    /// <summary>
    /// Retrieves all booking terms description templates asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a result object  with a list of <see
    /// cref="BookingTermsDescriptionTemplateDto"/> instances.</returns>
    Task<IBaseResult<List<BookingTermsDescriptionTemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a booking terms description template by its unique identifier.
    /// </summary>
    /// <remarks>The method performs an asynchronous operation to retrieve the specified booking terms
    /// description template. Ensure the provided <paramref name="id"/> corresponds to an existing template.</remarks>
    /// <param name="id">The unique identifier of the booking terms description template to retrieve. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// wrapping the <see cref="BookingTermsDescriptionTemplateDto"/> if found, or an appropriate result indicating the
    /// outcome.</returns>
    Task<IBaseResult<BookingTermsDescriptionTemplateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously adds a new booking terms description template.
    /// </summary>
    /// <param name="dto">The data transfer object containing the details of the booking terms description template to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object wrapping the added <see cref="BookingTermsDescriptionTemplateDto"/>.</returns>
    Task<IBaseResult<BookingTermsDescriptionTemplateDto>> AddAsync(BookingTermsDescriptionTemplateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing booking terms description template with the provided data.
    /// </summary>
    /// <param name="dto">The data transfer object containing the updated details of the booking terms description template.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests, allowing the operation to be canceled.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object wrapping the updated <see cref="BookingTermsDescriptionTemplateDto"/> if the operation succeeds.</returns>
    Task<IBaseResult<BookingTermsDescriptionTemplateDto>> EditAsync(BookingTermsDescriptionTemplateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the resource identified by the specified ID asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the resource to delete. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result contains an <see cref="IBaseResult"/> 
    /// indicating the outcome of the operation.</returns>
    Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default);
}