using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;

namespace AccomodationModule.Infrastructure.Implimentation;

/// <summary>
///     Provides a façade over data-access for <see cref="MeetAndGreetTemplate" />
///     entities, encapsulating all query and mutation logic as asynchronous operations.
/// </summary>
/// <remarks>
///     • Keeps higher layers (e.g., UI, API) persistence-agnostic.<br/>
///     • Guards every repository call with consistent result handling.<br/>
///     • Returns DTOs only, preserving domain encapsulation.
/// </remarks>
public class MeetAndGreetTemplateService(IRepository<MeetAndGreetTemplate, string> repository) : IMeetAndGreetTemplateService
{
    /// <summary>
    /// Retrieves every template in the system (eager-loading its associated
    /// <see cref="MediaTypeNames.Application.Entities.Contact"/>), maps them to DTOs, and returns them in a success
    /// envelope or returns infrastructure errors unchanged.
    /// </summary>
    public async Task<IBaseResult<IEnumerable<MeetAndGreetTemplateDto>>> AllAsync(CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<MeetAndGreetTemplate>(c => true);
        spec.AddInclude(q => q.Include(c => c.Contact)!);

        var result = await repository.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<List<MeetAndGreetTemplateDto>>.FailAsync(result.Messages);

        return await Result<List<MeetAndGreetTemplateDto>>.SuccessAsync(result.Data.Select(c => new MeetAndGreetTemplateDto(c)).ToList());
    }

    /// <summary>
    /// Retrieves a <see cref="MeetAndGreetTemplateDto"/> by its unique identifier.
    /// </summary>
    /// <remarks>If no template with the specified <paramref name="id"/> is found, the result will indicate
    /// failure with an appropriate error message.</remarks>
    /// <param name="id">The unique identifier of the Meet and Greet template to retrieve. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
    /// where <typeparamref name="T"/> is <see cref="MeetAndGreetTemplateDto"/>. If the operation succeeds, the result
    /// contains the requested template; otherwise, it contains error messages.</returns>
    public async Task<IBaseResult<MeetAndGreetTemplateDto>> ByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<MeetAndGreetTemplate>(c => c.Id == id);
        spec.AddInclude(q => q.Include(c => c.Contact)!);

        var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result<MeetAndGreetTemplateDto>.FailAsync(result.Messages.Count != 0 ? result.Messages : [$"No MeetAndGreetTemplate with id '{id}' was found"]);

        return await Result<MeetAndGreetTemplateDto>.SuccessAsync(new MeetAndGreetTemplateDto(result.Data));
    }

    /// <summary>
    /// Creates a new <see cref="MeetAndGreetTemplate"/> from its DTO
    /// representation and persists it.
    /// </summary>
    /// <remarks>
    /// The DTO itself is returned on success because callers often need
    /// the user-supplied data, not the internal entity.
    /// </remarks>
    public async Task<IBaseResult<MeetAndGreetTemplateDto>> AddAsync(MeetAndGreetTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var createResult = await repository.CreateAsync(new MeetAndGreetTemplate { Name = dto.Name, TimeDescription = dto.TimeDescription, Description = dto.Description, Location = dto.Location, ContactId = dto.Contact.ContactId }, cancellationToken);
        if (!createResult.Succeeded)
            return await Result<MeetAndGreetTemplateDto>.FailAsync(createResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<MeetAndGreetTemplateDto>.FailAsync(saveResult.Messages);

        return await Result<MeetAndGreetTemplateDto>.SuccessAsync(dto);
    }

    /// <summary>
    /// Modifies an existing template in-place. Only user-mutable fields
    /// are patched; auditing fields remain untouched.
    /// </summary>
    /// <param name="id">Identifier of the entity to update.</param>
    /// <param name="dto">New values coming from the client/UI layer.</param>
    public async Task<IBaseResult<MeetAndGreetTemplateDto>> EditAsync(MeetAndGreetTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<MeetAndGreetTemplate>(c => c.Id == dto.Id);
        spec.AddInclude(q => q.Include(c => c.Contact));

        var entityResult = await repository.FirstOrDefaultAsync(spec, true, cancellationToken);
        if (!entityResult.Succeeded || entityResult.Data == null)
            return await Result<MeetAndGreetTemplateDto>.FailAsync(entityResult.Messages.Count != 0 ? entityResult.Messages : [$"No MeetAndGreetTemplate with id '{dto.Id}' was found"]);
        
        entityResult.Data.Name = dto.Name;
        entityResult.Data.TimeDescription = dto.TimeDescription;
        entityResult.Data.Description = dto.Description;
        entityResult.Data.Location = dto.Location;
        entityResult.Data.ContactId = dto.Contact.ContactId;

        var updateResult = repository.Update(entityResult.Data);
        if (!updateResult.Succeeded)
            return await Result<MeetAndGreetTemplateDto>.FailAsync(updateResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<MeetAndGreetTemplateDto>.FailAsync(saveResult.Messages);

        return await Result<MeetAndGreetTemplateDto>.SuccessAsync(dto);
    }

    /// <summary>
    /// Removes a template permanently from the data store.
    /// </summary>
    /// <param name="id">Primary key of the template to delete.</param>
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
