using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;

namespace AccomodationModule.Infrastructure.Implimentation;

/// <summary>
///     Application‑layer service that provides a cohesive façade around the
///     <see cref="IRepository{TEntity,TKey}"/> abstraction for <see cref="DayTourActivityTemplate"/>
///     persistence.  Every public method returns an <see cref="IBaseResult"/> so callers
///     can act uniformly on success/failure without throwing exceptions.
/// </summary>
public class DayTourActivityTemplateService(IRepository<DayTourActivityTemplate, string> repository) : IDayTourActivityTemplateService
{
    /// <summary>
    ///     Retrieves <b>all</b> templates in the system.
    /// </summary>
    /// <remarks>
    ///     The specification "<c>c =&gt; true</c>" expresses an empty WHERE clause, effectively
    ///     requesting the full table. Because the entity is relatively small (template metadata)
    ///     we perform the operation in a single shot; for very large tables a paged API would
    ///     be preferable.
    /// </remarks>
    public async Task<IBaseResult<List<DayTourActivityTemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<DayTourActivityTemplate>(c => true);

        var result = await repository.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<List<DayTourActivityTemplateDto>>.FailAsync(result.Messages);

        var dtoList = result.Data.Select(c => new DayTourActivityTemplateDto(c)).ToList();
        return await Result<List<DayTourActivityTemplateDto>>.SuccessAsync(dtoList);
    }

    /// <summary>
    ///     Retrieves a single template by its identifier.
    /// </summary>
    /// <param name="id">Primary‑key of the template.</param>
    /// <param name="cancellationToken">Token propagated to the data‑access layer.</param>
    public async Task<IBaseResult<DayTourActivityTemplateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<DayTourActivityTemplate>(c => c.Id == id);

        var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded || result.Data is null)
            return await Result<DayTourActivityTemplateDto>.FailAsync(
                result.Messages.Count != 0 ? result.Messages : [$"No DayTourActivityTemplate with id '{id}' was found"]);

        return await Result<DayTourActivityTemplateDto>.SuccessAsync(new DayTourActivityTemplateDto(result.Data));
    }

    /// <summary>
    ///     Persists a new template.
    /// </summary>
    /// <param name="dto">Input DTO describing the template.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task<IBaseResult<DayTourActivityTemplateDto>> AddAsync(DayTourActivityTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var createResult = await repository.CreateAsync(new DayTourActivityTemplate(dto), cancellationToken);
        if (!createResult.Succeeded)
            return await Result<DayTourActivityTemplateDto>.FailAsync(createResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<DayTourActivityTemplateDto>.FailAsync(saveResult.Messages);

        return await Result<DayTourActivityTemplateDto>.SuccessAsync(dto);
    }

    /// <summary>
    ///     Updates an existing template. Only specific properties are mutable –
    ///     everything else remains unchanged.
    /// </summary>
    /// <param name="dto">DTO containing the new state. Its <see cref="DayTourActivityTemplateDto.Id"/> serves as lookup‑key.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task<IBaseResult<DayTourActivityTemplateDto>> EditAsync(DayTourActivityTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<DayTourActivityTemplate>(c => c.Id == dto.Id);

        var entityResult = await repository.FirstOrDefaultAsync(spec, true, cancellationToken);
        if (!entityResult.Succeeded || entityResult.Data is null)
            return await Result<DayTourActivityTemplateDto>.FailAsync(
                entityResult.Messages.Count != 0 ? entityResult.Messages : [$"No DayTourActivityTemplate with id '{dto.Id}' was found"]);

        entityResult.Data.Name = dto.Name;
        entityResult.Data.Summary = dto.Summary;
        entityResult.Data.Description = dto.Description;
        entityResult.Data.GuestType = (GuestType)dto.GuestType;
        entityResult.Data.DisplayInOverview = dto.DisplayInOverview;

        var updateResult = repository.Update(entityResult.Data);
        if (!updateResult.Succeeded)
            return await Result<DayTourActivityTemplateDto>.FailAsync(updateResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<DayTourActivityTemplateDto>.FailAsync(saveResult.Messages);

        return await Result<DayTourActivityTemplateDto>.SuccessAsync(dto);
    }

    /// <summary>
    ///     Deletes a template by its identifier.
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
