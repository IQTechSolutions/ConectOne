using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects;

/// <summary>
/// Data Transfer Object for <see cref="CustomVariableTag"/> entities.
/// </summary>
public record CustomVariableTagDto
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomVariableTagDto"/> class.
    /// </summary>
    public CustomVariableTagDto() { }

    /// <summary>
    /// Initializes a new instance from the specified entity.
    /// </summary>
    /// <param name="entity">The source entity.</param>
    public CustomVariableTagDto(CustomVariableTag entity)
    {
        Id = entity.Id;
        Name = entity.Name;
        Value = entity.Value;
        Description = entity.Description;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the unique identifier for the entity.
    /// </summary>
    public string Id { get; init; } = null!;

    /// <summary>
    /// Gets the name associated with the current instance.
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Gets or sets the value represented by this instance.
    /// </summary>
    public string Value { get; set; } = null!;

    /// <summary>
    /// Gets or initializes the description associated with the object.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets the placeholder text for this variable tag.
    /// </summary>
    public string VariablePlaceholder => $"<---{Name}--->";

    #endregion
}
