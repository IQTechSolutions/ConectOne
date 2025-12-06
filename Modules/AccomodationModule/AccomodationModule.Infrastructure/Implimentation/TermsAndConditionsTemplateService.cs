using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;

namespace AccomodationModule.Infrastructure.Implimentation;

/// <summary>
/// Service providing CRUD operations for <see cref="TermsAndConditionsTemplate"/> entities.
/// </summary>
public class TermsAndConditionsTemplateService(IRepository<TermsAndConditionsTemplate, string> repository) : ITermsAndConditionsTemplateService
{
    #region Query operations

    /// <summary>
    /// Retrieves all terms and conditions templates as a list of DTOs.
    /// </summary>
    /// <remarks>This method retrieves all terms and conditions templates from the repository and maps them to
    /// DTOs. If the operation fails, the result will include the failure messages.</remarks>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// object that includes a list of <see cref="TermsAndConditionsTemplateDto"/> instances if the operation succeeds,
    /// or error messages if it fails.</returns>
    public async Task<IBaseResult<List<TermsAndConditionsTemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<TermsAndConditionsTemplate>(_ => true);
        var result = await repository.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<List<TermsAndConditionsTemplateDto>>.FailAsync(result.Messages);
        var dtos = result.Data.Select(e => new TermsAndConditionsTemplateDto(e)).ToList();
        return await Result<List<TermsAndConditionsTemplateDto>>.SuccessAsync(dtos);
    }

    /// <summary>
    /// Retrieves a Terms and Conditions template by its unique identifier.
    /// </summary>
    /// <remarks>If the specified template is not found, the result will indicate failure with an appropriate
    /// error message.</remarks>
    /// <param name="id">The unique identifier of the Terms and Conditions template to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} object that
    /// includes the requested Terms and Conditions template  as a TermsAndConditionsTemplateDto if found, or an error
    /// message if not found.</returns>
    public async Task<IBaseResult<TermsAndConditionsTemplateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<TermsAndConditionsTemplate>(e => e.Id == id);
        var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result<TermsAndConditionsTemplateDto>.FailAsync(
                result.Succeeded ? $"Entity with ID {id} not found." : string.Join(',', result.Messages));
        return await Result<TermsAndConditionsTemplateDto>.SuccessAsync(new TermsAndConditionsTemplateDto(result.Data));
    }

    #endregion

    #region Command operations

    /// <summary>
    /// Adds a new terms and conditions template to the repository asynchronously.
    /// </summary>
    /// <remarks>This method performs the following steps: <list type="number"> <item>Creates a new terms and
    /// conditions template in the repository using the provided <paramref name="dto"/>.</item> <item>Saves the changes
    /// to the repository.</item> <item>Returns a success result containing the added template if all operations
    /// succeed, or a failure result with error messages otherwise.</item> </list></remarks>
    /// <param name="dto">The data transfer object containing the details of the terms and conditions template to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> with
    /// the added <see cref="TermsAndConditionsTemplateDto"/> if the operation succeeds, or error messages if it fails.</returns>
    public async Task<IBaseResult<TermsAndConditionsTemplateDto>> AddAsync(TermsAndConditionsTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var createResult = await repository.CreateAsync(new TermsAndConditionsTemplate
        {
            Id = dto.Id,
            TemplateName = dto.TemplateName,
            TemplateDescription = dto.TemplateDescription,
            Description = dto.Description
        }, cancellationToken);
        if (!createResult.Succeeded)
            return await Result<TermsAndConditionsTemplateDto>.FailAsync(createResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<TermsAndConditionsTemplateDto>.FailAsync(saveResult.Messages);

        return await Result<TermsAndConditionsTemplateDto>.SuccessAsync(dto);
    }

    /// <summary>
    /// Updates an existing Terms and Conditions template with the provided data.
    /// </summary>
    /// <remarks>The method attempts to locate the template by its ID, update its properties, and save the
    /// changes  to the repository. If the template is not found, or if any operation (update or save) fails,  the
    /// method returns a failure result with appropriate error messages.</remarks>
    /// <param name="dto">The data transfer object containing the updated values for the template.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} object with the
    /// updated template data if the operation succeeds,  or error messages if it fails.</returns>
    public async Task<IBaseResult<TermsAndConditionsTemplateDto>> EditAsync(TermsAndConditionsTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<TermsAndConditionsTemplate>(e => e.Id == dto.Id);
        var result = await repository.FirstOrDefaultAsync(spec, true, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result<TermsAndConditionsTemplateDto>.FailAsync(
                result.Succeeded ? $"Entity with ID {dto.Id} not found." : string.Join(',', result.Messages));

        result.Data.TemplateName = dto.TemplateName;
        result.Data.TemplateDescription = dto.TemplateDescription;
        result.Data.Description = dto.Description;

        var updateResult = repository.Update(result.Data);
        if (!updateResult.Succeeded)
            return await Result<TermsAndConditionsTemplateDto>.FailAsync(updateResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<TermsAndConditionsTemplateDto>.FailAsync(saveResult.Messages);

        return await Result<TermsAndConditionsTemplateDto>.SuccessAsync(dto);
    }

    /// <summary>
    /// Deletes an entity with the specified identifier and persists the changes asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the operation. The result will indicate success if the entity was deleted and changes
    /// were saved successfully; otherwise, it will contain error messages.</returns>
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
