using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces;

/// <summary>
/// Service contract for managing <see cref="GeneralInformationTemplate"/> entities.
/// </summary>
public interface IGeneralInformationTemplateService
{
    /// <summary>
    /// Asynchronously retrieves all general information templates.
    /// </summary>
    /// <remarks>The operation may return an empty list if no templates are available. Ensure proper handling
    /// of the <see cref="IBaseResult"/> to check for success or failure of the operation.</remarks>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object with a list of <see cref="GeneralInformationTemplateDto"/> instances representing the retrieved
    /// templates.</returns>
    Task<IBaseResult<IEnumerable<GeneralInformationTemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Fetches a single template by its identifier and maps it to a DTO.
    /// </summary>
    /// <param name="id">Unique identifier of the template to retrieve.</param>
    /// <param name="cancellationToken">Token that can be used to cancel the database call.</param>
    Task<IBaseResult<GeneralInformationTemplateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously adds a new general information template.
    /// </summary>
    /// <remarks>The operation may be canceled by passing a cancellation token. Ensure the provided <paramref
    /// name="dto"/> contains valid data to avoid validation errors.</remarks>
    /// <param name="dto">The data transfer object containing the details of the general information template to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object wrapping the added <see cref="GeneralInformationTemplateDto"/> if the operation is successful.</returns>
    Task<IBaseResult<GeneralInformationTemplateDto>> AddAsync(GeneralInformationTemplateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing general information template with the provided data.
    /// </summary>
    /// <remarks>The operation will fail if the provided data does not meet the required validation criteria.
    /// Ensure that the cancellationToken is used to cancel the operation if needed.</remarks>
    /// <param name="dto">The data transfer object containing the updated information for the template.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} object wrapping
    /// the updated GeneralInformationTemplateDto.</returns>
    Task<IBaseResult<GeneralInformationTemplateDto>> EditAsync(GeneralInformationTemplateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Permanently removes a template from the data store.
    /// </summary>
    Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default);
}
