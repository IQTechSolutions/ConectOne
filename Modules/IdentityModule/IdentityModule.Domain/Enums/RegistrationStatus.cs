namespace IdentityModule.Domain.Enums
{
    /// <summary>
    /// Specifies the possible statuses for a registration process.
    /// </summary>
    /// <remarks>Use this enumeration to represent the current state of a registration, such as when tracking
    /// user sign-up requests or application submissions. The values indicate whether the registration is awaiting
    /// review, has been rejected, or has been accepted.</remarks>
    public enum RegistrationStatus
    {
        Pending = 0, Rejected = 1, Accepted = 2
    }
}
