using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities;

/// <summary>
/// Represents a payment exclusion template entity.
/// This class inherits from <see cref="EntityBase{TId}"/> and stores
/// description content used for payment exclusion information.
/// </summary>
public class PaymentExclusionTemplate : EntityBase<string>
{
    /// <summary>
    /// Gets or sets the name of the short description template.
    /// </summary>
    public string TemplateName { get; set; }

    /// <summary>
    /// Gets or sets the description of the short description template.
    /// </summary>
    public string? TemplateDescription { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the booking term.
    /// This property is required and should not be null.
    /// </summary>
    public string Description { get; set; } = null!;
}
