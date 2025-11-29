using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.DataTransferObjects;

/// <summary>
/// Data transfer object for <see cref="SeverityScale"/>.
/// </summary>
public record SeverityScaleDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SeverityScaleDto"/> class.
    /// </summary>
    public SeverityScaleDto() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SeverityScaleDto"/> class using the specified <see
    /// cref="SeverityScale"/> entity.
    /// </summary>
    /// <param name="entity">The <see cref="SeverityScale"/> entity from which to populate the DTO properties. Cannot be <see
    /// langword="null"/>.</param>
    public SeverityScaleDto(SeverityScale entity)
    {
        SeverityScaleId = entity.Id;
        Name = entity.Name;
        Score = entity.Score;
        Description = entity.Description;
    }

    /// <summary>
    /// Gets the identifier of the severity scale associated with this instance.
    /// </summary>
    public string? SeverityScaleId { get; init; }

    /// <summary>
    /// Gets the name associated with the object.
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Gets the score associated with the current instance.
    /// </summary>
    public int Score { get; init; }

    /// <summary>
    /// Gets or initializes the description associated with the object.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Creates a new instance of the <see cref="SeverityScale"/> class with the current object's properties.
    /// </summary>
    /// <remarks>The method initializes a <see cref="SeverityScale"/> object using the current values of the 
    /// <see cref="SeverityScaleId"/>, <see cref="Name"/>, <see cref="Score"/>, and <see cref="Description"/>
    /// properties. If <see cref="SeverityScaleId"/> is null or empty, a new GUID is generated for the <c>Id</c>
    /// property.</remarks>
    /// <returns>A new <see cref="SeverityScale"/> instance populated with the current object's property values.</returns>
    public SeverityScale CreateSeverityScale() => new()
    {
        Id = string.IsNullOrEmpty(SeverityScaleId) ? Guid.NewGuid().ToString() : SeverityScaleId,
        Name = Name,
        Score = Score,
        Description = Description
    };
}
