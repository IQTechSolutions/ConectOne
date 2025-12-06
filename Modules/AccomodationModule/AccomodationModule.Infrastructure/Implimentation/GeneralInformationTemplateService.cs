using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;

namespace AccomodationModule.Infrastructure.Implimentation;

/// <summary>
///     Responsible for creating, reading, updating and deleting
///     <see cref="GeneralInformationTemplate"/> instances.  The class is built
///     to be highly reusable: callers supply mapping functions and optional
///     entity configuration logic via delegates so the same service can cater
///     for multiple bounded‑contexts with minimal code duplication.
/// </summary>
public class GeneralInformationTemplateService(IRepository<GeneralInformationTemplate, string> repository) : IGeneralInformationTemplateService
{
    /// <summary>
    /// Retrieves all general information templates asynchronously.
    /// </summary>
    /// <remarks>This method retrieves all general information templates from the repository. The result 
    /// includes a collection of <GeneralInformationTemplateDto/> objects representing  the templates. If the operation
    /// is unsuccessful, the result will include the failure  messages.</remarks>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  <IBaseResult{T}/> where <c>T</c>
    /// is an <IEnumerable{T}/> of  <GeneralInformationTemplateDto/> objects. If the operation fails, the result 
    /// contains error messages.</returns>
    public async Task<IBaseResult<IEnumerable<GeneralInformationTemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<GeneralInformationTemplate>(c => true);

        var result = await repository.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<List<GeneralInformationTemplateDto>>.FailAsync(result.Messages);

        return await Result<List<GeneralInformationTemplateDto>>.SuccessAsync(result.Data.Select(c => new GeneralInformationTemplateDto(c)).ToList());
    }

    /// <summary>
    ///     Fetches a single template by its identifier and maps it to a DTO.
    /// </summary>
    /// <param name="id">Unique identifier of the template to retrieve.</param>
    /// <param name="cancellationToken">Token that can be used to cancel the database call.</param>
    public async Task<IBaseResult<GeneralInformationTemplateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<GeneralInformationTemplate>(c => c.Id == id);

        var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result<GeneralInformationTemplateDto>.FailAsync(result.Messages.Count != 0 ? result.Messages : [$"No GeneralInformationTemplate with id '{id}' was found"]);

        return await Result<GeneralInformationTemplateDto>.SuccessAsync(new GeneralInformationTemplateDto(result.Data));
    }

    /// <summary>
    /// Asynchronously adds a new general information template to the repository.
    /// </summary>
    /// <remarks>The method performs the following steps: <list type="number"> <item>Creates a new general
    /// information template in the repository.</item> <item>Saves the changes to the repository.</item> </list> If any
    /// step fails, the operation returns a failure result with the corresponding error messages.</remarks>
    /// <param name="dto">The data transfer object containing the details of the general information template to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> with
    /// the added <see cref="GeneralInformationTemplateDto"/> if the operation succeeds, or error messages if it fails.</returns>
    public async Task<IBaseResult<GeneralInformationTemplateDto>> AddAsync(GeneralInformationTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var createResult = await repository.CreateAsync(new GeneralInformationTemplate(){ Id = dto.Id, Name = dto.Name, Information = dto.Information}, cancellationToken);
        if (!createResult.Succeeded)
            return await Result<GeneralInformationTemplateDto>.FailAsync(createResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<GeneralInformationTemplateDto>.FailAsync(saveResult.Messages);

        return await Result<GeneralInformationTemplateDto>.SuccessAsync(dto);
    }

    /// <summary>
    /// Updates an existing GeneralInformationTemplate entity with the provided data.
    /// </summary>
    /// <remarks>This method performs the following steps: <list type="number"> <item>Retrieves the existing
    /// entity based on the ID provided in the <paramref name="dto"/>.</item> <item>Updates the entity's properties with
    /// the values from the <paramref name="dto"/>.</item> <item>Attempts to save the changes to the repository.</item>
    /// </list> If the entity is not found, or if any step fails, the method returns a failure result with appropriate
    /// error messages.</remarks>
    /// <param name="dto">The data transfer object containing the updated information for the GeneralInformationTemplate. The <see
    /// cref="GeneralInformationTemplateDto.Id"/> property must match the ID of an existing entity.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. This allows the operation to be canceled if needed.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
    /// where <c>T</c> is <see cref="GeneralInformationTemplateDto"/>. If the operation succeeds, the result contains 
    /// the updated DTO. If the operation fails, the result contains error messages describing the failure.</returns>
    public async Task<IBaseResult<GeneralInformationTemplateDto>> EditAsync(GeneralInformationTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<GeneralInformationTemplate>(c => c.Id == dto.Id);

        var entityResult = await repository.FirstOrDefaultAsync(spec, true, cancellationToken);
        if (!entityResult.Succeeded || entityResult.Data == null)
            return await Result<GeneralInformationTemplateDto>.FailAsync(entityResult.Messages.Count != 0 ? entityResult.Messages : [$"No GeneralInformationTemplate with id '{dto.Id}' was found"]);

        entityResult.Data.Name = dto.Name;
        entityResult.Data.Information = dto.Information;

        var updateResult = repository.Update(entityResult.Data);
        if (!updateResult.Succeeded)
            return await Result<GeneralInformationTemplateDto>.FailAsync(updateResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<GeneralInformationTemplateDto>.FailAsync(saveResult.Messages);

        return await Result<GeneralInformationTemplateDto>.SuccessAsync(dto);
    }

    /// <summary>
    ///     Permanently removes a template from the data store.
    /// </summary>
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
