using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;

namespace AccomodationModule.Infrastructure.Implimentation;

/// <summary>
/// Application-layer service for CRUD operations on <see cref="ShortDescriptionTemplate"/>.
/// </summary>
public class ShortDescriptionTemplateService(IRepository<ShortDescriptionTemplate, string> repository) : IShortDescriptionTemplateService
{
    /// <summary>
    /// Asynchronously retrieves all short description templates.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an IBaseResult with a collection of
    /// ShortDescriptionTemplateDto objects representing all available short description templates. If the operation
    /// fails, the result contains error messages.</returns>
    public async Task<IBaseResult<IEnumerable<ShortDescriptionTemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<ShortDescriptionTemplate>(c => true);

        var result = await repository.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<List<ShortDescriptionTemplateDto>>.FailAsync(result.Messages);

        return await Result<List<ShortDescriptionTemplateDto>>.SuccessAsync(result.Data.Select(c => new ShortDescriptionTemplateDto(c)).ToList());
    }

    /// <summary>
    /// Asynchronously retrieves a short description template by its unique identifier.
    /// </summary>
    /// <remarks>If no template with the specified identifier exists, the result will indicate failure and
    /// include a message describing the reason.</remarks>
    /// <param name="id">The unique identifier of the short description template to retrieve. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an IBaseResult with the short
    /// description template data if found; otherwise, a failure result with an appropriate message.</returns>
    public async Task<IBaseResult<ShortDescriptionTemplateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<ShortDescriptionTemplate>(c => c.Id == id);

        var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result<ShortDescriptionTemplateDto>.FailAsync(result.Messages.Count != 0 ? result.Messages : [$"No ShortDescriptionTemplate with id '{id}' was found"]);

        return await Result<ShortDescriptionTemplateDto>.SuccessAsync(new ShortDescriptionTemplateDto(result.Data));
    }

    /// <summary>
    /// Asynchronously adds a new short description template to the repository.
    /// </summary>
    /// <param name="dto">The data transfer object containing the title and content for the new short description template. Cannot be
    /// null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see
    /// cref="IBaseResult{ShortDescriptionTemplateDto}"/> indicating the outcome of the add operation and the created
    /// template data if successful.</returns>
    public async Task<IBaseResult<ShortDescriptionTemplateDto>> AddAsync(ShortDescriptionTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var createResult = await repository.CreateAsync(new ShortDescriptionTemplate
        {
            Title = dto.Title,
            Content = dto.Content
        }, cancellationToken);
        if (!createResult.Succeeded)
            return await Result<ShortDescriptionTemplateDto>.FailAsync(createResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<ShortDescriptionTemplateDto>.FailAsync(saveResult.Messages);

        return await Result<ShortDescriptionTemplateDto>.SuccessAsync(new ShortDescriptionTemplateDto(createResult.Data));
    }

    /// <summary>
    /// Updates an existing short description template with the specified values asynchronously.
    /// </summary>
    /// <remarks>Returns a failed result if the template with the specified <c>Id</c> does not exist or if the
    /// update operation fails. The operation is performed asynchronously and supports cancellation via the <paramref
    /// name="cancellationToken"/> parameter.</remarks>
    /// <param name="dto">The data transfer object containing the updated values for the short description template. The template is
    /// identified by its <c>Id</c> property.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A result containing the updated short description template data if the update succeeds; otherwise, a result with
    /// error messages describing the failure.</returns>
    public async Task<IBaseResult<ShortDescriptionTemplateDto>> EditAsync(ShortDescriptionTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<ShortDescriptionTemplate>(c => c.Id == dto.Id);

        var entityResult = await repository.FirstOrDefaultAsync(spec, true, cancellationToken);
        if (!entityResult.Succeeded || entityResult.Data == null)
            return await Result<ShortDescriptionTemplateDto>.FailAsync(entityResult.Messages.Count != 0 ? entityResult.Messages : [$"No ShortDescriptionTemplate with id '{dto.Id}' was found"]);

        var entity = entityResult.Data;
        entity.Title = dto.Title;
        entity.Content = dto.Content;

        var updateResult = repository.Update(entity);
        if (!updateResult.Succeeded)
            return await Result<ShortDescriptionTemplateDto>.FailAsync(updateResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<ShortDescriptionTemplateDto>.FailAsync(saveResult.Messages);

        return await Result<ShortDescriptionTemplateDto>.SuccessAsync(dto);
    }

    /// <summary>
    /// Asynchronously deletes the template identified by the specified ID.
    /// </summary>
    /// <remarks>The operation first deletes the template and then saves the changes. If either step fails,
    /// the result will indicate failure and include relevant error messages.</remarks>
    /// <param name="id">The unique identifier of the template to delete. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A result indicating whether the delete operation succeeded. If the operation fails, the result contains error
    /// messages.</returns>
    public async Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var deleteResult = await repository.DeleteAsync(id, cancellationToken);
        if (!deleteResult.Succeeded)
            return await Result.FailAsync(deleteResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync($"Template with id '{id}' was successfully removed");
    }
}
