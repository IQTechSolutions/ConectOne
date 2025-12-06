using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccomodationModule.Infrastructure.Implimentation;

/// <summary>
///     Application-layer service responsible for creating, reading, updating and deleting
///     <see cref="MealAdditionTemplate"/> entities as well as mapping them to
///     transport-friendly <see cref="MealAdditionTemplateDto"/> objects.
/// </summary>
public class MealAdditionTemplateService(IRepository<MealAdditionTemplate, string> repository) : IMealAdditionTemplateService
{
    /// <summary>
    ///     Retrieves all meal‑addition templates including their owning <see cref="Restaurant"/> entity.
    /// </summary>
    /// <remarks>
    ///     The method uses a <see cref="LambdaSpec{TEntity}"/> to express an empty filter ("<c>true</c>") and explicitly
    ///     adds an include for the <c>Restaurant</c> navigation property to ensure it is eagerly loaded in a single
    ///     round‑trip to the database.
    /// </remarks>
    /// <param name="cancellationToken">
    ///     Optional token that can be used by the caller to cancel the request.
    /// </param>
    /// <returns>
    ///     An <see cref="IBaseResult"/> wrapping a <see cref="List{T}"/> of <see cref="MealAdditionTemplateDto"/>
    ///     objects.
    /// </returns>
    public async Task<IBaseResult<List<MealAdditionTemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<MealAdditionTemplate>(c => true);
        spec.AddInclude(q => q.Include(c => c.Restaurant));

        var result = await repository.ListAsync(spec, false, cancellationToken);
        if (!result.Succeeded)
            return await Result<List<MealAdditionTemplateDto>>.FailAsync(result.Messages);

        return await Result<List<MealAdditionTemplateDto>>.SuccessAsync(result.Data.Select(c => new MealAdditionTemplateDto(c)).ToList());
    }

    /// <summary>
    ///     Retrieves a single meal‑addition template by its unique identifier, including the related restaurant.
    /// </summary>
    /// <param name="id">Unique identifier of the template.</param>
    /// <param name="cancellationToken">Cancellation token propagated from the calling context.</param>
    /// <returns>Either a populated DTO wrapped in a success result or a failure result with meaningful messages.</returns>
    public async Task<IBaseResult<MealAdditionTemplateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<MealAdditionTemplate>(c => c.Id == id);
        spec.AddInclude(q => q.Include(c => c.Restaurant));

        var result = await repository.FirstOrDefaultAsync(spec, false, cancellationToken);
        if (!result.Succeeded || result.Data == null)
            return await Result<MealAdditionTemplateDto>.FailAsync(result.Messages.Count != 0 ? result.Messages : [$"No MealAdditionTemplate with id '{id}' was found"]);

        return await Result<MealAdditionTemplateDto>.SuccessAsync(new MealAdditionTemplateDto(result.Data));
    }

    /// <summary>
    ///     Persists a new meal‑addition template to the underlying store.
    /// </summary>
    /// <param name="dto">DTO carrying the state required to create the entity.</param>
    /// <param name="cancellationToken">Token enabling the caller to cancel the operation.</param>
    /// <returns>A success result containing the newly created DTO or a failure result with errors.</returns>
    public async Task<IBaseResult<MealAdditionTemplateDto>> AddAsync(MealAdditionTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new MealAdditionTemplate
        {
            Id = dto.Id,
            GuestType = (GuestType)dto.GuestType,
            MealType = (MealType)dto.MealType,
            Notes = dto.Notes,
            RestaurantId = dto.Restaurant.Id
        };

        var createResult = await repository.CreateAsync(entity, cancellationToken);
        if (!createResult.Succeeded)
            return await Result<MealAdditionTemplateDto>.FailAsync(createResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<MealAdditionTemplateDto>.FailAsync(saveResult.Messages);

        return await Result<MealAdditionTemplateDto>.SuccessAsync(new MealAdditionTemplateDto(entity));
    }

    /// <summary>
    ///     Updates an existing meal‑addition template. Only fails if the entity cannot be found or persistence fails.
    /// </summary>
    /// <param name="id">Identifier of the template to modify.</param>
    /// <param name="dto">DTO containing the new state.</param>
    /// <param name="cancellationToken">Operation cancellation token.</param>
    /// <returns>A result with the updated DTO or error messages.</returns>
    public async Task<IBaseResult<MealAdditionTemplateDto>> EditAsync(MealAdditionTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<MealAdditionTemplate>(c => c.Id == dto.Id);

        var entityResult = await repository.FirstOrDefaultAsync(spec, true, cancellationToken);
        if (!entityResult.Succeeded || entityResult.Data == null)
            return await Result<MealAdditionTemplateDto>.FailAsync(entityResult.Messages.Count != 0 ? entityResult.Messages : [$"No MealAdditionTemplate with id '{dto.Id}' was found"]);

        var entity = entityResult.Data;

        entity.GuestType = (GuestType)dto.GuestType;
        entity.MealType = (MealType)dto.MealType;
        entity.Notes = dto.Notes;
        entity.RestaurantId = dto.Restaurant.Id;

        var updateResult = repository.Update(entity);
        if (!updateResult.Succeeded)
            return await Result<MealAdditionTemplateDto>.FailAsync(updateResult.Messages);

        var saveResult = await repository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded)
            return await Result<MealAdditionTemplateDto>.FailAsync(saveResult.Messages);

        return await Result<MealAdditionTemplateDto>.SuccessAsync(dto);
    }
    
    /// <summary>
    ///     Deletes a meal‑addition template with the supplied identifier.
    /// </summary>
    /// <param name="id">Identifier of the template to remove.</param>
    /// <param name="cancellationToken">Token that allows the caller to cancel the request.</param>
    /// <returns>A success or failure result depending on the outcome.</returns>
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
