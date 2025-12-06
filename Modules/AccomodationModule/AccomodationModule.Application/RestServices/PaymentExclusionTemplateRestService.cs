using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Application.RestServices;

/// <summary>
/// Provides methods for managing payment exclusion templates through RESTful API calls.
/// </summary>
/// <remarks>This service allows clients to perform CRUD operations on payment exclusion templates, including
/// retrieving all templates,  fetching a specific template by ID, adding new templates, editing existing templates, and
/// deleting templates.  The service communicates with the underlying HTTP provider to execute these
/// operations.</remarks>
/// <param name="provider"></param>
public class PaymentExclusionTemplateRestService(IBaseHttpProvider provider) : IPaymentExclusionTemplateService
{
    /// <summary>
    /// Retrieves all payment exclusion templates.
    /// </summary>
    /// <remarks>This method fetches all available payment exclusion templates from the underlying data
    /// provider.</remarks>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// containing an <see cref="IEnumerable{T}"/> of <see cref="PaymentExclusionTemplateDto"/> objects.</returns>
    public async Task<IBaseResult<IEnumerable<PaymentExclusionTemplateDto>>> AllAsync(CancellationToken cancellationToken = default)
    {
        var result = await provider.GetAsync<IEnumerable<PaymentExclusionTemplateDto>>("payment-exclusion-templates/all");
        return result;
    }

    /// <summary>
    /// Retrieves a payment exclusion template by its unique identifier.
    /// </summary>
    /// <remarks>This method retrieves the payment exclusion template from the underlying data provider. If
    /// the specified identifier does not exist, the result may indicate a failure or an empty response, depending on
    /// the provider's behavior.</remarks>
    /// <param name="id">The unique identifier of the payment exclusion template to retrieve. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object wrapping the <see cref="PaymentExclusionTemplateDto"/> corresponding to the specified identifier.</returns>
    public async Task<IBaseResult<PaymentExclusionTemplateDto>> ByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var result = await provider.GetAsync<PaymentExclusionTemplateDto>($"payment-exclusion-templates/{id}");
        return result;
    }

    /// <summary>
    /// Adds a new payment exclusion template asynchronously.
    /// </summary>
    /// <remarks>This method sends the provided payment exclusion template to the underlying provider for
    /// storage. Ensure that the <paramref name="dto"/> contains valid data before calling this method.</remarks>
    /// <param name="dto">The data transfer object representing the payment exclusion template to be added.</param>
    /// <param name="cancellationToken">An optional token to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object wrapping the added <see cref="PaymentExclusionTemplateDto"/>.</returns>
    public async Task<IBaseResult<PaymentExclusionTemplateDto>> AddAsync(PaymentExclusionTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var result = await provider.PutAsync<PaymentExclusionTemplateDto, PaymentExclusionTemplateDto>("payment-exclusion-templates", dto);
        return result;
    }

    /// <summary>
    /// Updates an existing payment exclusion template asynchronously.
    /// </summary>
    /// <remarks>This method sends the updated payment exclusion template to the underlying provider for
    /// processing. Ensure that the <paramref name="dto"/> contains valid data before calling this method.</remarks>
    /// <param name="dto">The data transfer object representing the payment exclusion template to be updated.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object that includes the updated <see cref="PaymentExclusionTemplateDto"/> if the operation is successful.</returns>
    public async Task<IBaseResult<PaymentExclusionTemplateDto>> EditAsync(PaymentExclusionTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var result = await provider.PostAsync<PaymentExclusionTemplateDto, PaymentExclusionTemplateDto>("payment-exclusion-templates", dto);
        return result;
    }

    /// <summary>
    /// Deletes a payment exclusion template with the specified identifier.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to delete a payment exclusion template. Ensure
    /// that the specified <paramref name="id"/> corresponds to an existing template.</remarks>
    /// <param name="id">The unique identifier of the payment exclusion template to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the operation.</returns>
    public async Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var result = await provider.DeleteAsync("payment-exclusion-templates", id);
        return result;
    }
}