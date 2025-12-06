using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects;

/// <summary>
/// Data Transfer Object for <see cref="VacationTitleTemplate"/>.
/// </summary>
public record VacationTitleTemplateDto
{
    #region Constructors
    /// <summary>
    /// Default constructor
    /// </summary>
    public VacationTitleTemplateDto() {}

    /// <summary>
    /// Initializes a new instance from the specified entity.
    /// </summary>
    /// <param name="entity">Source entity.</param>
    public VacationTitleTemplateDto(VacationTitleTemplate entity)
    {
        Id = entity.Id;
        VacationTitle = entity.VacationTitle;
    }

    #endregion

    /// <summary>
    /// Gets the identifier.
    /// </summary>
    public string Id { get; init; } = null!;

    /// <summary>
    /// Gets the vacation title text.
    /// </summary>
    public string VacationTitle { get; init; } = null!;
}