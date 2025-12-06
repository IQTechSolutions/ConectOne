using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects;

/// <summary>
/// Data Transfer Object for <see cref="CancellationTermsTemplate"/> entities.
/// </summary>
public record CancellationTermsTemplateDto
{
    #region Constructors
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CancellationTermsTemplateDto"/> class.
    /// </summary>
    public CancellationTermsTemplateDto() { }

    /// <summary>
    /// Creates a new instance from an entity.
    /// </summary>
    /// <param name="entity">Entity source.</param>
    public CancellationTermsTemplateDto(CancellationTermsTemplate entity)
    {
        Id = entity.Id;
        TemplateName = entity.TemplateName;
        TemplateDescription = entity.TemplateDescription;
        Description = entity.Description;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the unique identifier for the entity.
    /// </summary>
    public string? Id { get; init; } = null!;

    /// <summary>
    /// Gets the name of the template associated with this instance.
    /// </summary>
    public string? TemplateName { get; init; } = null!;

    /// <summary>
    /// Gets the description of the template.
    /// </summary>
    public string? TemplateDescription { get; init; } = null!;

    /// <summary>
    /// Gets the description associated with the object.
    /// </summary>
    public string? Description { get; init; } = null!;

    #endregion
}
