namespace ShoppingModule.Domain.RequestFeatures;

/// <summary>
/// Represents redirect details for an external payment provider, including the target process URL and the
/// collection of form fields that must be posted to the provider.
/// </summary>
public class PaymentRedirectDetails
{
    /// <summary>
    /// Gets or sets the absolute URL the client must post the payment form to.
    /// </summary>
    public string? ProcessUrl { get; set; }

    /// <summary>
    /// Gets or sets the collection of form fields required by the payment provider.
    /// </summary>
    public Dictionary<string, string> Fields { get; set; } = new();
}

/// <summary>
/// Represents the result of a donation submission, including optional redirect information for immediate payment
/// processing.
/// </summary>
/// <param name="Succeeded">Indicates whether the donation was created successfully.</param>
/// <param name="ErrorMessage">Contains an error message when <paramref name="Succeeded"/> is <see langword="false"/>.</param>
/// <param name="DonationId">The unique identifier assigned to the donation.</param>
/// <param name="PayGateRedirect">Redirect metadata for PayGate payments.</param>
/// <param name="PayFastRedirect">Redirect metadata for PayFast payments.</param>
public record DonationSubmissionResult(bool Succeeded, string? ErrorMessage, string? DonationId = null, PaymentRedirectDetails? PayGateRedirect = null, PaymentRedirectDetails? PayFastRedirect = null)
{
    /// <summary>
    /// Creates a successful donation submission result with optional payment redirect information.
    /// </summary>
    /// <param name="donationId">The identifier of the created donation.</param>
    /// <param name="payGateRedirect">Optional PayGate redirect information.</param>
    /// <param name="payFastRedirect">Optional PayFast redirect information.</param>
    /// <returns>A <see cref="DonationSubmissionResult"/> representing a successful donation.</returns>
    public static DonationSubmissionResult Success(string? donationId = null, PaymentRedirectDetails? payGateRedirect = null, PaymentRedirectDetails? payFastRedirect = null) =>
        new(true, null, donationId, payGateRedirect, payFastRedirect);

    /// <summary>
    /// Creates a <see cref="DonationSubmissionResult"/> representing a failed donation submission.
    /// </summary>
    /// <param name="errorMessage">An optional error message describing the reason for the failure.</param>
    /// <returns>A <see cref="DonationSubmissionResult"/> instance with a failure status and the specified error message.</returns>
    public static DonationSubmissionResult Failure(string? errorMessage) => new(false, errorMessage);
}
