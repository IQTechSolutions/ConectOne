using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;

namespace AccomodationModule.Infrastructure.Implimentation;

/// <summary>
/// Provides operations for managing booking terms description templates, including querying, adding, editing, and
/// deleting templates.
/// </summary>
/// <remarks>This service acts as an abstraction layer for interacting with booking terms description templates, 
/// enabling CRUD operations and ensuring proper handling of business logic and repository interactions.</remarks>
/// <param name="repository"></param>
public class BookingTermsDescriptionTemplateService(IRepository<BookingTermsDescriptionTemplate, string> repository) : IBookingTermsDescriptionTemplateService
{
    #region Query operations

    /// <summary>
    /// Retrieves all booking terms description templates asynchronously.
    /// </summary>
    /// <remarks>This method retrieves all booking terms description templates from the repository. If the
    /// operation  fails, the result will include the failure messages. The caller can use the <see
    /// cref="IBaseResult.Succeeded"/>  property to determine whether the operation was successful.</remarks>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  <IBaseResult{T}/> containing a
    /// list of <BookingTermsDescriptionTemplateDto/>  objects if the operation succeeds, or error messages if it fails.</returns>
    public async Task<IBaseResult<List<BookingTermsDescriptionTemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<BookingTermsDescriptionTemplate>(_ => true);

        var result = await repository.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<List<BookingTermsDescriptionTemplateDto>>.FailAsync(result.Messages);

        var dtos = result.Data.Select(b => new BookingTermsDescriptionTemplateDto(b)).ToList();
        return await Result<List<BookingTermsDescriptionTemplateDto>>.SuccessAsync(dtos);
    }

    /// <summary>
    /// Retrieves a booking terms description template by its unique identifier.
    /// </summary>
    /// <remarks>If the entity with the specified <paramref name="id"/> does not exist, the result will
    /// indicate failure with an appropriate error message.</remarks>
    /// <param name="id">The unique identifier of the booking terms description template to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> with
    /// the requested <see cref="BookingTermsDescriptionTemplateDto"/> if found, or an error message if the operation
    /// fails.</returns>
    public async Task<IBaseResult<BookingTermsDescriptionTemplateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<BookingTermsDescriptionTemplate>(b => b.Id == id);

        var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result<BookingTermsDescriptionTemplateDto>.FailAsync(
                result.Succeeded ? $"Entity with ID {id} not found." : string.Join(",", result.Messages));

        return await Result<BookingTermsDescriptionTemplateDto>.SuccessAsync(new BookingTermsDescriptionTemplateDto(result.Data));
    }

    #endregion

    #region Command operations

    /// <summary>
    /// Asynchronously adds a new booking terms description template to the repository.
    /// </summary>
    /// <remarks>This method performs the following steps: <list type="number"> <item>Creates a new booking
    /// terms description template in the repository using the provided <paramref name="dto"/>.</item> <item>Saves the
    /// changes to the repository.</item> <item>Returns a success result containing the added template if all operations
    /// succeed, or a failure result with error messages if any step fails.</item> </list></remarks>
    /// <param name="dto">The data transfer object containing the details of the booking terms description template to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// indicating the success or failure of the operation. If successful, the result contains the added <see
    /// cref="BookingTermsDescriptionTemplateDto"/>; otherwise, it contains error messages.</returns>
    public async Task<IBaseResult<BookingTermsDescriptionTemplateDto>> AddAsync(BookingTermsDescriptionTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var createResult =
            await repository.CreateAsync(new BookingTermsDescriptionTemplate { TemplateName = dto.TemplateName, TemplateDescription = dto.TemplateDescription ,Description = dto.Description }, cancellationToken);
        if (!createResult.Succeeded)
            return await Result<BookingTermsDescriptionTemplateDto>.FailAsync(createResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<BookingTermsDescriptionTemplateDto>.FailAsync(saveResult.Messages);

        return await Result<BookingTermsDescriptionTemplateDto>.SuccessAsync(dto);
    }

    /// <summary>
    /// Updates an existing booking terms description template with the provided data.
    /// </summary>
    /// <remarks>This method retrieves the entity to be updated, applies the changes specified in the
    /// <paramref name="dto"/>, and commits the modifications to the data store. If the entity is not found or any
    /// operation fails, the method returns a failure result with appropriate error messages.</remarks>
    /// <param name="dto">The data transfer object containing the updated values for the booking terms description template.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> with
    /// the updated <see cref="BookingTermsDescriptionTemplateDto"/> if the operation succeeds; otherwise, it contains
    /// error messages describing the failure.</returns>
    public async Task<IBaseResult<BookingTermsDescriptionTemplateDto>> EditAsync(BookingTermsDescriptionTemplateDto dto, CancellationToken cancellationToken = default)
    {
        // Retrieve the entity with tracking enabled (true) so changes are detected automatically on SaveAsync.
        var spec = new LambdaSpec<BookingTermsDescriptionTemplate>(b => b.Id == dto.Id);
        var result = await repository.FirstOrDefaultAsync(spec, true, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result<BookingTermsDescriptionTemplateDto>.FailAsync(
                result.Succeeded ? $"Entity with ID {dto.Id} not found." : string.Join(",", result.Messages));

        // Apply business‑approved modifications.
        result.Data.TemplateName = dto.TemplateName;
        result.Data.TemplateDescription = dto.TemplateDescription;
        result.Data.Description = dto.Description;

        // Mark entity as modified. Some repository implementations may no‑op here when the entity is already tracked.
        var updateResult = repository.Update(result.Data);
        if (!updateResult.Succeeded)
            return await Result<BookingTermsDescriptionTemplateDto>.FailAsync(updateResult.Messages);

        // Commit modifications.
        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<BookingTermsDescriptionTemplateDto>.FailAsync(saveResult.Messages);

        return await Result<BookingTermsDescriptionTemplateDto>.SuccessAsync(dto);
    }

    /// <summary>
    /// Deletes an entity identified by the specified ID asynchronously.
    /// </summary>
    /// <remarks>The delete operation may be a soft delete or a hard delete, depending on the repository
    /// implementation. The operation is finalized by committing the unit of work. If the delete or commit operation
    /// fails, the result will contain the corresponding error messages.</remarks>
    /// <param name="id">The unique identifier of the entity to delete. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the delete operation.</returns>
    public async Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        // Soft‑ or hard‑delete depending on repository implementation.
        var result = await repository.DeleteAsync(id, cancellationToken);
        if (!result.Succeeded)
            return await Result.FailAsync(result.Messages);

        // Finalize operation by committing the unit of work.
        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync();
    }

    #endregion
}
