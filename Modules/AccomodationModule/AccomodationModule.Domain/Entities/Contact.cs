using System.ComponentModel.DataAnnotations;
using AccomodationModule.Domain.Enums;
using FilingModule.Domain.Entities;

namespace AccomodationModule.Domain.Entities;

/// <summary>
/// Represents a contact entity, extending the ImageFileCollection to include image file management.
/// </summary>
public class Contact : FileCollection<Contact, string>
{
    #region Properties

    /// <summary>
    /// Gets or sets the first name of the contact. This property is required.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the last name of the contact. This property is required.
    /// </summary>
    public string Surname { get; set; } = null!;

    /// <summary>
    /// Gets or sets the phone number of the contact. This property is required.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Gets or sets the email address of the contact. This property is required.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the biography or a short description of the contact. This property is required.
    /// </summary>
    public string? Bio { get; set; }

    /// <summary>
    /// Gets or sets the type of contact. This property is required.
    /// </summary>
    public ContactType ContactType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the item is featured.
    /// </summary>
    public bool Featured { get; set; }

    /// <summary>
    /// Gets or sets the selector for the vacation price.
    /// </summary>
    [MaxLength(1000)] public string? Selector { get; set; }

    /// <summary>
    /// Gets or sets the order of the vacation price.
    /// </summary>
    public int Order { get; set; }

    #endregion

    #region Many-to-One Relationships

    /// <summary>
    /// Gets or sets the collection of vacation contacts associated with this entity.
    /// </summary>
    public ICollection<VacationContact> Vacations { get; set; } = [];

    #endregion
}
