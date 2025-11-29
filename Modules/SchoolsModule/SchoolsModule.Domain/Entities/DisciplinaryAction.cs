using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace SchoolsModule.Domain.Entities;

/// <summary>
/// Represents an action that may be taken as a result of a disciplinary incident.
/// </summary>
public class DisciplinaryAction : EntityBase<string>
{
    /// <summary>
    /// Gets or sets the name associated with the object.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description associated with the object.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Gets or sets the identifier for the associated severity scale.
    /// </summary>
    [ForeignKey(nameof(SeverityScale))] public string? SeverityScaleId { get; set; }

    /// <summary>
    /// Gets or sets the severity scale associated with the current context.
    /// </summary>
    public SeverityScale? SeverityScale { get; set; }

    /// <summary>
    /// Gets or sets the collection of disciplinary incidents associated with this entity.
    /// </summary>
    public virtual ICollection<DisciplinaryIncident> Incidents { get; set; } = [];
}
