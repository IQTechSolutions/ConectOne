using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.Discipline;

namespace SchoolsModule.Infrastructure.Implementation.Disciplinary;

/// <summary>
/// Provides services for managing disciplinary actions and severity scales within a school module.
/// </summary>
/// <remarks>This service acts as a mediator between the repository layer and the application layer, offering
/// methods to retrieve, create, update, and delete disciplinary actions and severity scales. It ensures that data is
/// appropriately mapped to and from data transfer objects (DTOs) and handles repository operations, including error
/// handling and validation.</remarks>
/// <param name="repo"></param>
public class DisciplinaryActionService(ISchoolsModuleRepoManager repo) : IDisciplinaryActionService
{
    /// <summary>
    /// Retrieves all severity scales as a collection of data transfer objects (DTOs).
    /// </summary>
    /// <remarks>This method queries the repository for severity scales and maps the results to DTOs. If the
    /// repository operation fails, the method returns a failure result with the associated error messages.</remarks>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will terminate early if the token is canceled.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> with
    /// a collection of <see cref="SeverityScaleDto"/> objects if the operation succeeds, or error messages if it fails.</returns>
    public async Task<IBaseResult<IEnumerable<SeverityScaleDto>>> AllSeverityScalesAsync(CancellationToken cancellationToken)
    {
        var result = await repo.SeverityScales.ListAsync(false, cancellationToken);
        if (!result.Succeeded) return await Result<IEnumerable<SeverityScaleDto>>.FailAsync(result.Messages);
        var dtos = result.Data.Select(s => new SeverityScaleDto(s));
        return await Result<IEnumerable<SeverityScaleDto>>.SuccessAsync(dtos.ToList());
    }

    /// <summary>
    /// Retrieves the severity scale associated with the specified scale identifier.
    /// </summary>
    /// <remarks>This method queries the repository for a severity scale matching the provided identifier.  If
    /// no matching scale is found, the result will indicate failure with an appropriate error message.</remarks>
    /// <param name="scaleId">The unique identifier of the severity scale to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} of type
    /// SeverityScaleDto. If the scale is found,  the result contains the corresponding severity scale data; otherwise,
    /// the result indicates failure.</returns>
    public async Task<IBaseResult<SeverityScaleDto>> SeverityScaleAsync(string scaleId, CancellationToken cancellationToken)
    {
        var spec = new LambdaSpec<SeverityScale>(s => s.Id == scaleId);
        var result = await repo.SeverityScales.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded || result.Data is null)
            return await Result<SeverityScaleDto>.FailAsync("Scale not found");
        return await Result<SeverityScaleDto>.SuccessAsync(new SeverityScaleDto(result.Data));
    }

    /// <summary>
    /// Creates a new severity scale and persists it to the repository.
    /// </summary>
    /// <remarks>This method validates the provided severity scale, creates the corresponding entity, and
    /// saves it to the repository. If the creation or save operation fails, the result will contain the associated
    /// error messages.</remarks>
    /// <param name="scale">The data transfer object representing the severity scale to be created.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation. If successful, the result includes a success message.</returns>
    public async Task<IBaseResult> CreateSeverityScaleAsync(SeverityScaleDto scale, CancellationToken cancellationToken)
    {
        var entity = scale.CreateSeverityScale();
        var create = await repo.SeverityScales.CreateAsync(entity, cancellationToken);
        if (!create.Succeeded) return await Result.FailAsync(create.Messages);
        var save = await repo.SeverityScales.SaveAsync(cancellationToken);
        if (!save.Succeeded) return await Result.FailAsync(save.Messages);
        return await Result.SuccessAsync("Scale created");
    }

    /// <summary>
    /// Updates an existing severity scale with the specified details.
    /// </summary>
    /// <remarks>The method attempts to locate the severity scale by its ID. If the scale is found, its
    /// properties are updated with the provided values, and the changes are saved to the repository. If the scale is
    /// not found or the update fails, the result will indicate the failure with appropriate messages.</remarks>
    /// <param name="scale">The updated severity scale details, including the ID of the scale to update.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation. If successful, the result includes a success message;
    /// otherwise, it contains error messages.</returns>
    public async Task<IBaseResult> UpdateSeverityScaleAsync(SeverityScaleDto scale, CancellationToken cancellationToken)
    {
        var spec = new LambdaSpec<SeverityScale>(s => s.Id == scale.SeverityScaleId);
        var lookup = await repo.SeverityScales.FirstOrDefaultAsync(spec, true, cancellationToken);
        if (!lookup.Succeeded || lookup.Data is null)
            return await Result.FailAsync("Scale not found");
        var entity = lookup.Data;
        entity.Name = scale.Name;
        entity.Score = scale.Score;
        entity.Description = scale.Description;
        var update = repo.SeverityScales.Update(entity);
        if (!update.Succeeded) return await Result.FailAsync(update.Messages);
        var save = await repo.SeverityScales.SaveAsync(cancellationToken);
        if (!save.Succeeded) return await Result.FailAsync(save.Messages);
        return await Result.SuccessAsync("Scale updated");
    }

    /// <summary>
    /// Deletes a severity scale with the specified identifier.
    /// </summary>
    /// <remarks>This method performs two operations: deleting the severity scale and saving the changes. If
    /// either operation fails, the method returns a failure result with the corresponding error messages.</remarks>
    /// <param name="scaleId">The unique identifier of the severity scale to delete. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the operation. Returns a successful result if the severity scale was deleted and
    /// changes were saved successfully; otherwise, returns a failure result with error messages.</returns>
    public async Task<IBaseResult> DeleteSeverityScaleAsync(string scaleId, CancellationToken cancellationToken)
    {
        var delete = await repo.SeverityScales.DeleteAsync(scaleId, cancellationToken);
        if (!delete.Succeeded) return await Result.FailAsync(delete.Messages);
        var save = await repo.SeverityScales.SaveAsync(cancellationToken);
        if (!save.Succeeded) return await Result.FailAsync(save.Messages);
        return await Result.SuccessAsync("Scale deleted");
    }

    /// <summary>
    /// Retrieves all disciplinary actions as a collection of DTOs.
    /// </summary>
    /// <remarks>This method queries the repository for disciplinary actions and maps them to DTOs.  If the
    /// repository operation fails, the result will indicate failure and include the  corresponding error
    /// messages.</remarks>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} where T is an
    /// IEnumerable{T} of  DisciplinaryActionDto. If the operation succeeds, the result contains the  collection of
    /// disciplinary actions; otherwise, it contains error messages.</returns>
    public async Task<IBaseResult<IEnumerable<DisciplinaryActionDto>>> AllActionsAsync(CancellationToken cancellationToken)
    {
        var result = await repo.DisciplinaryActions.ListAsync(false, cancellationToken);
        if (!result.Succeeded) return await Result<IEnumerable<DisciplinaryActionDto>>.FailAsync(result.Messages);
        var dtos = result.Data.Select(a => new DisciplinaryActionDto(a));
        return await Result<IEnumerable<DisciplinaryActionDto>>.SuccessAsync(dtos.ToList());
    }

    /// <summary>
    /// Retrieves a disciplinary action by its unique identifier and returns the result as a DTO.
    /// </summary>
    /// <remarks>This method performs a lookup for a disciplinary action based on the provided <paramref
    /// name="actionId"/>. If the action is not found, the result will indicate failure with the message "Action not
    /// found."</remarks>
    /// <param name="actionId">The unique identifier of the disciplinary action to retrieve. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// where <c>T</c> is <see cref="DisciplinaryActionDto"/>. If the disciplinary action is found, the result contains
    /// the DTO; otherwise, the result indicates failure with an appropriate error message.</returns>
    public async Task<IBaseResult<DisciplinaryActionDto>> ActionAsync(string actionId, CancellationToken cancellationToken)
    {
        var spec = new LambdaSpec<DisciplinaryAction>(a => a.Id == actionId);
        var result = await repo.DisciplinaryActions.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded || result.Data is null)
            return await Result<DisciplinaryActionDto>.FailAsync("Action not found");
        return await Result<DisciplinaryActionDto>.SuccessAsync(new DisciplinaryActionDto(result.Data));
    }

    /// <summary>
    /// Creates a new disciplinary action asynchronously.
    /// </summary>
    /// <remarks>This method validates and persists the provided disciplinary action. If the creation or save
    /// operation fails, the result will indicate the failure along with the associated error messages.</remarks>
    /// <param name="action">The data transfer object containing the details of the disciplinary action to create.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the operation. If successful, the result includes a success message; otherwise, it
    /// contains error messages.</returns>
    public async Task<IBaseResult> CreateActionAsync(DisciplinaryActionDto action, CancellationToken cancellationToken)
    {
        var entity = action.CreateDisciplinaryAction();
        var create = await repo.DisciplinaryActions.CreateAsync(entity, cancellationToken);
        if (!create.Succeeded) return await Result.FailAsync(create.Messages);
        
        var save = await repo.DisciplinaryActions.SaveAsync(cancellationToken);
        if (!save.Succeeded) return await Result.FailAsync(save.Messages);
        
        return await Result.SuccessAsync("Action created");
    }

    /// <summary>
    /// Updates an existing disciplinary action with the provided details.
    /// </summary>
    /// <remarks>The method attempts to locate the disciplinary action by its identifier. If the action is not
    /// found, the operation fails with an appropriate error message. If the update or save operation fails, the result
    /// will include the corresponding error messages.</remarks>
    /// <param name="action">The data transfer object containing the updated details of the disciplinary action.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the update operation. If successful, the result includes a success message;
    /// otherwise, it contains error messages.</returns>
    public async Task<IBaseResult> UpdateActionAsync(DisciplinaryActionDto action, CancellationToken cancellationToken)
    {
        var spec = new LambdaSpec<DisciplinaryAction>(a => a.Id == action.DisciplinaryActionId);
        var lookup = await repo.DisciplinaryActions.FirstOrDefaultAsync(spec, true, cancellationToken);
        if (!lookup.Succeeded || lookup.Data is null)
            return await Result.FailAsync("Action not found");
        var entity = lookup.Data;
        entity.Name = action.Name;
        entity.Description = action.Description;
        entity.SeverityScaleId = action.SeverityScaleId;
        var update = repo.DisciplinaryActions.Update(entity);
        if (!update.Succeeded) return await Result.FailAsync(update.Messages);
        var save = await repo.DisciplinaryActions.SaveAsync(cancellationToken);
        if (!save.Succeeded) return await Result.FailAsync(save.Messages);
        return await Result.SuccessAsync("Action updated");
    }

    /// <summary>
    /// Deletes a disciplinary action identified by the specified action ID.
    /// </summary>
    /// <remarks>This method performs the deletion and ensures the changes are saved to the repository. If the
    /// deletion or save operation fails, the result will contain the corresponding error messages.</remarks>
    /// <param name="actionId">The unique identifier of the disciplinary action to delete. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the operation. Returns a successful result if the action is deleted successfully;
    /// otherwise, returns a failure result with error messages.</returns>
    public async Task<IBaseResult> DeleteActionAsync(string actionId, CancellationToken cancellationToken)
    {
        var delete = await repo.DisciplinaryActions.DeleteAsync(actionId, cancellationToken);
        if (!delete.Succeeded) return await Result.FailAsync(delete.Messages);
        var save = await repo.DisciplinaryActions.SaveAsync(cancellationToken);
        if (!save.Succeeded) return await Result.FailAsync(save.Messages);
        return await Result.SuccessAsync("Action deleted");
    }
}
