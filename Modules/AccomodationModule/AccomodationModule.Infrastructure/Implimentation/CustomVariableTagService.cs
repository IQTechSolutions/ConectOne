using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;

namespace AccomodationModule.Infrastructure.Implimentation;

/// <summary>
/// Provides CRUD operations for <see cref="CustomVariableTag"/> entities.
/// </summary>
public class CustomVariableTagService(IRepository<CustomVariableTag, string> repository) : ICustomVariableTagService
{
    #region Query operations

    /// <summary>
    /// Retrieves all custom variable tags as a list of data transfer objects (DTOs).
    /// </summary>
    /// <remarks>This method queries all custom variable tags from the repository, converts them into DTOs,
    /// and returns the result. If the operation fails, the returned result will include the failure messages.</remarks>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// object with a list of <see cref="CustomVariableTagDto"/> instances if the operation succeeds, or error messages
    /// if it fails.</returns>
    public async Task<IBaseResult<IEnumerable<CustomVariableTagDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<CustomVariableTag>(_ => true);
        var result = await repository.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<List<CustomVariableTagDto>>.FailAsync(result.Messages);
        var dtos = result.Data.Select(e => new CustomVariableTagDto(e)).ToList();
        return await Result<List<CustomVariableTagDto>>.SuccessAsync(dtos);
    }

    /// <summary>
    /// Retrieves a <see cref="CustomVariableTagDto"/> by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the custom variable tag to retrieve. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> with
    /// the requested <see cref="CustomVariableTagDto"/> if found, or an error message if the entity is not found or the
    /// operation fails.</returns>
    public async Task<IBaseResult<CustomVariableTagDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<CustomVariableTag>(e => e.Id == id);
        var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result<CustomVariableTagDto>.FailAsync(
                result.Succeeded ? $"Entity with ID {id} not found." : string.Join(",", result.Messages));
        return await Result<CustomVariableTagDto>.SuccessAsync(new CustomVariableTagDto(result.Data));
    }
    
    #endregion

    #region Command operations

    /// <summary>
    /// Asynchronously adds a new custom variable tag to the repository.
    /// </summary>
    /// <remarks>This method performs two operations: it creates the custom variable tag in the repository 
    /// and then saves the changes. If either operation fails, the method returns a failure result  with the
    /// corresponding error messages.</remarks>
    /// <param name="dto">The data transfer object containing the details of the custom variable tag to add.  The <paramref name="dto"/>
    /// must not be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Passing a canceled token will  result in the operation being
    /// canceled.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
    /// object, which indicates the success or failure of the operation.  If successful, the result contains the added
    /// <see cref="CustomVariableTagDto"/>; otherwise,  it contains error messages describing the failure.</returns>
    public async Task<IBaseResult<CustomVariableTagDto>> AddAsync(CustomVariableTagDto dto, CancellationToken cancellationToken = default)
    {
        var createResult = await repository.CreateAsync(new CustomVariableTag
        {
            Id = dto.Id,
            Name = dto.Name,
            Value = dto.Value,
            Description = dto.Description
        }, cancellationToken);
        if (!createResult.Succeeded)
            return await Result<CustomVariableTagDto>.FailAsync(createResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<CustomVariableTagDto>.FailAsync(saveResult.Messages);

        return await Result<CustomVariableTagDto>.SuccessAsync(dto);
    }

    /// <summary>
    /// Updates an existing custom variable tag with the specified data.
    /// </summary>
    /// <remarks>This method retrieves the custom variable tag by its ID, updates its properties with the
    /// values from the provided <paramref name="dto"/>, and saves the changes to the repository. If the entity is not
    /// found or any operation fails, an appropriate error result is returned.</remarks>
    /// <param name="dto">The data transfer object containing the updated values for the custom variable tag.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> with
    /// the updated <see cref="CustomVariableTagDto"/> if the operation succeeds, or error messages if it fails.</returns>
    public async Task<IBaseResult<CustomVariableTagDto>> EditAsync(CustomVariableTagDto dto, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<CustomVariableTag>(e => e.Id == dto.Id);
        var result = await repository.FirstOrDefaultAsync(spec, true, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result<CustomVariableTagDto>.FailAsync(
                result.Succeeded ? $"Entity with ID {dto.Id} not found." : string.Join(",", result.Messages));

        result.Data.Name = dto.Name;
        result.Data.Value = dto.Value;
        result.Data.Description = dto.Description;

        var updateResult = repository.Update(result.Data);
        if (!updateResult.Succeeded)
            return await Result<CustomVariableTagDto>.FailAsync(updateResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<CustomVariableTagDto>.FailAsync(saveResult.Messages);

        return await Result<CustomVariableTagDto>.SuccessAsync(dto);
    }

    /// <summary>
    /// Deletes an entity with the specified identifier asynchronously.
    /// </summary>
    /// <remarks>The operation involves deleting the entity and saving the changes to the underlying data
    /// store.  If the deletion or save operation fails, the result will include the corresponding error
    /// messages.</remarks>
    /// <param name="id">The unique identifier of the entity to delete. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation.</returns>
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
