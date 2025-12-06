using System.ComponentModel.DataAnnotations;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities;

/// <summary>
/// This class represents a template for short descriptions used in the accommodation module.
/// </summary>
public class ShortDescriptionTemplate : EntityBase<string>
{
    /// <summary>
    /// Gets or sets the name of the short description template.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the content of the short description template.
    /// </summary>
    [DataType(DataType.MultilineText)] public string? Content { get; set; }
}