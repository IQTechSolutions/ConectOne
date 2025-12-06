using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects;

/// <summary>
/// Data Transfer Object for <see cref="GeneralInformationTemplate"/>.
/// </summary>
public record GeneralInformationTemplateDto
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public GeneralInformationTemplateDto() {}

    /// <summary>
    /// Create a new instance from the entity
    /// </summary>
    /// <param name="entity">Entity to map from</param>
    public GeneralInformationTemplateDto(GeneralInformationTemplate entity)
    {
        Id = entity.Id;
        Name = entity.Name;
        Information = entity.Information;
    }
    
    #endregion

    /// <summary>
    /// Gets the template identifier
    /// </summary>
    public string Id { get; init; } = null!;

    /// <summary>
    /// Gets the template name
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Additional information for the template
    /// </summary>
    public string? Information { get; init; }
}
