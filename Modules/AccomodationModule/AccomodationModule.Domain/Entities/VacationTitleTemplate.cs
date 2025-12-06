using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities;

/// <summary>
/// Represents a template for generating vacation titles, including a name, description, and the title itself.
/// </summary>
/// <remarks>This class is used to define and manage templates for vacation titles, which may include a short
/// description and a title. It inherits from <see cref="EntityBase{TId}"/> with a key of type <see
/// cref="string"/>.</remarks>
public class VacationTitleTemplate : EntityBase<string>
{
    /// <summary>
    /// Gets or sets the title of the vacation.
    /// </summary>
    public string VacationTitle { get; set; } = null!;
}
