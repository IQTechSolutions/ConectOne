using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects;

/// <summary>
/// Data Transfer Object for <see cref="ShortDescriptionTemplate"/>.
/// </summary>
public record ShortDescriptionTemplateDto
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public ShortDescriptionTemplateDto() { }

    /// <summary>
    /// Initializes a new instance from the specified entity.
    /// </summary>
    /// <param name="entity">Entity to map from.</param>
    public ShortDescriptionTemplateDto(ShortDescriptionTemplate entity)
    {
        Id = entity.Id;
        Title = entity.Title;
        Content = entity.Content;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the template identifier.
    /// </summary>
    public string? Id { get; init; } = null!;

    /// <summary>
    /// Gets the template name.
    /// </summary>
    public string? Title { get; init; } = null!;

    /// <summary>
    /// Gets the template content.
    /// </summary>
    public string? Content { get; init; }

    #endregion
}
