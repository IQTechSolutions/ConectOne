using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;

namespace AccomodationModule.Domain.DataTransferObjects;

/// <summary>
/// Data Transfer Object for <see cref="DayTourActivityTemplate"/>.
/// </summary>
public record DayTourActivityTemplateDto
{
    #region Constructors
    /// <summary>
    /// Default constructor
    /// </summary>
    public DayTourActivityTemplateDto() {}

    /// <summary>
    /// Initializes a new instance from the entity.
    /// </summary>
    /// <param name="entity">Entity to map from</param>
    public DayTourActivityTemplateDto(DayTourActivityTemplate entity)
    {
        Id = entity?.Id;
        Name = entity?.Name;
        Summary = entity?.Summary;
        Description = entity?.Description;
        GuestType = entity?.GuestType == null ? GuestType.All : entity.GuestType;
        DisplayInOverview = entity?.DisplayInOverview == null ? true : entity.DisplayInOverview;
    }
    #endregion

    /// <summary>
    /// Gets the template identifier
    /// </summary>
    public string? Id { get; init; } = null!;

    /// <summary>
    /// Gets the template name
    /// </summary>
    public string? Name { get; init; } = null!;

    /// <summary>
    /// Summary information
    /// </summary>
    public string? Summary { get; init; }

    /// <summary>
    /// Activity description
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Guest type applicable to this activity
    /// </summary>
    public GuestType GuestType { get; init; }

    /// <summary>
    /// Indicates if the activity should be shown in overview
    /// </summary>
    public bool DisplayInOverview { get; init; }
}
