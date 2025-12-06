using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Application.ViewModels;

/// <summary>
/// View model for <see cref="ShortDescriptionTemplate"/> instances.
/// </summary>
public class ShortDescriptionTemplateViewModel : EntityBase<string>
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ShortDescriptionTemplateViewModel"/> class.
    /// </summary>
    public ShortDescriptionTemplateViewModel() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ShortDescriptionTemplateViewModel"/> class from a DTO.
    /// </summary>
    /// <param name="dto">DTO to map from.</param>
    public ShortDescriptionTemplateViewModel(ShortDescriptionTemplateDto dto)
    {
        Id = dto.Id;
        Title = dto.Title;
        Content = dto.Content;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the template identifier.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the template name.
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the template content.
    /// </summary>
    public string? Content { get; set; }

    #endregion

    #region Methods

    /// <summary>
    /// Converts the current instance to a <see cref="ShortDescriptionTemplateDto"/> object.
    /// </summary>
    /// <returns>A <see cref="ShortDescriptionTemplateDto"/> that contains the ID, title, and content of the current instance.</returns>
    public ShortDescriptionTemplateDto ToDto()
    {
        return new ShortDescriptionTemplateDto()
        {
            Id = Id,
            Title = Title,
            Content = Content
        };
    }

    #endregion

}
