using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces;

/// <summary>
/// Service contract for managing <see cref="VacationTitleTemplate"/> entities.
/// </summary>
public interface IVacationTitleTemplateService
{
    /// <summary>
    /// Retrieves all title templates.
    /// </summary>
    Task<IBaseResult<IEnumerable<VacationTitleTemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a template by its identifier.
    /// </summary>
    Task<IBaseResult<VacationTitleTemplateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new template.
    /// </summary>
    Task<IBaseResult<VacationTitleTemplateDto>> AddAsync(VacationTitleTemplateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing template.
    /// </summary>
    Task<IBaseResult<VacationTitleTemplateDto>> EditAsync(VacationTitleTemplateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a template by its identifier.
    /// </summary>
    Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default);
}
