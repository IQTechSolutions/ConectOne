using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities;

/// <summary>
/// Represents a booking term entity.
/// This class inherits from EntityBase and provides a description for the booking term.
/// </summary>
public class BookingTermsDescriptionTemplate : EntityBase<string>
{
    /// <summary>
    /// Gets or sets the name of the short description template.
    /// </summary>
    public string TemplateName { get; set; }

    /// <summary>
    /// Gets or sets the description of the short description template.
    /// </summary>
    public string TemplateDescription { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the booking term.
    /// This property is required and should not be null.
    /// </summary>
    public string Description { get; set; } = null!;
}
