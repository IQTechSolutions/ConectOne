using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices;

/// <summary>
/// Provides methods for managing short description templates through RESTful API calls.
/// </summary>
/// <remarks>This service allows clients to perform CRUD operations on short description templates, including
/// retrieving all templates,  fetching a specific template by ID, adding new templates, editing existing templates, and
/// deleting templates.  The service communicates with the underlying HTTP provider to execute these
/// operations.</remarks>
/// <param name="provider"></param>
public class ShortDescriptionTemplateRestService(IBaseHttpProvider provider) : IShortDescriptionTemplateService
{
    /// <summary>
    /// Retrieves all short description templates asynchronously.
    /// </summary>
    /// <remarks>This method fetches all available short description templates from the underlying data
    /// source. The operation is performed asynchronously and supports cancellation through the provided <paramref
    /// name="cancellationToken"/>.</remarks>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> of
    /// <see cref="IEnumerable{T}"/> containing <see cref="ShortDescriptionTemplateDto"/> objects.</returns>
    public async Task<IBaseResult<IEnumerable<ShortDescriptionTemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await provider.GetAsync<IEnumerable<ShortDescriptionTemplateDto>>("short-description-templates/all");
        return result;
    }

    /// <summary>
    /// Retrieves a short description template by its unique identifier.
    /// </summary>
    /// <remarks>This method retrieves the short description template from the underlying data provider. If
    /// the specified identifier does not exist, the result may indicate a failure or an empty response, depending on
    /// the provider's behavior.</remarks>
    /// <param name="id">The unique identifier of the short description template to retrieve. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object wrapping the <see cref="ShortDescriptionTemplateDto"/> corresponding to the specified identifier.</returns>
    public async Task<IBaseResult<ShortDescriptionTemplateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var result = await provider.GetAsync<ShortDescriptionTemplateDto>($"short-description-templates/{id}");
        return result;
    }

    /// <summary>
    /// Adds a new short description template asynchronously.
    /// </summary>
    /// <remarks>This method sends the provided short description template to the underlying provider for
    /// storage. Ensure that the <paramref name="dto"/> contains valid data before calling this method.</remarks>
    /// <param name="dto">The data transfer object representing the short description template to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object with the added <see cref="ShortDescriptionTemplateDto"/>.</returns>
    public async Task<IBaseResult<ShortDescriptionTemplateDto>> AddAsync(ShortDescriptionTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var result = await provider.PutAsync<ShortDescriptionTemplateDto, ShortDescriptionTemplateDto>("short-description-templates", dto);
        return result;
    }

    /// <summary>
    /// Updates an existing short description template asynchronously.
    /// </summary>
    /// <remarks>This method sends the updated template data to the underlying provider for processing. Ensure
    /// that the <paramref name="dto"/> parameter contains valid data before calling this method.</remarks>
    /// <param name="dto">The data transfer object containing the updated details of the short description template.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object wrapping the updated <see cref="ShortDescriptionTemplateDto"/>.</returns>
    public async Task<IBaseResult<ShortDescriptionTemplateDto>> EditAsync(ShortDescriptionTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var result = await provider.PostAsync<ShortDescriptionTemplateDto, ShortDescriptionTemplateDto>("short-description-templates", dto);
        return result;
    }

    /// <summary>
    /// Deletes a resource identified by the specified ID asynchronously.
    /// </summary>
    /// <remarks>This method performs the delete operation asynchronously. Ensure that the provided <paramref
    /// name="id"/> corresponds  to an existing resource. The operation may fail if the resource does not exist or if
    /// there are connectivity issues.</remarks>
    /// <param name="id">The unique identifier of the resource to delete. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result contains an <see cref="IBaseResult"/> 
    /// indicating the outcome of the operation.</returns>
    public async Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var result = await provider.DeleteAsync("short-description-templates", id);
        return result;
    }
}