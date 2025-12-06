using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Application.ViewModels;

/// <summary>
/// View model for <see cref="TermsAndConditionsTemplate"/> instances.
/// </summary>
public class TermsAndConditionsTemplateViewModel : EntityBase<string>
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="TermsAndConditionsTemplateViewModel"/> class.
    /// </summary>
    public TermsAndConditionsTemplateViewModel() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TermsAndConditionsTemplateViewModel"/> class using the specified
    /// data transfer object.
    /// </summary>
    /// <param name="dto">The data transfer object containing the terms and conditions template details. Cannot be null.</param>
    public TermsAndConditionsTemplateViewModel(TermsAndConditionsTemplateDto dto)
    {
        Id = dto.Id;
        TemplateName = dto.TemplateName;
        TemplateDescription = dto.TemplateDescription;
        Description = dto.Description;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the name of the template.
    /// </summary>
    public string TemplateName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the template.
    /// </summary>
    public string TemplateDescription { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description associated with the object.
    /// </summary>
    public string Description { get; set; } = null!;

    #endregion

    #region Methods

    /// <summary>
    /// Converts the current instance of the template to a data transfer object (DTO).
    /// </summary>
    /// <returns>A <see cref="TermsAndConditionsTemplateDto"/> representing the current template,  including its ID, name, and
    /// descriptions.</returns>
    public TermsAndConditionsTemplateDto ToDto()
    {
        return new TermsAndConditionsTemplateDto
        {
            Id = this.Id,
            TemplateName = this.TemplateName,
            TemplateDescription = this.TemplateDescription,
            Description = this.Description
        };
    }

    #endregion




}
