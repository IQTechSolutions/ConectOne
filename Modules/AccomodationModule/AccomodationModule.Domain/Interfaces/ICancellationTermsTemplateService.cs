using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces;

/// <summary>
/// Defines the contract for managing cancellation terms templates.
/// </summary>
/// <remarks>This service provides methods to retrieve, create, update, and delete cancellation terms templates.
/// It is designed to work with asynchronous operations and supports cancellation tokens for cooperative
/// cancellation.</remarks>
public interface ICancellationTermsTemplateService
{
    /// <summary>
    /// Asynchronously retrieves all cancellation terms templates.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// containing a list of <see cref="CancellationTermsTemplateDto"/> objects representing the cancellation terms
    /// templates.</returns>
    Task<IBaseResult<IEnumerable<CancellationTermsTemplateDto>>> AllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a cancellation terms template by its unique identifier.
    /// </summary>
    /// <remarks>Use this method to retrieve detailed information about a specific cancellation terms
    /// template. Ensure the provided <paramref name="id"/> corresponds to an existing template.</remarks>
    /// <param name="id">The unique identifier of the cancellation terms template to retrieve. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object wrapping the <see cref="CancellationTermsTemplateDto"/> if the template is found, or an appropriate
    /// result indicating the outcome of the operation.</returns>
    Task<IBaseResult<CancellationTermsTemplateDto>> ByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously adds a new cancellation terms template.
    /// </summary>
    /// <param name="dto">The data transfer object containing the details of the cancellation terms template to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object that includes the added <see cref="CancellationTermsTemplateDto"/> if the operation is successful.</returns>
    Task<IBaseResult<CancellationTermsTemplateDto>> AddAsync(CancellationTermsTemplateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing cancellation terms template with the provided data.
    /// </summary>
    /// <param name="dto">The data transfer object containing the updated cancellation terms template details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object that includes the updated <see cref="CancellationTermsTemplateDto"/> if the operation succeeds.</returns>
    Task<IBaseResult<CancellationTermsTemplateDto>> EditAsync(CancellationTermsTemplateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the resource identified by the specified ID asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the resource to delete. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the delete operation.</returns>
    Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default);
}
