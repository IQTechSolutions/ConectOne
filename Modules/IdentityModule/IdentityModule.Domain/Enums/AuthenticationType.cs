namespace IdentityModule.Domain.Enums
{
    /// <summary>
    /// Specifies the type of authentication used in the <see cref="AuthRequest"/>.
    /// </summary>
    public enum AuthenticationType
    {
        /// <summary>
        /// Authentication using a username.
        /// </summary>
        UserName,

        /// <summary>
        /// Authentication using an email address.
        /// </summary>
        Email
    }
}
