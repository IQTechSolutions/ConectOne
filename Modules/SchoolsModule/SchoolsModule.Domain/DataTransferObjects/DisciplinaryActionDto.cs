using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.DataTransferObjects;

/// <summary>
/// Data transfer object for <see cref="DisciplinaryAction"/>.
/// </summary>
public record DisciplinaryActionDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DisciplinaryActionDto"/> class.
    /// </summary>
    public DisciplinaryActionDto() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DisciplinaryActionDto"/> class using the specified disciplinary
    /// action entity.
    /// </summary>
    /// <param name="entity">The disciplinary action entity from which to populate the DTO. Cannot be <see langword="null"/>.</param>
    public DisciplinaryActionDto(DisciplinaryAction entity)
    {
        DisciplinaryActionId = entity.Id;
        Name = entity.Name;
        Description = entity.Description;
        SeverityScaleId = entity.SeverityScaleId;
    }

    /// <summary>
    /// Gets the unique identifier for the disciplinary action.
    /// </summary>
    public string? DisciplinaryActionId { get; init; }

    /// <summary>
    /// Gets the name associated with the object.
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Gets the description associated with the object.
    /// </summary>
    public string Description { get; init; } = null!;

    /// <summary>
    /// Gets the identifier of the severity scale associated with this instance.
    /// </summary>
    public string? SeverityScaleId { get; init; }

    /// <summary>
    /// Creates a new instance of the <see cref="DisciplinaryAction"/> class with the current object's properties.
    /// </summary>
    /// <remarks>If <see cref="DisciplinaryActionId"/> is null or empty, a new unique identifier is generated
    /// for the <see cref="DisciplinaryAction.Id"/> property.</remarks>
    /// <returns>A new <see cref="DisciplinaryAction"/> object populated with the current values of <see cref="Name"/>, <see
    /// cref="Description"/>, and <see cref="SeverityScaleId"/>.</returns>
    public DisciplinaryAction CreateDisciplinaryAction() => new()
    {
        Id = string.IsNullOrEmpty(DisciplinaryActionId) ? Guid.NewGuid().ToString() : DisciplinaryActionId,
        Name = Name,
        Description = Description,
        SeverityScaleId = SeverityScaleId
    };
}
