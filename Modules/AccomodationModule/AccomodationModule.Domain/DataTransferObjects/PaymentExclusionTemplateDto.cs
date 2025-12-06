using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects;

/// <summary>
/// Data Transfer Object for <see cref="PaymentExclusionTemplate"/> entities.
/// </summary>
public record PaymentExclusionTemplateDto
{
    #region Constructors
    
    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentExclusionTemplateDto"/> class.
    /// </summary>
    public PaymentExclusionTemplateDto() { }

    /// <summary>
    /// Creates a new instance from an entity.
    /// </summary>
    /// <param name="entity">Entity source.</param>
    public PaymentExclusionTemplateDto(PaymentExclusionTemplate entity)
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
}
