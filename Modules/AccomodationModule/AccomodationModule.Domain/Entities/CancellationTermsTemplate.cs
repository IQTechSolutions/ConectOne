using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities;

/// <summary>
/// Represents a template for cancellation terms, including its name and descriptions.
/// </summary>
public class CancellationTermsTemplate : EntityBase<string>
{
    /// <summary>
    /// Gets or sets the name of the template.
    /// </summary>
    public string TemplateName { get; set; }

    /// <summary>
    /// Gets or sets the description of the template.
    /// </summary>
    public string TemplateDescription { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description text.
    /// </summary>
    public string Description { get; set; } = null!;
}
