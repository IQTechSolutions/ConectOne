using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace SchoolsModule.Domain.Entities;

/// <summary>
/// Records an incident of learner misbehaviour.
/// </summary>
public class DisciplinaryIncident : EntityBase<string>
{
    /// <summary>
    /// Gets or sets the date associated with the current instance.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the description associated with the object.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the learner associated with this entity.
    /// </summary>
    [ForeignKey(nameof(Learner))] public string LearnerId { get; set; } = null!;

    /// <summary>
    /// Gets or sets the learner associated with the current context.
    /// </summary>
    public Learner Learner { get; set; } = null!;

    /// <summary>
    /// Gets or sets the unique identifier for the associated disciplinary action.
    /// </summary>
    [ForeignKey(nameof(DisciplinaryAction))] public string DisciplinaryActionId { get; set; } = null!;

    /// <summary>
    /// Gets or sets the disciplinary action associated with the entity.
    /// </summary>
    public DisciplinaryAction DisciplinaryAction { get; set; } = null!;

    /// <summary>
    /// Gets or sets the severity score associated with the current instance.
    /// </summary>
    public int SeverityScore { get; set; }
}
