using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces;

/// <summary>
/// Service contract for managing <see cref="PaymentExclusionTemplate"/> entities.
/// </summary>
public interface IPaymentExclusionTemplateService
{
    /// <summary>
    /// Retrieves all payment exclusion templates asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="PaymentExclusionTemplateDto"/> objects.</returns>
    Task<IBaseResult<IEnumerable<PaymentExclusionTemplateDto>>> AllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a payment exclusion template by its unique identifier.
    /// </summary>
    /// <remarks>Use this method to retrieve detailed information about a specific payment exclusion template.
    /// Ensure the provided <paramref name="id"/> corresponds to an existing template.</remarks>
    /// <param name="id">The unique identifier of the payment exclusion template to retrieve. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object wrapping the <see cref="PaymentExclusionTemplateDto"/> if the template is found; otherwise, an
    /// appropriate result indicating the failure.</returns>
    Task<IBaseResult<PaymentExclusionTemplateDto>> ByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously adds a new payment exclusion template.
    /// </summary>
    /// <param name="dto">The data transfer object containing the details of the payment exclusion template to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object that includes the added <see cref="PaymentExclusionTemplateDto"/> if the operation is successful.</returns>
    Task<IBaseResult<PaymentExclusionTemplateDto>> AddAsync(PaymentExclusionTemplateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing payment exclusion template with the specified details.
    /// </summary>
    /// <remarks>Use this method to modify an existing payment exclusion template. Ensure that the provided
    /// <paramref name="dto"/> contains valid data, as the operation may fail if the input is invalid.</remarks>
    /// <param name="dto">The data transfer object containing the updated details of the payment exclusion template.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object that includes the updated <see cref="PaymentExclusionTemplateDto"/> if the operation is successful.</returns>
    Task<IBaseResult<PaymentExclusionTemplateDto>> EditAsync(PaymentExclusionTemplateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the resource identified by the specified ID asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the resource to delete. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the delete operation.</returns>
    Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default);
}
