using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects;

/// <summary>
/// Data Transfer Object for <see cref="TermsAndConditionsTemplate"/> entities.
/// </summary>
public record TermsAndConditionsTemplateDto
{
    #region Constructors
    
    /// <summary>
    /// Initializes a new instance of the <see cref="TermsAndConditionsTemplateDto"/> class.
    /// </summary>
    public TermsAndConditionsTemplateDto() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TermsAndConditionsTemplateDto"/> class using the specified entity.
    /// </summary>
    /// <param name="entity">The <see cref="TermsAndConditionsTemplate"/> entity from which to populate the DTO properties. Cannot be
    /// <c>null</c>.</param>
    public TermsAndConditionsTemplateDto(TermsAndConditionsTemplate entity)
    {
        Id = entity.Id;
        TemplateName = entity.TemplateName;
        TemplateDescription = entity.TemplateDescription;
        Description = entity.Description;
    }

    #endregion

    /// <summary>
    /// Gets the unique identifier for the entity.
    /// </summary>
    public string Id { get; init; } = null!;

    /// <summary>
    /// Gets the name of the template associated with this instance.
    /// </summary>
    public string TemplateName { get; init; } = null!;

    /// <summary>
    /// Gets the description of the template.
    /// </summary>
    public string TemplateDescription { get; init; } = null!;

    /// <summary>
    /// Gets the description associated with the object.
    /// </summary>
    public string Description { get; init; } = null!;
}
