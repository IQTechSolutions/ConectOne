using ShoppingModule.Domain.Enums;

namespace ShoppingModule.Domain.RequestFeatures;

/// <summary>
/// Represents a PayFast payment notification payload that is required to persist a donation once payment succeeds.
/// </summary>
public class DonationPaymentNotification
{
    /// <summary>
    /// Gets or sets the identifier assigned to the donation during the initial payment request.
    /// </summary>
    public string? DonationId { get; set; }

    /// <summary>
    /// Gets or sets the status value reported by PayFast.
    /// </summary>
    public string? PaymentStatus { get; set; }

    /// <summary>
    /// Gets or sets the PayFast payment reference.
    /// </summary>
    public string? PaymentReference { get; set; }

    /// <summary>
    /// Gets or sets the donor's full name.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the individual.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets the donor's email address.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the donor's phone number.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the amount paid by the donor.
    /// </summary>
    public double Amount { get; set; }

    /// <summary>
    /// Gets or sets the donation designation captured during checkout.
    /// </summary>
    public string? Designation { get; set; }

    /// <summary>
    /// Gets or sets the donor's message captured during checkout.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the donation should remain anonymous.
    /// </summary>
    public bool IsAnonymous { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the donation is a recurring subscription.
    /// </summary>
    public bool IsRecurring { get; set; }

    /// <summary>
    /// Gets or sets the selected donation frequency.
    /// </summary>
    public DonationFrequency Frequency { get; set; } = DonationFrequency.OnceOff;

    /// <summary>
    /// Gets or sets the PayFast subscription token when the donation is recurring.
    /// </summary>
    public string? SubscriptionToken { get; set; }
}
