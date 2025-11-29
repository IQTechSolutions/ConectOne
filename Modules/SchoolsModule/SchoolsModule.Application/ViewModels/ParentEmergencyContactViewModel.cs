using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsModule.Application.ViewModels;

/// <summary>
/// View model representing an emergency contact for a parent.
/// </summary>
public class ParentEmergencyContactViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ParentEmergencyContactViewModel"/> class.
    /// </summary>
    public ParentEmergencyContactViewModel() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParentEmergencyContactViewModel"/> class from a DTO instance.
    /// </summary>
    /// <param name="dto">The DTO to map from.</param>
    public ParentEmergencyContactViewModel(ParentEmergencyContactDto dto)
    {
        ContactId = dto.Id;
        ParentId = dto.ParentId;
        FullName = dto.FullName;
        Relationship = dto.Relationship;
        PrimaryPhoneNumber = dto.PrimaryPhoneNumber;
        SecondaryPhoneNumber = dto.SecondaryPhoneNumber;
        EmailAddress = dto.EmailAddress;
        Notes = dto.Notes;
    }

    /// <summary>
    /// Gets or sets the identifier of the emergency contact.
    /// </summary>
    public string? ContactId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the identifier of the parent to whom the emergency contact belongs.
    /// </summary>
    public string? ParentId { get; set; }

    /// <summary>
    /// Gets or sets the full name of the emergency contact.
    /// </summary>
    public string FullName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the relationship of the contact to the parent or learner.
    /// </summary>
    public string? Relationship { get; set; }

    /// <summary>
    /// Gets or sets the primary phone number for the emergency contact.
    /// </summary>
    public string PrimaryPhoneNumber { get; set; } = null!;

    /// <summary>
    /// Gets or sets an optional secondary phone number for the emergency contact.
    /// </summary>
    public string? SecondaryPhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the email address for the emergency contact.
    /// </summary>
    public string? EmailAddress { get; set; }

    /// <summary>
    /// Gets or sets any additional notes about the emergency contact.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Converts the view model to a DTO instance.
    /// </summary>
    /// <returns>The populated <see cref="ParentEmergencyContactDto"/>.</returns>
    public ParentEmergencyContactDto ToDto()
    {
        return new ParentEmergencyContactDto
        {
            Id = ContactId ?? Guid.NewGuid().ToString(),
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
