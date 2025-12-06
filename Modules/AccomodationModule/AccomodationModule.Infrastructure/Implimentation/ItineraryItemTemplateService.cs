using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;

namespace AccomodationModule.Infrastructure.Implimentation;

/// <summary>
/// Provides operations for managing itinerary item templates, including querying, adding, editing, and deleting
/// templates.
/// </summary>
/// <remarks>This service interacts with a repository to perform CRUD operations on <see
/// cref="ItineraryEntryItemTemplate"/> entities. It returns results wrapped in <see cref="IBaseResult"/> to indicate
/// success or failure, along with any relevant messages.</remarks>
/// <param name="repository"></param>
public class ItineraryItemTemplateService(IRepository<ItineraryEntryItemTemplate, string> repository) : IItineraryItemTemplateService
{
    #region Query operations

    /// <summary>
    /// Retrieves all itinerary entry item templates asynchronously.
    /// </summary>
    /// <remarks>This method retrieves all available itinerary entry item templates from the repository.  If
    /// the operation fails, the returned result will include the failure messages.</remarks>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  <IBaseResult{T}/> containing a
    /// list of <ItineraryEntryItemTemplateDto/> objects  if the operation succeeds, or error messages if it fails.</returns>
    public async Task<IBaseResult<List<ItineraryEntryItemTemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<ItineraryEntryItemTemplate>(_ => true);
        var result = await repository.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<List<ItineraryEntryItemTemplateDto>>.FailAsync(result.Messages);
        var dtos = result.Data.Select(e => new ItineraryEntryItemTemplateDto(e)).ToList();
        return await Result<List<ItineraryEntryItemTemplateDto>>.SuccessAsync(dtos);
    }

    /// <summary>
    /// Retrieves an itinerary entry item template by its unique identifier.
    /// </summary>
    /// <remarks>If the entity with the specified <paramref name="id"/> is not found, the result will indicate
    /// failure with an appropriate error message.</remarks>
    /// <param name="id">The unique identifier of the itinerary entry item template to retrieve. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> with
    /// the requested <see cref="ItineraryEntryItemTemplateDto"/> if found, or an error message if the operation fails.</returns>
    public async Task<IBaseResult<ItineraryEntryItemTemplateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<ItineraryEntryItemTemplate>(e => e.Id == id);
        var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result<ItineraryEntryItemTemplateDto>.FailAsync(
                result.Succeeded ? $"Entity with ID {id} not found." : string.Join(',', result.Messages));
        return await Result<ItineraryEntryItemTemplateDto>.SuccessAsync(new ItineraryEntryItemTemplateDto(result.Data));
    }
    #endregion

    #region Command operations

    /// <summary>
    /// Asynchronously adds a new itinerary entry item template to the repository.
    /// </summary>
    /// <remarks>The method attempts to add the provided itinerary entry item template to the repository.  If
    /// the operation fails at any stage (creation or saving), the returned result will contain  the failure messages.
    /// The operation is performed asynchronously.</remarks>
    /// <param name="dto">The data transfer object representing the itinerary entry item template to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  <IBaseResult{T}/> object, which
    /// indicates the success or failure of the operation  and includes the added <ItineraryEntryItemTemplateDto/> if
    /// successful.</returns>
    public async Task<IBaseResult<ItineraryEntryItemTemplateDto>> AddAsync(ItineraryEntryItemTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var createResult = await repository.CreateAsync(new ItineraryEntryItemTemplate
        {
            Id = dto.Id,
            DayNr = dto.DayNr,
            Content = dto.Content,
            VacationId = dto.VacationId
        }, cancellationToken);
        if (!createResult.Succeeded)
            return await Result<ItineraryEntryItemTemplateDto>.FailAsync(createResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<ItineraryEntryItemTemplateDto>.FailAsync(saveResult.Messages);

        return await Result<ItineraryEntryItemTemplateDto>.SuccessAsync(dto);
    }

    /// <summary>
    /// Updates an existing itinerary entry item template with the provided data.
    /// </summary>
    /// <remarks>This method attempts to update an existing itinerary entry item template identified by the
    /// <c>Id</c> property in the provided <paramref name="dto"/>. If the entity is not found or the update fails, the
    /// result will contain appropriate error messages. The operation is transactional, ensuring that changes are only
    /// saved if all steps succeed.</remarks>
    /// <param name="dto">The data transfer object containing the updated values for the itinerary entry item template.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> with
    /// the updated <see cref="ItineraryEntryItemTemplateDto"/> if the operation succeeds, or error messages if it
    /// fails.</returns>
    public async Task<IBaseResult<ItineraryEntryItemTemplateDto>> EditAsync(ItineraryEntryItemTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<ItineraryEntryItemTemplate>(e => e.Id == dto.Id);
        var result = await repository.FirstOrDefaultAsync(spec, true, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result<ItineraryEntryItemTemplateDto>.FailAsync(
                result.Succeeded ? $"Entity with ID {dto.Id} not found." : string.Join(',', result.Messages));

        result.Data.DayNr = dto.DayNr;
        result.Data.Content = dto.Content;

        var updateResult = repository.Update(result.Data);
        if (!updateResult.Succeeded)
            return await Result<ItineraryEntryItemTemplateDto>.FailAsync(updateResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<ItineraryEntryItemTemplateDto>.FailAsync(saveResult.Messages);

        return await Result<ItineraryEntryItemTemplateDto>.SuccessAsync(dto);
    }

    /// <summary>
    /// Deletes an entity with the specified identifier and commits the changes asynchronously.
    /// </summary>
    /// <remarks>This method first attempts to delete the entity identified by <paramref name="id"/>.  If the
    /// deletion succeeds, it commits the changes to the underlying data store.  If either the deletion or the commit
    /// fails, the operation returns a failure result  with the associated error messages.</remarks>
    /// <param name="id">The unique identifier of the entity to delete. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  <IBaseResult> indicating the
    /// success or failure of the operation.  If the operation fails, the result includes error messages.</returns>
    public async Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var deleteResult = await repository.DeleteAsync(id, cancellationToken);
        if (!deleteResult.Succeeded)
            return await Result.FailAsync(deleteResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync();
    }
    #endregion
}
