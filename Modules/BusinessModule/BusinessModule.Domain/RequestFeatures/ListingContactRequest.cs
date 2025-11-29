using System.ComponentModel.DataAnnotations;

namespace BusinessModule.Domain.RequestFeatures;

/// <summary>
/// Represents the payload required to contact a business listing owner.
/// </summary>
public record ListingContactRequest
{
    /// <summary>
    /// Gets or sets the identifier of the listing that should receive the enquiry.
    /// </summary>
    [Required] public string ListingId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name supplied by the person submitting the enquiry.
    /// </summary>
    [Required] public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address supplied by the person submitting the enquiry.
    /// </summary>
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the message that should be delivered to the listing owner.
    /// </summary>
    [Required] public string Message { get; set; } = string.Empty;
}
