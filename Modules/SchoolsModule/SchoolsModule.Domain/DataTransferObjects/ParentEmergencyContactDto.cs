using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.DataTransferObjects;

/// <summary>
/// Data transfer object representing a parent's emergency contact.
/// </summary>
public record ParentEmergencyContactDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ParentEmergencyContactDto"/> class.
    /// </summary>
    public ParentEmergencyContactDto() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParentEmergencyContactDto"/> class from an entity instance.
    /// </summary>
    /// <param name="contact">The entity instance to map from.</param>
    public ParentEmergencyContactDto(ParentEmergencyContact contact)
    {
        Id = contact.Id;
        ParentId = contact.ParentId;
        FullName = contact.FullName;
        Relationship = contact.Relationship;
        PrimaryPhoneNumber = contact.PrimaryPhoneNumber;
        SecondaryPhoneNumber = contact.SecondaryPhoneNumber;
        EmailAddress = contact.EmailAddress;
        Notes = contact.Notes;
    }

    /// <summary>
    /// The unique identifier for the emergency contact.
    /// </summary>
    public string Id { get; init; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Identifier of the parent that owns the emergency contact.
    /// </summary>
    public string? ParentId { get; init; }

    /// <summary>
    /// The full name of the emergency contact.
    /// </summary>
    public string FullName { get; init; } = null!;

    /// <summary>
    /// The relationship of the contact to the parent or learner.
    /// </summary>
    public string? Relationship { get; init; }

    /// <summary>
    /// The primary phone number for the emergency contact.
    /// </summary>
    public string PrimaryPhoneNumber { get; init; } = null!;

    /// <summary>
    /// Optional secondary phone number for the emergency contact.
    /// </summary>
    public string? SecondaryPhoneNumber { get; init; }

    /// <summary>
    /// Optional email address for the emergency contact.
    /// </summary>
    public string? EmailAddress { get; init; }

    /// <summary>
    /// Additional notes about the emergency contact.
    /// </summary>
    public string? Notes { get; init; }

    /// <summary>
    /// Converts the DTO into an entity instance.
    /// </summary>
    /// <returns>The mapped <see cref="ParentEmergencyContact"/> entity.</returns>
    public ParentEmergencyContact ToEntity()
    {
        return new ParentEmergencyContact
        {
            Id = Id,
            ParentId = ParentId,
            FullName = FullName,
            Relationship = Relationship,
            PrimaryPhoneNumber = PrimaryPhoneNumber,
            SecondaryPhoneNumber = SecondaryPhoneNumber,
            EmailAddress = EmailAddress,
            Notes = Notes
        };
    }
}
