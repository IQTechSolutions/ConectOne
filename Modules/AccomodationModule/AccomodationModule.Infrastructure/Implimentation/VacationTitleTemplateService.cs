using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;

namespace AccomodationModule.Infrastructure.Implimentation;

/// <summary>
/// Service handling CRUD operations for <see cref="VacationTitleTemplate"/> entities.
/// </summary>
public class VacationTitleTemplateService(IRepository<VacationTitleTemplate, string> repository) : IVacationTitleTemplateService
{
    /// <summary>
    /// Retrieves all vacation title templates asynchronously.
    /// </summary>
    /// <remarks>This method retrieves all vacation title templates from the repository. The result  includes
    /// a collection of <VacationTitleTemplateDto/> objects representing  the templates. If the operation is
    /// unsuccessful, the result will indicate failure  and include the associated error messages.</remarks>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  <IBaseResult{T}/> where <c>T</c>
    /// is an <IEnumerable{T}/> of  <VacationTitleTemplateDto/> objects. If the operation fails, the result  includes
    /// error messages.</returns>
    public async Task<IBaseResult<IEnumerable<VacationTitleTemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<VacationTitleTemplate>(c => true);

        var result = await repository.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<List<VacationTitleTemplateDto>>.FailAsync(result.Messages);

        return await Result<List<VacationTitleTemplateDto>>.SuccessAsync(result.Data.Select(c => new VacationTitleTemplateDto(c)).ToList());
    }

    /// <summary>
    /// Retrieves a <see cref="VacationTitleTemplateDto"/> by its unique identifier.
    /// </summary>
    /// <remarks>If no vacation title template with the specified <paramref name="id"/> is found, the result
    /// will indicate failure and include an appropriate error message.</remarks>
    /// <param name="id">The unique identifier of the vacation title template to retrieve. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// where <typeparamref name="T"/> is <see cref="VacationTitleTemplateDto"/>. The result indicates whether the
    /// operation succeeded and includes the retrieved data if successful.</returns>
    public async Task<IBaseResult<VacationTitleTemplateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<VacationTitleTemplate>(c => c.Id == id);

        var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result<VacationTitleTemplateDto>.FailAsync(result.Messages.Count != 0 ? result.Messages : [$"No VacationTitleTemplate with id '{id}' was found"]);

        return await Result<VacationTitleTemplateDto>.SuccessAsync(new VacationTitleTemplateDto(result.Data));
    }

    /// <summary>
    /// Asynchronously adds a new vacation title template to the repository.
    /// </summary>
    /// <remarks>This method performs the following steps: <list type="number"> <item>Creates a new vacation
    /// title template in the repository using the provided <paramref name="dto"/>.</item> <item>Saves the changes to
    /// the repository.</item> <item>Returns a success result containing the added template, or a failure result with
    /// error messages if any step fails.</item> </list></remarks>
    /// <param name="dto">The data transfer object containing the details of the vacation title template to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> with
    /// the added <see cref="VacationTitleTemplateDto"/> if the operation succeeds, or error messages if it fails.</returns>
    public async Task<IBaseResult<VacationTitleTemplateDto>> AddAsync(VacationTitleTemplateDto dto, CancellationToken cancellationToken)
    {
        var createResult = await repository.CreateAsync(new VacationTitleTemplate
        {
            VacationTitle = dto.VacationTitle
        }, cancellationToken);
        if (!createResult.Succeeded)
            return await Result<VacationTitleTemplateDto>.FailAsync(createResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<VacationTitleTemplateDto>.FailAsync(saveResult.Messages);

        return await Result<VacationTitleTemplateDto>.SuccessAsync(new VacationTitleTemplateDto(createResult.Data));
    }

    /// <summary>
    /// Updates an existing vacation title template with the provided data.
    /// </summary>
    /// <remarks>This method performs the following steps: <list type="number"> <item><description>Checks if a
    /// vacation title template with the specified <see cref="VacationTitleTemplateDto.Id"/>
    /// exists.</description></item> <item><description>Updates the vacation title of the template if it
    /// exists.</description></item> <item><description>Attempts to save the changes to the
    /// repository.</description></item> </list> If the template does not exist or any operation fails, the method
    /// returns a failure result with detailed error messages.</remarks>
    /// <param name="dto">The data transfer object containing the updated vacation title template information. The <see
    /// cref="VacationTitleTemplateDto.Id"/> property must match an existing template.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> of
    /// type <see cref="VacationTitleTemplateDto"/>: <list type="bullet"> <item><description>A successful result if the
    /// update operation completes successfully, containing the updated template data.</description></item>
    /// <item><description>A failure result if the specified template is not found, or if the update or save operation
    /// fails, with appropriate error messages.</description></item> </list></returns>
    public async Task<IBaseResult<VacationTitleTemplateDto>> EditAsync(VacationTitleTemplateDto dto, CancellationToken cancellationToken)
    {
        var spec = new LambdaSpec<VacationTitleTemplate>(c => c.Id == dto.Id);

        var entityResult = await repository.FirstOrDefaultAsync(spec, true, cancellationToken);
        if (!entityResult.Succeeded || entityResult.Data == null)
            return await Result<VacationTitleTemplateDto>.FailAsync(entityResult.Messages.Count != 0 ? entityResult.Messages : [$"No VacationTitleTemplate with id '{dto.Id}' was found"]);

        entityResult.Data.VacationTitle = dto.VacationTitle;

        var updateResult = repository.Update(entityResult.Data);
        if (!updateResult.Succeeded)
            return await Result<VacationTitleTemplateDto>.FailAsync(updateResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<VacationTitleTemplateDto>.FailAsync(saveResult.Messages);

        return await Result<VacationTitleTemplateDto>.SuccessAsync(dto);
    }

    /// <summary>
    /// Deletes the entity with the specified identifier and persists the changes asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the operation. If the operation succeeds, the result contains a success message. If it
    /// fails, the result contains error messages.</returns>
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
