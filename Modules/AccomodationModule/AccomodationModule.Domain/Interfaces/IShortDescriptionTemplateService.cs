using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces;

/// <summary>
/// Service contract for managing <see cref="ShortDescriptionTemplate"/> entities.
/// </summary>
public interface IShortDescriptionTemplateService
{
    /// <summary>
    /// Retrieves all short description templates.
    /// </summary>
    Task<IBaseResult<IEnumerable<ShortDescriptionTemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a template by its identifier.
    /// </summary>
    Task<IBaseResult<ShortDescriptionTemplateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new template.
    /// </summary>
    Task<IBaseResult<ShortDescriptionTemplateDto>> AddAsync(ShortDescriptionTemplateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing template.
    /// </summary>
    Task<IBaseResult<ShortDescriptionTemplateDto>> EditAsync(ShortDescriptionTemplateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a template by its identifier.
    /// </summary>
    Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default);
}
