using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace SchoolsModule.Domain.Entities;

/// <summary>
/// Represents an emergency contact for a parent.
/// Stores the details of an individual who should be contacted in the event of an emergency.
/// </summary>
public class ParentEmergencyContact : EntityBase<string>
{
    /// <summary>
    /// Gets or sets the full name of the emergency contact.
    /// </summary>
    [Required]
    [MaxLength(128)]
    public string FullName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the relationship of the emergency contact to the parent or learners.
    /// </summary>
    [MaxLength(64)]
    public string? Relationship { get; set; }

    /// <summary>
    /// Gets or sets the primary phone number for the emergency contact.
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string PrimaryPhoneNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets an optional secondary phone number for the emergency contact.
    /// </summary>
    [MaxLength(20)]
    public string? SecondaryPhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the email address for the emergency contact.
    /// </summary>
    [EmailAddress]
    [MaxLength(256)]
    public string? EmailAddress { get; set; }

    /// <summary>
    /// Gets or sets additional notes about the emergency contact.
    /// </summary>
    [MaxLength(512)]
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated parent.
    /// </summary>
    [ForeignKey(nameof(Parent))]
    public string? ParentId { get; set; }

    /// <summary>
    /// Gets or sets the parent that owns this emergency contact entry.
    /// </summary>
    public Parent Parent { get; set; } = null!;
}
