using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;

namespace AccomodationModule.Infrastructure.Implimentation;

/// <summary>
/// Service providing CRUD operations for <see cref="PaymentExclusionTemplate"/> entities.
/// </summary>
public class PaymentExclusionTemplateService(IRepository<PaymentExclusionTemplate, string> repository) : IPaymentExclusionTemplateService
{
    #region Query operations

    /// <summary>
    /// Retrieves all payment exclusion templates as a collection of DTOs.
    /// </summary>
    /// <remarks>This method retrieves all payment exclusion templates from the repository and maps them to
    /// DTOs. If the operation fails, the result will contain error messages describing the failure.</remarks>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="PaymentExclusionTemplateDto"/>. The result
    /// indicates whether the operation succeeded and includes the retrieved data or error messages.</returns>
    public async Task<IBaseResult<IEnumerable<PaymentExclusionTemplateDto>>> AllAsync(CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<PaymentExclusionTemplate>(_ => true);
        var result = await repository.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<List<PaymentExclusionTemplateDto>>.FailAsync(result.Messages);
        var dtos = result.Data.Select(e => new PaymentExclusionTemplateDto(e)).ToList();
        return await Result<List<PaymentExclusionTemplateDto>>.SuccessAsync(dtos);
    }

    /// <summary>
    /// Retrieves a payment exclusion template by its unique identifier.
    /// </summary>
    /// <remarks>If the specified <paramref name="id"/> does not exist, the result will indicate failure with
    /// an appropriate error message.</remarks>
    /// <param name="id">The unique identifier of the payment exclusion template to retrieve. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> with
    /// the <see cref="PaymentExclusionTemplateDto"/> if the operation succeeds, or an error message if the template is
    /// not found or the operation fails.</returns>
    public async Task<IBaseResult<PaymentExclusionTemplateDto>> ByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<PaymentExclusionTemplate>(e => e.Id == id);
        var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result<PaymentExclusionTemplateDto>.FailAsync(
                result.Succeeded ? $"Entity with ID {id} not found." : string.Join(",", result.Messages));
        return await Result<PaymentExclusionTemplateDto>.SuccessAsync(new PaymentExclusionTemplateDto(result.Data));
    }

    #endregion

    #region Command operations

    /// <summary>
    /// Asynchronously adds a new payment exclusion template to the repository.
    /// </summary>
    /// <remarks>This method performs the following steps: <list type="number"> <item>Creates a new payment
    /// exclusion template in the repository using the provided <paramref name="dto"/>.</item> <item>Saves the changes
    /// to the repository.</item> </list> If the operation fails at any step, the result will contain the failure
    /// messages.</remarks>
    /// <param name="dto">The data transfer object containing the details of the payment exclusion template to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// where <typeparamref name="T"/> is <see cref="PaymentExclusionTemplateDto"/>. The result indicates whether the
    /// operation succeeded and includes the added template details if successful.</returns>
    public async Task<IBaseResult<PaymentExclusionTemplateDto>> AddAsync(PaymentExclusionTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var createResult = await repository.CreateAsync(new PaymentExclusionTemplate
        {
            TemplateName = dto.TemplateName,
            TemplateDescription = dto.TemplateDescription,
            Description = dto.Description
        }, cancellationToken);
        if (!createResult.Succeeded)
            return await Result<PaymentExclusionTemplateDto>.FailAsync(createResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<PaymentExclusionTemplateDto>.FailAsync(saveResult.Messages);

        return await Result<PaymentExclusionTemplateDto>.SuccessAsync(dto);
    }

    /// <summary>
    /// Updates an existing payment exclusion template with the provided data.
    /// </summary>
    /// <remarks>This method performs the following steps: <list type="number"> <item>Retrieves the existing
    /// payment exclusion template by its ID.</item> <item>Updates the template's properties with the values provided in
    /// <paramref name="dto"/>.</item> <item>Persists the changes to the repository.</item> </list> If the template with
    /// the specified ID does not exist, or if any repository operation fails, the method returns a failure result with
    /// appropriate error messages.</remarks>
    /// <param name="dto">The data transfer object containing the updated values for the payment exclusion template. The <see
    /// cref="PaymentExclusionTemplateDto.Id"/> property must match the ID of an existing template.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// where <typeparamref name="T"/> is <see cref="PaymentExclusionTemplateDto"/>. If the operation succeeds, the
    /// result contains the updated template. If the operation fails, the result contains error messages.</returns>
    public async Task<IBaseResult<PaymentExclusionTemplateDto>> EditAsync(PaymentExclusionTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<PaymentExclusionTemplate>(e => e.Id == dto.Id);
        var result = await repository.FirstOrDefaultAsync(spec, true, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result<PaymentExclusionTemplateDto>.FailAsync(
                result.Succeeded ? $"Entity with ID {dto.Id} not found." : string.Join(",", result.Messages));

        result.Data.TemplateName = dto.TemplateName;
        result.Data.TemplateDescription = dto.TemplateDescription;
        result.Data.Description = dto.Description;

        var updateResult = repository.Update(result.Data);
        if (!updateResult.Succeeded)
            return await Result<PaymentExclusionTemplateDto>.FailAsync(updateResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<PaymentExclusionTemplateDto>.FailAsync(saveResult.Messages);

        return await Result<PaymentExclusionTemplateDto>.SuccessAsync(dto);
    }

    /// <summary>
    /// Deletes an entity with the specified identifier asynchronously.
    /// </summary>
    /// <remarks>This method attempts to delete the entity identified by <paramref name="id"/> and save the
    /// changes to the underlying data store.  If the delete operation or save operation fails, the result will contain
    /// the corresponding error messages.</remarks>
    /// <param name="id">The unique identifier of the entity to delete. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the delete operation. The result will indicate success if the entity was deleted and
    /// changes were saved successfully; otherwise, it will contain error messages.</returns>
    public async Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var result = await repository.DeleteAsync(id, cancellationToken);
        if (!result.Succeeded)
            return await Result.FailAsync(result.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync();
    }

    #endregion
}
