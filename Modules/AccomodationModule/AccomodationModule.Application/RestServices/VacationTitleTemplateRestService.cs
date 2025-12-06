using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices;

/// <summary>
/// Provides a REST-based implementation of the <see cref="IVacationTitleTemplateService"/> interface for managing
/// vacation title templates.
/// </summary>
/// <remarks>This service facilitates CRUD operations on vacation title templates by interacting with an
/// underlying HTTP provider.  It supports retrieving, adding, updating, and deleting vacation title templates
/// asynchronously.</remarks>
/// <param name="provider"></param>
public class VacationTitleTemplateRestService(IBaseHttpProvider provider) : IVacationTitleTemplateService
{
    /// <summary>
    /// Retrieves all vacation title templates asynchronously.
    /// </summary>
    /// <remarks>This method fetches all vacation title templates from the underlying data source. If
    /// no templates are available, the result will contain an empty collection.</remarks>
    /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// containing an enumerable collection of <see cref="VacationTitleTemplateDto"/> objects.</returns>
    public async Task<IBaseResult<IEnumerable<VacationTitleTemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await provider.GetAsync<IEnumerable<VacationTitleTemplateDto>>("vacation-title-templates/all");
        return result;
    }

    /// <summary>
    /// Retrieves a vacation title template by its unique identifier.
    /// </summary>
    /// <remarks>This method sends a request to retrieve the vacation title template associated with
    /// the specified identifier. Ensure the identifier is valid and corresponds to an existing template.</remarks>
    /// <param name="id">The unique identifier of the vacation title template to retrieve. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object wrapping the <see cref="VacationTitleTemplateDto"/> if the template is found.</returns>
    public async Task<IBaseResult<VacationTitleTemplateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var result = await provider.GetAsync<VacationTitleTemplateDto>($"vacation-title-templates/{id}");
        return result;
    }

    /// <summary>
    /// Adds a new vacation title template asynchronously.
    /// </summary>
    /// <remarks>This method sends the provided vacation title template to the underlying data
    /// provider for storage.</remarks>
    /// <param name="dto">The data transfer object representing the vacation title template to be added.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object that includes the added vacation title template.</returns>
    public async Task<IBaseResult<VacationTitleTemplateDto>> AddAsync(VacationTitleTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var result = await provider.PutAsync<VacationTitleTemplateDto, VacationTitleTemplateDto>("vacation-title-templates", dto);
        return result;
    }

    /// <summary>
    /// Updates an existing vacation title template with the specified data.
    /// </summary>
    /// <remarks>This method sends the updated vacation title template data to the underlying provider
    /// for processing. Ensure that the <paramref name="dto"/> contains valid data before calling this
    /// method.</remarks>
    /// <param name="dto">The data transfer object containing the updated details of the vacation title template.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object with the updated <see cref="VacationTitleTemplateDto"/> if the operation is successful.</returns>
    public async Task<IBaseResult<VacationTitleTemplateDto>> EditAsync(VacationTitleTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var result = await provider.PostAsync<VacationTitleTemplateDto, VacationTitleTemplateDto>("vacation-title-templates", dto);
        return result;
    }

    /// <summary>
    /// Deletes a resource with the specified identifier asynchronously.
    /// </summary>
    /// <remarks>This method performs an asynchronous delete operation on the resource identified by
    /// <paramref name="id"/>. Ensure that the identifier corresponds to an existing resource before calling this
    /// method.</remarks>
    /// <param name="id">The unique identifier of the resource to delete. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result contains an <see
    /// cref="IBaseResult"/> indicating the outcome of the operation.</returns>
    public async Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var result = await provider.DeleteAsync("vacation-title-templates", id);
        return result;
    }
}