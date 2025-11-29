using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.DataTransferObjects;

/// <summary>
/// Data transfer object for <see cref="DisciplinaryIncident"/>.
/// </summary>
public record DisciplinaryIncidentDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DisciplinaryIncidentDto"/> class.
    /// </summary>
    public DisciplinaryIncidentDto() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DisciplinaryIncidentDto"/> class using the specified disciplinary
    /// incident entity.
    /// </summary>
    /// <remarks>This constructor maps the properties of the provided <see cref="DisciplinaryIncident"/>
    /// entity to the corresponding properties of the DTO. Ensure that the <paramref name="entity"/> parameter is not
    /// <see langword="null"/> before calling this constructor.</remarks>
    /// <param name="entity">The disciplinary incident entity from which to populate the DTO. Cannot be <see langword="null"/>.</param>
    public DisciplinaryIncidentDto(DisciplinaryIncident entity)
    {
        DisciplinaryIncidentId = entity.Id;
        Date = entity.Date;
        Description = entity.Description;
        LearnerId = entity.LearnerId;
        DisciplinaryActionId = entity.DisciplinaryActionId;
        SeverityScore = entity.SeverityScore;
    }

    /// <summary>
    /// Gets the unique identifier for the disciplinary incident.
    /// </summary>
    public string? DisciplinaryIncidentId { get; init; }

    /// <summary>
    /// Gets the date associated with this instance.
    /// </summary>
    public DateTime Date { get; init; }

    /// <summary>
    /// Gets the description associated with the object.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets the unique identifier for the learner.
    /// </summary>
    public string LearnerId { get; init; } = null!;

    /// <summary>
    /// Gets the unique identifier for the disciplinary action.
    /// </summary>
    public string DisciplinaryActionId { get; init; } = null!;

    /// <summary>
    /// Gets the severity score associated with the current instance.
    /// </summary>
    public int SeverityScore { get; init; }

    /// <summary>
    /// Creates a new instance of a <see cref="DisciplinaryIncident"/> with the current property values.
    /// </summary>
    /// <remarks>The method initializes a <see cref="DisciplinaryIncident"/> object using the values of the 
    /// current instance's properties. If <see cref="DisciplinaryIncidentId"/> is null or empty,  a new unique
    /// identifier is generated for the incident.</remarks>
    /// <returns>A <see cref="DisciplinaryIncident"/> object populated with the current property values.</returns>
    public DisciplinaryIncident CreateDisciplinaryIncident() => new()
    {
        Id = string.IsNullOrEmpty(DisciplinaryIncidentId) ? Guid.NewGuid().ToString() : DisciplinaryIncidentId,
        Date = Date,
        Description = Description,
        LearnerId = LearnerId,
        DisciplinaryActionId = DisciplinaryActionId,
        SeverityScore = SeverityScore
    };
}
