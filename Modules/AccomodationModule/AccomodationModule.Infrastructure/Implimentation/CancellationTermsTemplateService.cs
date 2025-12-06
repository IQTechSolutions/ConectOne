using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;

namespace AccomodationModule.Infrastructure.Implimentation;

/// <summary>
/// Service providing CRUD operations for <see cref="CancellationTermsTemplate"/> entities.
/// </summary>
public class CancellationTermsTemplateService(IRepository<CancellationTermsTemplate, string> repository) : ICancellationTermsTemplateService
{
    #region Query operations

    /// <summary>
    /// Retrieves all cancellation terms templates as a list of data transfer objects (DTOs).
    /// </summary>
    /// <remarks>This method retrieves all cancellation terms templates from the repository and maps them to
    /// DTOs. If the operation is unsuccessful, the result will indicate failure and include the relevant error
    /// messages.</remarks>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// where <c>T</c> is a list of <see cref="CancellationTermsTemplateDto"/> objects. If the operation fails, the
    /// result includes error messages.</returns>
    public async Task<IBaseResult<IEnumerable<CancellationTermsTemplateDto>>> AllAsync(CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<CancellationTermsTemplate>(_ => true);
        var result = await repository.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<List<CancellationTermsTemplateDto>>.FailAsync(result.Messages);
        var dtos = result.Data.Select(e => new CancellationTermsTemplateDto(e)).ToList();
        return await Result<List<CancellationTermsTemplateDto>>.SuccessAsync(dtos);
    }

    /// <summary>
    /// Retrieves a cancellation terms template by its unique identifier.
    /// </summary>
    /// <remarks>This method attempts to retrieve a cancellation terms template based on the provided
    /// identifier. If the template is not found, the result will indicate failure with an appropriate error
    /// message.</remarks>
    /// <param name="id">The unique identifier of the cancellation terms template to retrieve. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// where <c>T</c> is <see cref="CancellationTermsTemplateDto"/>. If the operation succeeds, the result contains the
    /// cancellation terms template; otherwise, it contains error messages.</returns>
    public async Task<IBaseResult<CancellationTermsTemplateDto>> ByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<CancellationTermsTemplate>(e => e.Id == id);
        var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result<CancellationTermsTemplateDto>.FailAsync(
                result.Succeeded ? $"Entity with ID {id} not found." : string.Join(',', result.Messages));
        return await Result<CancellationTermsTemplateDto>.SuccessAsync(new CancellationTermsTemplateDto(result.Data));
    }

    #endregion

    #region Command operations
    
    /// <summary>
    /// Asynchronously adds a new cancellation terms template to the repository.
    /// </summary>
    /// <remarks>This method performs two operations: it creates the cancellation terms template in the
    /// repository and then saves the changes. If either operation fails, the method returns a failure result with the
    /// corresponding error messages.</remarks>
    /// <param name="dto">The data transfer object containing the details of the cancellation terms template to add. The <see
    /// cref="CancellationTermsTemplateDto"/> must have valid values for <c>TemplateName</c>,
    /// <c>TemplateDescription</c>, and <c>Description</c>.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// where <c>T</c> is <see cref="CancellationTermsTemplateDto"/>. If the operation succeeds, the result contains the
    /// added template. If the operation fails, the result contains error messages.</returns>
    public async Task<IBaseResult<CancellationTermsTemplateDto>> AddAsync(CancellationTermsTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var createResult = await repository.CreateAsync(new CancellationTermsTemplate
        {
            TemplateName = dto.TemplateName!,
            TemplateDescription = dto.TemplateDescription!,
            Description = dto.Description!
        }, cancellationToken);
        if (!createResult.Succeeded)
            return await Result<CancellationTermsTemplateDto>.FailAsync(createResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<CancellationTermsTemplateDto>.FailAsync(saveResult.Messages);

        return await Result<CancellationTermsTemplateDto>.SuccessAsync(dto);
    }

    /// <summary>
    /// Updates an existing cancellation terms template with the provided data.
    /// </summary>
    /// <remarks>This method performs the following steps: <list type="number"> <item>Retrieves the existing
    /// cancellation terms template based on the <paramref name="dto"/> ID.</item> <item>Updates the template's
    /// properties with the values provided in <paramref name="dto"/>.</item> <item>Attempts to save the changes to the
    /// repository.</item> </list> If the template is not found or any operation fails, the method returns a failure
    /// result with appropriate error messages.</remarks>
    /// <param name="dto">The data transfer object containing the updated values for the cancellation terms template. The <see
    /// cref="CancellationTermsTemplateDto.Id"/> property must match an existing template.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// object: <list type="bullet"> <item> <description>A successful result containing the updated <see
    /// cref="CancellationTermsTemplateDto"/> if the operation completes successfully.</description> </item> <item>
    /// <description>A failure result with error messages if the template is not found, the update fails, or the save
    /// operation does not succeed.</description> </item> </list></returns>
    public async Task<IBaseResult<CancellationTermsTemplateDto>> EditAsync(CancellationTermsTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<CancellationTermsTemplate>(e => e.Id == dto.Id);
        var result = await repository.FirstOrDefaultAsync(spec, true, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result<CancellationTermsTemplateDto>.FailAsync(
                result.Succeeded ? $"Entity with ID {dto.Id} not found." : string.Join(',', result.Messages));

        result.Data.TemplateName = dto.TemplateName!;
        result.Data.TemplateDescription = dto.TemplateDescription!;
        result.Data.Description = dto.Description!;

        var updateResult = repository.Update(result.Data);
        if (!updateResult.Succeeded)
            return await Result<CancellationTermsTemplateDto>.FailAsync(updateResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<CancellationTermsTemplateDto>.FailAsync(saveResult.Messages);

        return await Result<CancellationTermsTemplateDto>.SuccessAsync(dto);
    }

    /// <summary>
    /// Deletes an entity with the specified identifier asynchronously.
    /// </summary>
    /// <remarks>This method attempts to delete the entity identified by <paramref name="id"/> and save the
    /// changes to the underlying data store. If the delete operation or save operation fails, the result will include
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
