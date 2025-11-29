namespace IdentityModule.Domain.RequestFeatures;

/// <summary>
/// Data Transfer Object for handling password reset token requests.
/// </summary>
public record ResetPasswordTokenResponse
{
    /// <summary>
    /// Gets or sets the unique identifier for the user associated with the password reset token.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the email address of the user associated with the password reset token.
    /// </summary>
    public string Code { get; init; }
}