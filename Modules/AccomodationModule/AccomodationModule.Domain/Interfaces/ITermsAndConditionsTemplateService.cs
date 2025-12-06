using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces;

/// <summary>
/// Service contract for managing <see cref="TermsAndConditionsTemplate"/> entities.
/// </summary>
public interface ITermsAndConditionsTemplateService
{
    /// <summary>
    /// Asynchronously retrieves all terms and conditions templates.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// object with a list of <see cref="TermsAndConditionsTemplateDto"/> representing the retrieved templates.</returns>
    Task<IBaseResult<List<TermsAndConditionsTemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a Terms and Conditions template by its unique identifier.
    /// </summary>
    /// <remarks>The method performs an asynchronous operation to retrieve the specified Terms and Conditions
    /// template. Ensure that the provided <paramref name="id"/> corresponds to an existing template.</remarks>
    /// <param name="id">The unique identifier of the Terms and Conditions template to retrieve. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> with
    /// the <see cref="TermsAndConditionsTemplateDto"/> if the template is found; otherwise, an appropriate result
    /// indicating the failure.</returns>
    Task<IBaseResult<TermsAndConditionsTemplateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously adds a new terms and conditions template.
    /// </summary>
    /// <remarks>The operation may fail if the provided template data is invalid or if a template with the
    /// same identifier already exists. Ensure that the <paramref name="dto"/> parameter contains valid and unique data
    /// before calling this method.</remarks>
    /// <param name="dto">The data transfer object containing the details of the terms and conditions template to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object that includes the added terms and conditions template or details about the operation's outcome.</returns>
    Task<IBaseResult<TermsAndConditionsTemplateDto>> AddAsync(TermsAndConditionsTemplateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing Terms and Conditions template with the specified data.
    /// </summary>
    /// <param name="dto">The data transfer object containing the updated details of the Terms and Conditions template.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object that indicates the outcome of the operation, including the updated template details if successful.</returns>
    Task<IBaseResult<TermsAndConditionsTemplateDto>> EditAsync(TermsAndConditionsTemplateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the resource identified by the specified ID asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the resource to delete. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result contains an  <see cref="IBaseResult"/>
    /// indicating the outcome of the operation.</returns>
    Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default);
}
