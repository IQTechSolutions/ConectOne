using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects;

/// <summary>
/// Represents a data transfer object (DTO) for booking terms description templates.
/// </summary>
/// <remarks>This DTO is used to encapsulate the details of a booking terms description template,  including its
/// unique identifier, name, and descriptions. It can be instantiated  directly or created from an entity of type <see
/// cref="BookingTermsDescriptionTemplate"/>.</remarks>
public record BookingTermsDescriptionTemplateDto
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public BookingTermsDescriptionTemplateDto() { }

    /// <summary>
    /// Creates a new instance from an entity
    /// </summary>
    /// <param name="entity">Entity source</param>
    public BookingTermsDescriptionTemplateDto(BookingTermsDescriptionTemplate entity)
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
    public string Id { get; init; } = null!;

    /// <summary>
    /// Gets the name of the template.
    /// </summary>
    public string TemplateName { get; init; } = null!;

    /// <summary>
    /// Gets the description of the template.
    /// </summary>
    public string TemplateDescription { get; init; } = null!;

    /// <summary>
    /// Gets the description associated with the current object.
    /// </summary>
    public string Description { get; init; } = null!;

    #endregion
}
