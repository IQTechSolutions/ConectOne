using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using ConectOne.Domain.ResultWrappers;

namespace AccomodationModule.Domain.Interfaces;

/// <summary>
/// Service contract for managing <see cref="DayTourActivityTemplate"/> entities.
/// </summary>
public interface IDayTourActivityTemplateService
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
    Task<IBaseResult<List<DayTourActivityTemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves a single template by its identifier.
    /// </summary>
    /// <param name="id">Primary‑key of the template.</param>
    /// <param name="cancellationToken">Token propagated to the data‑access layer.</param>
    Task<IBaseResult<DayTourActivityTemplateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Persists a new template.
    /// </summary>
    /// <param name="dto">Input DTO describing the template.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IBaseResult<DayTourActivityTemplateDto>> AddAsync(DayTourActivityTemplateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates an existing template. Only specific properties are mutable –
    ///     everything else remains unchanged.
    /// </summary>
    /// <param name="dto">DTO containing the new state. Its <see cref="DayTourActivityTemplateDto.Id"/> serves as lookup‑key.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IBaseResult<DayTourActivityTemplateDto>> EditAsync(DayTourActivityTemplateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deletes a template by its identifier.
    /// </summary>
    Task<IBaseResult> DeleteAsync(string id, CancellationToken cancellationToken = default);
}
