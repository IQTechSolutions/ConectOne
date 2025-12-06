using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices;

/// <summary>
/// Provides methods for managing cancellation terms templates through RESTful API calls.
/// </summary>
/// <remarks>This service allows clients to perform CRUD operations on cancellation terms templates, including
/// retrieving all templates,  fetching a specific template by its identifier, adding new templates, editing existing
/// templates, and deleting templates.</remarks>
/// <param name="provider"></param>
public class CancellationTermsTemplateRestService(IBaseHttpProvider provider) : ICancellationTermsTemplateService
{
    /// <summary>
    /// Retrieves all cancellation terms templates asynchronously.
    /// </summary>
    /// <remarks>This method fetches all available cancellation terms templates from the underlying data
    /// source. The operation can be canceled by passing a cancellation token.</remarks>
    /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// containing an <see cref="IEnumerable{T}"/> of <see cref="CancellationTermsTemplateDto"/> objects.</returns>
    public async Task<IBaseResult<IEnumerable<CancellationTermsTemplateDto>>> AllAsync(CancellationToken cancellationToken = default)
    {
        var result = await provider.GetAsync<IEnumerable<CancellationTermsTemplateDto>>("cancellation-terms-templates/all");
        return result;
    }

    /// <summary>
    /// Retrieves a cancellation terms template by its unique identifier.
    /// </summary>
    /// <remarks>The method performs an asynchronous operation to fetch the cancellation terms template from
    /// the underlying data provider. Ensure the <paramref name="id"/> corresponds to a valid template; otherwise, the
    /// result may indicate a failure.</remarks>
    /// <param name="id">The unique identifier of the cancellation terms template to retrieve. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object wrapping the <see cref="CancellationTermsTemplateDto"/> corresponding to the specified identifier.</returns>
    public async Task<IBaseResult<CancellationTermsTemplateDto>> ByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var result = await provider.GetAsync<CancellationTermsTemplateDto>($"cancellation-terms-templates/{id}");
        return result;
    }

    /// <summary>
    /// Adds a new cancellation terms template asynchronously.
    /// </summary>
    /// <remarks>This method sends the provided cancellation terms template to the underlying data provider
    /// for storage. Ensure that the <paramref name="dto"/> contains valid data before calling this method.</remarks>
    /// <param name="dto">The data transfer object representing the cancellation terms template to be added.</param>
    /// <param name="cancellationToken">An optional token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object wrapping the added <see cref="CancellationTermsTemplateDto"/>.</returns>
    public async Task<IBaseResult<CancellationTermsTemplateDto>> AddAsync(CancellationTermsTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var result = await provider.PutAsync<CancellationTermsTemplateDto, CancellationTermsTemplateDto>("cancellation-terms-templates", dto);
        return result;
    }

    /// <summary>
    /// Updates an existing cancellation terms template with the specified data.
    /// </summary>
    /// <remarks>This method sends the updated cancellation terms template data to the underlying provider for
    /// processing. Ensure that the <paramref name="dto"/> contains valid data before calling this method.</remarks>
    /// <param name="dto">The data transfer object containing the updated cancellation terms template details.</param>
    /// <param name="cancellationToken">An optional token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object wrapping the updated <see cref="CancellationTermsTemplateDto"/>.</returns>
    public async Task<IBaseResult<CancellationTermsTemplateDto>> EditAsync(CancellationTermsTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var result = await provider.PostAsync<CancellationTermsTemplateDto, CancellationTermsTemplateDto>("cancellation-terms-templates", dto);
        return result;
    }

    /// <summary>
    /// Deletes the specified resource asynchronously.
    /// </summary>
    /// <remarks>This method performs the delete operation on the resource identified by the specified
    /// <paramref name="id"/>. Ensure that the <paramref name="id"/> corresponds to an existing resource. The operation
    /// is asynchronous and  can be canceled by passing a <see cref="CancellationToken"/>.</remarks>
    /// <param name="id">The unique identifier of the resource to delete. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
    /// indicating the outcome of the delete operation.</returns>
    public async Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var result = await provider.DeleteAsync("cancellation-terms-templates", id);
        return result;
    }
}