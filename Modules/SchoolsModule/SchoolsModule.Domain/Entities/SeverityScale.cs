using ConectOne.Domain.Entities;

namespace SchoolsModule.Domain.Entities;

/// <summary>
/// Represents a scale describing the severity of a disciplinary incident.
/// </summary>
public class SeverityScale : EntityBase<string>
{
    /// <summary>
    /// Gets or sets the name associated with the object.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the score associated with the current instance.
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// Gets or sets the description associated with the object.
    /// </summary>
    public string? Description { get; set; }
}
